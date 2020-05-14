using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SLParkApi.Models;
using SLParkApi.Models.Dtos;
using SLParkApi.Repository.IRepository;

namespace SLParkApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[ApiExplorerSettings(GroupName = "ParkOpenApiSpecTrial")]
    public class TrailsController : ControllerBase
    {
        private readonly ITrailRepository _trailRepository;

        private readonly IMapper _mapper;

        public TrailsController(ITrailRepository trailRepository, IMapper mapper)
        {
            _trailRepository = trailRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Get list of All National Parks
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<TrialDto>))]
        public IActionResult GetTrails()
        {
            var nationaParks = _trailRepository.GetTrails();

            var trailDto = new List<TrialDto>();

            foreach (var park in nationaParks)
                trailDto.Add(_mapper.Map<TrialDto>(park));

            return Ok(trailDto);
        }

        /// <summary>
        /// Get individual national Park
        /// </summary>
        /// <param name="trailId">The Id Of the National Park</param>
        /// <returns></returns>

        [HttpGet("{trailId:int}", Name = "GetTrail")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TrialDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetTrail(int trailId)
        {
            var trail = _trailRepository.GetTrail(trailId);

            if (trail == null)
                return NotFound();

            var trailDto = _mapper.Map<TrialDto>(trail);

            return Ok(trailDto);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(TrialDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult CreateTrail([FromBody] TrailCreateDto trailDto)
        {
            if (trailDto == null)
                return BadRequest(ModelState);

            if (_trailRepository.TrailExists(trailDto.Name))
            {
                ModelState.AddModelError("", "National Park Already Exists!");
                return StatusCode(404, ModelState);
            }


            var trail = _mapper.Map<Trail>(trailDto);

            if (!_trailRepository.CreateTrail(trail))
            {
                ModelState.AddModelError("", $"Something went wrong when saving the record {trail.Name}");
                return StatusCode(500, ModelState);
            }

            return CreatedAtRoute("GetTrail", new { trailId = trail.Id }, trail);
        }

        [HttpPatch("{trailId:int}", Name = "UpdateTrail")]
        public IActionResult UpdateTrail(int trailId, [FromBody] TrailUpdateDto trailDto)
        {
            if (trailDto == null || trailId != trailDto.Id)
                return BadRequest(ModelState);

            var trail = _mapper.Map<Trail>(trailDto);

            if (!_trailRepository.UpdateTrail(trail))
            {
                ModelState.AddModelError("", $"Something went wrong when ipdate the {trail.Name}");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [HttpDelete("{trailId:int}", Name = "DeleteTrail")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult DeleteNationaPark(int trailId)
        {
            if (!_trailRepository.TrailExists(trailId))
                return NotFound();

            var trail = _trailRepository.GetTrail(trailId);

            if (!_trailRepository.DeleteTrail(trail))
            {
                ModelState.AddModelError("", $"Something went wrong when Delete the {trail.Name}");
                return StatusCode(500, ModelState);
            }

            return NoContent();

        }
    }
}