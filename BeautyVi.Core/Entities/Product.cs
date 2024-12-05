using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace BeautyVi.Core.Entities
{
    public class Product
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string Name { get; set; }
        public string Description { get; set; }
 
        public decimal Price { get; set; }

        public string? CoverPath { get; set; } = "\\img\\product\\no_cover.jpg";
        [NotMapped]
        public IFormFile? CoverFile { get; set; }

        public virtual Category? Category { get; set; }
        public int? CategoryId { get; set; }

        //public string EffectType { get; set; } // Наприклад: "Moisturizing", "Nourishing", "Anti-Aging"
        //public string SuitableFor { get; set; } // Наприклад: "Dry Skin", "Oily Hair", "Sensitive Skin"
        public int? EffectTypeId { get; set; } 
        public virtual EffectType? EffectType { get; set; } 

        public int? SuitableForId { get; set; } 
        public virtual SuitableFor? SuitableFor { get; set; }

        public virtual ICollection<ProductIngredient>? ProductIngredients { get; set; }
        public virtual ICollection<ProductAllergen>? ProductAllergens { get; set; }
        // Властивість для зв'язку з OrderItems
        public virtual ICollection<OrderItem>? OrderItems { get; set; }

        // Властивість для зв'язку з ProductRecommendations
        public virtual ICollection<ProductRecommendation>? ProductRecommendations { get; set; }
    }

}
