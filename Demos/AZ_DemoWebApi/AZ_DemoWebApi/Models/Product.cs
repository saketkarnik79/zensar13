using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AZ_DemoWebApi.Models
{
    [Table("Products")]
    public class Product
    {
        [Key]
        [Column("Id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ProductId { get; set; }

        [Required]
        [StringLength(50,MinimumLength = 3)]
        public string? Name { get; set; }

        [Required]
        [MaxLength(200)]
        public string? Description { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        public decimal Price { get; set; }
    }
}
