﻿namespace MicroDelivery.Customers.Api.Models
{
    public class Customer
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public bool IsPremium { get; set; }
        public Address Address { get; set; } = new Address();
    }
}
