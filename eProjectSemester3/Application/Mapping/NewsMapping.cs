using eProjectSemester3.Application.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace eProjectSemester3.Application.Mapping
{
    public class NewsMapping : EntityTypeConfiguration<News>
    {
        public NewsMapping()
        {
            HasKey(x => x.Id);
            Property(x => x.Title).IsRequired().HasMaxLength(500);
            Property(x => x.ShostContent).IsRequired().HasMaxLength(500);
            Property(x => x.Content).IsRequired();

          
        }
    }
}