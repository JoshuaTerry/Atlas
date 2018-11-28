using System;
using System.Collections.Generic;
using System.Text;
using DriveCentric.Core.Interfaces;
using DriveCentric.Model;
using ServiceStack.DataAnnotations;

namespace DriveCentric.Core.Models
{
    public class User : IBaseModel
    {
        [PrimaryKey]
        [Alias("pkUserID")]
        public int Id { get; set; }

        [Alias("fkStoreID")]
        public int StoreId { get; set; }

        public string Email { get; set; }

        [Alias("FirstName")]
        public string FirstName { get; set; }

        [Alias("LastName")]
        public string LastName { get; set; }

        public int? UserType { get; set; }
        public DateTime? DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
        public bool? IsDeleted { get; set; }
        public bool IsActive { get; set; }
        public bool IsReceivingAllSurveys { get; set; }

        [Alias("SessionStoreID")]
        public int SessionStoreId { get; set; }

        public string Biography { get; set; }

        [Alias("ImageURL")]
        public string ImageUrl { get; set; }

        public string Phone { get; set; }
        public string Title { get; set; }
        public int ReminderTypeTask { get; set; }
        public int ReminderTypeAppointment { get; set; }
        public int ReminderTimeTask { get; set; }
        public int ReminderTimeAppointment { get; set; }
        public bool IsPublic { get; set; }
        public DateTime? DateLastPing { get; set; }

        [Alias("DMSImportID")]
        public int DmsImportId { get; set; }

        public string DriveVelocityEmailAlias { get; set; }
        public string ForwardEmailTo { get; set; }

        [Alias("fkOrphanedToUserID")]
        public int OrphanedToUserId { get; set; }

        public bool IsWebSpotlight { get; set; }
        public bool IsShowingExtended { get; set; }

        [Alias("IsBDC")]
        public bool IsBdc { get; set; }

        public DateTime? DateInactive { get; set; }
        public string CellPhone { get; set; }
        public bool IsAllowedToSeeDupes { get; set; }
        public string PIN { get; set; }
        public string TextNumber { get; set; }

        [Alias("GUID")]
        public Guid ExternalId { get; set; }

        public Guid AuthenticationToken { get; set; }
        public bool IsClockedIn { get; set; }
        public string EmailSignature { get; set; }
        public bool IsOnVacation { get; set; }
        public string VacationResponse { get; set; }

        [Alias("GalaxyUserGUID")]
        public Guid GalaxyUserId { get; set; }

        [Alias("ProfileURL")]
        public string ProfileUrl { get; set; }

        public string DrivePhoneNumber { get; set; }
        public string Tags { get; set; }
        public string TextNumberProvider { get; set; }
        public DateTime ModifiedDateViaTrigger { get; set; }
    }
}
