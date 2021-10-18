using api.premium.calculator.Common;
using api.premium.calculator.Mapper;
using api.premium.calculator.Models;
using System.Collections.Generic;

namespace api.premium.calculator.Services
{
    public interface ICalculatorServices
    {
        ApiResult<decimal> Calculate(InsuranceDetails insuranceDetails);
        ApiResult<List<Occupation>> GetOccupations();
    }

    public class CalculatorServices : ICalculatorServices
    {
        private readonly IFileReader _fileReader;
        public CalculatorServices(IFileReader fileReader)
        {
            _fileReader = fileReader;
        }

        public ApiResult<decimal> Calculate(InsuranceDetails insuranceDetails)
        {
            var occupations = _fileReader.LoadJson<List<Occupation>>("Occupations.StaticData.json");
            var occupationRatings = _fileReader.LoadJson<List<OccupationRating>>("OccupationRating.StaticData.json");

            if (occupations == null || occupationRatings == null)
            {
                return new ApiResult<decimal>
                {                    
                    Status = Constants.Error,
                    Message = "Error loading static data"
                };
            }

            var premium = CalculatorMapper.CalculatePremium(insuranceDetails, occupations, occupationRatings);

            if (premium <= 0)
            {
                return new ApiResult<decimal>
                {
                    Status = Constants.Fail,
                    Message = "Error Calculating the premium"
                };

            }

            return new ApiResult<decimal>
            {
                Data = CalculatorMapper.CalculatePremium(insuranceDetails, occupations, occupationRatings),
                Status = Constants.Success
            };
        }

        public ApiResult<List<Occupation>> GetOccupations()
        {
            return new ApiResult<List<Occupation>>
            {
                Data = _fileReader.LoadJson<List<Occupation>>("Occupations.StaticData.json"),
                Status = Constants.Success
            };
        }     
    }
}
