﻿using System.Collections.Generic;

namespace Demo.Domain.Shared
{
    public interface IRepository<T> where T : Entity
    {
        void Add(T entity);

        void Add(params T[] entities);

        void Delete(T entity);

        void Delete(string id);

        void Delete(params T[] entities);

        void Delete(params string[] ids);

        T Load(string id);

        IList<T> LoadAll();
    }
}