using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace LINQtoXML
{
    public class Customer
    {
        public XElement CustomerElement { get; set; }
        public string ID { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public string Region { get; set; }
        public List<Order> Orders { get; set; }

        public Customer()
        {
            ID = null;
            Name = null;
            Address = null;
            City = null;
            PostalCode = null;
            Country = null;
            Phone = null;
            Fax = null;
            Region = null;
            Orders = null;
        }

        public Customer(XElement element)
        {
            CustomerElement = element;
            Orders = new List<Order>();
            GetDataFromXElement();
        }

        public Customer(string id, string name, string address, string city, string postalCode,
                            string country, string phone, string fax, string region, List<Order> orders)
        {
            ID = id;
            Name = name;
            Address = address;
            City = city;
            PostalCode = postalCode;
            Country = country;
            Phone = phone;
            Fax = fax;
            Region = region;
            Orders = orders;
        }

        void GetDataFromXElement()
        {
            ID = CustomerElement.Element("id")?.Value;
            Name = CustomerElement.Element("name")?.Value;
            Address = CustomerElement.Element("address")?.Value;
            City = CustomerElement.Element("city")?.Value;
            PostalCode = CustomerElement.Element("postalcode")?.Value;
            Country = CustomerElement.Element("country")?.Value;
            Phone = CustomerElement.Element("phone")?.Value;
            Fax = CustomerElement.Element("fax")?.Value;
            Region = CustomerElement.Element("region")?.Value;
            var orders = CustomerElement.Element("orders").Elements("order").ToList();
            foreach (XElement element in orders)
                Orders.Add(new Order(element));
        }

        public double GetOrdersSum()
        {
            return Orders.Sum(order => order.Total);
        }

        public double GetMaxOrder()
        {
            if (Orders.Count == 0)
                return 0.0;
            return Orders.Max(order => order.Total);
        }

        public override string ToString()
        {
            string customerString = $"{ID}\n{Name}\n{Address}\n{City}\n{PostalCode}\n{Country}\n{Phone}\n{Fax}\n";
            foreach (Order order in Orders)
                customerString += $"{order}\n";
            return customerString;
        }
    }
}