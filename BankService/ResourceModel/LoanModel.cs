using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BankService.ResourceModel
{
      public class LoanModel
    {
        [Required]
        public int LoanType { get; set; }

        [Required]
        public decimal LoanAmount { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public decimal RateOfInterest { get; set; }

        [Required]
        public int Duration { get; set; }

    }
}
