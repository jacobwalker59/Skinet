using System.Linq;
using Core.Entities;
using Core.Specifications;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class SpecificationEvaluator<TEntity> where TEntity:BaseEntity
    {
        public static IQueryable<TEntity> GetQuery(IQueryable<TEntity> inputQuery, 
        ISpecification<TEntity> spec)
        {
            var query = inputQuery;

            //assume it starts off an an empty query

            if(spec.Criteria != null)
            {
                query = query.Where(spec.Criteria);
                //query is an iqueryable which is part of linq, all your doing
                //is getting an empty queryable and inserting new information into it as and when
                //beginning with the where statement
            }

             if(spec.OrderBy != null)
            {
                query = query.OrderBy(spec.OrderBy);
                //does exactly the same for the orderby, your just inserting it 
                //into its name sake for linq.
                //remember everything inherits from base specification

            }

             if(spec.OrderByDescending != null)
            {
                query = query.OrderBy(spec.OrderByDescending);
                //can do this for as many linq statement as required. 

            }

            if(spec.IsPagingEnabled)
            {
                query = query.Skip(spec.Skip).Take(spec.Take);
                //pattern is getting there now
            }

            query = spec.Includes.Aggregate(query, (current, include) => current.Include(include));
            //check how the aggregate statement works but for all intents and purposes this is it in a nutshell
            //------------------------------SEE ABOVE -----------------------------------------------

            return query;

            /*
             return await _context.Products
            .Include(x => x.ProductType)
            .Include(x => x.ProductBrand)
            .ToListAsync();
            */


        }
    }
}