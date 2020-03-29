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
    [ApiConventionType(typeof(DefaultApiConventions))]
    public class TagsController : ControllerBase
    {

        private readonly ITagRepository _tagRepository;
        public TagsController(ITagRepository tagRepository)
        {
            this._tagRepository = tagRepository;
        }

        #region Get
        // [Get] /api/Tags 
        /// <summary>
        /// returns all tags 
        /// </summary>
        /// <returns>list of tags</returns>
        [HttpGet]
        public IEnumerable<Tag> GetTags()
        {
            return _tagRepository.GetAll().ToList();
        }
        // [Get] /api/Tags/{id}
        /// <summary>
        /// Gets a tag by it's id 
        /// </summary>
        /// <param name="id">the id of the tag</param>
        /// <returns>returns a tag </returns>
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
        // [Post] /api/Tags
        /// <summary>
        /// adds a tag to the list
        /// </summary>
        /// <param name="tagDTO">the dto of tag</param>
        /// <returns>returns a the created tag</returns>
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
        // [Put] /api/Tags/{id}
        /// <summary>
        /// updates an existing tag
        /// </summary>
        /// <param name="id">the id</param>
        /// <param name="tag">the tag</param>
        /// <returns>if id doesn't equal the tagid a badrequest wil be returned else a nocontent</returns>
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
        // [Delete] /api/Tags/{id}
        /// <summary>
        /// Deletes a tag
        /// </summary>
        /// <param name="id">the tagid</param>
        /// <returns>if id doesn't equal the tagid a badrequest wil be returned else a nocontent</returns>
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