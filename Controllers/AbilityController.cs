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
        private readonly CharactersWithAbilityContext _context;
        private readonly PackContext _packContext;

        public AbilityController(CharactersWithAbilityContext context, PackContext packContext)
        {
            _context = context;
            _packContext = packContext;
        }

        [HttpGet]
        public IActionResult GetAbilitiesAndCharacters(long id)
        {
            //filter on purchased packs and return character abilities
            var purchasedPacks = _packContext.Packs.Where(p => p.IsPurchased == true );
            
            //List<CharactersWithAbility> ownedCharactersWithAbility = new List<CharactersWithAbility>();

            foreach(Pack pack in purchasedPacks)
            {
                /*
                iterate through each pack, each character and add to AbilitiesOwned Character list
                
                    EX:
                    AbilityName: 'Acrobat',
                    OwnedCharacterWithAbility: Wildstyle, Chell, Gollum
                 */
                 //ToList'ing ICollection to get access to foreach's current and next
                var characters = pack.Characters.ToList();
                foreach(Character character in characters)
                {
                    var abilities = character.Abilities.ToList();
                    foreach(Ability ability in abilities)
                    {
                        Ability currentAbility = ability;
                        Character currentCharacter = character;

                        //may want to create a finished object before submitting to db/context
                        CharactersWithAbility stuff = new CharactersWithAbility();
                        stuff.AbilityName = currentAbility.Name;

                        //Get ability set
                        var contextAbility = _context.OwnedCharactersWithAbility.Where(a => a.AbilityName == currentAbility.Name).FirstOrDefault();

                        
                        
                        if(contextAbility != null){
                            contextAbility.Characters.Add(
                                currentCharacter.Name
                            );
                        }else{
                            stuff.Characters.Add(currentCharacter.Name);

                            _context.OwnedCharactersWithAbility.Add(
                                stuff
                            );
                        }
                    }
                }
            }

            _context.SaveChanges();


            if(purchasedPacks == null){
                return NotFound();
            }
            return new ObjectResult(purchasedPacks);
        }

    }

}