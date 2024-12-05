
using System;
using System.ComponentModel.DataAnnotations;

namespace BeautyVi.Core.Entities
{
    public class EffectType
    {
        [Key]
        public int Id { get; set; }

        public string NameEffectType { get; set; } // Наприклад: "Moisturizing", "Anti-Aging"

        public virtual ICollection<Product>? Products { get; set; } 

    }
}

