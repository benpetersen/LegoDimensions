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
			if(_context.Packs.Count() == 0)
			{
				List<Pack> packs = LoadPacks();
				packs.ForEach(p => _context.Packs.Add(p));
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
			}
			//Add new abilities to Abilities

			_context.Packs.Update(pack);
			_context.SaveChanges();
			return new NoContentResult();
		}

		private void AddPurchasedCharactersToAbilityList(Pack pack)
		{
			//Get Character abilities using character.ID (foreach inside Packs)
			//Add packs characters to ability list using pack.ID

			var characters = pack.Characters.ToList();
			foreach(Character character in characters){
				//Match character.ID to CharacterAbility.ID (characterAbilities.json), which gets abilities for each character
				//Foreach ability -  .Where(character.abilityName == ability)
				//Add character name and ID to AbilityList

				var contextAbility = _context.OwnedCharactersWithAbility.Where(a => a.AbilityName == currentAbility.Name).FirstOrDefault();
				if(contextAbility != null){
					contextAbility.Characters.Add(
						character.ID
						character.Name
					);
				}
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