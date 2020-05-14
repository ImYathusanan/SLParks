using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SLParkApi.Models;
using SLParkApi.Models.Dtos;
using SLParkApi.Repository;
using SLParkApi.Repository.IRepository;

namespace SLParkApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[ApiExplorerSettings(GroupName = "ParkOpenApiSpecNP")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public class NationalParksController : ControllerBase
    {
        private readonly INationalParkRepository _nationalParkRepository;

        private readonly IMapper _mapper;

        public NationalParksController(INationalParkRepository nationalParkRepository, IMapper mapper)
        {
            _nationalParkRepository = nationalParkRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Get list of All National Parks
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<NationalParkDto>))]
        public IActionResult GetNationalParks()
        {
            var nationaParks = _nationalParkRepository.GetNationalParks();

            var nationalParkDto = new List<NationalParkDto>();

            foreach (var park in nationaParks)
                nationalParkDto.Add(_mapper.Map<NationalParkDto>(park));

            return Ok(nationalParkDto);
        }

        /// <summary>
        /// Get individual national Park
        /// </summary>
        /// <param name="nationalParkId">The Id Of the National Park</param>
        /// <returns></returns>

        [HttpGet("{nationalParkId:int}", Name = "GetNationalPark")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(NationalParkDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetNationalPark(int nationalParkId)
        {
            var nationalPark = _nationalParkRepository.GetNationalPark(nationalParkId);

            if (nationalPark == null)
                return NotFound();

            var nationalParkDto = _mapper.Map<NationalParkDto>(nationalPark);

            return Ok(nationalParkDto);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(NationalParkDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult CreateNationalPark([FromBody] NationalParkDto nationalParkDto)
        {
            if (nationalParkDto == null)
                return BadRequest(ModelState);

            if(_nationalParkRepository.NationalParkExists(nationalParkDto.Name))
            {
                ModelState.AddModelError("", "National Park Already Exists!");
                return StatusCode(404, ModelState);
            }


            var nationalPark = _mapper.Map<NationalPark>(nationalParkDto);

            if(!_nationalParkRepository.CreateNationalPark(nationalPark))
            {
                ModelState.AddModelError("", $"Something went wrong when saving the record {nationalPark.Name}");
                return StatusCode(500, ModelState);
            }

            return CreatedAtRoute("GetNationalPark", new { nationalParkId = nationalPark.Id}, nationalPark);
        }

        [HttpPatch("{nationalParkId:int}", Name = "UpdateNationalPark")]
        public IActionResult UpdateNationalPark(int nationalParkId, [FromBody] NationalParkDto nationalParkDto)
        {
            if (nationalParkDto == null || nationalParkId != nationalParkDto.Id)
                return BadRequest(ModelState);

            var nationalPark = _mapper.Map<NationalPark>(nationalParkDto);

            if(!_nationalParkRepository.UpdateNationalPark(nationalPark))
            {
                ModelState.AddModelError("", $"Something went wrong when ipdate the {nationalPark.Name}");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [HttpDelete("{nationalParkId:int}", Name = "DeleteNationalPark")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult DeleteNationaPark(int nationalParkId)
        {
            if (!_nationalParkRepository.NationalParkExists(nationalParkId))
                return NotFound();

            var nationalPark = _nationalParkRepository.GetNationalPark(nationalParkId);

            if(!_nationalParkRepository.DeleteNationalPark(nationalPark))
            {
                ModelState.AddModelError("", $"Something went wrong when Delete the {nationalPark.Name}");
                return StatusCode(500, ModelState);
            }

            return NoContent();

        }
    }
}