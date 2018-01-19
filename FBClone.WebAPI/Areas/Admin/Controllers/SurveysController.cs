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

namespace FBClone.WebAPI.Areas.Admin.Controllers
{
    [CustomAuthorize()]
    public class SurveysController : BaseController
    {
        private readonly ISurveyService surveyService;
        private readonly IApplicationSettingService applicationSettingService;

        public SurveysController() {
        }

        public SurveysController(ISurveyService surveyService, IApplicationSettingService applicationSettingService)
        {
            this.surveyService = surveyService;
            this.applicationSettingService = applicationSettingService;
        }

        // GET: api/Survey
        [ResponseType(typeof(Survey))]
        public IHttpActionResult Get()
        {
            try {
                var surveys = surveyService.AllIncluding(x => x.UserId == userId).ToList();
                return Ok(surveys);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [ResponseType(typeof(Survey))]
        public IHttpActionResult Get(string id)
        {
            try {
                var survey = surveyService.GetById(id);
                if (survey == null)
                    return NotFound();
                else
                    return Ok(survey);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [ResponseType(typeof(Survey))]
        public IHttpActionResult Post([FromBody]Survey survey)
        {
            try
            {
                if (survey == null)
                    return BadRequest("Survey cannot be null");
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);
                survey.Id = Guid.NewGuid().ToString();
                var newSurvey = surveyService.Add(survey);
                if (newSurvey == null)
                    return Conflict();
                return Created<Survey>(Request.RequestUri + survey.Id.ToString(), survey);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [ResponseType(typeof(Survey))]
        public IHttpActionResult Put(string id, [FromBody]Survey survey)
        {
            try {
                if (survey == null)
                    return BadRequest("Survey cannot be null");
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);
                var updatedSurvey = surveyService.Update(survey);
                if (updatedSurvey == null)
                    return NotFound();
                return Ok<Survey>(survey);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        //Publish Method
        [ResponseType(typeof(Survey))]
        public IHttpActionResult Put(string id, [FromBody]Survey survey, bool active)
        {
            try
            {
                if (survey == null)
                    return BadRequest("Survey cannot be null");
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);
                if (survey.QuestionDrafts.Count == 0)
                    return BadRequest("You must have at least one question for this survey to be published!");
                surveyService.Publish(survey, active);
                var applicationSetting = applicationSettingService.GetById(userId);
                if (applicationSetting.SurveySetup == false)
                {
                    applicationSetting.SurveySetup = true;
                    applicationSettingService.Update(applicationSetting);
                }
                return Ok<Survey>(survey);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
        [ResponseType(typeof(Survey))]
        public void Delete(string id)
        {
            surveyService.Delete(id);
        }
    }
}
