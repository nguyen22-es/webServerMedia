using Common.DTOModels.Admin;
using Common.Entities;
using Common.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace webServerMedia.Controllers
{
    [Authorize(Policy = "Admin")]
    [Route("api/courses")]
    [ApiController]
    public class TopicController : ControllerBase
    {
        #region Properties and Variables
        private readonly IAdminService _db;
        private readonly LinkGenerator _linkGenerator;
        #endregion

        #region Constructor
        public TopicController(IAdminService db, LinkGenerator
        linkGenerator)
        {
            _db = db;
            _linkGenerator = linkGenerator;
        }
        #endregion

        #region Actions
        [HttpGet()]
        public async Task<ActionResult<List<TopicDTO>>> Get(bool include = false)
        {
            try
            {
                return await _db.GetAsync<Topic, TopicDTO>(include);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<TopicDTO>> Get(int id, bool include = false)
        {
            try
            {
                var dto = await _db.SingleAsync<Topic, TopicDTO>(s => s.Id.Equals(id), include);
                if (dto == null) return NotFound();
                return dto;
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }
        }

        [HttpPost]
        public async Task<ActionResult<TopicDTO>> Post(TopicDTO model)
        {
            try
            {
                if (model == null) return BadRequest("No entity provided");

                var id = await _db.CreateAsync<TopicDTO, Topic>(model);
                if (id < 1) return BadRequest("Unable to add the entity");

                var dto = await _db.SingleAsync<Topic, TopicDTO>(s => s.Id.Equals(id));
                if (dto == null) return BadRequest("Unable to add the entity");

                var uri = _linkGenerator.GetPathByAction("Get", "Courses", new { id });
                return Created(uri, dto);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Failed to add the entity");
            }
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<CourseDTO>> Put(int id, CourseDTO model)
        {
            try
            {
                if (model == null) return BadRequest("No entity provided");
                if (!id.Equals(model.Id)) return BadRequest("Differing ids");

                var exists = await _db.AnyAsync<Instructor>(a => a.Id.Equals(model.InstructorId));
                if (!exists) return NotFound("Could not find related entity");

                exists = await _db.AnyAsync<Course>(a => a.Id.Equals(id));
                if (!exists) return NotFound("Could not find entity");

                if (await _db.UpdateAsync<CourseDTO, Course>(model)) return NoContent();
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Failed to update the entity");
            }

            return BadRequest("Unable to update the entity");
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var exists = await _db.AnyAsync<Course>(a => a.Id.Equals(id));
                if (!exists) return BadRequest("Could not find entity");

                if (await _db.DeleteAsync<Course>(d => d.Id.Equals(id))) return NoContent();
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Failed to delete the entity");
            }

            return BadRequest("Failed to delete the entity");
        }
        #endregion
    }
}
