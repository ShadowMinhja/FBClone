using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.ModelBinding;
using FBClone.Model;
using FBClone.Service;
using System.Web.Http.OData;
using FBClone.WebAPI.Controllers;
using System.Data.Entity.Validation;
using System.Web.Http.Description;
using Microsoft.AspNet.Identity;
using FBClone.WebAPI.Common;
using System.Threading.Tasks;

namespace FBClone.WebAPI.Areas.Admin.Controllers
{

    [CustomAuthorize()]
    public class SearchController : BaseController
    {
        private readonly ISearchService searchService;

        public SearchController()
        {
        }

        public SearchController(ISearchService searchService)
        {
            this.searchService = searchService;
        }

        // GET: api/Search
        //Get all searches performed by a user(aka search history)
        [Route("api/Search/QueryUserName")]
        [EnableQuery()]
        [ResponseType(typeof(Model.SearchResult))]
        public IHttpActionResult QueryUserName([FromBody]Model.Search search)
        {
            var result = this.searchService.QueryUserName(search.SearchText);
            return Ok(result);
        }

        //Get all searches performed by a user(aka search history)
        [EnableQuery()]
        [ResponseType(typeof(Model.Search))]
        public IHttpActionResult Get()
        {
            this.searchService.SetUserId(this.userId);
            var results = this.searchService.SearchHistory();
            return Ok(results);
        }

        //Create a search record for history lookup
        [ResponseType(typeof(Model.Search))]
        public IHttpActionResult Post([FromBody]Model.Search search)
        {
            this.searchService.SetUserId(this.userId);
            Model.Search newSearch = searchService.Add(search);
            return Ok(newSearch);
        }
    }
}
