﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BeautyVi.Core.Entities
{
    public class Ingredient
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string Category { get; set; }

        public int LevelOfDanger { get; set; }

        public bool IsHarmful { get; set; }

        public string Description { get; set; }

        public virtual ICollection<ProductIngredient> ProductIngredients { get; set; }
    }
}
