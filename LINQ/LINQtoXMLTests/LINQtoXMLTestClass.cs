/*
1.	Выдайте список всех клиентов, чей суммарный оборот (сумма всех заказов)
        превосходит некоторую величину X. Продемонстрируйте выполнение запроса
        с различными X (подумайте, можно ли обойтись без копирования запроса несколько раз)
2.	Сгруппировать клиентов по странам.
3.	Найдите всех клиентов, у которых были заказы, превосходящие по сумме величину X
4.	Выдайте список клиентов с указанием, начиная с какого месяца какого года
        они стали клиентами (принять за таковые месяц и год самого первого заказа)
5.	Сделайте предыдущее задание, но выдайте список отсортированным
        по году, месяцу, оборотам клиента (от максимального к минимальному) и имени клиента
6.	Укажите всех клиентов, у которых указан нецифровой код, или не заполнен регион,
        или в телефоне не указан код оператора (для простоты считаем,
        что это равнозначно «нет круглых скобочек в начале»).
7.	Рассчитайте среднюю прибыльность каждого города (среднюю сумму заказа
        по всем клиентам из данного города) и среднюю интенсивность
        (среднее количество заказов, приходящееся на клиента из каждого города)
8.	Сделайте среднегодовую статистику активности клиентов по месяцам (без учета года),
        статистику по годам, по годам и месяцам (т.е. когда один месяц в разные годы имеет своё значение).
 */

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using LINQtoXML;
using System.Collections.Generic;

namespace LINQtoXMLTests
{
    [TestClass]
    public class LINQtoXMLTestClass
    {
        [TestMethod]
        public void TestOrdersSum()
        {
            Customers customers = new Customers();
            customers.ReadDataFromXML();
            List<double> values = new List<double>() { 10, 60000, 100000 };
            List<int> results = new List<int>();
            foreach (var value in values)
                results.Add(customers.GetCustomersWithMoreThanXOrdersSum(value).Count);
            for (int i = 0, j = 0; i < values.Count; i++, j++)
                if (i < j)
                    Assert.AreNotEqual(results[i], results[j], 1, $"Amounts of customers are equal for orders sum {values[i]} and {values[j]}");
        }

        [TestMethod]
        public void TestCountriesGrouping()
        {
            Customers customers = new Customers();
            customers.ReadDataFromXML();
            int groupMembersCount1 = customers.GetGroupedByCountryCustomers().Count;
            string country = "Germany";
            customers.CustomersList.Add(new Customer(
                        "Some ID", "John", "Some Address", "Leningrad", "Postal Code", country, "123456789", "123456789",
                        null, new List<Order>() { new Order("ID", DateTime.Now, 9876543.21) }));
            int groupMembersCount2 = customers.GetGroupedByCountryCustomers().Count;
            Assert.AreEqual(groupMembersCount1, groupMembersCount2, 0, "Amount of customers groups has changed when shouldn't");
        }

        [TestMethod]
        public void TestMoreThanXOrders()
        {
            Customers customers = new Customers();
            customers.ReadDataFromXML();
            List<double> values = new List<double>() { 10, 50000, 145000 };
            List<int> results = new List<int>();
            foreach (var value in values)
                results.Add(customers.GetCustomersWithMoreThanXOrder(value).Count);
            for (int i = 0, j = 0; i < values.Count; i++, j++)
                if (i < j)
                    Assert.AreNotEqual(results[i], results[j], 1, $"Amounts of customers are equal for existing orders more than {values[i]} and {values[j]}");
        }

        [TestMethod]
        public void TestFirstOrders()
        {
            Customers customers = new Customers();
            customers.ReadDataFromXML();
            var firstOrdersCount1 = customers.GetListOfFirstOrders().Count;
            customers.CustomersList.Add(new Customer("Some ID", "John", "Some Address", "Moscow",
                        "Postal Code", "Russia", "123456789", "123456789", null,
                        new List<Order>() { new Order("ID", DateTime.Now, 9876543.21) }));
            var firstOrdersCount2 = customers.GetListOfFirstOrders().Count;
            Assert.AreNotEqual(firstOrdersCount1, firstOrdersCount2, 0, "Wrong first orders checking");
        }

        [TestMethod]
        public void TestSortedFirstOrders()
        {
            Customers customers = new Customers();
            customers.ReadDataFromXML();
            var firstOrdersSortedCount1 = customers.GetSortedListOfFirstOrders().Count;
            customers.CustomersList.Add(new Customer("Some ID", "John", "Some Address", "Moscow",
                                            "Postal Code", "Russia", "123456789", "123456789", null,
                                            new List<Order>() { new Order("ID", DateTime.Now, 9876543.21) }));
            var firstOrdersSortedCount2 = customers.GetListOfFirstOrders().Count;
            Assert.AreNotEqual(firstOrdersSortedCount1, firstOrdersSortedCount2, 0, "Wrong first orders checking");
        }

        [TestMethod]
        public void TestPostalCodes()
        {
            Customers customers = new Customers();
            customers.ReadDataFromXML();
            int customersWithSpecialCodeCount1 = customers.GetCustomersLettersInPostalCodeNoRegionNoOperatorCode().Count;
            customers.CustomersList.Add(new Customer("Some ID", "John", "Some Address", "Moscow",
                                            "hhjk234567534", "Russia", "()123456789", "123456789", "region", null));
            int customersWithSpecialCodeCount2 = customers.GetCustomersLettersInPostalCodeNoRegionNoOperatorCode().Count;
            Assert.AreNotEqual(customersWithSpecialCodeCount1, customersWithSpecialCodeCount2, 0, "Wrong postal code is accepted");
        }

        [TestMethod]
        public void TestOperatorCodes()
        {
            Customers customers = new Customers();
            customers.ReadDataFromXML();
            int customersWithSpecialCodeCount1 = customers.GetCustomersLettersInPostalCodeNoRegionNoOperatorCode().Count;
            customers.CustomersList.Add(new Customer("Some ID", "John", "Some Address", "Moscow",
                                            "234567534", "Russia", ")123456789", "123456789", "region", null));
            int customersWithSpecialCodeCount2 = customers.GetCustomersLettersInPostalCodeNoRegionNoOperatorCode().Count;
            Assert.AreNotEqual(customersWithSpecialCodeCount1, customersWithSpecialCodeCount2, 0, "Wrong operator code is accepted");
        }

        [TestMethod]
        public void TestRegions()
        {
            Customers customers = new Customers();
            customers.ReadDataFromXML();
            int customersWithSpecialCodeCount1 = customers.GetCustomersLettersInPostalCodeNoRegionNoOperatorCode().Count;
            customers.CustomersList.Add(new Customer("Some ID", "John", "Some Address", "Moscow",
                                            "234567534", "Russia", "()123456789", "123456789", null, null));
            int customersWithSpecialCodeCount2 = customers.GetCustomersLettersInPostalCodeNoRegionNoOperatorCode().Count;
            Assert.AreNotEqual(customersWithSpecialCodeCount1, customersWithSpecialCodeCount2, 0, "Wrong postal code is accepted");
        }

        [TestMethod]
        public void TestCities()
        {
            Customers customers = new Customers();
            customers.ReadDataFromXML();
            var city1 = customers.GetCitiesAverageProfitabilityAverageIntencity()[0];
            customers.CustomersList.Add(new Customer("Some ID", "John", "Some Address", city1.CityName, "654346436", "Russia", "123456789",
                                            "123456789", null, new List<Order>() { new Order("ID", DateTime.Now, 9876543.21) }));
            var city2 = customers.GetCitiesAverageProfitabilityAverageIntencity()[0];
            Assert.AreNotEqual(city1.Profitability, city2.Profitability, 1, "Wrong profitability counting");
        }

        [TestMethod]
        public void TestMonthsStatistics()
        {
            Customers customers = new Customers();
            customers.ReadDataFromXML();
            int yearStatisticsCount1 = customers.GetStatisticsByMonths().Count;
            customers.CustomersList.Add(new Customer("Some ID", "John", "Some Address", "Moscow", "534662342", "Russia", "123456789",
                                            "123456789", null, new List<Order>() { new Order("ID", DateTime.Now, 9876543.21) }));
            int yearStatisticsCount2 = customers.GetStatisticsByYears().Count;
            Assert.AreNotEqual(yearStatisticsCount1, yearStatisticsCount2, 0, "Wrong month statistics counting");
        }

        [TestMethod]
        public void TestYearStatistics()
        {
            Customers customers = new Customers();
            customers.ReadDataFromXML();
            int yearStatisticsCount1 = customers.GetStatisticsByYears().Count;
            customers.CustomersList.Add(new Customer(
                "Some ID", "John", "Some Address", "Leningrad", "123444311", "Russia", "123456789", "123456789",
                null, new List<Order>() { new Order("ID", DateTime.Now, 9876543.21) }));
            int yearStatisticsCount2 = customers.GetStatisticsByYears().Count;
            Assert.AreNotEqual(yearStatisticsCount1, yearStatisticsCount2, 0, "Wrong year statistics counting");
        }

        [TestMethod]
        public void TestMonthsYearsStatistics()
        {
            Customers customers = new Customers();
            customers.ReadDataFromXML();
            int yearStatisticsCount1 = customers.GetStatisticsByYearAndMonth().Count;
            customers.CustomersList.Add(new Customer(
                "Some ID", "John", "Some Address", "Leningrad", "241241222", "Russia", "123456789", "123456789",
                null, new List<Order>() { new Order("ID", DateTime.Now, 9876543.21) }));
            int yearStatisticsCount2 = customers.GetStatisticsByYears().Count;
            Assert.AreNotEqual(yearStatisticsCount1, yearStatisticsCount2, 0, "Wrong year and month statistics counting");
        }
    }
}