using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eProjectSemester3.Application.Entities
{
    public class MembershipRole : Entity
    {
        public int Id { get; set; }
        public string RoleName { get; set; }

        public virtual IList<MembershipUser> Users { get; set; }
    }
}