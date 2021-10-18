using api.premium.calculator.Models;
using System;
using System.Collections.Generic;

namespace api.premium.calculator.tests.Common
{
    public class MockedConstants
    {
        public static List<User> SampleUsers = new List<User>
        {
            new User
            {
                Id = 1,
                FirstName = "Test",
                LastName = "User",
                Username = "Test",
                Password = "Test"
            }
        };

        public static List<Occupation> SampleOccupations = new List<Occupation>
        {
            new Occupation
            {
                Id = 1,
                Name = "Doctor",
                Rating = "Initial"
            }
        };

        public static List<OccupationRating> SampleOccupationRatings = new List<OccupationRating>
        {
            new OccupationRating
            {
                Rating = "Initial",
                Factor = 1
            }
        };

        public static InsuranceDetails mockedInsuranceDetails = new InsuranceDetails
        {
            Name = "SK",
            DateOfBirth = DateTime.Parse("1984-05-31"),
            DeathCoverAmount = 10000,
            Age = 36,
            OccupationId = 1
        };
    }
}
