using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Serialization
{
    class DeSerializationProvider
    {
        public Catalog Catalog { get; set; }
        public string StartFilePath {get; set;}
        public string ResultFilePath { get; set; }
        public XmlSerializer Serializer { get; set; }
        public XmlSerializerNamespaces Namespaces { get; set; }
        public XmlWriterSettings Settings { get; set; }

        public DeSerializationProvider(string startPath, string resultPath)
        {
            this.Catalog = new Catalog();
            this.StartFilePath = startPath;
            this.ResultFilePath = resultPath;
            this.Serializer = new XmlSerializer(Catalog.GetType());
            this.Namespaces = new XmlSerializerNamespaces();
            this.Namespaces.Add("", "http://library.by/catalog");
            this.Settings = new XmlWriterSettings();
            this.Settings.Encoding = Encoding.UTF8;
            this.Settings.Indent = true;
        }

        public void DeserializeData()
        {
            using (var fileStream = new FileStream(this.StartFilePath, FileMode.Open))
            {
                this.Catalog = (Catalog)this.Serializer.Deserialize(fileStream);
            }
            Console.WriteLine("Deserialization is complete.\n");
        }

        public void SerializeData()
        {
            using (var fileStream = new FileStream(ResultFilePath, FileMode.Create))
            {
                using (var xmlWriter = XmlWriter.Create(fileStream, this.Settings))
                {
                    this.Serializer.Serialize(xmlWriter, this.Catalog, this.Namespaces);
                }
            }
            Console.WriteLine("Serialization is complete.\n");
        }
    }
}