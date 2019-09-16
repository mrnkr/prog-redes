using System;
using System.Collections.Generic;

namespace SubarashiiDemo.BusinessLogic
{
    public interface IRepo <T> where T : class
    {
        void Add(T ObjectToCreate);

        void Modify(T OldObject, T NewObject);

        void Delete(T ObjectToDelete);

        List<T> GetAll();
    }
}
