using api.premium.calculator.Controllers;
using api.premium.calculator.Models;
using api.premium.calculator.Services;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using System;
using System.Collections.Generic;
using System.Net;
using Xunit;

namespace api.premium.Users.tests.Controllers
{
    public class UsersControllerTests
    {
        private readonly UsersController _controller;
        private readonly IUserService _service;

        public UsersControllerTests()
        {
            _service = Substitute.For<IUserService>();
            _controller = new UsersController(_service);
        }

        [Fact]
        public void ShouldReturnBadRequest_When_AuthenticationFailed()
        {
            // Arrange
            _service.Authenticate(Arg.Any<AuthenticateRequest>()).ReturnsNull();

            // Act
            var response = _controller.Authenticate(new AuthenticateRequest());

            // Assert
            response.Should().NotBeNull();
            response.Should().BeOfType<BadRequestObjectResult>();
            ((BadRequestObjectResult)response).StatusCode.Should().Be(Convert.ToInt32(HttpStatusCode.BadRequest));
        }

        [Fact]
        public void ShouldReturnData_When_AuthenticationPassed()
        {
            // Arrange
            _service.Authenticate(Arg.Any<AuthenticateRequest>()).Returns(new AuthenticateResponse(new User(), ""));

            // Act
            var response = _controller.Authenticate(new AuthenticateRequest());

            // Assert
            response.Should().NotBeNull();
            response.Should().BeOfType<OkObjectResult>();
            var result = response as OkObjectResult;
            result.StatusCode.Should().Be(Convert.ToInt32(HttpStatusCode.OK));
            result.Value.Should().NotBeNull();
            result.Value.Should().BeOfType<AuthenticateResponse>();
        }
    }
}
