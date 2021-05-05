using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RobberLanguageApi.Data;
using RobberLanguageApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RobberLanguageApi.Controllers
{
    
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/RobberLanguage")]
    public class TranslationController : ControllerBase
    {
        private RobberTranslationDbContext _context { get; set; }
        public TranslationController(RobberTranslationDbContext context)
        {
            _context = context;
        }

        [Route("CreateTranslation")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Translation>> Translate(Translation translation)
        {
            if (String.IsNullOrWhiteSpace(translation.OriginalSentence))
            {
                return BadRequest();
            }
            var newTranslation = new Translation(translation.OriginalSentence);
            await _context.AddAsync(newTranslation);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetTranslation), new { id = newTranslation.Id }, newTranslation);
        }

        /// <summary>
        /// Retrieves a specific translation by unique id
        /// </summary>
        /// <param name="id" example="1">The translation id</param>
        /// <response code="200">Translation retrieved</response>
        /// <response code="404">Translation not found</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Translation>> GetTranslation(int id)
        {
            var translation = await _context.Translations.FindAsync(id);
            if (translation == null)
            {
                return NotFound();
            }

            return Ok(translation);
        }

        /// <summary>
        /// Retrieves a list of all translations
        /// </summary>
        /// <response code="200">Translations retrieved</response>
        /// <response code="404">No translations found</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<Translation>>> GetTranslations()
        {
            return await _context.Translations.ToListAsync();
        }


        [HttpGet("query")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<Translation>>> GetQuery([FromQuery] string keyword)
        {
            return await _context.Translations.Where(t => t.OriginalSentence.Contains(keyword)).ToListAsync();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTranslation(long id, Translation translation)
        {
            if (id != translation.Id)
                return BadRequest();

            _context.Entry(translation).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) when (!_context.Translations.Any(t => t.Id == id))
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
