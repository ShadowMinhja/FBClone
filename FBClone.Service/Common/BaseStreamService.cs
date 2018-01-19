using FBClone.Model;
using Stream;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FBClone.Service
{
    public class BaseStreamService
    {
        public string userId { get; set; }
        public StreamClient streamClient { get; set; }
        public StreamFeed userFeed { get; set; }
        public StreamFeed userTimeLineFlatFeed { get; set; }
        public StreamFeed userTimeLineAggregatedFeed { get; set; }
        public StreamFeed notificationAggregatedFeed { get; set; }
        public readonly IAspNetUserService aspNetUserService;

        public BaseStreamService(IAspNetUserService aspNetUserService)
        {
            this.aspNetUserService = aspNetUserService;
            this.streamClient = new StreamClient(GlobalConstants.STREAM_KEY, GlobalConstants.STREAM_SECRET);
        }

        //Set User Id and Establishes the GetStream Feed to Target: Uses the logged in user if userName is not provided. Otherwise, will use the userName of the feed we are interested in
        public void SetUserId(string userName, string userId)
        {
            if (String.IsNullOrEmpty(userId))
                return;
            if (this.userId == null)
            {
                this.userId = GuidEncoder.Encode(userId);
            }
            //Find User by UserName
            AspNetUser user = aspNetUserService.GetByName(userName);
            //Retrieve user feed of found user
            if (user != null)
            {
                SetUserFeed(GuidEncoder.Encode(user.Id));
            }
            else
                SetUserFeed(this.userId);
        }

        //Set User Feed
        public void SetUserFeed(string userId)
        {
            // Reference a feed
            this.userFeed = this.streamClient.Feed(GlobalConstants.STREAM_USER_FEED, userId);
            this.userTimeLineFlatFeed = this.streamClient.Feed(GlobalConstants.STREAM_TIMELINE_FLAT, userId);
            this.userTimeLineAggregatedFeed = this.streamClient.Feed(GlobalConstants.STREAM_TIMELINE_AGGREGATED, userId);
            this.notificationAggregatedFeed = this.streamClient.Feed(GlobalConstants.STREAM_NOTIFICATION, userId);
        }
    }
}

