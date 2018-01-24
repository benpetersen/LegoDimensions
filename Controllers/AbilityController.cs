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

    [Route("api/ability")]
    public class AbilityController : Controller
    {
        private readonly LegoDimensionsContext _context;

        public AbilityController(LegoDimensionsContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetAbilitiesAndCharacters(long id)
        {

            var purchasedPacks = _context.Packs
                .Where(p => p.Characters.Any(c => c.IsPurchased == true ));

            foreach(Pack pack in purchasedPacks)
            {
                /*
                iterate through each owned pack, each character and add to AbilitiesOwned Character list
                
                    EX:
                    AbilityName: 'Acrobat',
                    OwnedCharacterWithAbility: Wildstyle, Chell, Gollum
                 */
                 
                 //TODO: Build out list as an endpoint to call using CharactersWithAbilities.cs


                // var characters = pack.Characters.ToList();
                // foreach(Character character in characters)
                // {
                //     var abilities = character.Abilities.ToList();
                //     foreach(Ability ability in abilities)
                //     {
                //         Ability currentAbility = ability;
                //         Character currentCharacter = character;

                //         //may want to create a finished object before submitting to db/context
                //         CharacterAbilities stuff = new CharacterAbilities();
                //         stuff.AbilityName = currentAbility.Name;

                //         //Get ability set
                //         var contextAbility = _context.CharacterAbilities.Where(a => a.AbilityName == currentAbility.Name).FirstOrDefault();

                        
                        
                //         if(contextAbility != null){
                //             contextAbility.Characters.Add(
                //                 currentCharacter.Name
                //             );
                //         }else{
                //             stuff.Characters.Add(currentCharacter.Name);

                //             _context.CharacterAbilities.Add(
                //                 stuff
                //             );
                //         }
                //     }
                // }
            }

            _context.SaveChanges();


            if(purchasedPacks == null){
                return NotFound();
            }
            return new ObjectResult(purchasedPacks);
        }

    }

}