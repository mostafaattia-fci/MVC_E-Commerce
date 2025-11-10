using DAL.Models;
using System.ComponentModel.DataAnnotations;

namespace DA.Models
{
    public class Address : BaseModel
    {
        [Required, MaxLength(200)]
        public string Street { get; set; } = string.Empty;

        [MaxLength(100)]
        public string City { get; set; } = string.Empty;

        [MaxLength(100)]
        public string Country { get; set; } = string.Empty;

        [MaxLength(20)]
        public string PostalCode { get; set; } = string.Empty;
    }
}
