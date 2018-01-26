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

	[Route("api/pack")]
	public class PackController : Controller
	{
		private readonly LegoDimensionsContext _context;
		
		public PackController(LegoDimensionsContext context)
		{
			context.ChangeTracker.AutoDetectChangesEnabled = false;
			_context = context;

			//Load Packs
			if(_context.Packs.Count() == 0)
			{
				LoadPacks();
			}
			//Load Character Abilities
			if(_context.CharacterAbilities.Count() == 0)
			{
				LoadCharacterAbilities();
			}
			//Load Abilities (to use as a definitive list to compare which ones don't I own)
			if(_context.Abilities.Count() == 0)
			{
				LoadAllAbilities();
			}
		}
		public void LoadPacks()
		{
			dynamic packs;
			//List<Pack> packs = new List<Pack>();
			using(StreamReader r = new StreamReader("packs.json"))
			{
				string json = r.ReadToEnd();
				packs = JsonConvert.DeserializeObject<List<Pack>>(json);
			}
			foreach (Pack pack in packs){
				_context.Packs.Add(pack);
			}
			_context.SaveChanges();
		}

		public void LoadCharacterAbilities()
		{
			dynamic characterAbilities;
			using(StreamReader r = new StreamReader("characterAbilities.json"))
			{
				string json = r.ReadToEnd();
				characterAbilities = JsonConvert.DeserializeObject<List<CharacterAbilities>>(json);
			}
			foreach (CharacterAbilities character in characterAbilities){
				_context.CharacterAbilities.Add(character);
			}
			_context.SaveChanges();
		}

		public void LoadAllAbilities()
		{
			using(StreamReader r = new StreamReader("abilities.json"))
			{
				string json = r.ReadToEnd();
				dynamic abilities = JsonConvert.DeserializeObject<List<Ability>>(json);

				foreach (Ability ability in abilities){
					_context.Abilities.Add(ability);
				}
				_context.SaveChanges();
			}
		}

		[HttpGet]
		public async Task<IQueryable<Pack>> GetAllPacksAsync()
		{
			var myTask = Task.Run( () => _context.Packs);
			IQueryable<Pack> packs = await myTask;
			return packs;
		}

		[HttpPut("{id}")]
		public IActionResult UpdatePackPurchased(int id, [FromBody] Pack item)
		{
			//Pack is purchased, set each character as owned.
			//Add abilities to owned list
			
			if (item == null || item.PackID != id)
			{
				return BadRequest();
			}

			var pack = _context.Packs.FirstOrDefault(t => t.PackID == id);
			if (pack == null)
			{
				return NotFound();
			}
			
			var characters = pack.Characters.ToList();
			foreach(Character character in characters)
			{
				character.IsPurchased = true;
				//TODO: assign character to pack if it's not by reference
				AddPurchasedCharactersToAbilityList(character);
			}
			//Add new abilities to Abilities

			_context.Packs.Update(pack);
			_context.SaveChanges();
			return new NoContentResult();
		}

		private void AddPurchasedCharactersToAbilityList(Character character)
		{
			//Add character abilities to owned ability list
			var _character = _context.CharacterAbilities.Where(a => a.Character.ID == character.ID).FirstOrDefault();
			_character.Character.IsPurchased = true;

			var abilities = _character.Abilities.ToList();
			foreach(Ability ability in abilities){
				//Add a character associated to an ability if it's available
				var purchased = _context.PurchasedAbilities.Where(a => a.Ability.Name == ability.Name).FirstOrDefault();
				if(purchased != null){
					purchased.Characters.Add(character);
				}else{
					//for each of the character abilites that aren't in the list, add it
					var newlyPurchased = new PurchasedAbilities(
						ability,
						character
					);
					_context.PurchasedAbilities.Add(
						newlyPurchased
					);
				}
				
			}

			// if(contextAbility != null){
			// 	contextAbility.Characters.Add(
			// 		character.ID
			// 		character.Name
			// 	);
			// }else{
			// 	stuff.Characters.Add(currentCharacter.Name);

			// 	_context.OwnedCharactersWithAbility.Add(
			// 		stuff
			// 	);
			// }
		}







		// [HttpPost]
		// public IActionResult Create([FromBody] Ability ability)
		// {
		//     if(ability == null){
		//         return BadRequest();
		//     }
		//     _context.Abilities.Add(ability);
		//     _context.SaveChanges();

		//     return CreatedAtRoute("GetAbility", new { id = ability.Id }, ability);
		// }

		// [HttpDelete("{id}")]
		// public IActionResult Delete(long id)
		// {
		//     var ability = _context.Abilities.FirstOrDefault(t => t.Id == id);
		//     if (ability == null)
		//     {
		//         return NotFound();
		//     }

		//     _context.Abilities.Remove(ability);
		//     _context.SaveChanges();
		//     return new NoContentResult();
		// }
	}

}