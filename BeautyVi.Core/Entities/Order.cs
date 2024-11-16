using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BeautyVi.Core.Entities;

namespace BeautyVi.Core.Entities
{
    public class Order
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

       // [Required]
        public virtual User? User { get; set; }
        public string? UserId { get; set; }  // Зв'язок з таблицею Users

        [Required]
        public DateTime OrderDate { get; set; }  // Дата створення замовлення

        [Required]
        [MaxLength(50)]
        public string Status { get; set; }  // Статус замовлення ("Очікує", "Відправлено", тощо)

        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal TotalAmount { get; set; }  // Загальна сума замовлення

        [Required]
        [MaxLength(255)]
        public string ShippingAddress { get; set; }  // Адреса доставки

        public virtual ICollection<OrderItem>? OrderItems { get; set; }  // Колекція товарів в замовленні
    }
}
