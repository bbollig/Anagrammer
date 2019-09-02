using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;

namespace Anagram.API.Classes
{
    public class Corpus : IDisposable
    {
        public List<string> Words { get; set; }

        public Corpus()
        {
            InitDictionary();
        }

        private void InitDictionary()
        {
            if (File.Exists($"Dictionary.txt"))
            {
                Words = File.ReadAllLines("Dictionary.txt").ToList();
            }
        }

        /// <summary>
        /// When Corpus object gets disposed of by Garbage Collection,
        /// save the Words back to the Dictionary file to persist changes 
        /// such as word removal
        /// </summary>
        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
