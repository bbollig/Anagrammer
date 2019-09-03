using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Anagram.API.Classes;
using Anagram.API.Services;

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
        public ActionResult<List<string>> Get(string word, int limit = -1)
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


        [HttpPost("api/corpus/insert/{word}")]
        public IActionResult Insert([FromBody] string words)
        {
            if (words == null)
            {
                return BadRequest();
            }

            _Anagrammer.InsertWords(words);

            return Created("", new object());
        }

        [HttpDelete("api/corpus/delete/{word}")]
        public IActionResult Delete(string word)
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
