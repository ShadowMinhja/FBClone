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
    public interface IBiteService
    {
        Bite GetById(string biteId);
        Task<IEnumerable<Bite>> GetAll(string lastId);
        Task<IEnumerable<Bite>> GetAllForWallFeed(string lastId);

        //IEnumerable<Bite> GetMany(string biteText);
        //IQueryable<Bite> Query(Expression<Func<Bite, bool>> where);

        //Bite Add(Bite bite);
        //Bite Update(Bite bite);
        //void Delete(string id);
        void SetUserId(string userName, string userId);
        Task<Bite> AddActivity(Bite bite, bool insertRecord);
    }
    public class BiteService : BaseStreamService, IBiteService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMongoService mongoService;
        private readonly new IAspNetUserService aspNetUserService;
        private readonly IQuestionResponseSetService questionResponseSetService;

        public BiteService(IUnitOfWork unitOfWork, IMongoService mongoService, IAspNetUserService aspNetUserService, IQuestionResponseSetService questionResponseSetService) :  base(aspNetUserService)
        {
            this.unitOfWork = unitOfWork;
            this.mongoService = mongoService;
            this.aspNetUserService = aspNetUserService;
            this.questionResponseSetService = questionResponseSetService;
            this.streamClient = new StreamClient(GlobalConstants.STREAM_KEY, GlobalConstants.STREAM_SECRET);
        }

        public Bite GetById(string biteId)
        {
            Bite bite = mongoService.Single<Bite>("Bite", biteId);
            if (bite == null)
            {
                return null;
            }
            //Rehydrate Actor
            if (bite.Actor == null)
            {
                AspNetUser user = aspNetUserService.GetByUserId(GuidEncoder.Decode(bite.UserId));
                Actor actor = aspNetUserService.ConvertToActor(user);
                bite.Actor = actor;
            }
            //Fix Image Url
            if (bite.Images != null && bite.Images.Any())
            {
                bite.Images = bite.Images.Select(x => String.Format("{0}/{1}", GlobalConstants.IMGIXURL, x)).ToList();
            }
            //Re-hydrate Question Response Set
            bite.QuestionResponseSet = RehydrateQuestionResponseSet(bite);
            return bite;

        }

        public async Task<IEnumerable<Bite>> GetAll(string lastId)
        {
            var bites = new List<Bite>();
            IEnumerable<Activity> results = null;
            if (this.userFeed != null)
            {
                try
                {
                    if (lastId != null)
                        results = await this.userFeed.GetActivities(0, 20, FeedFilter.Where().IdLessThan(lastId));
                    else
                        results = await this.userFeed.GetActivities(0, 20);
                }
                catch(Exception ex)
                {
                    return bites;
                }
            }
            if (results == null)
            {
                AspNetUser profileUser = aspNetUserService.GetByUserId(GuidEncoder.Decode(this.userId));                
                bites.Add(new Bite
                {
                    Actor = aspNetUserService.ConvertToActor(profileUser)
                });
            }
            else
            {
                //Re-hydrate data from Mongo
                foreach (var result in results)
                {
                    string foreignId = result.ForeignId;
                    if(foreignId == lastId) //Sometimes Get Stream Will Still Return the Same Item, so Skip It
                    {
                        continue;
                    }
                    Bite bite = mongoService.Single<Bite>("Bite", foreignId);
                    if(bite == null)
                    {
                        continue; //Skip this iteration
                    }
                    bite.StreamId = result.Id; //Assign StreamId to be used for paging later
                    //Rehydrate Actor
                    if (bite.Actor == null)
                    {
                        AspNetUser user = aspNetUserService.GetByUserId(GuidEncoder.Decode(bite.UserId));
                        Actor actor = aspNetUserService.ConvertToActor(user);
                        bite.Actor = actor;
                    }
                    //Fix Image Url
                    if (bite.Images != null && bite.Images.Any())
                    {
                        bite.Images = bite.Images.Select(x => String.Format("{0}/{1}", GlobalConstants.IMGIXURL, x)).ToList();
                    }
                    //Re-hydrate Question Response Set
                    bite.QuestionResponseSet = RehydrateQuestionResponseSet(bite);
                    bites.Add(bite);

                }
            }
            
            return bites;
        }

        public async Task<IEnumerable<Bite>> GetAllForWallFeed(string lastId)
        {
            var bites = new List<Bite>();
            IEnumerable<Activity> results = null;
            if (this.userTimeLineFlatFeed != null)
            {
                if (lastId != null)
                    results = await this.userTimeLineFlatFeed.GetActivities(0, 20, FeedFilter.Where().IdLessThan(lastId));
                else
                    results = await this.userTimeLineFlatFeed.GetActivities(0, 20);
            }
            //TODO: This face actor record may no longer be needed due to a client side refactor
            if (results == null)
            {
                AspNetUser profileUser = aspNetUserService.GetByUserId(GuidEncoder.Decode(this.userId));
                bites.Add(new Bite
                {
                    Actor = aspNetUserService.ConvertToActor(profileUser)
                });
            }
            else
            {   
                //Re-hydrate data from Mongo
                foreach (var result in results)
                {
                    string foreignId = result.ForeignId;
                    Bite bite = mongoService.Single<Bite>("Bite", foreignId);
                    if (bite == null)
                    {
                        continue;
                    }
                    else
                    {
                        bite.StreamId = result.Id; //Assign StreamId to be used for paging later
                        //Rehydrate Actor
                        if (bite.Actor == null)
                        {
                            AspNetUser user = aspNetUserService.GetByUserId(GuidEncoder.Decode(bite.UserId));
                            Actor actor = aspNetUserService.ConvertToActor(user);
                            bite.Actor = actor;
                        }
                        //Fix Image Url
                        if (bite.Images != null && bite.Images.Any())
                        {
                            bite.Images = bite.Images.Select(x => String.Format("{0}/{1}", GlobalConstants.IMGIXURL, x)).ToList();
                        }
                        //Re-hydrate Question Response Set
                        bite.QuestionResponseSet = RehydrateQuestionResponseSet(bite);
                        bites.Add(bite);
                    }
                }
            }

            return bites;
        }

        //public IEnumerable<Bite> GetMany(string biteText)
        //{
        //    var bites = new List<Bite>();
        //    bites.Add(new Bite());
        //    return bites;
        //}

        //public IQueryable<Bite> Query(Expression<Func<Bite, bool>> where)
        //{
        //    var bites = new List<Bite>();
        //    bites.Add(new Bite());
        //    return bites.AsQueryable();
        //}

        //public Bite Add(Bite bite)
        //{
        //    return new Bite();
        //}

        //public Bite Update(Bite bite)
        //{
        //    return new Bite();
        //}


        //public void Delete(string id)
        //{
        //    throw new NotImplementedException();
        //}

        public async Task<Bite> AddActivity(Bite bite, bool insertRecord = true)
        {
            bite.UserId = this.userId;
            AspNetUser user = aspNetUserService.GetByUserId(GuidEncoder.Decode(bite.UserId));
            Actor actor = aspNetUserService.ConvertToActor(user);
            bite.Actor = actor;

            if (insertRecord)
            {
                mongoService.InsertOne(bite);
            }

            // Create a new activity
            var activity = new Activity(this.userId, "pin", bite.Comment == null ? String.Empty : bite.Comment)
            {
                ForeignId = bite.Id.ToString()
            };
            var insertedActivity = await this.userFeed.AddActivity(activity);
            bite.StreamId = insertedActivity.Id;
            return bite;

        }

        private QuestionResponseSet RehydrateQuestionResponseSet(Bite bite)
        {
            if(bite.QuestionResponseSetId != null)
            {
                try
                {
                    QuestionResponseSet questionResponseSet = questionResponseSetService.GetById(GuidEncoder.Decode(bite.QuestionResponseSetId).ToString());
                    return questionResponseSet;
                }
                catch
                {
                    return null;
                }
            }
            return null;
        }
    }
}
