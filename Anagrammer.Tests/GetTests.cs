using Microsoft.VisualStudio.TestTools.UnitTesting;
using Anagram.API.Services;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace Anagrammer.Tests
{
    [TestClass]
    public class GetTests
    {
        IAnagrammer _Anagrammer;
        List<string> _Compare;

        public GetTests()
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
                "ared",
                "daer",
                "dare",
                "read"
            };

        }

        [TestMethod]
        public void GetAnagrams()
        {
            var results = _Anagrammer.GetAnagrams("dear");

            CollectionAssert.AreEqual(_Compare, results);

        }
    }
}
