using System;
using System.Xml.Linq;

namespace LINQtoXML
{
    public class Order
    {
        public XElement OrderElement;
        public string ID { get; set; }
        public DateTime OrderDate { get; set; }
        public double Total { get; set; }

        public Order()
        {
            ID = null;
            OrderDate = default(DateTime);
            Total = 0.0;
        }

        public Order(XElement element)
        {
            OrderElement = element;
            GetDataFromXElement();
        }

        public Order(string id, DateTime orderDate, double total)
        {
            ID = id;
            OrderDate = orderDate;
            Total = total;
        }

        public void GetDataFromXElement()
        {
            ID = OrderElement.Element("id").Value;
            OrderDate = DateTime.Parse(OrderElement.Element("orderdate").Value);
            Total = double.Parse(OrderElement.Element("total").Value.Replace('.', ','));
        }

        public override string ToString()
        {
            return $"\t{ID}\n\t{OrderDate}\n\t{Total.ToString("0.00")}\n";
        }
    }
}