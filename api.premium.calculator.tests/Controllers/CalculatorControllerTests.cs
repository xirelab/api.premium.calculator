using api.premium.calculator.Common;
using api.premium.calculator.Controllers;
using api.premium.calculator.Models;
using api.premium.calculator.Services;
using api.premium.calculator.tests.Common;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using System;
using System.Collections.Generic;
using System.Net;
using Xunit;

namespace api.premium.calculator.tests.Controllers
{
    public class CalculatorControllerTests
    {
        private readonly CalculatorController _controller;
        private readonly ICalculatorServices _service;

        public CalculatorControllerTests()
        {
            _service = Substitute.For<ICalculatorServices>();
            _controller = new CalculatorController(_service);
        }

        [Fact]
        public void ShouldReturnNotFound_When_OccupationsNotFound()
        {
            // Arrange
            _service.GetOccupations().ReturnsNull();

            // Act
            var response = _controller.GetOccupations();

            // Assert
            response.Should().NotBeNull();
            response.Should().BeOfType<NoContentResult>();
            ((NoContentResult)response).StatusCode.Should().Be(Convert.ToInt32(HttpStatusCode.NoContent));
        }

        [Fact]
        public void ShouldReturnData_When_OccupationsFound()
        {
            // Arrange
            _service.GetOccupations().Returns(new ApiResult<List<Occupation>>
            {
                Status = Constants.Success,
                Data = MockedConstants.SampleOccupations
            });

            // Act
            var response = _controller.GetOccupations();

            // Assert
            response.Should().NotBeNull();
            response.Should().BeOfType<OkObjectResult>();
            var result = response as OkObjectResult;
            result.StatusCode.Should().Be(Convert.ToInt32(HttpStatusCode.OK));
            result.Value.Should().NotBeNull();
            result.Value.Should().BeOfType<ApiResult<List<Occupation>>>();
        }

        [Fact]
        public void ShouldReturnPremiumAount_When_Calculated()
        {
            // Arrange
            _service.Calculate(Arg.Any<InsuranceDetails>()).Returns(new ApiResult<decimal>
            {
                Status = Constants.Success,
                Data = 1000
            });

            // Act
            var response = _controller.Calculate(MockedConstants.mockedInsuranceDetails);

            // Assert
            response.Should().NotBeNull();
            response.Should().BeOfType<OkObjectResult>();
            var result = response as OkObjectResult;
            result.StatusCode.Should().Be(Convert.ToInt32(HttpStatusCode.OK));
            result.Value.Should().NotBeNull();
            result.Value.Should().BeOfType<ApiResult<decimal>>();
        }

        [Fact]
        public void ShouldReturnError500_When_FileReadingFailed()
        {
            // Arrange
            _service.Calculate(Arg.Any<InsuranceDetails>()).Returns(new ApiResult<decimal>
            {
                Status = Constants.Error
            });

            // Act
            var response = _controller.Calculate(MockedConstants.mockedInsuranceDetails);

            // Assert
            response.Should().NotBeNull();
            response.Should().BeOfType<ObjectResult>();
            ((ObjectResult)response).StatusCode.Should().Be(Convert.ToInt32(HttpStatusCode.InternalServerError));
        }

        [Fact]
        public void ShouldReturnBadRequest_When_CalculationFailed()
        {
            // Arrange
            _service.Calculate(Arg.Any<InsuranceDetails>()).Returns(new ApiResult<decimal>
            {
                Status = Constants.Fail
            });

            // Act
            var response = _controller.Calculate(MockedConstants.mockedInsuranceDetails);

            // Assert
            response.Should().NotBeNull();
            response.Should().BeOfType<BadRequestObjectResult>();
            ((BadRequestObjectResult)response).StatusCode.Should().Be(Convert.ToInt32(HttpStatusCode.BadRequest));            
        }
    }
}
