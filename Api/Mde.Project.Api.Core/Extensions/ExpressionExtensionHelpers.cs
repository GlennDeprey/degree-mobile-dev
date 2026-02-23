using System.Collections;
using System.Linq.Expressions;
namespace Mde.Project.Api.Core.Extensions
{
    public static class ExpressionExtensionHelpers
    {
        public static Expression<Func<T, bool>> AndAlso<T>(this Expression<Func<T, bool>> expr1, Expression<Func<T, bool>> expr2)
        {
            var parameter = Expression.Parameter(typeof(T));

            var combined = new ReplaceParameterVisitor
            {
                { expr1.Parameters[0], parameter },
                { expr2.Parameters[0], parameter }
            }.Visit(Expression.AndAlso(expr1.Body, expr2.Body));

            return Expression.Lambda<Func<T, bool>>(combined, parameter);
        }

        private class ReplaceParameterVisitor : ExpressionVisitor, IEnumerable<ParameterExpression>
        {
            private readonly Dictionary<ParameterExpression, ParameterExpression> _map = new Dictionary<ParameterExpression, ParameterExpression>();

            public void Add(ParameterExpression from, ParameterExpression to)
            {
                _map[from] = to;
            }

            protected override Expression VisitParameter(ParameterExpression node)
            {
                if (_map.TryGetValue(node, out var replacement))
                {
                    node = replacement;
                }
                return base.VisitParameter(node);
            }

            public IEnumerator<ParameterExpression> GetEnumerator()
            {
                throw new NotImplementedException();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                throw new NotImplementedException();
            }
        }
    }
}
