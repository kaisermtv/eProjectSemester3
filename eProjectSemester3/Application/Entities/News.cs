using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eProjectSemester3.Application.Entities
{
    public class News : Entity
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string ShostContent { get; set; }
        public string Content { get; set; }
        
    }
}