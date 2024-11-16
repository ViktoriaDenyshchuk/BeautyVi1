using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BeautyVi.Core.Entities
{
    public class OrderItem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int OrderId { get; set; }
        public virtual Order Order { get; set; }  // Зв'язок з таблицею Orders

        [Required]
        public int ProductId { get; set; }
        public virtual Product Product { get; set; }  // Зв'язок з таблицею Products

        [Required]
        public int Quantity { get; set; }  // Кількість товару в замовленні

        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal UnitPrice { get; set; }  // Ціна за одиницю на момент замовлення

        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal TotalPrice { get; set; }  // Загальна ціна (Quantity * UnitPrice)
    }
}
