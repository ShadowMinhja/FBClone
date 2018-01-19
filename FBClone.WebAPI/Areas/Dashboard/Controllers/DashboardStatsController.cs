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
using System.Text.RegularExpressions;
using System.Data.SqlClient;
using System.Web.Http.OData.Query;
using System.Web.Http.OData.Extensions;

namespace FBClone.WebAPI.Areas.Dashboard.Controllers
{
    [CustomAuthorize()]
    public class DashboardStatsController : BaseController
    {
        private readonly IQuestionResponseSetService questionResponseSetService;
        private readonly IEntityDBService entityDBService;

        public DashboardStatsController() {
        }

        public DashboardStatsController(IQuestionResponseSetService questionResponseSetService, IEntityDBService entityDBService)
        {
            this.questionResponseSetService = questionResponseSetService;
            this.entityDBService = entityDBService;
        }

        [Route("api/DashboardStats/GetStatusTileCount")]
        public IHttpActionResult GetStatusTileCount()
        {
            try
            {
                var queryString = Request.GetQueryNameValuePairs().ToDictionary(x => x.Key, x => x.Value);
                DateTimeOffset beginDate;
                DateTimeOffset endDate;
                RetrieveBeginEndDate(queryString["$filter"], out beginDate, out endDate);
                var questionResponses = questionResponseSetService.Query(x => x.UserId == userId && x.CreatedAt >= beginDate && x.CreatedAt <= endDate);                
                return Ok(new { CustomerResponses = questionResponses.Count() });
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }


        [Route("api/DashboardStats/GetStatusTileAvg")]
        [EnableQuery()]
        public IHttpActionResult GetStatusTileAvg()
        {
            try
            {
                var queryString = Request.GetQueryNameValuePairs().ToDictionary(x => x.Key, x => x.Value);
                DateTimeOffset beginDate;
                DateTimeOffset endDate;
                RetrieveBeginEndDate(queryString["$filter"], out beginDate, out endDate);
                List<SqlParameter> sqlParams = new List<SqlParameter>() {
                    new SqlParameter("UserId", new Guid(userId)),
                    new SqlParameter("BeginDate", beginDate.ToString("yyyy-MM-dd")),
                    new SqlParameter("EndDate", endDate.ToString("yyyy-MM-dd"))
                };
                var questionResponseAverageScores = entityDBService.GetSPData<QuestionResponseSetAvg>("rspQuestionResponseSetAvgScore", sqlParams);
                return Ok(questionResponseAverageScores);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [Route("api/DashboardStats/GetPositivitySpread")]
        [EnableQuery()]
        public IHttpActionResult GetPositivitySpread()
        {
            try
            {
                var queryString = Request.GetQueryNameValuePairs().ToDictionary(x => x.Key, x => x.Value);
                DateTimeOffset beginDate;
                DateTimeOffset endDate;
                RetrieveBeginEndDate(queryString["$filter"], out beginDate, out endDate);
                List<SqlParameter> sqlParams = new List<SqlParameter>() {
                    new SqlParameter("UserId", new Guid(userId)),
                    new SqlParameter("BeginDate", beginDate.ToString("yyyy-MM-dd")),
                    new SqlParameter("EndDate", endDate.ToString("yyyy-MM-dd"))
                };
                var questionResponseSetPositivity = entityDBService.GetSPData<QuestionResponseSetPositivity>("rspQuestionResponseSetPositivitySpread", sqlParams);
                return Ok(questionResponseSetPositivity);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [Route("api/DashboardStats/GetQuestionTextAvgScore")]
        [EnableQuery()]
        public IHttpActionResult GetQuestionTextAvgScore()
        {
            try
            {
                var queryString = Request.GetQueryNameValuePairs().ToDictionary(x => x.Key, x => x.Value);
                DateTimeOffset beginDate;
                DateTimeOffset endDate;
                RetrieveBeginEndDate(queryString["$filter"], out beginDate, out endDate);
                List<SqlParameter> sqlParams = new List<SqlParameter>() {
                    new SqlParameter("UserId", new Guid(userId)),
                    new SqlParameter("BeginDate", beginDate.ToString("yyyy-MM-dd")),
                    new SqlParameter("EndDate", endDate.ToString("yyyy-MM-dd"))
                };
                var questionTextAvgScores = entityDBService.GetSPData<QuestionTextAvgScore>("rspQuestionTextAvgScore", sqlParams);
                return Ok(questionTextAvgScores);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [Route("api/DashboardStats/GetStaffMemberAvgScore")]
        [EnableQuery()]
        public IHttpActionResult GetStaffMemberAvgScore()
        {
            try
            {
                var queryString = Request.GetQueryNameValuePairs().ToDictionary(x => x.Key, x => x.Value);
                DateTimeOffset beginDate;
                DateTimeOffset endDate;
                RetrieveBeginEndDate(queryString["$filter"], out beginDate, out endDate);
                List<SqlParameter> sqlParams = new List<SqlParameter>() {
                    new SqlParameter("UserId", new Guid(userId)),
                    new SqlParameter("BeginDate", beginDate.ToString("yyyy-MM-dd")),
                    new SqlParameter("EndDate", endDate.ToString("yyyy-MM-dd"))
                };
                var questionTextAvgScores = entityDBService.GetSPData<StaffMemberAvgScore>("rspQuestionResponseSetAvgScoreByStaffMember", sqlParams);
                return Ok(questionTextAvgScores);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
        
        private void RetrieveBeginEndDate(string oDataFilter, out DateTimeOffset beginDate, out DateTimeOffset endDate)
        {
            Regex regex = new Regex(@"(\d{2,4}-\d{1,2}-\d{1,2})");
            string[] dateStrings = regex.Split(oDataFilter);
            bool beginDateFound = false;
            bool endDateFound = false;
            beginDate = DateTimeOffset.Now.AddYears(-1); //Default if no value found
            endDate = DateTimeOffset.Now; //Default if no value found
            if (dateStrings.Length > 1) {
                foreach(var dateString in dateStrings)
                {
                    if (beginDateFound == false)
                    {
                        var result = DateTimeOffset.TryParse(dateString, out beginDate);
                        if (result == true)
                        {
                            beginDateFound = true;
                            continue;
                        }
                    }
                    if(beginDateFound == true && !endDateFound)
                    {
                        var result2 = DateTimeOffset.TryParse(dateString, out endDate);
                        if(result2 == true)
                            endDateFound = true;
                    }
                }
            }
        }
    }
}
