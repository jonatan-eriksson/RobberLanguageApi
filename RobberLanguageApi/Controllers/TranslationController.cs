using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RobberLanguageApi.Data;
using RobberLanguageApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RobberLanguageApi.Controllers
{
    [Route("api/RobberLanguage")]
    [ApiController]
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
    }
}
