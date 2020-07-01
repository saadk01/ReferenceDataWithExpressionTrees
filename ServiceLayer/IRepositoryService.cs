using DomainLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace ServiceLayer
{
    public interface IRepositoryService
    {
        IEnumerable<T> FindAll<T>() where T : class, IEntity;
        IQueryable<T> FindWhere<T>(Expression<Func<T, bool>> predicate) where T : class, IEntity;
    }
}