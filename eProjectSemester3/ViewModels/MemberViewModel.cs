using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace eProjectSemester3.ViewModels
{
    public class LogOnViewModel
    {
        public string ReturnUrl { get; set; }

        [Required]
        [DisplayName("Username")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [DisplayName("Password")]
        public string Password { get; set; }

        [DisplayName("RememberMe")]
        public bool RememberMe { get; set; }
    }
}