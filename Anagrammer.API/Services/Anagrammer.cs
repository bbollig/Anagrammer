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
        /// outside of previously mentioned delete function. Optional third parameter to exclude
        /// proper nouns (words that start with an upper case letter) form the return set.
        /// </summary>
        /// <param name="word"></param>
        /// <param name="includeBaseWord"></param>
        /// /// <param name="returnProperNouns"></param>
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
                //sort candidate
                string sortedCand = SortWord(candidate);

                //if the two words are anagrams
                if (sortedCand.ToLower() == sortedWord.ToLower())
                {
                    anagrams.Add(candidate);
                }
            }

            //in standard cases, a word cannot be an anagram of itself, so remove it
            //unless we specify otherwise
            if (!includeBaseWord)
            {
                anagrams.Remove(word);
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

        public Dictionary<string, double> GetStats()
        {
            Dictionary<string, double> stats = new Dictionary<string, double>();

            if (CorpusCount == 0)
            {
                stats.Add("Count", CorpusCount);
                stats.Add("Min", 0);
                stats.Add("Max", 0);
                stats.Add("Average", 0);
                stats.Add("Median", 0);

                return stats;
            }

            stats.Add("Count", CorpusCount);

            var words = Corpus.Words.ToArray();
            List<int> lengths = new List<int>();

            foreach (var word in words)
            {
                lengths.Add(word.Length);
            }

            lengths.Sort();
            //in a sorted list, the smallest will be first
            int min = lengths[0];
            stats.Add("Min", min);
            //the largest will be last
            int max = lengths[CorpusCount - 1];
            stats.Add("Max", max);

            double average = lengths.Average();
            stats.Add("Average", average);

            //get the median
            int median;
            int size = lengths.Count() - 1;
            int mid = size / 2;

            //if odd-numbered array
            if (size % 2 != 0)
            {
                //median will be the middle number
                median = lengths[mid];
            }
            else
            {
                //in even array, need to get the middle two numbers and 
                //add then divide by two
                median = (int)(lengths[mid] + (double)lengths[mid - 1]) / 2;
            }

            stats.Add("Median", median);

            return stats;
        }
        public bool CheckSetForAnagrams(JArray words)
        {
            //change to list so we can use linq
            List<string> listWords = words.ToObject<List<string>>();

            //check if any of the words lengths don't match the top word
            if (listWords.Any(w => w.Length != listWords[0].Length))
            {
                //if the lengths don't match, they can't be anagrams
                return false;
            }

            //sort all the words in the lists
            for (int i = 0; i < listWords.Count(); i++)
            {
                listWords[i] = SortWord(listWords[i]);
            }

            var firstWord = listWords[0].ToString();

            //check the rest for equality with the first
            for (int i = 1; i < listWords.Count() - 1; i++)
            {
                if (listWords[i] != firstWord)
                {
                    //return at the earliest instance of inequality
                    return false;
                }
            }

            return true;
        }

        //Abstracting our sorting logic away since our input and
        //candidates will both be using it in multiple calls
        private string SortWord(string word)
        {
            var wordAsArray = word.ToCharArray();
            Array.Sort(wordAsArray);

            return new string(wordAsArray);
        }
    }
}
