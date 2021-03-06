﻿using System;
using System.Collections.Generic;
using CASNApp.Core.Interfaces;

namespace CASNApp.Core.Entities
{
    public class Badge : ICreatedDate, IUpdatedDate, ISoftDelete
    {
        public Badge()
        {
            VolunteerBadges = new HashSet<VolunteerBadge>();
        }

        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string MessageText { get; set; }
        public string Path { get; set; }
        public int TriggerType { get; set; }
        public string ProcedureName { get; set; }
        public int? ServiceProviderId { get; set; }
        public int? CountTarget { get; set; }
        public bool IncludeVolunteerDriveLogId { get; set; }
        public int? AppointmentTypeId { get; set; }
        public bool IsActive { get; set; }
        public bool IsHidden { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Updated { get; set; }
        public int DisplayOrdinal { get; set; }

        public ICollection<VolunteerBadge> VolunteerBadges { get; set; }

    }
}
