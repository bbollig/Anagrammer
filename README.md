## Anagrammer

### Table of Contents
1. [Overview & Discussion](/README.md#overview--discussion)
2. [Up and Running Guide](/README.md#up-and-running-guide)
3. [Testing with Postman](/README.md#testing-with-postman)
4. [Final Thoughts](/README.md#final-thoughts)

---
### Overview & Discussion

This is my first attempt at making a truly cross-platform API using dotnet core. It was an interesting exercise learning how this would be gotten up and running on say, a Mac, for example.

The solution consists of two projects, the first being the API itself (Anagram.API) and the second a Unit Test project that I created to help me in my developement process (Anagrammer.Tests). I should note here, the Test project focuses on the heavy lifter class of the API, where most of the functonality is located and ignores the  API Controller that exposes the endpoints. I left the endpoint testing to a tool called Postman that I find very useful for working with APIs. I chose to use Postman for this because it allows me the ability to export/import entire test suites for sharing. You will find the file to import these tests alongside the projectfolders for the API and Unit Tests, as well as this README.md (more on Postman and importing later).

Now onto an overview of the API project structure. Inside the Anagram.API folder, you will find several folders and files. I will keep my discussion to only those items that are noteworthy:

* *Program.cs* - the main entrypoint for the application.
* *Startup.cs* - Where we can inject any services the application might depend on. dotnet core has a Dependency Injection container built right into the framework, making it super slick and easy to swap services in and out. Probably one of my favorite features. /endrant.
* *dictionary.txt* - is the file provided for the exercise. I chose to import it on startup, meaning that when first firing up the API, you will not need to insert any words to begin testing out endpoints. And as I write this, I realize that I forgot to finish the functionality to save back to this file to persist changes to the dictionary across sessions. I think it will leave it for a future feature, at this point. So go ahead and delete the entire Corpus when testing. It will be there when you restart the service.
* *Controllers folder* - Where the AnagramsController file lives, that exposes all the endpoints of the API
  + *AnagramController* - Gets the Anagrammer class injected in it's constructor. All endpoints are written with a "bouncer" or "blocker" pattern, to filter out bad or invalid content before passing it on to the Anagrammer class. Exposes Endpoints:
    - **GetAnagrams** (HttpGet("api/anagrams/{word}")) Takes an string for parameter and returns a number of anagrams for that word. Optional parameters include 'limit' to limit the size of the collection returned and returnProperNouns to configure whether or not you want words starting with a capital letter included in the return collection. Success Returns 200 Ok
    - **Contains** (HttpGet("api/corpus/contains/{word}")) Takes a string parameter and returns true/false if the Corpus already contains it. Not a requirement in the project parameters, but I found it was nice functionality to have while developing and decided to expose it. Found returns 200 Ok response, not found returns 404 Not Found
    - **InsertWords** (HttpPost("api/corpus/insertwords")) Takes a Json Array of words and inserts them into the Corpus *if they do not alread exist*. Returns 201 Created Response.
    - **DeleteWord** (HttpDelete("api/corpus/delete/{word}")) Takes a string parameter and deletes the word if the Corpus contains it. On success, returns 204 No Content, on failure returns 404 Not Found.
    - **DeleteCorpus** (HttpDelete("api/corpus/deletecorpus")) Deletes all the contents of the Corpus. On success, returns 204 No Content, on failure returns 404 Not Found (in the case that its already been deleted).
    - **DeleteWordAndAnagrams** (HttpDelete("api/anagrams/deleteanagrams/{word}")) Takes a string parameter and deletes the word and all anagrams of the word. On success, returns 204 No Content, on failure returns 404 Not Found.
    - **GetStats** (HttpGet("api/corpus/getstats")) Returns a Json Array with the total word count, min and max lengths of all the words in the Corpus and the Median and Average calculation of the words. Returns 200 Ok.
    - **CheckForAnagrams** (HttpGet("api/anagrams/checkforanagrams")) Takes a Json Array of words and tests if they are anagrams of each other. Returns 200 Ok with a True or False value in the body of the response.
* *Services Folder* - Where the IAnagrammer interface and Anagrammer class live.
  + *Angagrammer.cs* - Has two constructors, one default that looks in the root of the project file for the 'dictionary.txt' file, and another that accepts a path to an entirely different dictionary file. I used the second constructor for injecting a much smaller dictionary file for use in the Unit Testing. The Anagrammer exposes a Corpus class as property and feeds the Corpus on instantiation the dictionary file, creating the in-memory store. I won't go into minute detail of the internal methods of the class here (since it is fairly well documented with comments in the code) but I will list them in no particular order: CorpusContains, DeleteCorpus, DeleteWord, DeleteWordAndAnagrams, GetAnagrams, InsertWords, GetStats, CheckSetForAngrams, and SortWord.
* *Classes Folder* - Where the Corpus class lives
  + *Corpus.cs* - simply wraps a List<string> object for easy manipulation. Take a filepath to the dictionary on construction. Also inherits IDisposable to use the Dispose method. This is where I originally intended to save the changes to the Corpus back the the dictionary file, after the Anagram Service is shut down and when Garbage Collection cleaned the class up. This is a feature for another day however.
  
### Up and Running Guide

Since this is cross-platform, I've done some work to ensure that the steps to set up are as close as possible across both Mac and Windows environments, which is why, for instance, this guide uses VS Code instead of Visual Studio 2017 or Visual Studio for Mac. I found that the steps and pitfalls in setup (and I did test on both Windows and Mac machines) were too different to make writing this guide in an acceptable timeframe feasible. If there is a difference, I will note so at the appropriate time.

**Another thing to note, the Unit Tests in Anagrammer.Tests will not be able to be run from VS Code without some additional setup, not included in these steps. If you have access to VS for Mac or VS 2017 or up on Windows, you will should be able to run the Unit Tests.**

1. First, you will need to download and install VS Code and Postman, if you don't have them already.
[https://code.visualstudio.com/download]
[https://www.getpostman.com/downloads/]
2. After installing VS Code, make sure that you are have installed a version of dotnet core 2.2. I'm not exactly sure which version comes with VS Code on a fresh download these days, so best to be safe and double check. If you don't have 2.2, you can install it here for Mac:
[https://dotnet.microsoft.com/download/thank-you/dotnet-sdk-2.2.401-macos-x64-installer]
or here for Windows:
[https://dotnet.microsoft.com/download/thank-you/dotnet-sdk-2.2.401-windows-x64-installer]
3. Now that we have the correct version of dotnetcore installed, we can run VS Code.
  * Once its running go to File -> Open Folder and browse to wheverever you have the Angrammer folder stored. Click on the Anagrammer folder and click on the Select Folder button. **Once opening this folder, you may see a popup in the bottom right asking your permission to install some missing components for VS Code. Do so.**
  * Now that the workspace folder is open, at the top menu of VS Code a little ways down from File you will see Terminal. Click Terminal -> Run New Terminal. You will see the Terminal screen pop up at the bottom of the window.
  * On fresh Terminal, you will likely need to move into the Anagram.API folder. You can do so by typing `cd Anagrammer.API`. 
  * Now that the terminal is in the correct location(the one with Anagram.API.csproj file) you can start the service by typing `dotnet run --urls http://0.0.0.0:3000/` This command starts the API service and ensures that it is listening over HTTP localhost on port 3000.
4. Now we can switch to Postman. Once it is started, find and click the Import button in the top left menu area next to the Big Orange New button. Find the Anagram.API.postman_collection.json file, located in the top level Anagrammer folder, alongside folders for Anagrammer.API, Anagrammer.Tests and the README.md file, and import it. Now you will see all the tests for all the endpoints ready to go in the left side-bar, under Anagram.API folder. You will see the tests are grouped by Http type; so there are folders for GetTests, PostTests and DeleteTests. 

### Testing With Postman

Now with the service running and Postman ready to go, you can start testing. You can click on any of the tests under the folder, such as GetAnagrams. You will see in the main window, the URL it will test against `http://localhost:3000/api/anagrams/dear` and the HTTP method it will use `GET`. To the right you will see a blue SEND button. The URL shows that it will point at localhost, port 3000 and the final word, "dear" is the word it is going to search for anagrams for. Clicking send should return a 200 OK status and a Json array of anagrams for the word dear. You can change "dear" to anything you want from here.

For the rest, I'll write a quick list on what each one does. Some of these are noted functionality at the top of the discussion section, some of these are used to test edge cases:
* GetTests
  * *GetAnagrams* - Tests the GetAnagrams endpoint with good information
  * *GetAnagramsWithMaxReturn* - Tests GetAnagrams with an optional query parameter, 'limit' to ensure that the API doesn't return more than what you send in the query string.
  * *GetAnagramsWithMaxZeroReturn* - Same test as the previous but sends zero instead. Wrote this one to test and edge case.
  * *GetAnagramsWithoutProperNouns* - Tests GetAnagrams with another optional query parameter, 'returnProperNouns'. This defaults to true on the API and when left blank or not inlcuded, will return strings with the first letter being uppercase. Setting it to false in the query string will tell the API to disinclude proper nouns in the return set.
  * *GetStats* - Returns count, min letters, max letters, average letter count and median letter count.
  * *TestDoesContain* - This is to test whether or not the Corpus already contains a word. This test should return true without changing the word used in the URL.
  * *TestDoesNotContain* - Like above, only using a word known to not be in the Corpus on startup. Should return false as written.
  * *CheckSetForAnagramsTrue* - Tests CheckForAnagrams endpoint by passing a JSON array of words and seeing if they are anagrams of each other. The array passed through in the Body of the request are indeed anagrams in this test, so it should return true.
  * *CheckSetForAnagramsFalse* - Same as above, except the words included in the body are not anagrams. Should return false.
  * *CheckSetForAnagramsDifferentLengths* - Same as above but the JSON array of words passed in the body are all different lengths. Wrote this one to be an early detector of anagrams that can return false early before having to actually interact with the collection much.
* PostTests
  * *PostWord* - Tests the Insert functionality by passing a JSON array of words and adding them to the Corpus. I suggest testing this after testing *DeleteWordAndAnagrams* or *DeleteCorpus*. 
*DeleteTests
  * *DeleteWord* - Deletes a single word from the Corpus. Can test if it worked using *TestDoesContain* after running this.
  * *DeleteWordAndAnagrams* Deletes the word passed as parameter and all its anagrams. Can test if it worked using *TestDoesContain* after running this.
  * *DeleteCorpus* - Deletes all words from the Corpus. Can test if it worked using *TestDoesContain* after running this.

### Final Thoughts

Currently the app has no real data store. I could have created a simple database using something like Entity Framework Core but I chose not to due to the fact that including it would be increasing the complexity of the up and running section a bit too far. I wanted to keep this as layman as possible, not knowing the background of whoever is looking at this. 

If the app did include data-permanence, like if deleting the entire Corpus did, in fact, irretrievably delete all the contents of the Corpus, I would certainly want some sort of User-Authentication and Authorization service. 

[Back to Top](/README.md#Anagrammer)
