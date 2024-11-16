using BeautyVi.Core.Entities;
using BeautyVi.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeautyVi.Repositories.Repos
{
    internal class UserNoSQLRepository : IUserRepository
    {
        //private readonly MongoDbConnection connection;
        public UserNoSQLRepository()
        {

        }
        public void Add(User obj)
        {
            throw new NotImplementedException();
        }

        public void Delete(User obj)
        {
            throw new NotImplementedException();
        }

        public User Get(string id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<User> GetAll()
        {
            throw new NotImplementedException();
        }

        public void Save()
        {
            throw new NotImplementedException();
        }

        public void Update(User obj)
        {
            throw new NotImplementedException();
        }

        public User GetUserById(string? userId)
        {
            throw new NotImplementedException();
        }

        public void UpdateUser(User user)
        {
            throw new NotImplementedException();
        }

    }
}