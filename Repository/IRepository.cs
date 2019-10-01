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

        void Update(T e);

        void Remove(T e);
    }
}
