using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace FBClone.WebAPI.Controllers
{
    public class BaseController : ApiController
    {
        public string userId = String.Empty;
    }
}