using System;
using System.Linq.Expressions;
using Core.Entities;

namespace Core.Specifications
{
    public class ProductWithTypesAndBrandsSpecification :BaseSpecification<Product>
    {
        //the above we need a constructor that has no parameters
        //its taking care of the include statements and then the where
        public ProductWithTypesAndBrandsSpecification(ProductSpecParams specParams):base(x =>
        (string.IsNullOrEmpty(specParams.Search) || x.Name.ToLower().Contains
        (specParams.Search)) &&
        (!specParams.BrandId.HasValue || x.ProductBrandId == specParams.BrandId) && 
        (!specParams.TypeId.HasValue || x.ProductTypeId == specParams.TypeId)
        )
        
        
        {
            // when we filter something we have base specification
            //goes from base specification, look below, the base includes an expression tree that we can pass into it
            //criteria = criteria, meaning that the criteria variable is set equal to what is passed in the linq
            // CRITERIA ACTS AS A WHERE CLAUSE!!!
            /*
             public BaseSpecification(Expression<Func<T, bool>> criteria)
        {
            //generalised specification is going to be replaced with an actual expression from each
            //of the specifications
            Criteria = criteria;
        }
            */


            AddInclude(x => x.ProductType);
            AddInclude(x => x.ProductBrand);
            AddOrderBy(x => x.Name);
            ApplyPaging(specParams.PageSize *(specParams.PageIndex -1), specParams.PageSize);
            //page size time page index -1 because if page is 1, and if where multiplying 5 x1, we dont want
            //to skip the first page x 1. 
            //so 5 x 0 is still zero
            if(!string.IsNullOrEmpty(specParams.Sort))
            {
                switch(specParams.Sort)
                {
                    case "priceAsc":
                    AddOrderBy(p => p.Price);
                    break;

                    case "priceDesc":
                    AddOrderByDescending(p => p.Price);
                    break;

                    default:
                    AddOrderBy(n => n.Name);
                    break;

                }
            }
        }

        public ProductWithTypesAndBrandsSpecification(int id) 
        : base(x => x.Id == id)
        {
            AddInclude(x => x.ProductType);
            AddInclude(x => x.ProductBrand);
        }
    }
}