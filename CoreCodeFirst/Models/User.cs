using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CoreCodeFirst.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [Column(TypeName = "nvarchar(50)")]
        [Display(Name = "User Name")]
        public string UserName { get; set; }     
        [Required]
        [DataType(DataType.EmailAddress)]
        [Column(TypeName = "nvarchar(50)")]
        public string? Email { get; set; }
        [Column(TypeName = "nvarchar(50)")]
        public string? Password { get; set; }
        [Required]      
        public int UserTypeId { get; set; }
        [ForeignKey("UserTypeId")]
        [ValidateNever]
        public UserType UserType { get; set; }
       // public string? UserType { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
     /*   [Column(TypeName = "nvarchar(500)")]
        public string? Address { get; set; }*/
        [Required]
        public string PhoneNumber { get; set; }
        [Required]
        [Column(TypeName = "nvarchar(50)")]
        public string StreetAddress { get; set; }
        [Required]
        [Column(TypeName = "nvarchar(50)")]
        public string City { get; set; }
        [Required]
        [Column(TypeName = "nvarchar(50)")]
        public string State { get; set; }
        [Required]
        [Column(TypeName = "nvarchar(50)")]
        public string PostalCode { get; set; }
      
    }
}
