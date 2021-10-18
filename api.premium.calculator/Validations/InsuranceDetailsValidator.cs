using api.premium.calculator.Models;
using FluentValidation;

namespace api.premium.calculator.Validations
{
    public class InsuranceDetailsValidator :  AbstractValidator<InsuranceDetails>
    {
        public InsuranceDetailsValidator()
        {
            RuleFor(x => x.Age).NotNull().InclusiveBetween(18, 100);
            RuleFor(x => x.OccupationId).NotNull().GreaterThan(0);
            RuleFor(x => x.DeathCoverAmount).NotNull().GreaterThan(0);
        }
    }
}
