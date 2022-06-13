using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CoreCodeFirst.Models
{
    public class Product
    {
        [Key]
        [Display(Name = "Product ID")]
        public int Id { get; set; }
        [Required]
        [Column(TypeName = "nvarchar(100)")]
        [Display(Name = "Product Name")]
        public string? ProductName { get; set; }
        [Column(TypeName = "nvarchar(1000)")]
        [Display(Name = "Product ImageUrl")]
        public string? ImageUrl { get; set; }
        [Required]
        [Display(Name = "Product Price")]
        public int? ProductPrice { get; set; }
        [Display(Name = "Create Date")]
        public DateTime? CreateDate { get; set; }
        [Display(Name = "Create Modify")]
        public DateTime? ModifyDate { get; set; }
        // public byte[] Photo { get; set; }
        [Column(TypeName = "nvarchar(1000)")]
        public string? Describtion { get; set; }
        [Required]
        [Display(Name = "Category Id")]
        public int CategoryId { get; set; }
        [ValidateNever]
        public Category? Category { get; set; } 
    }
}
