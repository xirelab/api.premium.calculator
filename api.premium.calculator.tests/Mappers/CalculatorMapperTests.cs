using FluentAssertions;
using System.Collections.Generic;
using Xunit;
using api.premium.calculator.Mapper;
using api.premium.calculator.Models;
using api.premium.calculator.tests.Common;
using System.Linq;
using System;

namespace api.premium.calculator.tests.Mappers
{
    public class CalculatorMapperTests
    {
        [Fact]
        public void ShouldReturnZero_When_OccupationEmpty()
        {
            // Arrange
            InsuranceDetails insuranceDetails = null;
            List<Occupation> occupations = null;
            List<OccupationRating> occupationRatings = new List<OccupationRating>();

            // Act
            var response = CalculatorMapper.CalculatePremium(insuranceDetails, occupations, occupationRatings);

            // Assert
            response.Should().Be(0);
        }        

        [Fact]
        public void ShouldReturnZero_When_OccupationNotFound()
        {
            // Arrange
            var insuranceDetails = MockedConstants.mockedInsuranceDetails;
            var occupations = MockedConstants.SampleOccupations;
            var occupationRatings = MockedConstants.SampleOccupationRatings;
            insuranceDetails.OccupationId = 121;

            // Act
            var response = CalculatorMapper.CalculatePremium(insuranceDetails, occupations, occupationRatings);

            // Assert
            response.Should().Be(0);
        }

        [Fact]
        public void ShouldReturnZero_When_OccupationRatingNotFound()
        {
            // Arrange
            var insuranceDetails = MockedConstants.mockedInsuranceDetails;
            var occupations = MockedConstants.SampleOccupations;
            var occupationRatings = MockedConstants.SampleOccupationRatings;
            occupationRatings.FirstOrDefault().Rating = "Dummy";

            // Act
            var response = CalculatorMapper.CalculatePremium(insuranceDetails, occupations, occupationRatings);

            // Assert
            response.Should().Be(0);
        }

        [Fact]
        public void ShouldReturnPremuin_When_CalculationPassed()
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

            // Act
            var response = CalculatorMapper.CalculatePremium(insuranceDetails, occupations, occupationRatings);

            // Assert
            response.Should().Be(4320M);
        }
    }
}
