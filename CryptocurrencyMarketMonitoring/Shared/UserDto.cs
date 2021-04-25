using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CryptocurrencyMarketMonitoring.Shared
{
    public class UserDto
    {
        public string Id { get; set; }

        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public string Token { get; set; }
        [JsonIgnore]
        public string PasswordHash { get; set; }
        [RegularExpression("^(?=.*[0-9]).{8,}$", ErrorMessage = "Password should contain both letter and number, with minimum length of 8 characters")]
        [Required]
        public string Password { get; set; }
    }
}
