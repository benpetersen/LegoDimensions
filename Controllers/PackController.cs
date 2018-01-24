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
		private readonly PackContext _context;
		
		public PackController(PackContext context)
		{
			_context = context;
			//Load Packs
			if(_context.Packs.Count() == 0)
			{
				List<Pack> packs = LoadPacks();
				packs.ForEach(p => _context.Packs.Add(p));
				_context.SaveChanges();
			}
			//Load Character Abilities
			if(_context.CharacterAbilities.Count() == 0)
			{
				List<CharacterAbilities> characterAbilities = LoadCharacterAbilities();
				characterAbilities.ForEach(c => _context.CharacterAbilities.Add(c));
				_context.SaveChanges();
			}
			//Load Abilities (to use as a definitive list, which ones don't I own)
			if(_context.Abilities.Count() == 0)
			{
				List<Abilities> abilities = LoadAllAbilities();
				abilities.ForEach(c => _context.Abilities.Add(c));
				_context.SaveChanges();
			}
		}
		public List<Pack> LoadPacks()
		{
			List<Pack> packs = new List<Pack>();
			using(StreamReader r = new StreamReader("packs.json"))
			{
				string json = r.ReadToEnd();
				packs = JsonConvert.DeserializeObject<List<Pack>>(json);
			}
			return packs;
		}

		public List<CharacterAbilities> LoadCharacterAbilities()
		{
			List<Pack> packs = new List<Pack>();
			using(StreamReader r = new StreamReader("characterAbilities.json"))
			{
				string json = r.ReadToEnd();
				characterAbilities = JsonConvert.DeserializeObject<List<CharacterAbilities>>(json);
			}
			return characterAbilities;
		}

		public List<Ability> LoadAllAbilities()
		{
			List<Pack> packs = new List<Pack>();
			using(StreamReader r = new StreamReader("abilities.json"))
			{
				string json = r.ReadToEnd();
				characterAbilities = JsonConvert.DeserializeObject<List<Ability>>(json);
			}
			return characterAbilities;
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
			//
			if (item == null || item.ID != id)
			{
				return BadRequest();
			}

			var pack = _context.Packs.FirstOrDefault(t => t.ID == id);
			if (pack == null)
			{
				return NotFound();
			}
			
			var characters = pack.Characters.ToList();
			foreach(Character character in characters)
			{
				character.IsPurchased = item.IsPurchased;
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
			//Get Character abilities using character.ID (foreach inside Packs)
			//Add packs characters to ability list using pack.ID

			//Match character.ID to CharacterAbility.ID (characterAbilities.json), which gets abilities for each character
			//Foreach ability -  .Where(character.abilityName == ability)
			//Add character name and ID to AbilityList
			
			var characterAbilities = _context.CharacterAbilities.Where(a => a.ID == character.ID).FirstOrDefault();

			var abilities = characterAbilities.Abilities.ToList();
			foreach(Ability ability in abilities){
				//Abilities aren't mapped by ID
			}

			if(contextAbility != null){
				contextAbility.Characters.Add(
					character.ID
					character.Name
				);
			}
			
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