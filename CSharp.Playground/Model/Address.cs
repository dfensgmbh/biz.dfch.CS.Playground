using System;
using System.ComponentModel.DataAnnotations;

namespace Database.Model
{
    public class Address
    {
        [Key]
        public Int32 Id { get; set; }

        [Required]
        public String Name { get; set; }
        [Required]
        public String LastName { get; set; }
        [Required]
        public String Address1 { get; set; }
        public String Address2 { get; set; }
        [MaxLength(5)]
        [Required]
        public String ZIP { get; set; }
        [Required]
        public String City { get; set; }
        public String Company { get; set; }
        [Required]
        public String Country { get; set; }
        [Required]
        public String Phone { get; set; }
    }
}
