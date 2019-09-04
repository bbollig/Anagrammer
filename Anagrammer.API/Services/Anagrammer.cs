using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Anagram.API.Classes;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Anagram.API.Services
{
    public class Anagrammer : IAnagrammer
    {
        private Corpus Corpus { get; set; }

        public int CorpusCount
        {
            get { return Corpus.Words.Count; }
        }

        public Anagrammer()
        {
            var path = "Dictionary.txt";
            Corpus = new Corpus(path);
        }

        public Anagrammer(string corpusPath)
        {
            Corpus = new Corpus(corpusPath);
        }

        public bool CorpusContains(string word)
        {
            return Corpus.Words.Contains(word);
        }

        public int DeleteCorpus()
        {
            try
            {
                return Corpus.Words.RemoveAll(w => true);
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public bool DeleteWord(string word)
        {
            if (CorpusContains(word))
            {
                return Corpus.Words.Remove(word);
            }

            return false;
        }

        public int DeleteWordAndAnagrams(string word)
        {
            if (CorpusContains(word))
            {
                var deleteTargets = GetAnagrams(word, true);

                foreach (var target in deleteTargets)
                {
                    Corpus.Words.Remove(target);
                }

                return deleteTargets.Count();
            }

            return 0;
        }

        /// <summary>
        /// Gets anagrams of parameter word. Optional second paramater includeBaseWord
        /// is to allow DeleteWordAndAnagrams function the ability to get a list of
        /// anagrams that include the base word. Will not expose this second parameter to the API
        /// outside of previously mentioned delete function.
        /// </summary>
        /// <param name="word"></param>
        /// <param name="includeBaseWord"></param>
        /// <returns></returns>

        public List<string> GetAnagrams(string word, bool includeBaseWord = false)
        {
            //one array to house potentials and one to return
            //actual anagrams
            var candidates = new List<string>();
            var anagrams = new List<string>();

            //filter out all words that don't have the same count
            //to make our foreach a bit more efficient
            candidates = Corpus.Words.Where(w => w.Length == word.Length).ToList();

            //sort word
            var sortedWord = SortWord(word);

            foreach (var candidate in candidates)
            {
                //a word cannot be an anagram of itself
                if (candidate == word && !includeBaseWord)
                {
                    continue;
                }

                //sort candidate
                string sortedCand = SortWord(candidate);

                //check equality
                if (sortedCand == sortedWord)
                {
                    anagrams.Add(candidate);
                }
            }

            return anagrams;
        }

        public void InsertWords(JArray words)
        {

            foreach (var word in words)
            {
                if (!CorpusContains(word.ToString()))
                {
                    Corpus.Words.Add(word.ToString());
                }
            }
        }

        //Abstracting our sorting logic away since our input and
        //candidates will both be using it at different stages
        private string SortWord(string word)
        {
            var wordAsArray = word.ToCharArray();
            Array.Sort(wordAsArray);

            return new string(wordAsArray);
        }
    }
}
