using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using LegoDimensions.Models;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace LegoDimensions.Controllers
{
    /*
        MVP
        - API sends list of packs
        - Razor Page - Shows entire list and asks which packs they've purchased
            -  API Update - Update IsPurchased to characters list
        - API GET - send list of abilities and characters with it
        - API 
        - Search and show characters I own (or don't own)
        - Search and show abilities I own (or don't own)
        - Show gifs using http://lego-dimensions.wikia.com/wiki/List_of_Abilities#Common Abilities

    */

    [Route("api/charactersWithAbility")]
    public class AbilityController : Controller
    {
        private readonly LegoDimensionsContext _context;

        public AbilityController(LegoDimensionsContext context)
        {
            _context = context;
        }
        [Route("{ability}")]
        public IActionResult GetPurchasedAbilitiesAndCharacters(string ability)
        {
            //Return purchased characters with a specific ability
            PurchasedAbilities data = _context.PurchasedAbilities.Where(a => a.AbilityName == ability).FirstOrDefault();
            var apiResult = new ApiResult(data.CharacterList, ability);
            return new ObjectResult(apiResult);
        }

    }

    public class ApiResult
    {
        public ApiResult(){}
        public ApiResult(List<string> characters, string searchTerm){
            Characters = characters;
            SearchTerm = searchTerm;
        }
        public string SearchTerm { get; set; }
        public List<string> Characters { get; set; }
    }

}