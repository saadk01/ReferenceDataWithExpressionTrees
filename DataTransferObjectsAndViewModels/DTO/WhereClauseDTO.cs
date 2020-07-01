using System;
using System.Collections.Generic;
using System.Text;

namespace DataTransferObjectsAndViewModels.DTO
{
    public class WhereClauseDTO
    {
        public string PropertyName { get; set; }
        public string Operator { get; set; }
        public dynamic PropertyValue { get; set; }
        public string WhereAndOrCondition { get; set; }
    }
}
