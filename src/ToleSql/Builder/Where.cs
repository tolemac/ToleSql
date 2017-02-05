﻿namespace ToleSql.Builder
{
    public enum WhereOperator
    {
        And,
        Or
    }

    public class Where
    {
        public WhereOperator PreOperator { get; set; }
        public string Condition { get; set; }

        public Where(WhereOperator preOperator, string condition)
        {
            PreOperator = preOperator;
            Condition = condition;
        }
    }
}