# Lego Dimensions API

Searves up API endpoints for Lego Dimensions to assist searching for characters they own with specific abilities. This was created because the game has more than 80 characters, each with unique abilities and it is difficult for the r/LegoDimensions community to know which characters have which abilities.

### Prerequisites

.NET Core 2.0
Visual Studio Code
Postman (to verify endpoints)

### Getting Started

Pull down the project and run in Visual Studio

### Endpoint explanations
* /api/pack/all - Gets all packs (as of Wave 7.5)
* /api/pack/update/{packNumber} - Updates Pack - {packNumber} as Purchased and adds the abilities for each character in that pack to a list. Currently no success response is returned
* /api/charactersWithAbility/{AbilityName} - Gets characters with that ability
### Quick code explanation
* Models
```
CharacterAbilities.cs - Holds the Characters and a string representation of abilities (from packs.json). Also has a List of Abilities to easier filter
```

```
PurchasedAbilities.cs - Holds the purchased abilities and the characters with that ability
```

```
Ability.cs - Holds the abilities - to be used later to show which abilities needed to complete the game
```

```
LegoDimensionsContext.cs - Holds the datasets in memory
```

* Controllers
```
AbilityController.cs - /api/charactersWithAbility/{AbilityName} Gets purchased abilities an array of characters
```

```
PackController.cs - /pack/all and /pack/update/{packNumber} - Gets or Updates Packs as purchased
```

* API Requests 
Query string param was used to identify which resource to post or patch and pass arguments. I did this on purpose and probably isn't 'Best Practices' but for a side project that isn't exposed to a front-end or in acceptance, it's okay for now.

## Running the tests
In the solution, find "Lego Dimensions.postman_collection.json"
Open Postman and Import
Run these Postman requests by clicking "Send"
Get All Packs (optional) - GET - http://localhost:5000/api/pack/all
Purchased Starter Pack - PUT - http://localhost:5000/api/pack/update/2
Purchased Portal Pack - PUT - http://localhost:5000/api/pack/update/4
Get Characters with Acrobat - GET - http://localhost:5000/api/charactersWithAbility/Acrobat

These tests show that by buying the Lego Dimensions Start Park and the Portal Level Pack, you will have two characters with Acrobat. The response from "Get Characters with Acrobat" is below

{
    "searchTerm": "Acrobat",
    "characters": [
        "Wildstyle",
        "Chell"
    ]
}

## Languages it's built with

* ASP.NET Core 2.0
* Entity Framework

## Limitations

* Not inner-joining json responses or adding Entity Framework's foreign keys, connecting the two instead of for loops
* By using two frameworks/languages that I didn't have experience with (hence the early learning curve commits)
* By relying on .JSON files as my database (which helped limit scope and reduce debugging time)
* Steps that are required to show owned characters with abilities, every time the project is ran. (see limitation above)

## Authors
* [Ben Petersen](https://github.com/benpetersen)