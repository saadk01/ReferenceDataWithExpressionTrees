using DataTransferObjectsAndViewModels.DTO;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using Utilities.Constants;

namespace DataTransferObjectsAndViewModels.ViewModel
{
    public class SelectBoxDisplayViewModel
    {
        public int? UserSelectedValue { get; set; }
        public string ElementName { get; set; }
        public IEnumerable<SelectListItem> SelectListItems { get; set; }
        public IDictionary<string, object> HtmlAttributes { get; set; }
    }

    public class SelectionCriteria
    {
        private IList<WhereClauseDTO> _clauseMembers { get; set; }

        /// <summary>
        /// Can only be used as a first sequence if method chaining is being employed.
        /// </summary>
        /// <param name="propName"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public SelectionCriteria PrepareSelectionCriteriaProperties(string propName, dynamic val)
        {
            return PrepareSelectionCriteriaProperties(propName, val, ProjectConstants.Operators.Comparison.EQUAL, null);
        }

        /// <summary>
        /// Can only be used as a first sequence if method chaining is being employed.
        /// </summary>
        /// <param name="propName"></param>
        /// <param name="val"></param>
        /// <param name="op"></param>
        /// <returns></returns>
        public SelectionCriteria PrepareSelectionCriteriaProperties(string propName, dynamic val, string op)
        {
            return PrepareSelectionCriteriaProperties(propName, val, op, null);
        }

        public SelectionCriteria PrepareSelectionCriteriaProperties(string propName, dynamic val, string op, string whrAndOr)
        {
            var wc = new WhereClauseDTO()
            {
                PropertyName = propName,
                Operator = op,
                PropertyValue = val,
                WhereAndOrCondition = whrAndOr
            };

            if (_clauseMembers == null)
                _clauseMembers = new List<WhereClauseDTO>();
            _clauseMembers.Add(wc);

            return this;
        }

        public IList<WhereClauseDTO> ReturnCollection()
        {
            return _clauseMembers ?? new List<WhereClauseDTO>();
        }

        /// <summary>
        /// Set an empty selection criteria; this will enable the system to return all items.
        /// </summary>
        /// <returns></returns>
        public IList<WhereClauseDTO> SetNoCriteria()
        {
            return new List<WhereClauseDTO>();
        }
    }
}
