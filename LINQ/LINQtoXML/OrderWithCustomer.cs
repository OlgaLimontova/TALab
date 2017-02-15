namespace LINQtoXML
{
    public class OrderWithCustomer
    {
        public Order Order { get; set; }
        public string ID { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public string Region { get; set; }

        public OrderWithCustomer(Customer customer, Order order)
        {
            Order = order;
            ID = customer.ID;
            Name = customer.Name;
            Address = customer.Address;
            City = customer.City;
            PostalCode = customer.PostalCode;
            Country = customer.Country;
            Phone = customer.Phone;
            Fax = customer.Fax;
            Region = customer.Region;
        }

        public override string ToString()
        {
            return $"{ Name}\n{Order}\n";
        }
    }
}