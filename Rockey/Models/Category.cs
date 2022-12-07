using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Rockey.Models
{
    public class Category
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [DisplayName("Disolay Order")]
        [Required]
        [Range(1,int.MaxValue,ErrorMessage ="Display Order must be greater than 0")]
        public int DisplayOrder { get; set; }
    }
}
