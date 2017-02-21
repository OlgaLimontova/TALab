using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Serialization
{
    [XmlRootAttribute("catalog", Namespace = "http://library.by/catalog", IsNullable = false)]
    public class Catalog
    {
        [XmlIgnore]
        public DateTime Date { get; set; }

        [XmlAttribute("date")]
        public string DateString
        {
            get { return this.Date.ToString("yyyy-MM-dd"); }
            set { this.Date = DateTime.Parse(value); }
        }
        
        [XmlElement("book")]
        public List<Book> Books { get; set; }
        
        public Catalog()
        {
            this.Books = new List<Book>();
            this.Date = default(DateTime);
        }
        
        public Catalog(List<Book> books, DateTime date)
        {
            this.Books.AddRange(books);
            this.Date = date;
        }
    }
}