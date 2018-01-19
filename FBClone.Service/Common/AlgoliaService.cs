using Algolia.Search;
using FBClone.Data.Infrastructure;
using FBClone.Data.Repositories;
using FBClone.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace FBClone.Service
{
    public interface IAlgoliaService
    {
        Task AddUserIndex(AspNetUser user);
        Task AddBiteIndex(Bite bite);
    }

    public class AlgoliaService : IAlgoliaService
    {
        private AlgoliaClient algoliaClient { get; set; }

        public AlgoliaService()
        {
            string algoliaAppId = ConfigurationManager.AppSettings["ALGOLIA_APP_ID"];
            //string algoliaSearchKey = ConfigurationManager.AppSettings["ALGOLIA_SEARCH_ONLY_KEY"];
            string algoliaApiKey = ConfigurationManager.AppSettings["ALGOLIA_API_KEY"];

            this.algoliaClient = new AlgoliaClient(algoliaAppId, algoliaApiKey);
            this.algoliaClient.ConfigureAwait(false);
        }

        public async Task AddUserIndex(AspNetUser user)
        {
            string userIndexKey = ConfigurationManager.AppSettings["ALGOLIA_USER_INDEX"];
            Index index = this.algoliaClient.InitIndex(userIndexKey);
            UserIndex userIndex = new UserIndex
            {
                Id = GuidEncoder.Encode(user.Id),
                UserName = user.UserName,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                OrganizationName = user.OrganizationName == "\"N/A\"" ? String.Empty : user.OrganizationName
            };
            await index.AddObjectAsync(userIndex);
        }

        public async Task AddBiteIndex(Bite bite)
        {
            string bitesIndexKey = ConfigurationManager.AppSettings["ALGOLIA_USER_INDEX"];
            Index index = this.algoliaClient.InitIndex(bitesIndexKey);
            BiteIndex biteIndex = new BiteIndex
            {
                Id = bite.Id,
                UserId = bite.Actor.Id,
                MenuItem = new MenuItem(),
                Survey = new Survey()
            };
            await index.AddObjectAsync(biteIndex);
        }
    }
}
