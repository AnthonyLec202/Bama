using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Core.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Core.UseCases.Abstractions;

namespace Api.EndPoints;

public static class CustomerRoutes
{
    private static (CustomerOrder customerOrder, int customerId) GetCustomerOrderByCustomerId(ICustomerOrderUseCases customerOrderUseCases, HttpContext httpContext)
    {
        var customerIdClaim = httpContext.User.FindFirst(ClaimTypes.NameIdentifier);

        if (customerIdClaim == null)
        {
            throw new UnauthorizedAccessException("Customer is not authenticated.");
        }

        if (!int.TryParse(customerIdClaim.Value, out var customerId))
        {
            throw new ArgumentException("Invalid customer identifier format in token.");
        }

        return (customerOrderUseCases.GetCustomerOrderByCustomerId(customerId), customerId);
    }

    public static WebApplication AddCustomerRoutes(this WebApplication app)
    {
        var group = app.MapGroup("api/customers")
            .RequireAuthorization()
            .WithTags("Customers");

        group.MapGet("", (ICustomerUseCases customerUseCases) =>
        {
            var customers = customerUseCases.GetAllCustomers();
            return Results.Ok(customers);
        })
        .WithName("GetAllCustomers")
        .Produces<IEnumerable<Customer>>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status401Unauthorized)
        .Produces(StatusCodes.Status500InternalServerError);

        group.MapPost("/auth", ([FromBody] AuthenticationRequest request, ICustomerUseCases customerUseCases, IConfiguration configuration) =>
        {
            var customer = customerUseCases.AuthenticateAndGetCustomer(request);

            if (customer != null)
            {
                var issuer = configuration["Jwt:Issuer"];
                var audience = configuration["Jwt:Audience"];
                var key = Encoding.ASCII.GetBytes(configuration["Jwt:Key"]!);
                var expireTime = configuration["Jwt:ExpireTimeInMinutes"];
                var expiration = DateTime.UtcNow.AddMinutes(Convert.ToDouble(expireTime ?? "5"));

                var claims = new List<Claim>
                {
                    new Claim(JwtRegisteredClaimNames.Name, customer.Username),
                    new Claim(ClaimTypes.NameIdentifier, customer.CustomerId.ToString()),
                };

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(claims),
                    Expires = expiration,
                    Issuer = issuer,
                    Audience = audience,
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512Signature)
                };

                var tokenHandler = new JwtSecurityTokenHandler();
                var token = tokenHandler.CreateToken(tokenDescriptor);
                var jwtToken = tokenHandler.WriteToken(token);

                return Results.Ok(new { token = jwtToken });
            }
            else
            {
                return Results.Unauthorized();
            }
        })
        .AllowAnonymous()
        .WithName("Auth")
        .Produces<object>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status401Unauthorized)
        .Produces(StatusCodes.Status500InternalServerError);

        group.MapPost("/register", ([FromBody] RegisterRequest request, ICustomerUseCases customerUseCases) =>
        {
            customerUseCases.Register(request);
            return Results.Ok(new { message = "Customer registered successfully" });
        })
        .AllowAnonymous()
        .WithName("Register")
        .Produces<object>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status401Unauthorized)
        .Produces(StatusCodes.Status500InternalServerError);

        group.MapGet("me/order", (ICustomerOrderUseCases customerOrderUseCases, HttpContext httpContext) =>
        {
            var customerOrder = GetCustomerOrderByCustomerId(customerOrderUseCases, httpContext).customerOrder;
            return Results.Ok(customerOrder);
        })
        .WithName("GetMyOrder")
        .Produces<CustomerOrder>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status401Unauthorized)
        .Produces(StatusCodes.Status500InternalServerError);

        group.MapPost("me/order/items", ([FromBody] IEnumerable<Models.OrderItem> items, ICustomerOrderUseCases customerOrderUseCases, HttpContext httpContext) =>
        {
            var res = GetCustomerOrderByCustomerId(customerOrderUseCases, httpContext);
            var customerOrder = res.customerOrder;
            var customerId = res.customerId;

            customerOrderUseCases.AddOrUpdateItemsToCustomerOrder(customerOrder.CustomerOrderId, items.Select(b => new OrderItem
            {
                Burger = new Burger { BurgerId = b.BurgerId },
                Quantity = b.Quantity,
                Price = b.Price
            }));

            customerOrder = customerOrderUseCases.GetCustomerOrderByCustomerId(customerId);

            return Results.Ok(customerOrder);
        })
        .WithName("AddOrUpdateOrderItems")
        .Produces<CustomerOrder>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status401Unauthorized)
        .Produces(StatusCodes.Status500InternalServerError);

        group.MapDelete("me/order/items", (ICustomerOrderUseCases customerOrderUseCases, HttpContext httpContext) =>
        {
            var customerId = GetCustomerOrderByCustomerId(customerOrderUseCases, httpContext).customerId;

            customerOrderUseCases.ClearCustomerOrder(customerId);

            return Results.Ok();
        })
        .WithName("ClearCustomerOrder")
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status401Unauthorized)
        .Produces(StatusCodes.Status500InternalServerError);

        group.MapPost("me/order/finalize", (ICustomerOrderUseCases customerOrderUseCases, HttpContext httpContext) =>
        {
            var customerIdClaim = httpContext.User.FindFirst(ClaimTypes.NameIdentifier);
            if (customerIdClaim == null || !int.TryParse(customerIdClaim.Value, out var customerId))
            {
                return Results.Unauthorized();
            }

            try
            {
                customerOrderUseCases.FinalizeOrder(customerId);
                return Results.Ok(new { message = "Commande validée avec succès." });
            }
            catch (InvalidOperationException ex)
            {
                return Results.BadRequest(new { message = ex.Message });
            }
        })
        .WithName("FinalizeMyOrder")
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status401Unauthorized);

        
        group.MapGet("me/orders", (ICustomerOrderUseCases customerOrderUseCases, HttpContext httpContext) =>
        {
            var customerIdClaim = httpContext.User.FindFirst(ClaimTypes.NameIdentifier);
            if (customerIdClaim == null || !int.TryParse(customerIdClaim.Value, out var customerId))
            {
                return Results.Unauthorized();
            }

            var orders = customerOrderUseCases.GetAllOrdersByCustomerId(customerId);
            return Results.Ok(orders);
        })
        .WithName("GetMyOrders")
        .Produces<IEnumerable<CustomerOrder>>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status401Unauthorized);

         
        group.MapGet("me", (ICustomerUseCases customerUseCases, HttpContext httpContext) =>
        {
            var username = httpContext.User.Identity?.Name;
            if (string.IsNullOrEmpty(username))
            {
                return Results.Unauthorized();
            }
            var customer = customerUseCases.GetCustomerByUsername(username);
            return customer == null ? Results.NotFound() : Results.Ok(customer);
        })
        .WithName("GetMyInfo");

        
        group.MapPut("me", (Customer customerData, ICustomerUseCases customerUseCases, HttpContext httpContext) =>
        {
            var customerIdClaim = httpContext.User.FindFirst(ClaimTypes.NameIdentifier);
            if (customerIdClaim == null || !int.TryParse(customerIdClaim.Value, out var customerId))
            {
                return Results.Unauthorized();
            }

            customerData.CustomerId = customerId;
            customerUseCases.UpdateCustomer(customerData);
            return Results.Ok(new { message = "Informations mises à jour avec succès." });
        })
        .WithName("UpdateMyInfo");

        return app;
    }
}
