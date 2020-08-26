using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Core.Specifications
{
    public class BaseSpecification<T> : ISpecification<T>
    {
        public BaseSpecification()
        {
        }

        public BaseSpecification(Expression<Func<T, bool>> criteria)
        {
            //generalised specification is going to be replaced with an actual expression from each
            //of the specifications
            Criteria = criteria;
            
        }

        public Expression<Func<T, bool>> Criteria {get;}

        public List<Expression<Func<T, object>>> Includes {get;} = new List<Expression<Func<T, object>>>();

        public Expression<Func<T, object>> OrderBy {get; private set;}

        public Expression<Func<T, object>> OrderByDescending {get; private set;}

        public int Take {get; private set;}

        public int Skip  {get; private set;}

        public bool IsPagingEnabled {get; private set;}

        //at the moment our products have two includes statements for the differnt product types and brands
        //includes is a list of include statements that we can pass to our to list async method

        protected void AddInclude(Expression<Func<T, object>> includeExpression){
            Includes.Add(includeExpression);
            //gets the add method because expression is directly taken form linq
        }

        protected void AddOrderBy(Expression<Func<T,object>> orderByExpression)
        {
            OrderBy = orderByExpression;
        }

        protected void AddOrderByDescending(Expression<Func<T,object>> orderByDescendingExpression)
        {
            OrderByDescending = orderByDescendingExpression;
        }

        //these methods need speciifed by our evaluator. 
        protected void ApplyPaging(int skip, int take)
        {
            Skip = skip;
            Take = take;
            IsPagingEnabled = true;
        }
        
            
    }
}