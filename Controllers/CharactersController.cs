using DisneyAPI.Models;
using DisneyAPI.Services;
using DisneyAPI.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DisneyAPI.Controllers
{
    [ApiController]
    [Route("/characters")]
    [Authorize]
    public class CharactersController : Controller
    {
        private readonly ICharactersService _service;
        public CharactersController(ICharactersService service)
        {
            _service = service;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Get()
        {
            var characters = await _service.Get();
            return Ok(characters);
        }

        [HttpGet("{Id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetCharacter(int Id)
        {
            if (Id == 0)
                return BadRequest(new Response {
                    Status = "Error",
                    Message = "Character invalid"
                });

            var character = await _service.GetById(Id);

            if (character == null)
                return NotFound();

            return Ok(character);
        }

        [HttpGet("age")]
        [AllowAnonymous]
        public async Task<IActionResult> GetByAge([FromQuery] int age, double weight)
        {
            if (age == 0)
                return BadRequest(new Response { 
                    Status = "Error",
                    Message = "Invalid age"
                });

            var character = await _service.GetByAgeOrWeight(age, weight);

            if (character == null)
                return NotFound();

            return Ok(character);
        }

        [HttpGet("name")]
        [AllowAnonymous]
        public async Task<IActionResult> GetByName(string name)
        {
            if (string.IsNullOrEmpty(name))
                return BadRequest(new Response { Status = "Error", Message = "Invalid name" });

            var character = await _service.GetByName(name);

            if (character == null)
                return NotFound();

            return Ok(character);
        }

        [HttpGet("movieId")]
        [AllowAnonymous]
        public async Task<IActionResult> GetByMovie(int movieId)
        {
            if (movieId == 0)
                return BadRequest("Ingrese su película o serie.");

            return Ok(await _service.GetByMovie(movieId));
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddCharacter character)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _service.Add(character);

            return Created($"https://localhost:7000/characters/{result.CharacterId}", null);
        }

        [HttpDelete("{Id}")]
        public async Task<IActionResult> Delete(int Id)
        {
            if (Id <= 0)
                return BadRequest(new Response { Status = "Error", Message = "Invalid character" });

            var result = await _service.Delete(Id);

            if (!result)
                return NotFound();

            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> Update(UpdateCharacter character)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var result = await _service.Update(character);

            if (result == null)
                return NotFound();

            return Ok(result);
        }
    }
}
