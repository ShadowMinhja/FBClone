using System;
using System.Collections.Generic;
using System.Linq;
using FBClone.Data;
using FBClone.Data.Infrastructure;
using FBClone.Data.Repositories;
using FBClone.Model;
using System.Linq.Expressions;
using Stream;
using System.Threading.Tasks;

namespace FBClone.Service
{
    public interface IProfileService
    {
        bool IsValidUserName(string targetUserName);
        Task<FollowContainer> CheckIfFollowing(string targetUserName);
        Task<FollowContainer> FollowFeed(FollowContainer followContainer);
        Task<FollowContainer> FollowFeed(FollowContainer followContainer, string userId);
        Task<FollowContainer> UnfollowFeed(FollowContainer followContainer);
        void SetUserId(string userName, string userId);
    }
    public class ProfileService : BaseStreamService, IProfileService
    {
        private AspNetUser user { get; set; }
        private readonly IUnitOfWork unitOfWork;
        private readonly IMongoService mongoService;
        private readonly new IAspNetUserService aspNetUserService;

        public ProfileService(IAspNetUserService aspNetUserService, IUnitOfWork unitOfWork, IMongoService mongoService) : base(aspNetUserService)
        {
            this.aspNetUserService = aspNetUserService;
            this.unitOfWork = unitOfWork;
            this.mongoService = mongoService;
        }

        public bool IsValidUserName(string targetUserName)
        {
            user = aspNetUserService.GetByName(targetUserName);
            if (user == null)
                return false;
            else
                return true;
        }

        public async Task<FollowContainer> CheckIfFollowing(string targetUserName)
        {
            Actor actor= aspNetUserService.ConvertToActor(user);
            //Retrieve user feed of found user
            string targetUserId = null;
            if (this.user != null)
            {
                targetUserId = GuidEncoder.Encode(user.Id);
                try
                {
                    var followers = await userTimeLineFlatFeed.Following(0, 2, new String[] { String.Format("user:{0}", targetUserId) });
                    if (followers.Any())
                        return new FollowContainer
                        {
                            Source = this.userId,
                            Target = targetUserId,
                            IsFollowing = true,
                            Actor = actor
                        };
                    else
                    {
                        return new FollowContainer
                        {
                            Source = this.userId,
                            Target = targetUserId,
                            IsFollowing = false,
                            Actor = actor
                        };
                    }
                }
                catch (Exception ex)
                {
                    return new FollowContainer
                    {
                        Source = GuidEncoder.Encode(this.userId),
                        Target = targetUserId,
                        IsFollowing = false
                    };
                }
            }
            else
            {
                return new FollowContainer
                {
                    Source = GuidEncoder.Encode(this.userId),
                    Target = null,
                    IsFollowing = false
                };
            }
        }

        public async Task<FollowContainer> FollowFeed(FollowContainer followContainer)
        {
            try
            {
                await userTimeLineFlatFeed.FollowFeed("user", followContainer.Target);
                followContainer.IsFollowing = true;
                return followContainer;
            }
            catch(Exception ex)
            {
                followContainer.IsFollowing = false;
                return followContainer;
            }
        }

        /// <summary>
        /// Used for first time registration --> Follow FBClone by default
        /// </summary>
        /// <param name="followContainer"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<FollowContainer> FollowFeed(FollowContainer followContainer, string userId)
        {
            if (userTimeLineFlatFeed == null)
            {
                userTimeLineFlatFeed = this.streamClient.Feed(GlobalConstants.STREAM_TIMELINE_FLAT, userId);
            }
            var result = await FollowFeed(followContainer);
            return result;
        }

        public async Task<FollowContainer> UnfollowFeed(FollowContainer followContainer)
        {
            try
            {
                await userTimeLineFlatFeed.UnfollowFeed("user", followContainer.Target);
                followContainer.IsFollowing = false;
                return followContainer;
            }
            catch (Exception ex)
            {
                followContainer.IsFollowing = true;
                return followContainer;
            }

        }

    }
}
