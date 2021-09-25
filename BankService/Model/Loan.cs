using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BankService.Model
{
    [Table("Loans")]
    public class Loan
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public int LoanType{ get; set; }

        [Required] 
        public decimal LoanAmount { get; set; }

        [Required] 
        public DateTime Date { get; set; }

        [Required] 
        public decimal RateOfInterest { get; set; }

        [Required] 
        public int Duration { get; set; }

        [ForeignKey("CustomerId")]
        public Customer Customer { get; set; }

        public Guid CustomerId { get; set; }
    }
}