using System;
using System.Collections.Generic;

namespace Subarashii.Repository
{
    public interface IRepository<T> where T : class
    {
        void Add(T objectToCreate);

        void Modify(T obj);

        void Delete(T objectToDelete);

        List<T> GetAll();
    }
}
