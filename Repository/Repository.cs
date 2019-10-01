using Gestion.Model;
using Gestion.Repository.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Gestion.Repository
{
    public class Repository<T> : IRepository<T> where T : BaseEntity
    {
        private IDictionary<string, T> Data { get; }

        public Repository()
        {
            Data = new Dictionary<string, T>();
        }

        public T Get(string id)
        {
            lock (Data)
            {
                if (Data.TryGetValue(id, out var ret))
                {
                    return (T)ret.Clone();
                }
            }
            
            throw new NonExistentEntityException();
        }

        public IEnumerable<T> GetAll()
        {
            lock (Data)
            {
                return Data.Values.Select(e => (T)e.Clone());
            }
        }

        public IEnumerable<T> Find(Func<T, bool> predicate)
        {
            lock (Data)
            {
                return Data.Values.Where(predicate).Select(e => (T)e.Clone());
            }
        }

        public void Add(T e)
        {
            lock (Data)
            {
                Data.Add(e.Id, e);
            }
        }

        public void AddRange(IEnumerable<T> es)
        {
            foreach (var e in es)
            {
                Add(e);
            }
        }

        public void Update(T e)
        {
            lock (Data)
            {
                Data.Remove(e.Id);
                Data.Add(e.Id, e);
            }
        }

        public void UpdateRange(IEnumerable<T> es)
        {
            foreach (var e in es)
            {
                Update(e);
            }
        }

        public void Remove(T e)
        {
            lock (Data)
            {
                Data.Remove(e.Id);
            }
        }

        public void RemoveRange(IEnumerable<T> es)
        {
            foreach (var e in es)
            {
                Remove(e);
            }
        }
    }
}
