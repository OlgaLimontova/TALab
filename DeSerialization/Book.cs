using System;
using System.Xml.Serialization;

namespace Serialization
{
    public class Book
    {
        [XmlAttribute("id")]
        public string ID { get; set; }
        
        [XmlElement("isbn")]
        public string ISBN { get; set; }
        
        [XmlElement("author")]
        public string Author { get; set; }
        
        [XmlElement("title")]
        public string Title { get; set; }
        
        [XmlElement("genre")]
        public string Genre { get; set; }
        
        [XmlElement("publisher")]
        public string Publisher { get; set; }
        
        [XmlIgnore]
        public DateTime PublishDate { get; set; }
        
        [XmlElement("publish_date")]
        public string PublishDateString
        {
            get { return this.PublishDate.ToString("yyyy-MM-dd"); }
            set { this.PublishDate = DateTime.Parse(value); }
        }
        
        [XmlElement("description")]
        public string Desccription { get; set; }
        
        [XmlIgnore]
        public DateTime RegistrationDate { get; set; }
        
        [XmlElement("registration_date")]
        public string RegistrationDateString
        {
            get { return this.RegistrationDate.ToString("yyyy-MM-dd"); }
            set { this.RegistrationDate = DateTime.Parse(value); }
        }

        public Book()
        {
            this.ID = null;
            this.ISBN = null;
            this.Author = null;
            this.Title = null;
            this.Genre = null;
            this.Publisher = null;
            this.PublishDate = default(DateTime);
            this.Desccription = null;
            this.RegistrationDate = default(DateTime);
        }
        
        public Book(string id, string isbn, string author, string title, string genre, string publisher,
            DateTime publishDate, string description, DateTime registrationDate)
        {
            this.ID = id;
            this.ISBN = isbn;
            this.Author = author;
            this.Title = title;
            this.Genre = genre;
            this.Publisher = publisher;
            this.PublishDate = publishDate;
            this.Desccription = description;
            this.RegistrationDate = registrationDate;
        }
    }
}