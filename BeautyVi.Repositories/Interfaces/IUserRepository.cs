using BeautyVi.Core.Entities;
using BeautyVi.Repositories.Interfaces;

namespace BeautyVi.Repositories.Interfaces
{
    public interface IUserRepository : ISave
    {
        User Get(string id);
        IEnumerable<User> GetAll();
        void Add(User obj);
        void Update(User obj);
        void Delete(User obj);
        User GetUserById(string? userId);
        void UpdateUser(User user);
    }
}
