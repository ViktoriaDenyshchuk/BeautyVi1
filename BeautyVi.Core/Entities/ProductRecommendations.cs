using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BeautyVi.Core.Entities;

namespace BeautyVi.Core.Entities
{
    public class ProductRecommendation
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public virtual User? User { get; set; } //з таблички юзер витягується юзкр id
        public string? UserId { get; set; }

        public int ProductId { get; set; }  // Ідентифікатор продукту
        public virtual Product Product { get; set; }  // Зв'язок з продуктом

    }
}
