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

namespace FBClone.WebAPI.Areas.Admin.Controllers
{
    [CustomAuthorize()]
    public class QuestionDraftsController : BaseController
    {
        private readonly IQuestionDraftService questionDraftService;
        private readonly IAnswerDraftService answerDraftService;
        private readonly ICategoryService categoryService;

        public QuestionDraftsController() {
        }

        public QuestionDraftsController(IQuestionDraftService questionDraftService, IAnswerDraftService answerDraftService, ICategoryService categoryService)
        {
            this.questionDraftService = questionDraftService;
            this.answerDraftService = answerDraftService;
            this.categoryService = categoryService;
        }

        [ResponseType(typeof(QuestionDraft))]
        public IHttpActionResult Post([FromBody]QuestionDraft questionDraft)
        {
            try
            {
                if (questionDraft == null)
                    return BadRequest("QuestionDraft cannot be null");
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);
                questionDraft.Id = Guid.NewGuid().ToString();
                questionDraft.Category = categoryService.GetById(questionDraft.CategoryId);
                var newQuestionDraft = questionDraftService.Add(questionDraft);
                if (newQuestionDraft == null)
                    return Conflict();
                return Created<QuestionDraft>(Request.RequestUri + questionDraft.Id.ToString(), questionDraft);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [ResponseType(typeof(QuestionDraft))]
        public IHttpActionResult Put(string id, [FromBody]QuestionDraft questionDraft)
        {
            try {
                if (questionDraft == null)
                    return BadRequest("QuestionDraft cannot be null");
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);
                var updatedQuestionDraft = questionDraftService.Update(questionDraft);
                if (updatedQuestionDraft == null)
                    return NotFound();
                return Ok();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [ResponseType(typeof(QuestionDraft))]
        public void Delete(string id)
        {
            questionDraftService.Delete(id);
        }
    }
}
