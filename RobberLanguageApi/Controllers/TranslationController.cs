using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        [Route("CreateTranslation")]
        [HttpPost]
        public ActionResult<Translation> Translate(Translation translation)
        {
            return new Translation(translation.OriginalSentence);
        }
    }
}
