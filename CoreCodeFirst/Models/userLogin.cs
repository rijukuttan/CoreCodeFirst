using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CoreCodeFirst.Models
{
    public class userLogin
    {            
        [Required]
        public string? Email { get; set; }
        [Column(TypeName = "nvarchar(50)")]
        [Required]     
        public string? Password { get; set; }
    }
}
