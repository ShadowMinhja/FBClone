using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FBClone.WebAPI.Models
{
    public class ResetPasswordModel
    {
        public string UserId { get; set; }
        public string Code { get; set; }
        public string Password { get; set; }
    }
}