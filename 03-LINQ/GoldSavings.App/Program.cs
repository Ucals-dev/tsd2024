using GoldSavings.App.Model;
using GoldSavings.App.Client;
namespace GoldSavings.App;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Xml.Linq;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Hello, Gold Saver!");

        GoldClient goldClient = new GoldClient();

        GoldPrice currentPrice = goldClient.GetCurrentGoldPrice().GetAwaiter().GetResult();
        Console.WriteLine($"The price for today is {currentPrice.Price}");

        List<GoldPrice> thisMonthPrices = goldClient.GetGoldPrices(new DateTime(2024, 03, 01), new DateTime(2024, 03, 11)).GetAwaiter().GetResult();
        foreach (var goldPrice in thisMonthPrices)
        {
            Console.WriteLine($"The price for {goldPrice.Date} is {goldPrice.Price}");
        }


        //Question 3
        List<GoldPrice> lastYearPrices = goldClient.GetGoldPrices(new DateTime(2023, 01, 01), new DateTime(2023, 12, 31)).GetAwaiter().GetResult();
        IEnumerable<GoldPrice> topPrices =
            lastYearPrices.OrderByDescending(goldPrice => goldPrice.Price).Take(3);

        Console.WriteLine("Question3");
        foreach (var goldPrice in topPrices)
        {
            Console.WriteLine($"The price for {goldPrice.Date} is {goldPrice.Price}");
        }

        //Question 4
        List<GoldPrice> januaryPrices = goldClient.GetGoldPrices(new DateTime(2020, 01, 01), new DateTime(2020, 01, 31)).GetAwaiter().GetResult();
        double buyPrice = januaryPrices[0].Price;
        double goalPrice = buyPrice * 1.05;
        Console.WriteLine(goalPrice);

        List<GoldPrice> year2020Prices = goldClient.GetGoldPrices(new DateTime(2020, 01, 01), new DateTime(2020, 12, 31)).GetAwaiter().GetResult();
        IEnumerable<GoldPrice> fivePercent =
            year2020Prices.Where(p => p.Price >= goalPrice).OrderBy(goldPrice => goldPrice.Date).Take(1);

        Console.WriteLine("Question4");
        Console.WriteLine($"More than 5% augmentation on {fivePercent.First().Date}");

        //Question 5 
        List<GoldPrice> year2019Prices = goldClient.GetGoldPrices(new DateTime(2019, 01, 01), new DateTime(2019, 12, 31)).GetAwaiter().GetResult();
        List<GoldPrice> year2021Prices = goldClient.GetGoldPrices(new DateTime(2021, 01, 01), new DateTime(2021, 12, 31)).GetAwaiter().GetResult();

        IEnumerable<GoldPrice> top32019 =
            year2019Prices.OrderByDescending(goldPrice => goldPrice.Price).Take(3);

        IEnumerable<GoldPrice> top32020 =
            year2020Prices.OrderByDescending(goldPrice => goldPrice.Price).Take(3);
        IEnumerable<GoldPrice> top32021 =
            year2021Prices.OrderByDescending(goldPrice => goldPrice.Price).Take(3);

        List<GoldPrice> top3Prices = (top32019.ToList().Concat(top32020.ToList().Concat(top32021.ToList()))).ToList();

        IEnumerable<GoldPrice> top3Q5 =
            top3Prices.OrderByDescending(goldPrice => goldPrice.Price).Take(3);

        Console.WriteLine("Question5");
        foreach (var goldPrice in top3Q5)
        {
            Console.WriteLine($"The price for {goldPrice.Date} is {goldPrice.Price}");
        }

        //Question6
        Console.WriteLine("Question6");
        List<GoldPrice> year2022Prices = goldClient.GetGoldPrices(new DateTime(2022, 01, 01), new DateTime(2022, 12, 31)).GetAwaiter().GetResult();

        // Calculate the average price using LINQ
        double year2021avg = year2021Prices.Average(item => item.Price);
        double year2022avg = year2022Prices.Average(item => item.Price);
        double year2023avg = lastYearPrices.Average(item => item.Price);

        Console.WriteLine($"The avg price for 2021 is {year2021avg}");
        Console.WriteLine($"The avg price for 2022 is {year2022avg}");
        Console.WriteLine($"The avg price for 2023 is {year2023avg}");

        //Question7
        Console.WriteLine("Question7");

        IEnumerable<GoldPrice> top12019 =
            year2019Prices.OrderByDescending(goldPrice => goldPrice.Price).Take(1);
        IEnumerable<GoldPrice> top12020 =
            year2019Prices.OrderByDescending(goldPrice => goldPrice.Price).Take(1);
        IEnumerable<GoldPrice> top12021 =
            year2019Prices.OrderByDescending(goldPrice => goldPrice.Price).Take(1);
        IEnumerable<GoldPrice> top12022 =
            year2019Prices.OrderByDescending(goldPrice => goldPrice.Price).Take(1);
        IEnumerable<GoldPrice> top12023 =
            lastYearPrices.OrderByDescending(goldPrice => goldPrice.Price).Take(1);
        List<GoldPrice> top1Prices = (top12019.ToList().Concat(top12020.ToList().Concat(top12021.ToList().Concat(top12022.ToList().Concat(top12023.ToList()))))).ToList();
        IEnumerable<GoldPrice> top1 =
            top1Prices.OrderByDescending(goldPrice => goldPrice.Price).Take(1);


        IEnumerable<GoldPrice> low12019 =
    year2019Prices.OrderBy(goldPrice => goldPrice.Price).Take(1);
        IEnumerable<GoldPrice> low12020 =
            year2019Prices.OrderBy(goldPrice => goldPrice.Price).Take(1);
        IEnumerable<GoldPrice> low12021 =
            year2019Prices.OrderBy(goldPrice => goldPrice.Price).Take(1);
        IEnumerable<GoldPrice> low12022 =
            year2019Prices.OrderBy(goldPrice => goldPrice.Price).Take(1);
        IEnumerable<GoldPrice> low12023 =
            lastYearPrices.OrderBy(goldPrice => goldPrice.Price).Take(1);
        List<GoldPrice> low1Prices = (low12019.ToList().Concat(low12020.ToList().Concat(low12021.ToList().Concat(low12022.ToList().Concat(low12023.ToList()))))).ToList();
        IEnumerable<GoldPrice> low1 =
            low1Prices.OrderBy(goldPrice => goldPrice.Price).Take(1);

        GoldPrice lowPrice = low1.First();
        GoldPrice topPrice = top1.First();

        Console.WriteLine($"The lowest gold value is reached on {lowPrice.Date} and is of value {lowPrice.Price}");
        Console.WriteLine($"The highest gold value is reached on {topPrice.Date} and is of value {topPrice.Price}");

        Console.WriteLine($"The return on investment would be of {100+(lowPrice.Price/topPrice.Price) * 100}%");

        //Question8
        Console.WriteLine("Question8");

        void saveToXML(IEnumerable<GoldPrice> goldPrices,string title)
        {
            // Create XML document
            XDocument xmlDocument = new XDocument(
                new XElement("Items",
                    from item in goldPrices
                    select new XElement("Item",
                        new XElement("Price", item.Price),
                        new XElement("Date", item.Date)
                    )
                )
            );

            // Save XML document to a file
            xmlDocument.Save(title);

            Console.WriteLine("XML file created successfully.");
        }
        saveToXML(year2021Prices,"2021.xml");

        //Question9
        Console.WriteLine("Question9");

        void readXML(string title)
        {
            Console.WriteLine(File.ReadAllText(title));
        }
        readXML("2021.xml");

    }
}
