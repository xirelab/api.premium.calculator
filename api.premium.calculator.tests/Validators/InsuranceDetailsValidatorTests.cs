using api.premium.calculator.Models;
using api.premium.calculator.Validations;
using FluentValidation.TestHelper;
using System;
using Xunit;

namespace api.premium.calculator.tests.Validators
{
    public class InsuranceDetailsValidatorTests
    {
        private InsuranceDetailsValidator validator;

        //[SetUp]
        public InsuranceDetailsValidatorTests()
        {
            validator = new InsuranceDetailsValidator();
        }

        [Fact]
        public void Should_have_error_when_Age_Is_Lessthan_18()
        {
            var model = new InsuranceDetails
            {
                Name = "SK",
                DateOfBirth = DateTime.Parse("1984-05-31"),
                DeathCoverAmount = 10000,
                Age = 6,
                OccupationId = 2
            };
            var result = validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(data => data.Age);
        }

        [Fact]
        public void Should_have_error_when_Age_Is_Greaterthan_100()
        {
            var model = new InsuranceDetails
            {
                Name = "SK",
                DateOfBirth = DateTime.Parse("1984-05-31"),
                DeathCoverAmount = 10000,
                Age = 160,
                OccupationId = 2
            };
            var result = validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(data => data.Age);
        }

        [Fact]
        public void Should_have_error_when_Age_Is_Null()
        {
            var model = new InsuranceDetails
            {
                Name = "SK",
                DateOfBirth = DateTime.Parse("1984-05-31"),
                DeathCoverAmount = 10000,
                OccupationId = 2
            };
            var result = validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(data => data.Age);
        }

        [Fact]
        public void Should_have_error_when_SumInsured_LessThan_0()
        {
            var model = new InsuranceDetails
            {
                Name = "SK",
                DateOfBirth = DateTime.Parse("1984-05-31"),
                DeathCoverAmount = 00,
                Age = 36,
                OccupationId = 2
            };
            var result = validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(data => data.DeathCoverAmount);
        }

        [Fact]
        public void Should_have_error_when_Occupation_IsInvalid()
        {
            var model = new InsuranceDetails
            {
                Name = "SK",
                DateOfBirth = DateTime.Parse("1984-05-31"),
                DeathCoverAmount = 1000,
                Age = 36,
                OccupationId = 0
            };
            var result = validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(data => data.OccupationId);
        }

        [Fact]
        public void Should_have_error_when_Occupation_IsEmpty()
        {
            var model = new InsuranceDetails
            {
                Name = "SK",
                DateOfBirth = DateTime.Parse("1984-05-31"),
                DeathCoverAmount = 1000,
                Age = 36
            };
            var result = validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(data => data.OccupationId);
        }
    }
}
