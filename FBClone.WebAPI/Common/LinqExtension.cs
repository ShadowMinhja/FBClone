using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FBClone.WebAPI.Common
{
    public static class LinqExtension
    {
        public static bool Contains(this string target, string value, StringComparison comparison)
        {
            return target.IndexOf(value, comparison) >= 0;
        }
    }
}