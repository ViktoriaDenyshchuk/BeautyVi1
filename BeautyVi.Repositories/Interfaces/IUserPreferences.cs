using System;
using BeautyVi.Core.Entities;

namespace BeautyVi.Repositories.Interfaces
{
    public interface IUserPreferenceRepository : ISave
    {
        UserPreference Get(int id);
        IEnumerable<UserPreference> GetAll();
        void Add(UserPreference userPreference);
        void Update(UserPreference userPreference);
        void Delete(UserPreference userPreference);
    }

}

