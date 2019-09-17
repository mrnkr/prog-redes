﻿using System;
using System.Collections.Generic;

namespace SubarashiiDemo.BusinessLogic
{
    public interface IRepository <T> where T : class
    {
        void Add(T objectToCreate);

        void Modify(T oldObject, T newObject);

        void Delete(T objectToDelete);

        List<T> GetAll();
    }
}