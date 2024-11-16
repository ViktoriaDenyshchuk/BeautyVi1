using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BeautyVi.Core.Entities
{
    public class ProductIngredient
    {
        public int ProductId { get; set; }
        public virtual Product Product { get; set; }

        public int IngredientId { get; set; }
        public virtual Ingredient Ingredient { get; set; }
    }
}
