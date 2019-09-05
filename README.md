# Anagrammer

## Table of Contents
1. Overview
2. Discussion
3. Up and Running Guide
4. Suggested Test Path

---

# Completed Endpoints
- `POST /words.json`: Takes a JSON array of English-language words and adds them to the corpus (data store).

- `GET /anagrams/:word.json`:
  - Returns a JSON array of English-language words that are anagrams of the word passed in the URL.
  - This endpoint should support an optional query param that indicates the maximum number of results to return.
  - *BONUS OPTIONAL:* Respect a query param for whether or not to include proper nouns in the list of anagrams

- `DELETE /words/:word.json`: Deletes a single word from the data store.

- `DELETE /words.json`: Deletes all contents of the data store.

#Completed OPTIONAL Endpoints

- Endpoint to check if the Corpus already contains a word.

- Endpoint to delete a word *and all of its anagrams*

- Endpoint that returns a count of words in the corpus and min/max/median/average word length

- Endpoint that takes a set of words and returns whether or not they are all anagrams of each other


