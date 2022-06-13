using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CoreCodeFirst.Models
{
    public class Category
    {
        [Display(Name = "Category Id")]
        public int Id { get; set; }
        [Column(TypeName = "nvarchar(100)")]
        [Display(Name = "Category Name")]
        public string? CategoryName { get; set; }
    }
}
