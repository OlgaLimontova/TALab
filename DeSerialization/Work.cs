using System.Configuration;

namespace Serialization
{
    class MainClass
    {
        static void Main(string[] args)
        {
            string StartFilePath = ConfigurationManager.AppSettings["FilePath"] +
                ConfigurationManager.AppSettings["FileName"];
            string ResultFilePath = ConfigurationManager.AppSettings["FilePath"] +
                "New_" + ConfigurationManager.AppSettings["FileName"];
            DeSerializationProvider provider = new DeSerializationProvider(StartFilePath, ResultFilePath);
            provider.DeserializeData();
            provider.SerializeData();
        }
    }
}