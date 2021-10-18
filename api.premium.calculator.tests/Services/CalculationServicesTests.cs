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
    public class CalculationServicesTests
    {
        private readonly CalculatorServices _service;
        private readonly IFileReader _fileReader;

        public CalculationServicesTests()
        {
            _fileReader = Substitute.For<IFileReader>();
            _service = new CalculatorServices(_fileReader);
        }

        [Fact]
        public void ShouldReturnDataNull_When_NoOccupationsFound()
        {
            // Arrange
            _fileReader.LoadJson<List<Occupation>>(Arg.Any<string>()).ReturnsNull();

            // Act
            var response = _service.GetOccupations();

            // Assert
            response.Should().NotBeNull();
            response.Data.Should().BeNull();
        }

        [Fact]
        public void ShouldReturnOccupations_When_FileLoaded()
        {
            // Arrange
            _fileReader.LoadJson<List<Occupation>>(Arg.Any<string>()).Returns(MockedConstants.SampleOccupations);

            // Act
            var response = _service.GetOccupations();

            // Assert
            response.Should().NotBeNull();
            response.Data.Should().NotBeNull();
            response.Should().BeOfType<ApiResult<List<Occupation>>>();
            response.Status.Should().Be("success");
        }

        [Fact]
        public void ShouldReturnError_When_NoOccupationsFound()
        {
            // Arrange
            _fileReader.LoadJson<List<Occupation>>(Arg.Any<string>()).ReturnsNull();

            // Act
            var response = _service.Calculate(MockedConstants.mockedInsuranceDetails);

            // Assert
            response.Should().NotBeNull();
            response.Status.Should().Be("error");
            response.Message.Should().Be("Error loading static data");
        }

        [Fact]
        public void ShouldReturnFail_When_CalculationFailed()
        {
            // Arrange
            _fileReader.LoadJson<List<Occupation>>(Arg.Any<string>()).Returns(MockedConstants.SampleOccupations);
            _fileReader.LoadJson<List<OccupationRating>>(Arg.Any<string>()).Returns(MockedConstants.SampleOccupationRatings);
            var request = MockedConstants.mockedInsuranceDetails;
            request.DeathCoverAmount = 0;
            request.OccupationId = 1;

            // Act
            var response = _service.Calculate(request);

            // Assert
            response.Should().NotBeNull();
            response.Status.Should().Be("fail");
            response.Message.Should().Be("Error Calculating the premium");
        }

        [Fact]
        public void ShouldReturnPremiun_When_CalculationPassed()
        {
            // Arrange
            var occupations = new List<Occupation>
            {
                new Occupation
                {
                    Id = 1,
                    Name = "Doctor",
                    Rating = "Initial"
                }
            };

            var occupationRatings = new List<OccupationRating>
            {
                new OccupationRating
                {
                    Rating = "Initial",
                    Factor = 1
                }
            };

            var insuranceDetails = new InsuranceDetails
            {
                Name = "SK",
                DateOfBirth = DateTime.Parse("1984-05-31"),
                DeathCoverAmount = 10000,
                Age = 36,
                OccupationId = 1
            };
            _fileReader.LoadJson<List<Occupation>>(Arg.Any<string>()).Returns(occupations);
            _fileReader.LoadJson<List<OccupationRating>>(Arg.Any<string>()).Returns(occupationRatings);            

            // Act
            var response = _service.Calculate(insuranceDetails);

            // Assert
            response.Should().NotBeNull();
            response.Status.Should().Be("success");
            response.Data.Should().Be(4320M);
        }
    }
}
