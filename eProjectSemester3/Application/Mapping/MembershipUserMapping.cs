using eProjectSemester3.Application.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Data.Entity.Infrastructure.Annotations;

namespace eProjectSemester3.Application.Mapping
{
    public class MembershipUserMapping : EntityTypeConfiguration<MembershipUser>
    {
        public MembershipUserMapping()
        {
            HasKey(x => x.Id);
            Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(x => x.UserName).IsRequired().HasMaxLength(256).HasColumnAnnotation("Index",
                    new IndexAnnotation(new IndexAttribute("IX_MembershipUser_UserName", 1) { IsUnique = true })); 

            Property(x => x.Password).IsRequired().HasMaxLength(256);
            Property(x => x.PasswordSalt).IsOptional().HasMaxLength(256);

            Property(x => x.IsApproved).IsRequired();
            Property(x => x.IsLockedOut).IsRequired();
            Property(x => x.IsBanned).IsRequired();

            Property(x => x.FailedPasswordAttemptCount).IsRequired();

            Property(x => x.CreateDate).IsRequired();
            Property(x => x.LastLoginDate).IsRequired();
            Property(x => x.LastPasswordChangedDate).IsRequired();
            Property(x => x.LastLockoutDate).IsRequired();
            Property(x => x.LastActivityDate).IsOptional();

            HasMany(x => x.Roles).WithMany(t => t.Users).Map(m =>
            {
                m.ToTable("MembershipUsersInRoles");
                m.MapLeftKey("UserIdentifier");
                m.MapRightKey("RoleIdentifier");
            });
        }
    }
}