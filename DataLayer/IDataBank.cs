using DomainLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace DataLayer
{
    public interface IDataBank
    {
        IEnumerable<T> All<T>() where T : class, IEntity;

        IQueryable<T> Where<T>(Expression<Func<T, bool>> predicate) where T : class, IEntity;
    }
}