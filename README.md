# Anagrammer

## Table of Contents
1. Overview & Discussion
3. Up and Running Guide
4. Suggested Test Path

---
## Overview & Discussion

### Completed Endpoints
- `POST /words.json`: Takes a JSON array of English-language words and adds them to the corpus (data store).
- `GET /anagrams/:word.json`:
  - Returns a JSON array of English-language words that are anagrams of the word passed in the URL.
  - This endpoint should support an optional query param that indicates the maximum number of results to return.
  - *BONUS FROM OPTIONAL LIST:* Respect a query param for whether or not to include proper nouns in the list of anagrams
- `DELETE /words/:word.json`: Deletes a single word from the data store.
- `DELETE /words.json`: Deletes all contents of the data store.

### Completed OPTIONAL Endpoints
- Endpoint to check if the Corpus already contains a word.
- Endpoint to delete a word *and all of its anagrams*
- Endpoint that returns a count of words in the corpus and min/max/median/average word length
- Endpoint that takes a set of words and returns whether or not they are all anagrams of each other

I chose to write the API in the latest version of dotnet core, 2.2. I chose dotnet core because 1- I have a strong background in .net and only windows machine to develop on and 2- I know that Ibotta is Mac organization, so the cross-platform capability of dotnet core was a must have.

The solution consists of two projects, the first being the API itself (Anagram.API) and the second a Unit Test project that I created to help me in my developement process (Anagrammer.Tests). I should note here, the Test project focuses on the heavy lifter class of the API, where most of the functonality is located and ignores the  API Controller that exposes the endpoints. I left the endpoint testing to a tool called Postman that I find very useful for working with APIs. I chose to use Postman for this because it allows me the ability to export/import entire test suites for sharing. You will find the file to import these tests alongside the projectfolders for the API and Unit Tests, as well as this README.md (more on Postman and importing later).

Now onto an overview of the API project structure. Inside the Anagram.API folder, you will find several folders and files. I will keep my discussion to only those items that are noteworthy:

* *Program.cs* - the main entrypoint for the application.
* *Startup.cs* - Where we can inject any services the application might depend on. dotnet core has a Dependency Injection container built right into the framework, making it super slick and easy to swap services in and out. Probably one of my favorite features. /endrant.
* *dictionary.txt* - is the file provided for the exercise. I chose to import it on startup, meaning that when first firing up the API, you will not need to insert any words to begin testing out endpoints. And as I write this, I realize that I forgot to finish the functionality to save back to this file to persist changes to the dictionary across sessions. I think it will leave it for a future feature, at this point. So go ahead and delete the entire Corpus when testing. It will be there when you restart the service.
* *Controllers folder* - Where the AnagramsController file lives, that exposes all the endpoints of the API
  + *AnagramController* - Gets the Anagrammer class injected in it's constructor. All endpoints are written with a "bouncer" or "blocker" pattern, to filter out bad or invalid content before passing it on to the Anagrammer class. Exposes Endpoints:
    - GetAnagrams (HttpGet("api/anagrams/{word}")) Takes an string for parameter and returns a number of anagrams for that word. Optional parameters include 'limit' to limit the size of the collection returned and returnProperNouns to configure whether or not you want words starting with a capital letter included in the return collection. Sucess Returns 200 Ok
    - Contains (HttpGet("api/corpus/contains/{word}")) Takes a string parameter and returns true/false if the Corpus already contains it. Not a requirement in the project parameters, but I found it was nice functionality to have while developing and decided to expose it. Found returns 200 Ok response, not found returns 404 Not Found
    - InsertWords (HttpPost("api/corpus/insertwords")) Takes a Json Array of words and inserts them into the Corpus *if they do not alread exist*. Returns 201 Created Response.
    - DeleteWord (HttpDelete("api/corpus/delete/{word}")) Takes a string parameter and deletes the word if the Corpus contains it. On success, returns 204 No Content, on failure returns 404 Not Found.
    - DeleteCorpus (HttpDelete("api/corpus/deletecorpus")) Deletes all the contents of the Corpus. On success, returns 204 No Content, on failure returns 404 Not Found (in the case that its already been deleted).
    - DeleteWordAndAnagrams (HttpDelete("api/anagrams/deleteanagrams/{word}")) Takes a string parameter and deletes the word and all anagrams of the word. On success, returns 204 No Content, on failure returns 404 Not Found.
    - GetStats (HttpGet("api/corpus/getstats")) Returns a Json Array with the total word count, min and max lengths of all the words in the Corpus and the Median and Average calculation of the words. Returns 200 Ok.
    - CheckForAnagrams (HttpGet("api/anagrams/checkforanagrams")) Takes a Json Array of words and tests if they are anagrams of each other. Returns 200 Ok with a True or False value in the body of the response.




