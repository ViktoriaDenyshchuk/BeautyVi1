using BeautyVi.Core.Context;
using BeautyVi.Core.Entities;
using BeautyVi.Repositories.Interfaces;

namespace BeautyVi.Repositories.Repos
{
    public class UserPreferenceRepository : IUserPreferenceRepository
    {
        private readonly BeautyViContext _context;

        public UserPreferenceRepository(BeautyViContext context)
        {
            _context = context;
        }

        public void Add(UserPreference userPreference)
        {
            _context.UserPreferences.Add(userPreference);
            Save();
        }

        public void Delete(UserPreference userPreference)
        {
            _context.UserPreferences.Remove(userPreference);
            Save();
        }

        public UserPreference Get(int id)
        {
            return _context.UserPreferences.Find(id);
        }

        public IEnumerable<UserPreference> GetAll()
        {
            return _context.UserPreferences.ToList();
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public void Update(UserPreference userPreference)
        {
            _context.UserPreferences.Update(userPreference);
            Save();
        }
    }
}
