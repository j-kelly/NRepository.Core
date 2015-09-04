﻿namespace NRepository.Core.Query.Specification
{
    using System.Collections.Generic;
    using System.Linq.Expressions;

    /// <summary>
    /// Helper for rebinder parameters without use Invoke method in expressions 
    /// ( this methods is not supported in all linq query providers, 
    /// for example in Linq2Entities is not supported)
    /// </summary>
    public sealed class ParameterRebinder : ExpressionVisitor
    {
        /// <summary>
        /// The mapper.
        /// </summary>
        private readonly Dictionary<ParameterExpression, ParameterExpression> _Map;

        /// <summary>
        /// Initialises a new instance of the <see cref="ParameterRebinder"/> class.
        /// </summary>
        /// <param name="map">Map specification</param>
        public ParameterRebinder(Dictionary<ParameterExpression, ParameterExpression> map)
        {
            _Map = map ?? new Dictionary<ParameterExpression, ParameterExpression>();
        }
        
        /// <summary>
        /// Replaces parameters in expression with a Map information
        /// </summary>
        /// <param name="map">Map information</param>
        /// <param name="exp">Expression to replace parameters</param>
        /// <returns>Expression with parameters replaced</returns>
        public static Expression ReplaceParameters(Dictionary<ParameterExpression, ParameterExpression> map, Expression exp)
        {
            return new ParameterRebinder(map).Visit(exp);
        }
        
        /// <summary>
        /// Visit pattern method
        /// </summary>
        /// <param name="p">A Parameter expression</param>
        /// <returns>New visited expression</returns>
        protected override Expression VisitParameter(ParameterExpression p)
        {
            ParameterExpression replacement;
            if (_Map.TryGetValue(p, out replacement))
            {
                p = replacement;
            }

            return base.VisitParameter(p);
        }
    }
}