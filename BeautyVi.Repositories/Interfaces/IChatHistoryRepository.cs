using System;
using BeautyVi.Core.Entities;

namespace BeautyVi.Repositories.Interfaces
{
    public interface IChatHistoryRepository : ISave
    {
        // Отримати історію чату для конкретного користувача
        IEnumerable<ChatHistory> GetAllByUserId(string userId);

        // Отримати певну історію чату по її ідентифікатору
        ChatHistory Get(int id);

        void Add(ChatHistory chatHistory);
        void Update(ChatHistory chatHistory);
        void Delete(ChatHistory chatHistory);
        //void DeleteOldMessages();
        void DeleteAllByUserId(string userId);
    }

}

