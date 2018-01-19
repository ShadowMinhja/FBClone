using System;
using System.Collections.Generic;
using System.Linq;
using FBClone.Data;
using FBClone.Data.Infrastructure;
using FBClone.Data.Repositories;
using FBClone.Model;
using System.Linq.Expressions;
using System.Configuration;

namespace FBClone.Service
{
    public interface ISearchService
    {
        IEnumerable<Search> SearchHistory();
        Search Add(Search search);
        void Delete(string id);

        void SetUserId(string userId);
        SearchResult QueryUserName(string searchText);
    }
    public class SearchService : ISearchService
    {
        private string userId { get; set; }
        private readonly IUnitOfWork unitOfWork;
        private readonly IMongoService mongoService;
        private readonly IAspNetUserService aspNetUserService;

        public SearchService(IUnitOfWork unitOfWork, IMongoService mongoService, IAspNetUserService aspNetUserService)
        {
            this.unitOfWork = unitOfWork;
            this.mongoService = mongoService;
            this.aspNetUserService = aspNetUserService;
        }

        public IEnumerable<Search> SearchHistory()
        {
            return mongoService.GetAll<Search>("Search", this.userId, 10);
        }

        public Search Add(Search search)
        {
            search.UserId = this.userId;
            search.CreatedAt = DateTimeOffset.Now;
            mongoService.InsertOne(search);
            return search;
        }

        public void Delete(string id)
        {
            
        }

        //Set User Id
        public void SetUserId(string userId)
        {
            if (this.userId == null)
            {
                this.userId = GuidEncoder.Encode(userId);
            }
        }

        public SearchResult QueryUserName(string searchText)
        {
            var aspUser = this.aspNetUserService.GetByNameExactMatch(searchText);
            if (aspUser == null)
            {
                var searchResult = new SearchResult
                {
                    ResultText = null,
                    ResultUrl = null,
                    SearchText = searchText,
                    Historical = true
                };
                //Check if UserName on Restricted List
                string[] restrictedNames = ConfigurationManager.AppSettings["RestrictedUserNames"].Split(',');
                if (restrictedNames.Contains(searchText.ToLower()))
                {
                    searchResult.ResultText = searchText;
                }
                return searchResult;
            }
            else
            {
                return new SearchResult
                {
                    ResultText = aspUser.UserName,
                    ResultUrl = null,
                    SearchText = searchText,
                    Historical = true
                };
            }
        }
    }
}
