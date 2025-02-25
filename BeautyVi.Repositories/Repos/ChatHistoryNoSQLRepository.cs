using System;
using BeautyVi.Core.Entities;
using BeautyVi.Repositories.Interfaces;

namespace BeautyVi.Repositories.Repos
{
    internal class ChatHistoryNoSQLRepository : IChatHistoryRepository
    {
        public ChatHistoryNoSQLRepository()
        {

        }

        public void Add(ChatHistory obj)
        {
            throw new NotImplementedException();
        }

        public void Delete(ChatHistory obj)
        {
            throw new NotImplementedException();
        }

        public ChatHistory Get(int id)
        {
            throw new NotImplementedException();
        }

        public void Update(ChatHistory obj)
        {
            throw new NotImplementedException();
        }

        public void Save()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ChatHistory> GetAllByUserId(string userId)
        {
            throw new NotImplementedException();
        }

        /*public void DeleteOldMessages()
        {
            throw new NotImplementedException();
        }*/

        public void DeleteAllByUserId(string userId)
        {
            throw new NotImplementedException();
        }
    }
}