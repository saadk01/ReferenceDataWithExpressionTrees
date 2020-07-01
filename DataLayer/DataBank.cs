using DomainLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace DataLayer
{
    /// <summary>
    /// An overly simplistic replacement for database and ORMs for demonstration purposes. The content in this class can easily
    /// be replaced by real-life alternatives like Entity Framework or NHibernate without chaning other layers.  
    /// </summary>
    public class DataBank : IDataBank
    {
        private readonly Dictionary<int, string> _provinces = new Dictionary<int, string>()
        {
            {1, "Alberta" },
            {2, "British Columbia" },
            {3, "Manitoba" },
            {4, "New Brunswick" },
            {5, "Newfoundland and Labrador" },
            {6, "Northwest Territories" },
            {7, "Nova Scotia" },
            {8, "Nunavut" },
            {9, "Ontario" },
            {10, "Prince Edward Island" },
            {11, "Quebec" },
            {12, "Saskatchewan" },
            {13, "Yukon" },
        };

        private readonly Dictionary<int, string> _seasons = new Dictionary<int, string>()
        {
            { 1, "Spring" },
            { 2, "Summer/Construction" },
            { 3, "Fall" },
            { 4, "Winter" }
        };

        public IEnumerable<T> All<T>() where T : class, IEntity
        {
            return GetInternalList<T>();
        }

        public IQueryable<T> Where<T>(Expression<Func<T, bool>> predicate) where T : class, IEntity
        {
            var all = GetInternalList<T>();
            return all.AsQueryable().Where(predicate);
        }


        private IEnumerable<T> GetInternalList<T>()
        {
            var entityName = typeof(T).Name;
            var methodName = $"PopulateList{entityName}";
            MethodInfo method = this.GetType().GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Instance);

            return method.Invoke(this, null) as IEnumerable<T>;
        }

        private List<Province> PopulateListProvince()
        {
            var provs =  IterateDataList<Province>(_provinces, true);
            foreach (var p in provs)
            {
                // This is less than ideal, but I'm more focused on demonstration of expression trees and resulting infrastructure
                // to support reference data with no repetition or boilerplate code without adding complexity of database or ORM.
                switch (p.Name)
                {
                    case "Alberta":
                        p.EffectiveFromDate = new DateTime(1905, 9, 1);
                        break;
                    case "British Columbia":
                        p.EffectiveFromDate = new DateTime(1871, 7, 1);
                        break;
                    case "Manitoba":
                        p.EffectiveFromDate = new DateTime(1870, 7, 15);
                        break;
                    case "New Brunswick":
                        p.EffectiveFromDate = new DateTime(1867, 7, 1);
                        break;
                    case "Newfoundland and Labrador":
                        p.EffectiveFromDate = new DateTime(1905, 9, 1);
                        break;
                    case "Northwest Territories":
                        p.EffectiveFromDate = new DateTime(1870, 7, 15);
                        break;
                    case "Nova Scotia":
                        p.EffectiveFromDate = new DateTime(1867, 7, 1);
                        break;
                    case "Nunavut":
                        p.EffectiveFromDate = new DateTime(1999, 4, 1);
                        break;
                    case "Ontario":
                        p.EffectiveFromDate = new DateTime(1867, 7, 1);
                        break;
                    case "Prince Edward Island":
                        p.EffectiveFromDate = new DateTime(1873, 7, 1);
                        break;
                    case "Quebec":
                        p.EffectiveFromDate = new DateTime(1867, 7, 1);
                        break;
                    case "Saskatchewan":
                        p.EffectiveFromDate = new DateTime(1905, 9, 1);
                        break;
                    case "Yukon":
                        p.EffectiveFromDate = new DateTime(1898, 6, 13);
                        break;
                }
                p.EffectiveTillDate = DateTime.MaxValue;
            }

            return provs;
        }

        private List<Season> PopulateListSeason()
        {
            return IterateDataList<Season>(_seasons);
        }

        private List<T> IterateDataList<T>(Dictionary<int, string> dataList) where T: new()
        {
            return IterateDataList<T>(dataList, false);
    }

        private List<T> IterateDataList<T>(Dictionary<int, string> dataList, bool idNameOnly) where T : new()
        {
            var res = new List<T>();
            foreach (var d in dataList)
            {
                var instance = new T();
                instance.GetType().GetProperty("ID", BindingFlags.Public | BindingFlags.Instance).SetValue(instance, d.Key);
                instance.GetType().GetProperty("Name", BindingFlags.Public | BindingFlags.Instance).SetValue(instance, d.Value);
                if (!idNameOnly)
                {
                    instance.GetType().GetProperty("EffectiveFromDate", BindingFlags.Public | BindingFlags.Instance).SetValue(instance, new DateTime(2000, 1, 1));
                    instance.GetType().GetProperty("EffectiveTillDate", BindingFlags.Public | BindingFlags.Instance).SetValue(instance, DateTime.MaxValue);
                }
                
                res.Add(instance);
            }

            return res;
        }
    }
}
