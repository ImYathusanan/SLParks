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
    public class NationalParksVersion2Controller : ControllerBase
    {
        private readonly INationalParkRepository _nationalParkRepository;

        private readonly IMapper _mapper;

        public NationalParksVersion2Controller(INationalParkRepository nationalParkRepository, IMapper mapper)
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

    }
}