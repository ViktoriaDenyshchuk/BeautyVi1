using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using BeautyVi.Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeautyVi.Core.Entities
{
    public class User : IdentityUser
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        //public string Email { get; set; }
        //public string FullName { get; set; }

        // Властивість для зв'язку з UserPreferences (1 до 1)
        public virtual UserPreferences? UserPreferences { get; set; }

        // Властивість для зв'язку з Orders (1 до багатьох)
        public virtual ICollection<Order>? Orders { get; set; }

        // Додана властивість для зв'язку з ProductRecommendations (1 до багатьох)
        public virtual ICollection<ProductRecommendation>? ProductRecommendations { get; set; }
    }
}
