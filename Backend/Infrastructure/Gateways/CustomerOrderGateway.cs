using Core.IGateways;
using Core.Models;
using Infrastructure.Repositories.Abstractions;


namespace Infrastructure.Gateways;

public class CustomerOrderGateway : ICustomerOrderGateway
{
    private readonly IBurgerRepository _burgerRepository;
    private readonly ICustomerOrderRepository _customerOrderRepository;

    public CustomerOrderGateway(IBurgerRepository burgerRepository, ICustomerOrderRepository customerOrderRepository)
    {
        _burgerRepository = burgerRepository ?? throw new ArgumentNullException(nameof(burgerRepository));
        _customerOrderRepository = customerOrderRepository ?? throw new ArgumentNullException(nameof(customerOrderRepository));
    }



    public void AddOrUpdateItemsToCustomerOrder(int customerOrderId, IEnumerable<OrderItem> items)
    {
        var itemsDb = items.Select(i => new Models.OrderItem
        {
            Quantity = i.Quantity,
            BurgerId = i.Burger.BurgerId,
            Price = i.Price
        });

        var itemsToAdd = new List<Models.OrderItem>();
        var itemsToUpdate = new List<Models.OrderItem>();

        foreach (var item in itemsDb)
        {
            var orderItem = _customerOrderRepository.GetOrderItemById(customerOrderId, item.BurgerId);

            if (orderItem == null)
            {
                itemsToAdd.Add(item);
            }
            else
            {
                orderItem.Quantity += item.Quantity;
                orderItem.Price = item.Price;

                itemsToUpdate.Add(orderItem);
            }
        }

        if (itemsToAdd.Count != 0)
        {
            _customerOrderRepository.AddItemsToCustomerOrder(customerOrderId, itemsToAdd);
        }
        if (itemsToUpdate.Count != 0)
        {
            _customerOrderRepository.UpdateItemsInCustomerOrder(customerOrderId, itemsToUpdate);
        }
    }

    public void ClearCustomerOrder(int customerOrderId)
    {
        _customerOrderRepository.ClearCustomerOrder(customerOrderId);
    }

    public CustomerOrder CreateCustomerOrder(int customerId)
    {
        var customerOrderDb = _customerOrderRepository.CreateCustomerOrder(customerId);
        var customerOrder = new CustomerOrder
        {
            CustomerOrderId = customerOrderDb.CustomerOrderId,
            OrderItems = new List<OrderItem>()
        };

        return customerOrder;
    }

    public CustomerOrder? GetCustomerOrderByCustomerId(int customerId)
    {
        CustomerOrder? res = null;
        var customerOrderDb = _customerOrderRepository.GetCustomerOrderByCustomerId(customerId);

        if (customerOrderDb != null)
        {
            res = new CustomerOrder
            {
                CustomerOrderId = customerOrderDb.CustomerOrderId,
                OrderItems = customerOrderDb.OrderItems.Select(i =>
                {
                    return new OrderItem
                    {
                        Burger = new Burger
                        {
                            BurgerId = i.BurgerId,
                        },
                        Quantity = i.Quantity,
                        Price = i.Price
                    };
                }).ToList()
            };

            foreach (var item in res.OrderItems)
            {
                var burger = _burgerRepository.GetBurgerById(item.Burger.BurgerId);
                if (burger != null)
                {
                    item.Burger = new Burger
                    {
                        BurgerId = burger.BurgerId,
                        Name = burger.Name,
                        Description = burger.Description,
                        Price = burger.Price,
                        Stock = burger.Stock,
                    };
                }
            }
        }

        return res;
    }

    public void FinalizeOrder(CustomerOrder order)
    {
        foreach (var item in order.OrderItems)
        {
            _burgerRepository.UpdateStock(item.Burger.BurgerId, item.Quantity);
        }
        _customerOrderRepository.UpdateOrderStatus(order.CustomerOrderId, "Completed");
    }
    public IEnumerable<CustomerOrder> GetAllOrdersByCustomerId(int customerId)
    {
        var customerOrdersDb = _customerOrderRepository.GetAllOrdersByCustomerId(customerId);
        var customerOrders = new List<CustomerOrder>();

        foreach (var orderDb in customerOrdersDb)
        {
            var order = new CustomerOrder
            {
                CustomerOrderId = orderDb.CustomerOrderId,
                OrderItems = orderDb.OrderItems.Select(i =>
                {
                    return new OrderItem
                    {
                        Burger = new Burger
                        {
                            BurgerId = i.BurgerId,
                        },
                        Quantity = i.Quantity,
                        Price = i.Price
                    };
                }).ToList()
            };

            foreach (var item in order.OrderItems)
            {
                var burger = _burgerRepository.GetBurgerById(item.Burger.BurgerId);
                if (burger != null)
                {
                    item.Burger = new Burger
                    {
                        BurgerId = burger.BurgerId,
                        Name = burger.Name,
                        Description = burger.Description,
                        Price = burger.Price,
                        Stock = burger.Stock,
                    };
                }
            }

            customerOrders.Add(order);
        }

        return customerOrders;
    }
}
