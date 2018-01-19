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
    public class AnswersController : BaseController
    {
        private readonly IAnswerService answerService;

        public AnswersController() {
        }

        public AnswersController(IAnswerService answerService)
        {
            this.answerService = answerService;
        }

        // GET: api/Answers
        [EnableQuery()]
        [ResponseType(typeof(Answer))]
        public IHttpActionResult Get()
        {
            try {
                var answers = answerService.Query(x => x.UserId == userId);
                return Ok(answers);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [ResponseType(typeof(Answer))]
        public IHttpActionResult Get(string id)
        {
            try {
                var answer = answerService.GetById(id);
                if (answer == null)
                    return NotFound();
                else
                    return Ok(answer);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [ResponseType(typeof(Answer))]
        public IHttpActionResult Post([FromBody]Answer answer)
        {
            try
            {
                if (answer == null)
                    return BadRequest("Answer cannot be null");
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);
                answer.Id = Guid.NewGuid().ToString();
                var newAnswer = answerService.Add(answer);
                if (newAnswer == null)
                    return Conflict();
                return Created<Answer>(Request.RequestUri + answer.Id.ToString(), answer);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [ResponseType(typeof(Answer))]
        public IHttpActionResult Put(string id, [FromBody]Answer answer)
        {
            try {
                if (answer == null)
                    return BadRequest("Answer cannot be null");
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);
                var updatedAnswer = answerService.Update(answer);
                if (updatedAnswer == null)
                    return NotFound();
                return Ok();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [ResponseType(typeof(Answer))]
        public void Delete(string id)
        {
            answerService.Delete(id);
        }
    }
}
