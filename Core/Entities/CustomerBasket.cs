using System.Collections.Generic;

namespace Core.Entities
{
    public class CustomerBasket
    {
        public CustomerBasket()
        {
        }

        public CustomerBasket(string id)
        {
            Id = id;
        }

        public string Id { get; set; }
        // create unique identifier for each basket that we make
        public List<BasketItem> Items {get;set;} = new List<BasketItem>();

    }
}