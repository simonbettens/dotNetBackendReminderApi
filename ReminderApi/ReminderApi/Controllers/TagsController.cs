using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ReminderApi.Models.Domain;
using ReminderApi.Models.DTOs;

namespace ReminderApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class TagsController : ControllerBase
    {

        private readonly ITagRepository _tagRepository;
        public TagsController(ITagRepository tagRepository)
        {
            this._tagRepository = tagRepository;
        }

        #region Get
        [HttpGet]
        public IEnumerable<Tag> GetTags()
        {
            return _tagRepository.GetAll().ToList();
        }
        [HttpGet("{id}")]
        public ActionResult<Tag> GetTag(int id)
        {
            Tag tag = _tagRepository.GetById(id);
            if (tag == null)
            {
                return NotFound();
            }
            
            return tag;
        }
        #endregion

        #region Post & Put & Delete Tag
        [HttpPost]
        public ActionResult<Tag> PostTag(TagDTO tagDTO)
        {
            Tag createTag = _tagRepository.GetByName(tagDTO.Name);
            if (createTag == null)
            {
                createTag = new Tag(tagDTO.Name, tagDTO.Color);
                _tagRepository.Add(createTag);

            }
            _tagRepository.SaveChanges();
            return CreatedAtAction(nameof(GetTag), new { id = createTag.TagId }, createTag);
        }
        [HttpPut("{id}")]
        public IActionResult PutTag(int id, Tag tag)
        {
            if (id != tag.TagId)
            {
                return BadRequest();
            }
            _tagRepository.Update(tag);
            _tagRepository.SaveChanges();
            return NoContent();
        }
        
        [HttpDelete("{id}")]
        public IActionResult DeleteTag(int id)
        {
            Tag tag = _tagRepository.GetById(id);
            if (tag == null)
            {
                return NotFound();
            }
            _tagRepository.Delete(tag);
            _tagRepository.SaveChanges();
            return NoContent();
        }
        #endregion
    }
}