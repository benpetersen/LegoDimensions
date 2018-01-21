using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using LegoDimensions.Models;
using System.Linq;

namespace LegoDimensions.Controllers
{
    [Route("api/ability")]
    public class AbilityController : Controller
    {
        private readonly AbilityContext _context;
        
        public AbilityController(AbilityContext context)
        {
            _context = context;
            if(_context.Abilities.Count() == 0)
            {
                _context.Abilities.Add(new Ability { Name = "Item1"});
                _context.SaveChanges();
            }
        }
        [HttpGet]
        public IEnumerable<Ability> GetAll()
        {
            return _context.Abilities.ToList();
        }

        [HttpGet("{id}", Name = "GetAbility")]
        public IActionResult GetById(long id)
        {
            var item = _context.Abilities.FirstOrDefault(t => t.Id == id);
            if(item == null){
                return NotFound();
            }
            return new ObjectResult(item);
        }

        [HttpPost]
        public IActionResult Create([FromBody] Ability ability)
        {
            if(ability == null){
                return BadRequest();
            }
            _context.Abilities.Add(ability);
            _context.SaveChanges();

            return CreatedAtRoute("GetAbility", new { id = ability.Id }, ability);
        }

        [HttpPut("{id}")]
        public IActionResult Update(long id, [FromBody] Ability item)
        {
            if (item == null || item.Id != id)
            {
                return BadRequest();
            }

            var todo = _context.Abilities.FirstOrDefault(t => t.Id == id);
            if (todo == null)
            {
                return NotFound();
            }

            todo.Name = item.Name;

            _context.Abilities.Update(todo);
            _context.SaveChanges();
            return new NoContentResult();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(long id)
        {
            var ability = _context.Abilities.FirstOrDefault(t => t.Id == id);
            if (ability == null)
            {
                return NotFound();
            }

            _context.Abilities.Remove(ability);
            _context.SaveChanges();
            return new NoContentResult();
        }
    }

}