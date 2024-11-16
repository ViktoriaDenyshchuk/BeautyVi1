using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BeautyVi.Core.Entities
{
    public class ProductAllergen
    {
        public int ProductId { get; set; }
        public virtual Product Product { get; set; }

        public int AllergenId { get; set; }
        public virtual Allergen Allergen { get; set; }
    }
}
