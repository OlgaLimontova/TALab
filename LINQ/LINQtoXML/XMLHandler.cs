using System.Configuration;
using System.Xml.Linq;

namespace LINQtoXML
{
    class XMLHandler
    {
        XDocument Document { get; set; }
        public XElement RootElement { get; set; }

        public XMLHandler()
        {
            Document = XDocument.Load(ConfigurationManager.AppSettings["FilePath"]);
            RootElement = Document.Root;
        }
    }
}