using BeautyVi.Core.Context;
using BeautyVi.Core.Entities;
using BeautyVi.Repositories.Interfaces;

namespace BeautyVi.Repositories.Repos
{
    public class SuitableForRepository : ISuitableForRepository
    {
        private readonly BeautyViContext _context;

        public SuitableForRepository(BeautyViContext context)
        {
            _context = context;
        }

        public void Add(SuitableFor suitableFor)
        {
            _context.SuitableForOptions.Add(suitableFor);
            Save();
        }

        public void Delete(SuitableFor suitableFor)
        {
            _context.SuitableForOptions.Remove(suitableFor);
            Save();
        }

        public SuitableFor Get(int id)
        {
            return _context.SuitableForOptions.Find(id);
        }

        public IEnumerable<SuitableFor> GetAll()
        {
            return _context.SuitableForOptions.ToList();
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public void Update(SuitableFor suitableFor)
        {
            _context.SuitableForOptions.Update(suitableFor);
            Save();
        }
    }
}
