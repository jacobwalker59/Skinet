using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Core.Entities;

namespace API.DTOs
{
    public class CustomerBasketDTO
    {
        [Required]
        public string Id { get; set; }
        // create unique identifier for each basket that we make
        public List<BasketItemDTO> Items {get;set;} = new List<BasketItemDTO>();

    }
}