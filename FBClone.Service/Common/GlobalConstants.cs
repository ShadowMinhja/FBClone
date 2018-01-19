using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace FBClone.Service
{
    public static class GlobalConstants
    {
        public static string fbClone_USERID = GetProperty("fbCloneUserId");
        public static string ENVIRONMENT = GetProperty("ENVIRONMENT");
        public static string STREAM_APP_ID = GetProperty("STREAM_APP_ID");
        public static string STREAM_KEY = GetProperty("STREAM_KEY");
        public static string STREAM_SECRET = GetProperty("STREAM_SECRET");

        public static string STREAM_USER_FEED = GetProperty("STREAM_USER_FEED");
        public static string STREAM_TIMELINE_FLAT = GetProperty("STREAM_TIMELINE_FLAT");
        public static string STREAM_TIMELINE_AGGREGATED = GetProperty("STREAM_TIMELINE_AGGREGATED");
        public static string STREAM_NOTIFICATION = GetProperty("STREAM_NOTIFICATION");

        public static string S3_BUCKET = GetProperty("S3Bucket");
        public static string IMGIXURL = GetProperty("IMGIXURL");

        public static string GetProperty(string name)
        {
            return ConfigurationManager.AppSettings[name];
        }

        public static Dictionary<string, string> EmailTypes = new Dictionary<string, string>()
        {
            { "ConfirmAccount", "Confirm your fbClone account" },
            { "ResetPassword", "Your fbClone Reset Password Request" }
        };
    }
}