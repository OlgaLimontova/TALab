using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace LINQtoXML
{
    public class Customers
    {
        public List<Customer> CustomersList { get; set; }

        public Customers() { this.CustomersList = new List<Customer>(); }

        public Customers(List<Customer> list) { this.CustomersList = list; }

        public void ReadDataFromXML()
        {
            XMLHandler handler = new XMLHandler();
            foreach (XElement element in handler.RootElement.Elements())
                CustomersList.Add(new Customer(element));
        }

        public List<Customer> GetCustomersWithMoreThanXOrdersSum(double value)
        {
            return CustomersList
                .Where(customer => customer.GetOrdersSum() > value)
                .ToList();
        }

        public List<IGrouping<string, Customer>> GetGroupedByCountryCustomers()
        {
            return CustomersList
                .GroupBy(element => element.Country)
                .ToList();
        }

        public List<Customer> GetCustomersWithMoreThanXOrder(double value)
        {
            return CustomersList
                .Where(customer => customer.GetMaxOrder() > value)
                .ToList();
        }

        public Dictionary<string, DateTime?> GetListOfFirstOrders()
        {
            return CustomersList
                .ToDictionary(x => x.Name, c => c.Orders.Min(m => m?.OrderDate));     
        }

        public Dictionary<string, DateTime?> GetSortedListOfFirstOrders()
        {
            var SortedList = CustomersList
                .ToDictionary(x => x, c => c.Orders.Min(m => m?.OrderDate));
            return SortedList
                .OrderBy(d => d.Value)
                .ThenByDescending(x => x.Key.GetOrdersSum())
                .ThenBy(x => x.Key.Name)
                .ToDictionary(d => d.Key.Name, c => c.Value);
        }

        public List<Customer> GetCustomersLettersInPostalCodeNoRegionNoOperatorCode()
        {
            int result;
            return CustomersList
                .Where(element => int.TryParse(element.PostalCode, out result) == false)
                .ToList()
                .Union(CustomersList
                    .Where(element => element.Region == null))
                .ToList()
                .Union(CustomersList
                    .Where(element => element.Phone.Trim()[0] != '('))
                .ToList()
                .Distinct()
                .ToList();
        }

        List<string> GetCitiesNamesList()
        {
            List<string> citiesList = new List<string>();
            foreach (Customer customer in CustomersList)
                if (!citiesList.Contains(customer.City))
                    citiesList.Add(customer.City);
            return citiesList;
        }

        public List<City> GetCitiesAverageProfitabilityAverageIntencity()
        {
            List<City> cities = new List<City>();
            List<string> citiesNamesList = GetCitiesNamesList();
            foreach (string city in citiesNamesList)
            {
                double averageProfitability = CustomersList
                    .Where(element => element.City == city)
                    .ToList()
                    .Average(element => element.GetOrdersSum());
                double averageIntencity = CustomersList
                    .Where(element => element.City == city)
                    .ToList()
                    .Average(element => element.Orders.Count);
                cities.Add(new City(city, averageProfitability, averageIntencity));
            }
            return cities;
        }

        public List<IGrouping<int, OrderWithCustomer>> GetStatisticsByMonths()
        {
            List<OrderWithCustomer> ordersWithCustomers = new List<OrderWithCustomer>();
            foreach (Customer customer in CustomersList)
                foreach (Order order in customer.Orders)
                    ordersWithCustomers.Add(new OrderWithCustomer(customer, order));
            return ordersWithCustomers
                .GroupBy(element => element.Order.OrderDate.Month)
                .ToList();
        }

        public List<IGrouping<int, OrderWithCustomer>> GetStatisticsByYears()
        {
            List<OrderWithCustomer> ordersWithCustomer = new List<OrderWithCustomer>();
            foreach (Customer customer in CustomersList)
                foreach (Order order in customer.Orders)
                    ordersWithCustomer.Add(new OrderWithCustomer(customer, order));
            return ordersWithCustomer
                .GroupBy(element => element.Order.OrderDate.Year)
                .ToList();
        }

        public List<IGrouping<string, OrderWithCustomer>> GetStatisticsByYearAndMonth()
        {
            List<OrderWithCustomer> ordersWithCustomer = new List<OrderWithCustomer>();
            foreach (Customer customer in CustomersList)
                foreach (Order order in customer.Orders)
                    ordersWithCustomer.Add(new OrderWithCustomer(customer, order));
            return ordersWithCustomer
                .GroupBy(element => element.Order.OrderDate.ToString("Y"))
                .ToList();
        }
    }
}