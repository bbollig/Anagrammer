using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Anagram.API.Services
{
    public interface IAnagrammer
    {
        int CorpusCount { get; }

        List<string> GetAnagrams(string word, bool includeBaseWord =  false);

        Dictionary<string, double> GetStats();

        bool CorpusContains(string word);

        int DeleteCorpus();

        bool DeleteWord(string word);

        int DeleteWordAndAnagrams(string word);

        void InsertWords(JArray words);

        bool CheckSetForAnagrams(JArray words);
    }
}
