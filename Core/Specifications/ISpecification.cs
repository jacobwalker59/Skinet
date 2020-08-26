using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Core.Specifications
{
    //so you have the ISpecification which is inhereited by Base Specification
    //Specification Evaluator, which is used to aggregate all the includes and where statements into one IQueryable
    //specificiation evaulator is applied in generic repository
    //the generic repository uses the BaseSpecification's static getQuery method, which takes in a query and the specification that you 
    //want to match it against
    //a spec for each unique specification is created and inherits from base specification
    //this takes in the entity that you will be surrounding the spec with
    //it has two constructors, one with just includes methods for products and one without parameters
    //which will be used for individual products
    //this one takes from the base criteria, initially passed into the base constructor 
    //so we have an integer id, which is then passed into the base specification's criteria/where
    
    
    public interface ISpecification<T> 
    {
        Expression<Func<T,bool>> Criteria {get;}
        //acts as a where statement
        List<Expression<Func<T, Object>>> Includes {get;}
        Expression<Func<T, object>> OrderBy {get;}
        Expression<Func<T, object>> OrderByDescending {get;}
        int Take {get;}
        int Skip {get;}
        bool IsPagingEnabled {get;}

    }
}