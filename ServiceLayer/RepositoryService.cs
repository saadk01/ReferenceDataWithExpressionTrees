using DataLayer;
using DomainLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace ServiceLayer
{
    public class RepositoryService : IRepositoryService
    {
        /// <summary>
        /// Currently represents an instance of IDataBanck but can easily represent an ORM's instance (e.g. Context
        /// in Entity Framework).
        /// </summary>
        protected IDataBank _data;

        public RepositoryService(IDataBank db)
        {
            _data = db;
        }
        public IEnumerable<T> FindAll<T>() where T : class, IEntity
        {
            return _data.All<T>();
        }

        public IQueryable<T> FindWhere<T>(Expression<Func<T, bool>> predicate) where T : class, IEntity
        {
            return _data.Where<T>(predicate);
        }
    }
}
