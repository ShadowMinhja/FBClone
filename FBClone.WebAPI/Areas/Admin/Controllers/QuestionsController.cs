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
    public class QuestionsController : BaseController
    {
        private readonly IQuestionService questionService;

        public QuestionsController() {
        }

        public QuestionsController(IQuestionService questionService)
        {
            this.questionService = questionService;
        }

        // GET: api/StaffMembers
        [EnableQuery()]
        [ResponseType(typeof(Question))]
        public IHttpActionResult Get()
        {
            try {
                var questions = questionService.Query(x => x.UserId == userId);
                return Ok(questions);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [ResponseType(typeof(Question))]
        public IHttpActionResult Get(string id)
        {
            try {
                var question = questionService.GetById(id);
                if (question == null)
                    return NotFound();
                else
                    return Ok(question);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [ResponseType(typeof(Question))]
        public IHttpActionResult Post([FromBody]Question question)
        {
            try
            {
                if (question == null)
                    return BadRequest("Question cannot be null");
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);
                question.Id = Guid.NewGuid().ToString();
                var newQuestion = questionService.Add(question);
                if (newQuestion == null)
                    return Conflict();
                return Created<Question>(Request.RequestUri + question.Id.ToString(), question);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [ResponseType(typeof(Question))]
        public IHttpActionResult Put(string id, [FromBody]Question question)
        {
            try {
                if (question == null)
                    return BadRequest("Question cannot be null");
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);
                var updatedQuestion = questionService.Update(question);
                if (updatedQuestion == null)
                    return NotFound();
                return Ok();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [ResponseType(typeof(Question))]
        public void Delete(string id)
        {
            questionService.Delete(id);
        }
    }
}
