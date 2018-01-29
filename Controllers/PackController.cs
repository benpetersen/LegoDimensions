using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using LegoDimensions.Models;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;

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

			if(_context.Packs.Count() == 0)
			{
				LoadPacks();
			}
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

		/// <summary>
		/// Returns all packs
		/// </summary>
		/// <returns> A JSON list of all lego dimensions packs.!-- </returns>
		[Route("all")]
		[HttpGet]
		public async Task<IQueryable<Pack>> GetAllPacksAsync()
		{
			var myTask = Task.Run( () => _context.Packs);
			IQueryable<Pack> packs = await myTask;
			return packs;
		}

		[Route("update/{id}")]
		public IActionResult UpdatePackPurchased(int id)
		{
			//Pack is purchased, set each character as owned.
			//Add abilities to owned list
			
			if (id < 0 || id > 150)
			{
				return BadRequest();
			}

			var pack = _context.Packs.FirstOrDefault(t => t.PackID == id);
			if (pack == null)
			{
				return NotFound();
			}

			foreach (int characterID in pack.CharacterList){
				var characterAbility = _context.CharacterAbilities.FirstOrDefault(c => c.CharacterID == characterID);
				characterAbility.IsPurchased = true;
				AddPurchasedCharactersToAbilityList(characterAbility);
			}
			_context.SaveChanges();
			return new NoContentResult();
		}

		private void AddPurchasedCharactersToAbilityList(CharacterAbilities _character)
		{
			//Add character abilities to owned ability list
			_character.IsPurchased = true;

			foreach(string ability in _character.AbilityList)
			{
				//Add a character associated to an ability if it's available
				var purchased = _context.PurchasedAbilities.Where(a => a.AbilityName == ability).FirstOrDefault();
				
				if(purchased == null)
				{
					//Does not own ability - insert ability and characters with that ability to list. It's similar to sql pivot
					var newlyPurchased = new PurchasedAbilities();
					newlyPurchased.CharacterID = _character.CharacterID;
					newlyPurchased.AbilityName = ability;
					newlyPurchased.Characters = _character.CharacterName;

					_context.PurchasedAbilities.Add(
						newlyPurchased
					);
				}else if(!purchased.Characters.Contains(_character.CharacterName))
				{
					//Owns ability and Character isn't in list already - Update database
					_context.PurchasedAbilities.Attach(purchased);
					var entry = _context.Entry(purchased);
					entry.Property(p => p.Characters).IsModified = true;
					purchased.Characters = purchased.Characters + "," + _character.CharacterName;
					_context.SaveChanges();
				}
			}
		}

	}

}