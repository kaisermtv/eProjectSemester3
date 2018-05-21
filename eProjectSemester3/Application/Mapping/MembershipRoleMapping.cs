using eProjectSemester3.Application.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace eProjectSemester3.Application.Mapping
{
    public class MembershipRoleMapping : EntityTypeConfiguration<MembershipRole>
    {
        public MembershipRoleMapping()
        {
            HasKey(x => x.Id);
            Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(x => x.RoleName).IsRequired().HasMaxLength(256).HasColumnAnnotation("Index",
                    new IndexAnnotation(new IndexAttribute("IX_MembershipRole_RoleName", 1) { IsUnique = true }));
        }
    }
}