using BeautyVi.Core.Context;
using BeautyVi.Core.Entities;
using BeautyVi.Repositories.Interfaces;

namespace BeautyVi.Repositories.Repos
{
    public class EffectTypeRepository : IEffectTypeRepository
    {
        private readonly BeautyViContext _context;

        public EffectTypeRepository(BeautyViContext context)
        {
            _context = context;
        }

        public void Add(EffectType effectType)
        {
            _context.EffectTypes.Add(effectType);
            Save();
        }

        public void Delete(EffectType effectType)
        {
            _context.EffectTypes.Remove(effectType);
            Save();
        }

        public EffectType Get(int id)
        {
            return _context.EffectTypes.Find(id);
        }

        public IEnumerable<EffectType> GetAll()
        {
            return _context.EffectTypes.ToList();
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public void Update(EffectType effectType)
        {
            _context.EffectTypes.Update(effectType);
            Save();
        }
    }
}
