using api.premium.calculator.Models;
using System.Collections.Generic;
using System.Linq;

namespace api.premium.calculator.Mapper
{
    public static class CalculatorMapper
    {
        public static decimal CalculatePremium(
            InsuranceDetails insuranceDetails,
            List<Occupation> occupations,
            List<OccupationRating> occupationRatings)
        {
            if (insuranceDetails == null || 
                occupations == null || occupations.Count == 0 || 
                occupationRatings == null || occupationRatings.Count == 0)
            {
                return 0;
            }

            var factor = from x in occupations
                         join y in occupationRatings on x.Rating equals y.Rating
                         where x.Id.Equals(insuranceDetails.OccupationId)
                         select y.Factor;

            return (insuranceDetails.DeathCoverAmount * factor.FirstOrDefault() * insuranceDetails.Age) / 1000 * 12;
        }
    }
}
