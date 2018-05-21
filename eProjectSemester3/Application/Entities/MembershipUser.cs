using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eProjectSemester3.Application.Entities
{
    public class MembershipUser : Entity
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string PasswordSalt { get; set; }

        public bool IsApproved { get; set; }
        public bool IsLockedOut { get; set; }
        public bool IsBanned { get; set; }

        public int FailedPasswordAttemptCount { get; set; }

        public DateTime CreateDate { get; set; }
        public DateTime LastLoginDate { get; set; }
        public DateTime LastPasswordChangedDate { get; set; }
        public DateTime LastLockoutDate { get; set; }
        public DateTime? LastActivityDate { get; set; }

        public virtual IList<MembershipRole> Roles { get; set; }
    }
}