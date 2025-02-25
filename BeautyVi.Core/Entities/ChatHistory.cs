
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BeautyVi.Core.Entities
{
    public class ChatHistory
    {
        [Key]
        public int Id { get; set; }

        public string UserId { get; set; } // Посилання на користувача

        public string UserMessage { get; set; }  // Використовуємо string замість byte[]
        public string AIResponse { get; set; }   // Використовуємо string замість byte[]

        public DateTime Timestamp { get; set; }

        public virtual User User { get; set; }
    }

}

