using Microsoft.VisualStudio.TestTools.UnitTesting;
using Anagram.API.Services;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace Anagrammer.Tests
{
    [TestClass]
    public class InterfaceTests
    {
        IAnagrammer _Anagrammer;
        List<string> _Compare;
        List<string> _Compare2;
        List<string> _Compare3;


        public InterfaceTests()
        {
            _Anagrammer = new Anagram.API.Services.Anagrammer($"smallDictionary.txt");
            //smallDictionary has just 5 items in its set: 
            //ared
            //daer
            //dare
            //dear
            //read

            //Using this to compare results retrieved from our smallDictionary
            _Compare = new List<string>()
            {
                "Ared",
                "daer",
                "dare",
                "read"
            };

            //This one includes 'dear' and is used to compare for GetAnagramsIncludeBaseWord
            _Compare2 = new List<string>()
            {
                "Ared",
                "daer",
                "dare",
                "dear",
                "read"
            };

            //Using this to compare with 'Ared' removed for being a proper noun
            _Compare3 = new List<string>()
            {
                "daer",
                "dare",
                "read"
            };
        }
        #region Get Tests
        [TestMethod]
        public void GetAnagrams()
        {
            var results = _Anagrammer.GetAnagrams("dear");

            CollectionAssert.AreEqual(_Compare, results);
        }

        [TestMethod]
        public void GetAnagramsIncludeBaseWord()
        {
            var results = _Anagrammer.GetAnagrams("dear", true);

            CollectionAssert.AreEqual(_Compare2, results);
        }

        [TestMethod]
        public void GetAnagramsWithoutProperNouns()
        {
            var results = _Anagrammer.GetAnagrams("dear", false, true);

            CollectionAssert.AreEqual(_Compare, results);

        }

        [TestMethod]
        public void TestContains()
        {
            //Test word we know corpus has
            var contains = _Anagrammer.CorpusContains("dear");
            Assert.AreEqual(true, contains);

            //Test word we know corpus doesn't have
            var doesNotContain = _Anagrammer.CorpusContains("periwinkle");
            Assert.AreEqual(false, doesNotContain);
        }
        #endregion

        #region Insert Tests
        [TestMethod]
        public void TestInsertSingleWord()
        {
            //Test word we know corpus doesn't have
            var doesNotContain = _Anagrammer.CorpusContains("periwinkle");
            Assert.AreEqual(false, doesNotContain);

            //insert word
            JArray inserts = new JArray()
            {
                "periwinkle"
            };
            _Anagrammer.InsertWords(inserts);

            //test contains to see if word is there
            var contains = _Anagrammer.CorpusContains("periwinkle");
            Assert.AreEqual(true, contains);
        }

        [TestMethod]
        public void TestInsertMultipleWords()
        {
            //Test words we know corpus doesn't have
            List<string> words = new List<string>()
            {
                "periwinkle",
                "might",
                "destroy"
            };

            foreach (var word in words)
            {
                var doesNotContain = _Anagrammer.CorpusContains($"{word}");
                Assert.AreEqual(false, doesNotContain);
            }

            //insert words
            JArray inserts = JArray.FromObject(words);
            _Anagrammer.InsertWords(inserts);

            //now test to see if the words are there
            foreach (var word in words)
            {
                var contains = _Anagrammer.CorpusContains($"{word}");
                Assert.AreEqual(true, contains);
            }
        }
        #endregion

        #region Delete Tests
        [TestMethod]
        public void DeleteWord()
        {
            //Test word we know corpus doesn't have
            var doesNotContain = _Anagrammer.CorpusContains("periwinkle");
            Assert.AreEqual(false, doesNotContain);

            //insert word
            JArray inserts = new JArray()
            {
                "periwinkle"
            };
            _Anagrammer.InsertWords(inserts);

            //test contains to see if word is there
            var contains = _Anagrammer.CorpusContains("periwinkle");
            Assert.AreEqual(true, contains);

            //Delete new word
            _Anagrammer.DeleteWord("periwinkle");
            var deleted = _Anagrammer.CorpusContains("periwinkle");
            Assert.AreEqual(false, deleted);
        }

        [TestMethod]
        public void DeleteWordAndAnagrams()
        {
            //Test words we know corpus doesn't have
            List<string> words = new List<string>()
            {
                "angel",
                "agnel",
                "angle",
                "genal",
                "glean",
                "lagen"
            };

            foreach (var word in words)
            {
                var doesNotContain = _Anagrammer.CorpusContains($"{word}");
                Assert.AreEqual(false, doesNotContain);
            }

            //insert words
            JArray inserts = JArray.FromObject(words);
            _Anagrammer.InsertWords(inserts);

            //now test to see if the words are there
            foreach (var word in words)
            {
                var contains = _Anagrammer.CorpusContains($"{word}");
                Assert.AreEqual(true, contains);
            }

            //Delete new words
            int numberDeleted = _Anagrammer.DeleteWordAndAnagrams("angel");
            int numberCreated = words.Count;

            //test if count returned matches number inserted 
            Assert.AreEqual(numberCreated, numberDeleted);

            foreach (var word in words)
            {
                var deleted = _Anagrammer.CorpusContains($"{word}");
                Assert.AreEqual(false, deleted);
            }
        }

        [TestMethod]
        public void DeleteCorpus()
        {
            //get count of corpus items
            int countBefore = _Anagrammer.CorpusCount;
            _Anagrammer.DeleteCorpus();
            int countAfter = _Anagrammer.CorpusCount;

            Assert.AreNotEqual(countBefore, countAfter);
        }
        #endregion

    }
}
