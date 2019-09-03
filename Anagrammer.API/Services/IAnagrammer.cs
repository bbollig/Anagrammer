using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Anagram.API.Services
{
    public interface IAnagrammer
    {
        List<string> GetAnagrams(string word);

        bool CorpusContains(string word);

        int DeleteCorpus();

        bool DeleteWord(string word);

        void InsertWords(string words);
    }
}
