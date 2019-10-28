using System;
using System.Linq.Expressions;

namespace EntityFrameworkCore.TemporalTables.Query
{
    public class ForSystemTimeExpression : Expression
    {
        public override Type Type => typeof(ForSystemTimeExpression);
    }
}
