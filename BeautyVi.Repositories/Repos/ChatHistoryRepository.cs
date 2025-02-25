using System;
using System.Collections.Generic;
using System.Linq;
using BeautyVi.Core.Context;
using BeautyVi.Core.Entities;
using BeautyVi.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BeautyVi.Repositories.Repos
{
    public class ChatHistoryRepository : IChatHistoryRepository
    {
        private readonly BeautyViContext _context;

        public ChatHistoryRepository(BeautyViContext context)
        {
            _context = context;
        }

        public void Add(ChatHistory chatHistory)
        {
            _context.ChatHistories.Add(chatHistory);
            Save();
        }

        public void Delete(ChatHistory chatHistory)
        {
            _context.ChatHistories.Remove(chatHistory);
            Save();
        }

        public ChatHistory Get(int id)
        {
            return _context.ChatHistories.Find(id);
        }

        public IEnumerable<ChatHistory> GetAllByUserId(string userId)
        {
            return _context.ChatHistories
                           .Where(ch => ch.UserId == userId)
                           .ToList();
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public void Update(ChatHistory chatHistory)
        {
            _context.ChatHistories.Update(chatHistory);
            Save();
        }

        /*public void DeleteOldMessages()
        {
            var cutoffDate = DateTime.Now.AddMinutes(-20); // Видалення повідомлень старіших за 30 днів
            var oldMessages = _context.ChatHistories
                                      .Where(ch => ch.Timestamp < cutoffDate)
                                      .ToList();

            if (oldMessages.Any())
            {
                _context.ChatHistories.RemoveRange(oldMessages);
                Save();
            }
        }
        */
        public void DeleteAllByUserId(string userId)
        {
            _context.ChatHistories
                .Where(ch => ch.UserId == userId)
                .ExecuteDelete();
            /* var userMessages = _context.ChatHistories
                                        .Where(ch => ch.UserId == userId)
                                        .ToList();
             _context.ChatHistories.RemoveRange(userMessages);
             Save();*/
        }

    }
}
