using api.premium.calculator.Common;
using api.premium.calculator.Models;
using api.premium.calculator.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.premium.calculator.Controllers
{
    [Route("api/calculator")]
    [ApiController]
    public class CalculatorController : Controller
    {
        private readonly ICalculatorServices _service;
        public CalculatorController(ICalculatorServices services)
        {
            _service = services;
        }

        [HttpPost]
        [Route("calculate")]
        [ProducesResponseType(typeof(ApiResult<Decimal>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult Calculate([FromBody] InsuranceDetails insuranceDetails)
        {
            var response = _service.Calculate(insuranceDetails);
            if ((response?.Status ?? Constants.Error) == Constants.Error)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
            return response.Status == Constants.Fail ? BadRequest(response) : Ok(response);
        }

        [HttpGet]
        [Route("occupations")]
        [ProducesResponseType(typeof(ApiResult<Occupation>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GetOccupations()
        {
            var response = _service.GetOccupations();
            return response?.Data == null || response.Data.Count == 0 ? NoContent() : Ok(response);
        }
    }
}
