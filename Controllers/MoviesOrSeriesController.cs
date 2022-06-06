using DisneyAPI.Models;
using DisneyAPI.Services;
using DisneyAPI.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DisneyAPI.Controllers
{
    [ApiController]
    [Route("movies")]
    [Authorize]
    public class MoviesOrSeriesController : Controller
    {
        private readonly IMoviesOrSeriesService _service;
        public MoviesOrSeriesController(IMoviesOrSeriesService service)
        {
            _service = service;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Get()
        {
            var moviesOrSeries = await _service.Get();
            return Ok(moviesOrSeries);
        }

        [HttpGet("{Id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetMovieOrSerie(int Id)
        {
            var movieOrSerie = await _service.GetById(Id);

            if (movieOrSerie == null)
                return NotFound();

            return Ok(movieOrSerie);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] AddMovieOrSerie movieOrSerie) 
        {
            if(!ModelState.IsValid)
                return BadRequest();

            var result = await _service.Add(movieOrSerie.ToMovieOrSerieModel());

            return Created($"https://localhost:7000/movies/{result.MovieOrSerieId}", null);
        }

        [HttpGet("genre")]
        [AllowAnonymous]
        public async Task<IActionResult> GetByGenre(int genre)
        {
            if(genre <= 0)
                return BadRequest(new Response { Status = "Error", Message = "Invalid Genre" });

            var moviesOrSeries = await _service.GetByGenre(genre);

            if (moviesOrSeries == null)
                return NotFound();

            return Ok(moviesOrSeries);
        }

        [HttpGet("order")]
        [AllowAnonymous]
        public async Task<IActionResult> GetOrdered(string order)
        {
            if (order == string.Empty)
                return BadRequest(new Response{ Status = "Error", Message = "Invalid entry" });

            order = order.ToUpper();

            return Ok(await _service.GetOrdered(order));
        }

        [HttpDelete("{Id}")]
        public async Task<IActionResult> Delete(int Id)
        {
            if (Id <= 0)
                return BadRequest(new Response { Status = "Error", Message = "Invalid movie or serie" });

            var result = await _service.Delete(Id);

            if (!result)
                return NotFound();

            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> Update(UpdateMovieOrSerie movieOrSerie)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var result = await _service.Update(movieOrSerie);

            if (result == null)
                return NotFound();

            return Ok(result);
        }
    }
}
