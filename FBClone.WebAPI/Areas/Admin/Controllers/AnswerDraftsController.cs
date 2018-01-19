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
    public class AnswerDraftsController : BaseController
    {
        private readonly IAnswerDraftService answerDraftService;

        public AnswerDraftsController() {
        }

        public AnswerDraftsController(IAnswerDraftService answerDraftService)
        {
            this.answerDraftService = answerDraftService;
        }

        // GET: api/StaffMembers
        [EnableQuery()]
        [ResponseType(typeof(AnswerDraft))]
        public IHttpActionResult Get(string questionId)
        {
            try {
                var answerDrafts = answerDraftService.Query(x => x.UserId == userId && x.QuestionId == questionId);
                return Ok(answerDrafts);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [ResponseType(typeof(AnswerDraft))]
        public IHttpActionResult Get(string questionId, string id)
        {
            try {
                var answerDraft = answerDraftService.GetById(id);
                if (answerDraft == null)
                    return NotFound();
                else
                    return Ok(answerDraft);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [ResponseType(typeof(AnswerDraft))]
        public IHttpActionResult Post([FromBody]AnswerDraft answerDraft)
        {
            try
            {
                if (answerDraft == null)
                    return BadRequest("AnswerDraft cannot be null");
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);
                answerDraft.Id = Guid.NewGuid().ToString();
                var newAnswerDraft = answerDraftService.Add(answerDraft);
                if (newAnswerDraft == null)
                    return Conflict();
                return Created<AnswerDraft>(Request.RequestUri + answerDraft.Id.ToString(), answerDraft);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [ResponseType(typeof(AnswerDraft))]
        public IHttpActionResult Put(string id, [FromBody]AnswerDraft answerDraft)
        {
            try {
                if (answerDraft == null)
                    return BadRequest("AnswerDraft cannot be null");
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);
                var updatedAnswerDraft = answerDraftService.Update(answerDraft);
                if (updatedAnswerDraft == null)
                    return NotFound();
                return Ok();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [ResponseType(typeof(AnswerDraft))]
        public void Delete(string id)
        {
            answerDraftService.Delete(id);
        }
    }
}
