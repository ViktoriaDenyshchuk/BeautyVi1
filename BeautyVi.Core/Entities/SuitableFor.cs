using System;
using System.ComponentModel.DataAnnotations;

namespace BeautyVi.Core.Entities
{
    public class SuitableFor
    {
        [Key]
        public int Id { get; set; }

        public string NameSuitableFor { get; set; } // Наприклад: "Dry Skin", "Oily Hair"

        public virtual ICollection<Product>? Products { get; set; }
    }
}

