using api.premium.calculator.Common.Helpers;
using api.premium.calculator.Models;
using api.premium.calculator.Services;
using api.premium.calculator.tests.Common;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using System;
using System.Collections.Generic;
using Xunit;

namespace api.premium.calculator.tests.Services
{
    public class UserServicesTests
    {
        private readonly UserService _service;
        private readonly IJwtTokenHelper _jwtTokenHelper;
        private readonly IFileReader _fileReader;

        public UserServicesTests()
        {
            _fileReader = Substitute.For<IFileReader>();
            _jwtTokenHelper = Substitute.For<IJwtTokenHelper>();
            _service = new UserService(_jwtTokenHelper, _fileReader);
        }

        [Fact]
        public void ShouldReturnNull_When_NoUserFound()
        {
            // Arrange
            _fileReader.LoadJson<List<User>>(Arg.Any<string>()).ReturnsNull();

            // Act
            var response = _service.Authenticate(new AuthenticateRequest());

            // Assert
            response.Should().BeNull();
        }

        [Fact]
        public void ShouldReturnNull_When_UserDoesntMatch()
        {
            // Arrange
            _fileReader.LoadJson<List<User>>(Arg.Any<string>()).Returns(MockedConstants.SampleUsers);
            var request = new AuthenticateRequest
            {
                Username = "Test",
                Password = "Test2"
            };

            // Act
            var response = _service.Authenticate(request);

            // Assert
            response.Should().BeNull();
        }

        [Fact]
        public void ShouldReturnToken_When_UserMatch()
        {
            // Arrange
            _fileReader.LoadJson<List<User>>(Arg.Any<string>()).Returns(MockedConstants.SampleUsers);
            _jwtTokenHelper.generateJwtToken(Arg.Any<User>()).Returns("token");
            var request = new AuthenticateRequest
            {
                Username = "Test",
                Password = "Test"
            };

            // Act
            var response = _service.Authenticate(request);

            // Assert
            response.Should().NotBeNull();
            response.Should().BeOfType<AuthenticateResponse>();
        }

        [Fact]
        public void ShouldReturUserDetails_When_UserMatch()
        {
            // Arrange
            _fileReader.LoadJson<List<User>>(Arg.Any<string>()).Returns(MockedConstants.SampleUsers);
            var request = new AuthenticateRequest
            {
                Username = "Test",
                Password = "Test2"
            };

            // Act
            var response = _service.GetById(1);

            // Assert
            response.Should().NotBeNull();
            response.Should().BeOfType<User>();
        }
    }
}
