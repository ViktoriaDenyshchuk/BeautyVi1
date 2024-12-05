using System;
using BeautyVi.Core.Entities;

namespace BeautyVi.Repositories.Interfaces
{
    public interface ISuitableForRepository : ISave
    {
        SuitableFor Get(int id);
        IEnumerable<SuitableFor> GetAll();
        void Add(SuitableFor suitableFor);
        void Update(SuitableFor suitableFor);
        void Delete(SuitableFor suitableFor);
    }

}

