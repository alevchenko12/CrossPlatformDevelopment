﻿namespace LAB5.Models
{
    public class Customer
    {
        public int CustomerId { get; set; }
        public string PaymentMethodCode { get; set; }
        public string CustomerCode { get; set; }
        public string CustomerName { get; set; }
        public string CustomerAddress { get; set; }
        public string CustomerEmail { get; set; }
        public string CustomerPhone { get; set; }
        public string OtherCustomerDetails { get; set; }

        public ICollection<CustomerOrder> CustomerOrders { get; set; }
    }
}
