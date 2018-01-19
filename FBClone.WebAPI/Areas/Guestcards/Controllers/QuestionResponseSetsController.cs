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
using FBClone.WebAPI.Mappers;
using AutoMapper;
using System.Web.Http.OData.Extensions;
using System.Web.Http.OData.Query;
using System.Data.SqlClient;
using FBClone.WebAPI.Areas.Guestcards;

namespace FBClone.WebAPI.Areas.Guestcards.Controllers
{
    //[CustomAuthorize()]
    public class QuestionResponseSetsController : BaseController
    {
        private readonly IQuestionResponseSetService questionResponseSetService;
        private readonly IEntityDBService entityDBService;

        public QuestionResponseSetsController() {
        }

        public QuestionResponseSetsController(IQuestionResponseSetService questionResponseSetService, IEntityDBService entityDBService)
        {
            this.questionResponseSetService = questionResponseSetService;
            this.entityDBService = entityDBService;
        }

        // GET: api/QuestionResponseSets
        //[EnableQuery(PageSize = 2)]
        //[ResponseType(typeof(QuestionResponseSet))]
        //public IHttpActionResult Get()
        public PageResult<QuestionResponseSet> Get(ODataQueryOptions<QuestionResponseSet> options)
        {
            ODataQuerySettings settings = new ODataQuerySettings()
            {
                PageSize = 15
            };
            var questionResponseSets = questionResponseSetService.Query(x => x.UserId == userId);
            var filteredResults = options.ApplyTo(questionResponseSets.AsQueryable(), settings);
            long? countRecords = Request.ODataProperties().TotalCount;
            if (countRecords == null) {
                countRecords = (options.ApplyTo(questionResponseSets.AsQueryable()) as IEnumerable<QuestionResponseSet>).Count();
            }
            return new PageResult<QuestionResponseSet>(
                filteredResults as IEnumerable<QuestionResponseSet>,
                Request.ODataProperties().NextLink,
                countRecords
            );
        }

        [ResponseType(typeof(QuestionResponseSet))]
        public IHttpActionResult Get(string id)
        {
            try {
                var questionResponseSet = questionResponseSetService.GetById(id);
                if (questionResponseSet == null)
                    return NotFound();
                else
                    return Ok(questionResponseSet);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        //Public facing route to resolve location reviews
        [Route("api/Reviews/GetLocationReviews")]
        [EnableQuery()]
        public IHttpActionResult GetLocationStats(string locationId)
        {
            try
            {
                List<SqlParameter> sqlParams = new List<SqlParameter>() {
                    new SqlParameter("LocationId", locationId),
                };
                var questionResponseAverageScores = entityDBService.GetSPData<LocationReview>("spGetLocationReviews", sqlParams);
                return Ok(questionResponseAverageScores);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        //Public facing route to resolve menu item reviews
        [Route("api/Reviews/GetMenuItemReviews")]
        [EnableQuery()]
        public IHttpActionResult GetMenuItemStats(string menuItemId)
        {
            try
            {
                List<SqlParameter> sqlParams = new List<SqlParameter>() {
                    new SqlParameter("MenuItemId", menuItemId),
                };
                var questionResponseAverageScores = entityDBService.GetSPData<MenuItemReview>("spGetMenuItemReviews", sqlParams);
                return Ok(questionResponseAverageScores);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [ResponseType(typeof(QuestionResponseSet))]
        public void Delete(string id)
        {
            questionResponseSetService.Delete(id);
        }
    }
}
