using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BeautyVi.Core.Entities;

namespace BeautyVi.Core.Entities
{
    public class UserPreferences
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public virtual User? User { get; set; } 
        public string? UserId { get; set; }

        public string HairType { get; set; }

        public string SkinType { get; set; }

        public bool AvoidAllergens { get; set; }

        public string AvoidedAllergens { get; set; }
    }
}
