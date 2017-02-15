namespace LINQtoXML
{
    public class City
    {
        public string CityName { get; set; }
        public double Profitability { get; set; }
        public double Intencity { get; set; }

        public City(string cityName, double profitability, double intencity)
        {
            CityName = cityName;
            Profitability = profitability;
            Intencity = intencity;
        }

        public override string ToString()
        {
            return $"{CityName.PadRight(15)} Average profitability is {Profitability.ToString("0.00").PadRight(10)}, Average intencity is {Intencity.ToString("0.00")}";
//            return CityName.PadRight(15) + " Average profitability is" + Profitability.ToString("0.00").PadRight(10) +
//                        ", Average intencity is " + Intencity.ToString("0.00");
        }
    }
}