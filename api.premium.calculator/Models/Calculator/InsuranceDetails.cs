using System;

namespace api.premium.calculator.Models
{    
    public class InsuranceDetails
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public DateTime DateOfBirth { get; set; }
        public int OccupationId { get; set; }
        public decimal DeathCoverAmount { get; set; }        
    }
}
