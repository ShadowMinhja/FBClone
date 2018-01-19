using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FBClone.Model
{
    public partial class ExternalLoginMobile
    {
        public string UserId { get; set; } //UserId
        public bool HasRegistered { get; set; } //HasRegistered
        public bool IsConfirmed { get; set; } //IsConfirmed
        public string LoginProvider { get; set; } //LoginProvider
        public string ExternalAccessToken { get; set; } //ExternalAccessToken
    }
}
