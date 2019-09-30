using Gestion.Model;
using System;
using System.Collections.Generic;

namespace Gestion.Repository
{
    public interface IRepository<T> where T : BaseEntity
    {
        T Get(string id);

        IEnumerable<T> GetAll();
        IEnumerable<T> Find(Func<T, bool> predicate);

        void Add(T e);
        void AddRange(IEnumerable<T> es);

        void Update(T e);
        void UpdateRange(IEnumerable<T> es);

        void Remove(T e);
        void RemoveRange(IEnumerable<T> es);
    }
}
