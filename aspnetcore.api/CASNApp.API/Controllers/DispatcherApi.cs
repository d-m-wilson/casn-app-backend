/*
 * CASN API
 *
 * This is a test CASN API
 *
 * OpenAPI spec version: 1.0.0
 * Contact: katie@clinicaccess.org
 * Generated by: https://openapi-generator.tech
 */

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using CASNApp.API.Attributes;
using CASNApp.API.Extensions;
using CASNApp.Core.Commands;
using CASNApp.Core.Misc;
using CASNApp.Core.Models;
using CASNApp.Core.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Annotations;

namespace CASNApp.API.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Authorize]
    public class DispatcherApiController : Controller
    {
        private readonly Core.Entities.casn_appContext dbContext;
        private readonly ILoggerFactory loggerFactory;
        private readonly ILogger<DispatcherApiController> logger;
        private readonly string googleApiKey;
        private readonly double vagueLocationMinOffset;
        private readonly double vagueLocationMaxOffset;
        private readonly bool twilioIsEnabled;
		private readonly string twilioAccountSID;
		private readonly string twilioAuthKey;
		private readonly string twilioPhoneNumber;
        private readonly bool badgesAreEnabled;
        private readonly string userTimeZoneName;
        private readonly string appUrl;

        public DispatcherApiController(Core.Entities.casn_appContext dbContext, IConfiguration configuration, ILoggerFactory loggerFactory)
        {
            this.dbContext = dbContext;
            googleApiKey = configuration[Core.Constants.GoogleApiKey];
            vagueLocationMinOffset = double.Parse(configuration[Core.Constants.VagueLocationMinOffset]);
            vagueLocationMaxOffset = double.Parse(configuration[Core.Constants.VagueLocationMaxOffset]);
            this.loggerFactory = loggerFactory;
            logger = loggerFactory.CreateLogger<DispatcherApiController>();
            twilioIsEnabled = bool.Parse(configuration[Core.Constants.TwilioIsEnabled]);
            twilioAccountSID = configuration[Core.Constants.TwilioAccountSID];
			twilioAuthKey = configuration[Core.Constants.TwilioAuthKey];
			twilioPhoneNumber = configuration[Core.Constants.TwilioPhoneNumber];
            badgesAreEnabled = bool.Parse(configuration[Core.Constants.BadgesAreEnabled]);
            userTimeZoneName = configuration[Core.Constants.UserTimeZoneName];
            appUrl = configuration[Core.Constants.CASNAppURL];
        }

        /// <summary>
        /// adds a new appointment
        /// </summary>
        /// <remarks>Adds appointment (and drives) to the system</remarks>
        /// <param name="appointmentDTO">appointmentData to add</param>
        /// <response code="200">Success. Created appointment.</response>
        /// <response code="400">Client Error - please check your request format &amp; try again.</response>
        /// <response code="409">Error. That appointment already exists.</response>
        [HttpPost]
        [Route("api/dispatcher/appointments")]
        [ValidateModelState]
        [SwaggerOperation("AddAppointment")]
        [SwaggerResponse(statusCode: 200, type: typeof(AppointmentDTO), description: "Success. Created appointment.")]
        public virtual async Task<IActionResult> AddAppointment([FromBody]AppointmentDTO appointmentDTO)
        {
            var userEmail = HttpContext.GetUserEmail();
            var volunteerQuery = new VolunteerQuery(dbContext);
            var volunteer = volunteerQuery.GetActiveDispatcherByEmail(userEmail, true);

            if (volunteer == null)
            {
                return Forbid();
            }

            if (!appointmentDTO.Validate())
            {
                return BadRequest(appointmentDTO);
            }

            var appointment = appointmentDTO.Appointment;
            var driveTo = appointmentDTO.DriveTo;
            var driveFrom = appointmentDTO.DriveFrom;

            var serviceProvider = await dbContext.ServiceProvider
                .AsNoTracking()
                .Where(c => c.Id == appointment.ServiceProviderId)
                .FirstOrDefaultAsync();

            if (serviceProvider == null)
            {
                return BadRequest(appointmentDTO);
            }

            var caller = await dbContext.Caller
                .AsNoTracking()
                .Where(p => p.Id == appointment.CallerId)
                .FirstOrDefaultAsync();

            if (caller == null)
            {
                return BadRequest(appointmentDTO);
            }

            var appointmentEntity = new Core.Entities.Appointment
            {
                DispatcherId = volunteer.Id,
                CallerId = caller.Id,
                ServiceProviderId = serviceProvider.Id,
                PickupLocationVague = appointment.PickupLocationVague,
                DropoffLocationVague = appointment.DropoffLocationVague,
                AppointmentDate = appointment.AppointmentDate.Value,
                AppointmentTypeId = appointment.AppointmentTypeId.Value,
                IsActive = true,
                Created = DateTime.UtcNow,
                Updated = null,
            };

            Core.Entities.Drive driveToEntity = null;

            if (driveTo != null)
            {
                driveToEntity = new Core.Entities.Drive
                {
                    Appointment = appointmentEntity,
                    Direction = Drive.DirectionToServiceProvider,
                    DriverId = null,
                    StartAddress = driveTo.StartAddress,
                    StartCity = driveTo.StartCity,
                    StartState = driveTo.StartState,
                    StartPostalCode = driveTo.StartPostalCode,
                    EndAddress = serviceProvider.Address,
                    EndCity = serviceProvider.City,
                    EndState = serviceProvider.State,
                    EndPostalCode = serviceProvider.PostalCode,
                    EndLatitude = serviceProvider.Latitude,
                    EndLongitude = serviceProvider.Longitude,
                    EndGeocoded = serviceProvider.Geocoded,
                    IsActive = true,
                    Created = DateTime.UtcNow,
                    Updated = null,
                };
            }

            Core.Entities.Drive driveFromEntity = null;

            if (driveFrom != null)
            {
                driveFromEntity = new Core.Entities.Drive
                {
                    Appointment = appointmentEntity,
                    Direction = Drive.DirectionFromServiceProvider,
                    DriverId = null,
                    StartAddress = serviceProvider.Address,
                    StartCity = serviceProvider.City,
                    StartState = serviceProvider.State,
                    StartPostalCode = serviceProvider.PostalCode,
                    StartLatitude = serviceProvider.Latitude,
                    StartLongitude = serviceProvider.Longitude,
                    StartGeocoded = serviceProvider.Geocoded,
                    EndAddress = driveFrom.EndAddress,
                    EndCity = driveFrom.EndCity,
                    EndState = driveFrom.EndState,
                    EndPostalCode = driveFrom.EndPostalCode,
                    IsActive = true,
                    Created = DateTime.UtcNow,
                    Updated = null,
                };
            }

            var geocoder = new GeocoderQuery(googleApiKey, loggerFactory.CreateLogger<GeocoderQuery>());

            var driveToAddress = driveToEntity?.GetCallerAddress();
            GeocoderQuery.LatLng driveToLocation = null;

            if (driveToAddress != null)
            {
                driveToLocation = await geocoder.ForwardLookupAsync(driveToAddress);
            }

            if (driveTo != null && driveToLocation == null)
            {
                return StatusCode((int)System.Net.HttpStatusCode.UnprocessableEntity, "Geocoding failed for Pickup Address.");
            }

            driveToEntity?.SetCallerLocation(driveToLocation);
            driveTo?.SetCallerLocation(driveToLocation);

            var driveFromAddress = driveFromEntity?.GetCallerAddress();
            GeocoderQuery.LatLng driveFromLocation = null;

            if (driveFromAddress != null)
            {
                if (driveToAddress != null &&
                    string.Equals(driveToAddress, driveFromAddress, StringComparison.CurrentCultureIgnoreCase))
                {
                    driveFromLocation = driveToLocation;
                }
                else
                {
                    driveFromLocation = await geocoder.ForwardLookupAsync(driveFromAddress);
                }
            }

            if (driveFrom != null && driveFromLocation == null)
            {
                return StatusCode((int)System.Net.HttpStatusCode.UnprocessableEntity, "Geocoding failed for Dropoff Address.");
            }

            driveFromEntity?.SetCallerLocation(driveFromLocation);
            driveFrom?.SetCallerLocation(driveFromLocation);

            var random = new Random();

            var pickupVagueLocation = driveToLocation?.ToVagueLocation(random, vagueLocationMinOffset, vagueLocationMaxOffset);

            if (pickupVagueLocation != null)
            {
                appointmentEntity.PickupVagueLatitude = pickupVagueLocation.Latitude;
                appointmentEntity.PickupVagueLongitude = pickupVagueLocation.Longitude;
            }

            GeocoderQuery.LatLng dropoffVagueLocation;

            if (driveToAddress != null && driveFromAddress != null &&
                string.Equals(driveToAddress, driveFromAddress, StringComparison.CurrentCultureIgnoreCase))
            {
                dropoffVagueLocation = pickupVagueLocation;
            }
            else
            {
                dropoffVagueLocation = driveFromLocation?.ToVagueLocation(random, vagueLocationMinOffset, vagueLocationMaxOffset);
            }

            if (dropoffVagueLocation != null)
            {
                appointmentEntity.DropoffVagueLatitude = dropoffVagueLocation.Latitude;
                appointmentEntity.DropoffVagueLongitude = dropoffVagueLocation.Longitude;
            }

            dbContext.Appointment.Add(appointmentEntity);

            if (driveToEntity != null)
            {
                dbContext.Drive.Add(driveToEntity);
            }

            if (driveFromEntity != null)
            {
                dbContext.Drive.Add(driveFromEntity);
            }

            await dbContext.SaveChangesAsync();

            appointment.Id = appointmentEntity.Id;
            appointment.Created = appointmentEntity.Created;

            appointment.PickupVagueLatitude = appointmentEntity.PickupVagueLatitude;
            appointment.PickupVagueLongitude = appointmentEntity.PickupVagueLongitude;
            appointment.DropoffVagueLatitude = appointmentEntity.DropoffVagueLatitude;
            appointment.DropoffVagueLongitude = appointmentEntity.DropoffVagueLongitude;

            if (driveToEntity != null)
            {
                driveTo.Id = driveToEntity.Id;
                driveTo.EndAddress = driveToEntity.EndAddress;
                driveTo.EndCity = driveToEntity.EndCity;
                driveTo.EndState = driveToEntity.EndState;
                driveTo.EndPostalCode = driveToEntity.EndPostalCode;
                driveTo.Created = driveToEntity.Created;
            }

            if (driveFromEntity != null)
            {
                driveFrom.Id = driveFromEntity.Id;
                driveFrom.EndAddress = driveFromEntity.EndAddress;
                driveFrom.EndCity = driveFromEntity.EndCity;
                driveFrom.EndState = driveFromEntity.EndState;
                driveFrom.EndPostalCode = driveFromEntity.EndPostalCode;
                driveFrom.Created = driveFromEntity.Created;
            }

            if (twilioIsEnabled)
            {
                try
                {
                    //send initial text message to drivers
                    var twilioCommand = new TwilioCommand(twilioAccountSID, twilioAuthKey, twilioPhoneNumber, loggerFactory.CreateLogger<TwilioCommand>(),
                        dbContext, userTimeZoneName, appUrl);
                    twilioCommand.SendAppointmentMessage(appointmentEntity, driveToEntity, driveFromEntity, TwilioCommand.MessageType.Unknown, false);
					await dbContext.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, $"{nameof(AddAppointment)}(): Exception");
                }
            }
			
            return new ObjectResult(appointmentDTO);
        }

        /// <summary>
        /// approves a volunteer for a drive
        /// </summary>
        /// <remarks>Adds driverId to a drive</remarks>
        /// <param name="body1"></param>
        /// <response code="200">Success. Added driver to drive.</response>
        /// <response code="400">Client Error - please check your request format &amp; try again.</response>
        /// <response code="404">Error. The driveId or volunteerId was not found.</response>
        [HttpPost]
        [Route("api/drives/approve")]
        [ValidateModelState]
        [SwaggerOperation("AddDriver")]
        public virtual IActionResult AddDriver([FromBody]Body1 body1)
        {
            var userEmail = HttpContext.GetUserEmail();
            var volunteerQuery = new VolunteerQuery(dbContext);
            var volunteer = volunteerQuery.GetActiveDispatcherByEmail(userEmail, true);

            if (volunteer == null)
            {
                return Forbid();
            }

            if (!body1.VolunteerDriveId.HasValue)
            {
                return BadRequest(body1);
            }

            var volunteerDriveLogQuery = new VolunteerDriveLogQuery(dbContext);
            var volunteerDriveLog = volunteerDriveLogQuery.GetByIdAsync(body1.VolunteerDriveId.Value, false).Result;

            if (volunteerDriveLog == null)
            {
                return NotFound(body1);
            }

            if (volunteerDriveLog.Drive.StatusId != Drive.StatusPending ||
                volunteerDriveLog.Drive.DriverId.HasValue)
            {
                return Conflict(body1);
            }

            var utcNow = DateTime.UtcNow;

            var driver = volunteerQuery.GetActiveDriverById(volunteerDriveLog.VolunteerId, false);

            var drive = volunteerDriveLog.Drive;

            drive.DriverId = volunteerDriveLog.VolunteerId;
            drive.StatusId = Drive.StatusApproved;
            drive.Approved = utcNow;
            drive.ApprovedById = volunteer.Id;
            drive.Updated = utcNow;

            if (volunteerDriveLog != null)
            {
                volunteerDriveLog.DriveLogStatusId = Core.Entities.DriveLogStatus.APPROVED;
                volunteerDriveLog.Approved = DateTime.UtcNow;
                volunteerDriveLog.Updated = DateTime.UtcNow;
            }
            else
            {
                logger.LogError($"VolunteerDriveLog not found for volunteer {volunteer.Id}, drive {drive.Id}.");
            }

            dbContext.SaveChanges();

			//send Drive Approved for Drive message
			if (twilioIsEnabled)
			{
				try
				{
					//send initial text message to drivers
					var twilioCommand = new TwilioCommand(twilioAccountSID, twilioAuthKey, twilioPhoneNumber, loggerFactory.CreateLogger<TwilioCommand>(),
                        dbContext, userTimeZoneName, appUrl);
					twilioCommand.SendDispatcherMessage(drive, driver, TwilioCommand.MessageType.DriverApprovedForDrive);
				}
				catch (Exception ex)
				{
					logger.LogError(ex, $"{nameof(AddDriver)}(): Exception");
				}
			}

            // check and award badges
            if (badgesAreEnabled)
            {
                try
                {
                    var badgeCommand = new BadgeCommand(dbContext, loggerFactory.CreateLogger<BadgeCommand>());
                    var badgeQuery = new BadgeQuery(dbContext);
                    var relevantUnearnedBadges = badgeQuery.GetRelevantUnearnedBadgesForVolunteerIdAsync(driver.Id, BadgeTriggerType.ApprovedForDrive, false).Result;

                    foreach (var badge in relevantUnearnedBadges)
                    {
                        var badgeAwarded = badgeCommand.CheckAndAwardBadgeAsync(badge, driver, volunteerDriveLog).Result;

                        if (badgeAwarded)
                        {
                            dbContext.SaveChanges();

                            //if we're going to text the volunteer about the badge they just earned, here's where to do that
                        }
                    }

                }
                catch (Exception ex)
                {
                    logger.LogError($"{nameof(AddDriver)}(): Exception: {ex}");
                }
            }

			return Ok();
        }

        /// <summary>
        /// cancels a drive
        /// </summary>
        /// <remarks>Updates statusId to canceled and applies the given cancelReasonId</remarks>
        /// <param name="driveId">Id of the drive to cancel</param>
        /// <param name="cancelReasonId">Id of the DriveCancelReason to apply</param>
        /// <response code="200">Success. Drive canceled.</response>
        /// <response code="400">Client Error - please check your request format and try again.</response>
        /// <response code="404">Error. The driveId was not found.</response>
        /// <response code="409">Error. The drive cannot be canceled in its current state.</response>
        [HttpPost]
        [Route("api/drives/{driveId}/cancel")]
        [ValidateModelState]
        [SwaggerOperation("CancelDrive")]
        public virtual async Task<IActionResult> CancelDrive([FromRoute]int driveId, [FromQuery]int cancelReasonId)
        {
            var userEmail = HttpContext.GetUserEmail();
            var volunteerQuery = new VolunteerQuery(dbContext);
            var volunteer = volunteerQuery.GetActiveDispatcherByEmail(userEmail, true);

            if (volunteer == null)
            {
                return Forbid();
            }

            var driveCommand = new DriveCommand(dbContext);

            var result = await driveCommand.CancelDriveAsync(driveId, cancelReasonId);

            await dbContext.SaveChangesAsync();

			//send Caller Canceled Drive message
			if (twilioIsEnabled)
			{
				try
				{
					//send initial text message to driver
					DriveQuery driveQuery = new DriveQuery(dbContext);
					var drive = await driveQuery.GetDriveAsync(driveId);
					var twilioCommand = new TwilioCommand(twilioAccountSID, twilioAuthKey, twilioPhoneNumber, loggerFactory.CreateLogger<TwilioCommand>(),
                        dbContext, userTimeZoneName, appUrl);
					twilioCommand.SendDispatcherMessage(drive, volunteer, TwilioCommand.MessageType.DriveCanceled);
				}
				catch (Exception ex)
				{
					logger.LogError(ex, $"{nameof(CancelDrive)}(): Exception");
				}
			}

			switch (result.ErrorCode)
            {
                case Core.Misc.ErrorCode.None:
                    return Ok(result.Data);
                case Core.Misc.ErrorCode.NotFound:
                    return NotFound();
                case Core.Misc.ErrorCode.InvalidOperation:
                    return Conflict();
                default:
                    return BadRequest();
            }
        }

        /// <summary>
        /// adds a caller
        /// </summary>
        /// <remarks>Adds caller to the system</remarks>
        /// <param name="caller">callerData to add</param>
        /// <response code="201">item created</response>
        /// <response code="400">invalid input, object invalid</response>
        /// <response code="409">the item already exists</response>
        [HttpPost]
        [Route("api/dispatcher/caller")]
        [ValidateModelState]
        [SwaggerOperation("AddCaller")]
        public virtual async Task<IActionResult> AddCaller([FromBody]Caller caller)
        {
            var userEmail = HttpContext.GetUserEmail();
            var volunteerQuery = new VolunteerQuery(dbContext);
            var volunteer = volunteerQuery.GetActiveDispatcherByEmail(userEmail, true);

            if (volunteer == null)
            {
                return Forbid();
            }

            if (!ModelState.IsValid ||
                caller == null ||
                string.IsNullOrWhiteSpace(caller.CallerIdentifier) ||
                (string.IsNullOrWhiteSpace(caller.FirstName) && string.IsNullOrWhiteSpace(caller.LastName)) ||
                string.IsNullOrWhiteSpace(caller.Phone))
            {
                return BadRequest();
            }

            var exists = await dbContext.Caller
                .Where(p => p.CallerIdentifier == caller.CallerIdentifier)
                .AnyAsync();

            if (exists)
            {
                return Conflict();
            }

            var callerEntity = new Core.Entities.Caller(caller);

            dbContext.Caller.Add(callerEntity);
            await dbContext.SaveChangesAsync();

            caller.Id = callerEntity.Id;
            caller.Created = callerEntity.Created;
            caller.Updated = callerEntity.Updated;

            return CreatedAtAction(nameof(AddCaller), caller);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="appointmentID">pass an appointmentIdentifier</param>
        /// <response code="200">Success. Appointment with ID {appointmentID} was deleted.</response>
        /// <response code="400">Client Error - please check your request format &amp; try again.</response>
        /// <response code="404">No appointment found.</response>
        [HttpDelete]
        [Route("api/dispatcher/appointments/{appointmentID}")]
        [ValidateModelState]
        [SwaggerOperation("DeleteAppointment")]
        [SwaggerResponse(statusCode: 200, type: typeof(DeleteSuccessMessage), description: "Success. Appointment with ID {appointmentID} was deleted.")]
        public virtual IActionResult DeleteAppointment([FromRoute][Required]string appointmentID)
        {
            //TODO: Uncomment the next line to return response 200 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
            // return StatusCode(200, default(DeleteSuccessMessage));

            //TODO: Uncomment the next line to return response 400 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
            // return StatusCode(400);

            //TODO: Uncomment the next line to return response 404 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
            // return StatusCode(404);

            string exampleJson = null;
            exampleJson = "{\r\n  \"message\" : \"Success. Your {dataType} of ID {objectID} has been deleted.\"\r\n}";
            
            var example = exampleJson != null
            ? JsonConvert.DeserializeObject<DeleteSuccessMessage>(exampleJson)
            : default(DeleteSuccessMessage);
            //TODO: Change the data returned
            return new ObjectResult(example);
        }

        /// <summary>
        /// gets caller by callerIdentifier
        /// </summary>
        /// <remarks>Search for existing callers by the dispatcher created callerIdentifier (caller ID) </remarks>
        /// <param name="callerIdentifier">pass a search string for looking up callerIdentifier</param>
        /// <response code="200">search results matching criteria</response>
        /// <response code="400">GAH IT IS SO BROKEN</response>
        /// <response code="404">caller not found frownies</response>
        [HttpGet]
        [Route("api/dispatcher/caller")]
        [ValidateModelState]
        [SwaggerOperation("GetCallerByCallerIdentifier")]
        [SwaggerResponse(statusCode: 200, type: typeof(Caller), description: "search results matching criteria")]
        public virtual async Task<IActionResult> GetCallerByCallerIdentifier([FromQuery][Required()] [MinLength(4)]string callerIdentifier)
        {
            var userEmail = HttpContext.GetUserEmail();
            var volunteerQuery = new VolunteerQuery(dbContext);
            var volunteer = volunteerQuery.GetActiveDispatcherByEmail(userEmail, true);

            if (volunteer == null)
            {
                return Forbid();
            }

            //TODO: Uncomment the next line to return response 200 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
            // return StatusCode(200, default(Caller));

            //TODO: Uncomment the next line to return response 404 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
            // return StatusCode(404);

            var callers = await dbContext.Caller
                .AsNoTracking()
                .Where(p => p.CallerIdentifier == callerIdentifier)
                .Select(p => new Core.Models.Caller(p))
                .ToListAsync();

            if (callers.Count == 0)
            {
                return NotFound();
            }
            else if (callers.Count == 1)
            {
                return new ObjectResult(callers.First());
            }
            else
            {
                return new BadRequestResult();
            }

        }

        /// <summary>
        /// get list of applicants for a drive
        /// </summary>
        /// <param name="driveId">id of drive</param>
        /// <response code="200">successful operation</response>
        [HttpGet]
        [Route("api/volunteerDrive")]
        [ValidateModelState]
        [SwaggerOperation("GetVolunteerDrives")]
        [SwaggerResponse(statusCode: 200, type: typeof(List<VolunteerDrive>), description: "successful operation")]
        public virtual IActionResult GetVolunteerDrives([FromQuery][Required()]long? driveId)
        {
            var userEmail = HttpContext.GetUserEmail();
            var volunteerQuery = new VolunteerQuery(dbContext);
            var volunteer = volunteerQuery.GetActiveDispatcherByEmail(userEmail, true);

            if (volunteer == null)
            {
                return Forbid();
            }

            if (!driveId.HasValue || !dbContext.Drive.Any(d => d.Id == driveId.Value))
            {
                return BadRequest();
            }

            var results = dbContext.VolunteerDriveLog
                .AsNoTracking()
                .Include(vdl => vdl.Volunteer)
                .Where(vdl => vdl.DriveId == driveId.Value && vdl.IsActive)
                .OrderBy(vdl => vdl.Created)
                .Select(vdl => new Core.Models.VolunteerDrive(vdl))
                .ToList();

            return Ok(results);
        }

        /// <summary>
        /// updates an existing appointment
        /// </summary>
        /// <remarks>Updates appointment (and corresponding drive) information</remarks>
        /// <param name="appointmentID">pass an appointmentIdentifier</param>
        /// <param name="appointmentDTO">appointmentData with updated fields</param>
        /// <response code="200">Success. Created appointment.</response>
        /// <response code="400">Client Error - please check your request format &amp; try again.</response>
        /// <response code="404">Error. The appointment ID you requested does not exist.</response>
        [HttpPut]
        [Route("api/dispatcher/appointments/{appointmentID}")]
        [ValidateModelState]
        [SwaggerOperation("UpdateAppointment")]
        [SwaggerResponse(statusCode: 200, type: typeof(AppointmentDTO), description: "Success. Updated appointment.")]
        public virtual async Task<IActionResult> UpdateAppointment([FromRoute][Required]string appointmentID, [FromBody]AppointmentDTO appointmentDTO)
        {
            var userEmail = HttpContext.GetUserEmail();
            var volunteerQuery = new VolunteerQuery(dbContext);
            var volunteer = volunteerQuery.GetActiveDispatcherByEmail(userEmail, true);

            if (volunteer == null)
            {
                return Forbid();
            }

            if (!appointmentDTO.Validate())
            {
                return BadRequest(appointmentDTO);
            }

            var callerModel = appointmentDTO.Caller;

            if (!ModelState.IsValid ||
                callerModel == null ||
                string.IsNullOrWhiteSpace(callerModel.CallerIdentifier) ||
                (string.IsNullOrWhiteSpace(callerModel.FirstName) && string.IsNullOrWhiteSpace(callerModel.LastName)) ||
                string.IsNullOrWhiteSpace(callerModel.Phone))
            {
                return BadRequest();
            }

            var appointmentModel = appointmentDTO.Appointment;

            var serviceProvider = await dbContext.ServiceProvider
                .AsNoTracking()
                .Where(c => c.Id == appointmentModel.ServiceProviderId && c.IsActive)
                .FirstOrDefaultAsync();

            if (serviceProvider == null)
            {
                return BadRequest(appointmentDTO);
            }

            var callerEntity = await dbContext.Caller
                .Where(c => c.Id == appointmentModel.CallerId && c.IsActive)
                .FirstOrDefaultAsync();

            if (callerEntity == null)
            {
                return BadRequest(appointmentDTO);
            }

            callerEntity.UpdateFromModel(callerModel);

            var appointmentEntity = await dbContext.Appointment
                .Include(a => a.Drives)
                .Where(a => a.Id == appointmentModel.Id && a.IsActive)
                .FirstOrDefaultAsync();

            if (appointmentEntity == null)
            {
                return BadRequest(appointmentDTO);
            }

            appointmentEntity.DispatcherId = volunteer.Id;
            appointmentEntity.ServiceProviderId = serviceProvider.Id;
            appointmentEntity.PickupLocationVague = appointmentModel.PickupLocationVague;
            appointmentEntity.DropoffLocationVague = appointmentModel.DropoffLocationVague;
            appointmentEntity.AppointmentDate = appointmentModel.AppointmentDate.Value;
            appointmentEntity.AppointmentTypeId = appointmentModel.AppointmentTypeId.Value;
            appointmentEntity.IsActive = true;
            appointmentEntity.Updated = DateTime.UtcNow;


            var driveToEntity = appointmentEntity.Drives
                .Where(d => d.Direction == Drive.DirectionToServiceProvider && d.IsActive)
                .SingleOrDefault();

            var driveFromEntity = appointmentEntity.Drives
                .Where(d => d.Direction == Drive.DirectionFromServiceProvider && d.IsActive)
                .SingleOrDefault();


            var driveToModel = appointmentDTO.DriveTo;
            var driveFromModel = appointmentDTO.DriveFrom;

            if (driveToModel != null)
            {
                if (driveToEntity != null)
                {
                    // update it
                    driveToEntity.UpdateFromModel(driveToModel);
                }
                else
                {
                    // add it
                    driveToEntity = Core.Entities.Drive.CreateFromModel(
                        driveToModel,
                        appointmentEntity,
                        Drive.DirectionToServiceProvider,
                        serviceProvider);

                    dbContext.Drive.Add(driveToEntity);
                }
            }
            else
            {
                if (driveToEntity != null)
                {
                    // delete it
                    driveToEntity.Sanitize();
                    dbContext.Remove(driveToEntity);
                }
                else
                {
                    // do nothing
                }
            }

            if (driveFromModel != null)
            {
                if (driveFromEntity != null)
                {
                    // update it
                    driveFromEntity.UpdateFromModel(driveFromModel);
                }
                else
                {
                    // add it
                    driveFromEntity = Core.Entities.Drive.CreateFromModel(
                        driveFromModel,
                        appointmentEntity,
                        Drive.DirectionFromServiceProvider,
                        serviceProvider);

                    dbContext.Drive.Add(driveFromEntity);
                }
            }
            else
            {
                if (driveFromEntity != null)
                {
                    // delete it
                    driveFromEntity.Sanitize();
                    dbContext.Remove(driveFromEntity);
                }
                else
                {
                    // do nothing
                }
            }


            var geocoder = new GeocoderQuery(googleApiKey, loggerFactory.CreateLogger<GeocoderQuery>());

            var driveToAddress = driveToEntity?.GetCallerAddress();
            GeocoderQuery.LatLng driveToLocation = null;

            if (driveToAddress != null)
            {
                driveToLocation = await geocoder.ForwardLookupAsync(driveToAddress);
            }

            if (driveToModel != null && driveToLocation == null)
            {
                return StatusCode((int)System.Net.HttpStatusCode.UnprocessableEntity, "Geocoding failed for Pickup Address.");
            }

            driveToEntity?.SetCallerLocation(driveToLocation);
            driveToModel?.SetCallerLocation(driveToLocation);

            var driveFromAddress = driveFromEntity?.GetCallerAddress();
            GeocoderQuery.LatLng driveFromLocation = null;

            if (driveFromAddress != null)
            {
                if (driveToAddress != null &&
                    string.Equals(driveToAddress, driveFromAddress, StringComparison.CurrentCultureIgnoreCase))
                {
                    driveFromLocation = driveToLocation;
                }
                else
                {
                    driveFromLocation = await geocoder.ForwardLookupAsync(driveFromAddress);
                }
            }

            if (driveFromModel != null && driveFromLocation == null)
            {
                return StatusCode((int)System.Net.HttpStatusCode.UnprocessableEntity, "Geocoding failed for Dropoff Address.");
            }

            driveFromEntity?.SetCallerLocation(driveFromLocation);
            driveFromModel?.SetCallerLocation(driveFromLocation);

            var random = new Random();

            var pickupVagueLocation = driveToLocation?.ToVagueLocation(random, vagueLocationMinOffset, vagueLocationMaxOffset);

            if (pickupVagueLocation != null)
            {
                appointmentEntity.PickupVagueLatitude = pickupVagueLocation.Latitude;
                appointmentEntity.PickupVagueLongitude = pickupVagueLocation.Longitude;
            }

            GeocoderQuery.LatLng dropoffVagueLocation;

            if (driveToAddress != null && driveFromAddress != null &&
                string.Equals(driveToAddress, driveFromAddress, StringComparison.CurrentCultureIgnoreCase))
            {
                dropoffVagueLocation = pickupVagueLocation;
            }
            else
            {
                dropoffVagueLocation = driveFromLocation?.ToVagueLocation(random, vagueLocationMinOffset, vagueLocationMaxOffset);
            }

            if (dropoffVagueLocation != null)
            {
                appointmentEntity.DropoffVagueLatitude = dropoffVagueLocation.Latitude;
                appointmentEntity.DropoffVagueLongitude = dropoffVagueLocation.Longitude;
            }

            await dbContext.SaveChangesAsync();

            appointmentModel.Id = appointmentEntity.Id;
            appointmentModel.Created = appointmentEntity.Created;
            appointmentModel.Updated = appointmentEntity.Updated;

            appointmentModel.PickupVagueLatitude = appointmentEntity.PickupVagueLatitude;
            appointmentModel.PickupVagueLongitude = appointmentEntity.PickupVagueLongitude;
            appointmentModel.DropoffVagueLatitude = appointmentEntity.DropoffVagueLatitude;
            appointmentModel.DropoffVagueLongitude = appointmentEntity.DropoffVagueLongitude;

            if (driveToEntity != null)
            {
                driveToModel.Id = driveToEntity.Id;
                driveToModel.Created = driveToEntity.Created;
                driveToModel.Updated = driveToEntity.Updated;
            }

            if (driveFromEntity != null)
            {
                driveFromModel.Id = driveFromEntity.Id;
                driveFromModel.Created = driveFromEntity.Created;
                driveFromModel.Updated = driveFromEntity.Updated;
            }

            return new ObjectResult(appointmentDTO);
        }

    }
}
