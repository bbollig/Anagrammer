﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Anagram.API.Classes;

namespace Anagram.API.Services
{
    public class Anagrammer : IAnagrammer
    {
        public Corpus Corpus { get; set; } = new Corpus();

        public bool CorpusContains(string word)
        {
            return Corpus.Words.Contains(word);
        }

        public bool DeleteWord(string word)
        {
            if (CorpusContains(word)
            {
                Corpus.Words.Remove(word);
                return true;
            }

            return false;
        }

        public List<string> GetAnagrams(string word)
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
                if (candidate == word)
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