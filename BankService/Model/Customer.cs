using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BankService.Model
{
    [Table("Customers")]
    public class Customer
    {
        [Key]
        public Guid id { get; set; }

        [Required]
        public string Name { get; set; }
        
        [Required] 
        public string Username { get; set; }
        
        [Required] 
        public string Password { get; set; }

        [Required] 
        public string Address { get; set; }
        
        [Required] 
        public string State { get; set; }

        [Required] 
        public string Country { get; set; }

        [Required] 
        public string EmailAddress { get; set; }

        [Required] 
        public string PAN { get; set; }

        [Required] 
        [RegularExpression("^[0-9]*$")]
        public int ContactNo { get; set; }

        [Required] 
        public DateTime DOB { get; set; }

        [Required] 
        public int AccountType { get; set; }

        public ICollection<Loan> Loan { get; set; } = new List<Loan>();
    }
}
