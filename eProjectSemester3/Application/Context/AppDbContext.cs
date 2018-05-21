using eProjectSemester3.Application.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace eProjectSemester3.Application.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext() : base("name=AppDbContext")
        {
            Configuration.LazyLoadingEnabled = true;
        }

        public DbSet<MembershipRole> MembershipRole { get; set; }
        public DbSet<MembershipUser> MembershipUser { get; set; }
    }
}