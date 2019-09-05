using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Anagram.API.Classes;
using Anagram.API.Services;
using Newtonsoft.Json.Linq;

namespace Anagram.API.Controllers
{
    [ApiController]
    public class AnagramController : ControllerBase
    {

        private IAnagrammer _Anagrammer { get; set; }

        public AnagramController(IAnagrammer anagrammer)
        {
            _Anagrammer = anagrammer;
        }

        // GET api/values
        [HttpGet("api/anagrams/{word}")]
        public ActionResult<List<string>> GetAnagrams(string word, int limit = -1, bool returnProperNouns = true)
        {
            if (word == null)
            {
                return BadRequest();
            }

            var anagrams = _Anagrammer.GetAnagrams(word);

            if (anagrams.Count() > 0)
            {
                if (limit >= 0)
                {
                    return Ok(RemoveAnagramsFromReturn(limit, anagrams));
                }

                //if we do not want proper nouns
                if (!returnProperNouns)
                {
                    for (int i = 0; i < anagrams.Count(); i++)
                    {
                        if (char.IsUpper(anagrams[i][0]))
                        {
                            anagrams.Remove(anagrams[i]);
                        }
                    }
                }

                return Ok(anagrams);
            }

            return NotFound();
        }

        [HttpGet("api/corpus/contains/{word}")]
        public ActionResult<List<string>> Contains(string word, int maxReturns = -1)
        {
            if (word == null)
            {
                return BadRequest();
            }

            var exists = _Anagrammer.CorpusContains(word);

            if (exists)
            {
                return Ok();
            }

            return NotFound();
        }


        [HttpPost("api/corpus/insertwords")]
        public IActionResult InsertWords([FromBody] JArray words)
        {
            if (words == null)
            {
                return BadRequest();
            }

            _Anagrammer.InsertWords(words);

            return Created("", new object());
        }

        [HttpDelete("api/corpus/delete/{word}")]
        public IActionResult DeleteWord(string word)
        {
            if (word == null)
            {
                return BadRequest();
            }

            var deleted = _Anagrammer.DeleteWord(word);

            if (deleted)
            {
                return NoContent();
            }

            return NotFound();
        }

        [HttpDelete("api/corpus/deletecorpus")]
        public IActionResult DeleteCorpus()
        {
            var deleted = _Anagrammer.DeleteCorpus();

            if (deleted > 0)
            {
                return NoContent();
            }

            return NotFound();
        }

        [HttpDelete("api/anagrams/deleteanagrams/{word}")]
        public IActionResult DeleteWordAndAnagrams(string word)
        {
            if (word == null)
            {
                return BadRequest();
            }

            var deleted = _Anagrammer.DeleteWordAndAnagrams(word);

            if (deleted > 0)
            {
                return NoContent();
            }

            return NotFound();
        }

        [HttpGet("api/corpus/getstats")]
        public IActionResult GetStats()
        {
            return Ok(_Anagrammer.GetStats());
        }

        [HttpGet("api/anagrams/checkforanagrams/{words}")]
        public IActionResult CheckForAnagrams([FromBody] JArray words)
        {
            if (words == null || words.Count() <= 0)
            {
                return BadRequest();
            }

            return Ok(_Anagrammer.CheckSetForAnagrams(words));
        }

        #region Private Funcs
        private static List<string> RemoveAnagramsFromReturn(int maxReturns, List<string> anagrams)
        {
            //Creating anCount because using anagrams.Count directly in the 
            //for loop messes with the number of iterations
            var ansForRemoval = new List<string>();
            foreach (var an in anagrams)
            {
                ansForRemoval.Add(an);
            }

            for (int i = 0; i < anagrams.Count(); i++)
            {
                var anagram = anagrams[i];
                if (i < maxReturns)
                {
                    continue;
                }

                ansForRemoval.Remove(anagram);
            }

            return ansForRemoval;
        }

        #endregion //Private Funcs
    }
}
