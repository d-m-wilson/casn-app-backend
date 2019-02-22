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
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text;
using Newtonsoft.Json;

namespace CASNApp.API.Models
{
    /// <summary>
    /// 
    /// </summary>
    [DataContract]
    public partial class DriverDrive : IEquatable<DriverDrive>
    { 
        /// <summary>
        /// Gets or Sets Id
        /// </summary>
        [Required]
        [DataMember(Name="id")]
        public long? Id { get; set; }

        /// <summary>
        /// Gets or Sets VolunteerDriveId
        /// </summary>
        [Required]
        [DataMember(Name="volunteerDriveId")]
        public long? VolunteerDriveId { get; set; }

        /// <summary>
        /// Gets or Sets AppointmentId
        /// </summary>
        [Required]
        [DataMember(Name="appointmentId")]
        public long? AppointmentId { get; set; }

        /// <summary>
        /// Gets or Sets AppointmentTypeId
        /// </summary>
        [Required]
        [DataMember(Name="appointmentTypeId")]
        public int? AppointmentTypeId { get; set; }

        /// <summary>
        /// Gets or Sets ClinicId
        /// </summary>
        [Required]
        [DataMember(Name="clinicId")]
        public long? ClinicId { get; set; }

        /// <summary>
        /// Gets or Sets AppointmentDate
        /// </summary>
        [Required]
        [DataMember(Name="appointmentDate")]
        public DateTime? AppointmentDate { get; set; }

        /// <summary>
        /// 1 &#x3D; toClinic, 2 &#x3D; fromClinic
        /// </summary>
        /// <value>1 &#x3D; toClinic, 2 &#x3D; fromClinic</value>
        [Required]
        [DataMember(Name="direction")]
        public int? Direction { get; set; }

        /// <summary>
        /// Gets or Sets IsApproved
        /// </summary>
        [Required]
        [DataMember(Name="isApproved")]
        public bool? IsApproved { get; set; }

        /// <summary>
        /// Gets or Sets StartLocation
        /// </summary>
        [DataMember(Name="startLocation")]
        public string StartLocation { get; set; }

        /// <summary>
        /// Gets or Sets EndLocation
        /// </summary>
        [DataMember(Name="endLocation")]
        public string EndLocation { get; set; }

        /// <summary>
        /// Gets or Sets CallerIdentifier
        /// </summary>
        [Required]
        [DataMember(Name="callerIdentifier")]
        public string CallerIdentifier { get; set; }

        /// <summary>
        /// Gets or Sets CallerName
        /// </summary>
        [DataMember(Name="callerName")]
        public string CallerName { get; set; }

        /// <summary>
        /// Gets or Sets CallerNote
        /// </summary>
        [DataMember(Name="callerNote")]
        public string CallerNote { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class DriverDrive {\n");
            sb.Append("  Id: ").Append(Id).Append("\n");
            sb.Append("  VolunteerDriveId: ").Append(VolunteerDriveId).Append("\n");
            sb.Append("  AppointmentId: ").Append(AppointmentId).Append("\n");
            sb.Append("  AppointmentTypeId: ").Append(AppointmentTypeId).Append("\n");
            sb.Append("  ClinicId: ").Append(ClinicId).Append("\n");
            sb.Append("  AppointmentDate: ").Append(AppointmentDate).Append("\n");
            sb.Append("  Direction: ").Append(Direction).Append("\n");
            sb.Append("  IsApproved: ").Append(IsApproved).Append("\n");
            sb.Append("  StartLocation: ").Append(StartLocation).Append("\n");
            sb.Append("  EndLocation: ").Append(EndLocation).Append("\n");
            sb.Append("  CallerIdentifier: ").Append(CallerIdentifier).Append("\n");
            sb.Append("  CallerName: ").Append(CallerName).Append("\n");
            sb.Append("}\n");
            return sb.ToString();
        }

        /// <summary>
        /// Returns the JSON string presentation of the object
        /// </summary>
        /// <returns>JSON string presentation of the object</returns>
        public string ToJson()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }

        /// <summary>
        /// Returns true if objects are equal
        /// </summary>
        /// <param name="obj">Object to be compared</param>
        /// <returns>Boolean</returns>
        public override bool Equals(object obj)
        {
            if (obj is null) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((DriverDrive)obj);
        }

        /// <summary>
        /// Returns true if DriverDrive instances are equal
        /// </summary>
        /// <param name="other">Instance of DriverDrive to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(DriverDrive other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;

            return 
                (
                    Id == other.Id ||
                    Id != null &&
                    Id.Equals(other.Id)
                ) && 
                (
                    VolunteerDriveId == other.VolunteerDriveId ||
                    VolunteerDriveId != null &&
                    VolunteerDriveId.Equals(other.VolunteerDriveId)
                ) && 
                (
                    AppointmentId == other.AppointmentId ||
                    AppointmentId != null &&
                    AppointmentId.Equals(other.AppointmentId)
                ) && 
                (
                    AppointmentTypeId == other.AppointmentTypeId ||
                    AppointmentTypeId != null &&
                    AppointmentTypeId.Equals(other.AppointmentTypeId)
                ) && 
                (
                    ClinicId == other.ClinicId ||
                    ClinicId != null &&
                    ClinicId.Equals(other.ClinicId)
                ) && 
                (
                    AppointmentDate == other.AppointmentDate ||
                    AppointmentDate != null &&
                    AppointmentDate.Equals(other.AppointmentDate)
                ) && 
                (
                    Direction == other.Direction ||
                    Direction != null &&
                    Direction.Equals(other.Direction)
                ) && 
                (
                    IsApproved == other.IsApproved ||
                    IsApproved != null &&
                    IsApproved.Equals(other.IsApproved)
                ) && 
                (
                    StartLocation == other.StartLocation ||
                    StartLocation != null &&
                    StartLocation.Equals(other.StartLocation)
                ) && 
                (
                    EndLocation == other.EndLocation ||
                    EndLocation != null &&
                    EndLocation.Equals(other.EndLocation)
                ) && 
                (
                    CallerIdentifier == other.CallerIdentifier ||
                    CallerIdentifier != null &&
                    CallerIdentifier.Equals(other.CallerIdentifier)
                ) && 
                (
                    CallerName == other.CallerName ||
                    CallerName != null &&
                    CallerName.Equals(other.CallerName)
                );
        }

        /// <summary>
        /// Gets the hash code
        /// </summary>
        /// <returns>Hash code</returns>
        public override int GetHashCode()
        {
            unchecked // Overflow is fine, just wrap
            {
                var hashCode = 41;
                // Suitable nullity checks etc, of course :)
                    if (Id != null)
                    hashCode = hashCode * 59 + Id.GetHashCode();
                    if (VolunteerDriveId != null)
                    hashCode = hashCode * 59 + VolunteerDriveId.GetHashCode();
                    if (AppointmentId != null)
                    hashCode = hashCode * 59 + AppointmentId.GetHashCode();
                    if (AppointmentTypeId != null)
                    hashCode = hashCode * 59 + AppointmentTypeId.GetHashCode();
                    if (ClinicId != null)
                    hashCode = hashCode * 59 + ClinicId.GetHashCode();
                    if (AppointmentDate != null)
                    hashCode = hashCode * 59 + AppointmentDate.GetHashCode();
                    if (Direction != null)
                    hashCode = hashCode * 59 + Direction.GetHashCode();
                    if (IsApproved != null)
                    hashCode = hashCode * 59 + IsApproved.GetHashCode();
                    if (StartLocation != null)
                    hashCode = hashCode * 59 + StartLocation.GetHashCode();
                    if (EndLocation != null)
                    hashCode = hashCode * 59 + EndLocation.GetHashCode();
                    if (CallerIdentifier != null)
                    hashCode = hashCode * 59 + CallerIdentifier.GetHashCode();
                    if (CallerName != null)
                    hashCode = hashCode * 59 + CallerName.GetHashCode();
                return hashCode;
            }
        }

        #region Operators
        #pragma warning disable 1591

        public static bool operator ==(DriverDrive left, DriverDrive right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(DriverDrive left, DriverDrive right)
        {
            return !Equals(left, right);
        }

        #pragma warning restore 1591
        #endregion Operators
    }
}
