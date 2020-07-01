namespace Utilities.Constants
{
    public class ProjectConstants
    {
        public enum ReferenceDataTypes
        {
            Province,
            Season
        }

        public class Operators
        {
            public class Comparison
            {
                public const string EQUAL = "==";
                public const string NOT_EQUAL = "!=";
                public const string LESS = "<";
                public const string LESS_EQ = "<=";
                public const string GREATER = ">";
                public const string GREATER_EQ = ">=";
                public const string LIKE = "LIKE";
            }

            public class SqlWhere
            {
                public const string AND = "AND";
                public const string OR = "OR";
            }
        }
    }
}
