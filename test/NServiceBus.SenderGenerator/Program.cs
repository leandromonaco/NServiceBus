using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

public static class Program
{
    static Random random;

    public static async Task Main()
    {
        random = new Random();

        while (true)
        {
            var key = Console.ReadKey();
            Console.WriteLine();

            if (key.Key != ConsoleKey.Enter)
            {
                break;
            }

            var orderSubmitted = new OrderSubmitted
            {
                OrderId = Guid.NewGuid(),
                Value = random.Next(100)
            };


            var httpClient = new HttpClient(new HttpClientHandler());

            var jsonContent = JsonSerializer.Serialize(orderSubmitted);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            var result = await httpClient.PostAsync("http://localhost:52001/order", content);

            Console.WriteLine("Published OrderSubmitted message");
        }
    }
}