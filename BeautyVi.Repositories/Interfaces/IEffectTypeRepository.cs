using System;
using BeautyVi.Core.Entities;

namespace BeautyVi.Repositories.Interfaces
{
    public interface IEffectTypeRepository : ISave
    {
        EffectType Get(int id);
        IEnumerable<EffectType> GetAll();
        void Add(EffectType effectType);
        void Update(EffectType effectType);
        void Delete(EffectType effectType);
    }

}

