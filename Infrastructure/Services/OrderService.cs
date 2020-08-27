using System.Xml.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Entities.OrderAggregate;
using Core.Interfaces;
using Core.Entities;
using System.Linq;
using Core.Specifications;

namespace Infrastructure.Services
{
    public class OrderService : IOrderService
    {
        private readonly IBasketRepository _basketRepo;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPaymentService _paymentService;
        public OrderService(IUnitOfWork unitOfWork, IBasketRepository basketRepo,
        
        IPaymentService paymentService)
        {
            _paymentService = paymentService;
            _unitOfWork = unitOfWork;
            _basketRepo = basketRepo;
        }

    public async Task<Order> CreateOrderAsync(string buyerEmail, int deliveryMethodId,
    string basketId, Address shippingAddress)
    {
        // get basket from basket repo
        var basket = await _basketRepo.GetBasketAsync(basketId);
        // we dont trust whats in the basket, we cant trust what the price has been set to
        var items = new List<OrderItem>();
        foreach (var item in basket.Items)
        {
            var productItem = await _unitOfWork.Repository<Product>().GetByIdAsync(item.Id);
            var itemOrdered = new ProductItemOrdered(productItem.Id, productItem.Name,
            productItem.PictureUrl
            );
            var orderItem = new OrderItem(itemOrdered, productItem.Price, item.Quantity);
            // we are concerned about the price
            items.Add(orderItem);

        }
        // we need to check with whats inside our database
        // get delivery method from repo
        var deliveryMethod = await _unitOfWork.Repository<DeliveryMethod>().GetByIdAsync(deliveryMethodId);
        // calc subtotal
        var subtotal = items.Sum(item => item.Price * item.Quantity);
        //check to see if order exists
        var spec = new OrderByPaymentIdSpecification(basket.PaymentIntentId);
        var existingOrder = await _unitOfWork.Repository<Order>().GetEntityWithSpec(spec);
        if (existingOrder != null)
        {
            _unitOfWork.Repository<Order>().Delete(existingOrder);
            await _paymentService.CreateOrderUpdatePaymentIntent(basket.PaymentIntentId);
        }
        // create order
        var order = new Order(items, buyerEmail, shippingAddress, deliveryMethod, subtotal,
        basket.PaymentIntentId);
        // save to the db
        _unitOfWork.Repository<Order>().Add(order);

        var result = await _unitOfWork.Complete();
        // any changes here will be rolled back if there is an error
        if (result <= 0) return null;

        // delete basket
        
        return order;
    }

    public async Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodAsync()
    {
        return await _unitOfWork.Repository<DeliveryMethod>().ListAllAsync();
    }

    public async Task<Order> GetOrderByIdAsync(int id, string buyerEmail)
    {
        var spec = new OrdersWithItemsAndOrderingSpecification(id, buyerEmail);
        return await _unitOfWork.Repository<Order>().GetEntityWithSpec(spec);

    }

    public async Task<IReadOnlyList<Order>> GetOrdersForUserAsync(string buyerEmail)
    {
        var spec = new OrdersWithItemsAndOrderingSpecification(buyerEmail);
        return await _unitOfWork.Repository<Order>().ListOnlyAsync(spec);
    }
}
}