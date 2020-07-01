using DataTransferObjectsAndViewModels.DTO;
using DomainLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Utilities.Constants;
using static Utilities.Constants.ProjectConstants;

namespace ServiceLayer
{
    public class ReferenceDataService : IReferenceDataService
    {
        protected IRepositoryService _repositoryService;
        public Dictionary<ReferenceDataTypes, Type> AllRefDataTypes { get; private set; }

        public ReferenceDataService(IRepositoryService rs)
        {
            _repositoryService = rs;
            AllRefDataTypes = GetAllRefDataTypes();
        }

        public dynamic FetchItemsList(ReferenceDataTypes refDataType, IList<WhereClauseDTO> selectionCriteria, IList<int> userSelectedValues)
        {
            ParameterExpression parameter = Expression.Parameter(AllRefDataTypes[refDataType], "x");
            MemberExpression activeFrom = Expression.Property(parameter, "EffectiveFromDate");
            MemberExpression activeTill = Expression.Property(parameter, "EffectiveTillDate");
            ConstantExpression today = Expression.Constant(DateTime.Now);
            BinaryExpression criteria1 = Expression.GreaterThanOrEqual(today, activeFrom); // Today >= x.EffectiveFromDate
            BinaryExpression criteria2 = Expression.LessThanOrEqual(today, activeTill); // Today <= x.EffectiveTillDate
            Expression body = Expression.AndAlso(criteria1, criteria2);

            if (userSelectedValues.Count > 0)
                foreach (int usv in userSelectedValues)
                {
                    MemberExpression member3 = Expression.Property(parameter, "ID");
                    ConstantExpression constant2 = Expression.Constant(usv);
                    BinaryExpression criteria3 = Expression.Equal(member3, constant2); // x.ID == userSelectValue

                    body = Expression.Or(body, criteria3);
                }

            foreach (var prop in selectionCriteria)
            {
                PropertyInfo thisProp = AllRefDataTypes[refDataType].GetProperty(prop.PropertyName);
                Expression propertyExp = Expression.Property(parameter, thisProp);
                Expression valueExp = Expression.Constant(prop.PropertyValue);
                valueExp = Expression.Convert(valueExp, thisProp.PropertyType);
                Expression criteria = ComposeComparisonExpression(prop.Operator, propertyExp, valueExp);
                body = AppendWhereExpressions(prop.WhereAndOrCondition ?? ProjectConstants.Operators.SqlWhere.AND, body, criteria);
            }

            Type delegateType = typeof(Func<,>).MakeGenericType(AllRefDataTypes[refDataType], typeof(bool));
            LambdaExpression predicate = Expression.Lambda(delegateType, body, parameter);

            MethodInfo findWhereMethod = _repositoryService.GetType().GetMethod("FindWhere");
            MethodInfo findWhereMethodGeneric = findWhereMethod.MakeGenericMethod(AllRefDataTypes[refDataType]);
            dynamic items = findWhereMethodGeneric.Invoke(_repositoryService, new object[] { predicate });

            return items;
        }

        private Dictionary<ReferenceDataTypes, Type> GetAllRefDataTypes()
        {
            var output = new Dictionary<ReferenceDataTypes, Type>();
            var bpRefDataTypes = Assembly.Load("DomainLayer").GetTypes()
                .Where(x => typeof(IReferenceData).IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract);
            foreach (var e in Enum.GetValues(typeof(ReferenceDataTypes)))
            {
                var name = Enum.GetName(typeof(ReferenceDataTypes), (int)e);
                foreach (var t in bpRefDataTypes)
                {
                    if (name == t.Name)
                    {
                        // ContainsKey check is intentionally not done as every class and enum should be unique
                        output.Add((ReferenceDataTypes)e, t);
                        continue;
                    }
                }
            }
             
            return output;
        }

        private Dictionary<string, PropertyInfo> GetPropertiesInformation(Type refDataEntity)
        {
            return new Dictionary<string, PropertyInfo>
            {
                { "ID", refDataEntity.GetProperty("ID") },
                { "Name", refDataEntity.GetProperty("Name") },
                { "EffectiveFromDate", refDataEntity.GetProperty("EffectiveFromDate") },
                { "EffectiveTillDate", refDataEntity.GetProperty("EffectiveTillDate") },
                { "Description", refDataEntity.GetProperty("Description") }
            };
        }

        private Expression ComposeComparisonExpression(string op, Expression property, Expression value)
        {
            return op switch
            {
                Operators.Comparison.GREATER => Expression.GreaterThan(property, value),
                Operators.Comparison.GREATER_EQ => Expression.GreaterThanOrEqual(property, value),
                Operators.Comparison.LESS => Expression.LessThan(property, value),
                Operators.Comparison.LESS_EQ => Expression.LessThanOrEqual(property, value),
                Operators.Comparison.EQUAL => Expression.Equal(property, value),
                Operators.Comparison.NOT_EQUAL => Expression.NotEqual(property, value),
                Operators.Comparison.LIKE => Expression.Call(property, typeof(string).GetMethod("Contains", new Type[] { typeof(string) }), value),
                _ => throw new ArgumentException($"Unrecognized operator ({op}) provided."),
            };
        }

        private Expression AppendWhereExpressions(string andOrWhr, Expression body, Expression criteria)
        {
            return andOrWhr switch
            {
                Operators.SqlWhere.AND => Expression.And(body, criteria),
                Operators.SqlWhere.OR => Expression.Or(body, criteria),
                _ => throw new ArgumentException($"Unrecognized operator ({andOrWhr}) provided."),
            };
        }
    }
}
