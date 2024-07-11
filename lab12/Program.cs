using lab12;
using System.Net.Http.Headers;
using test;



public class MainClass
{
    public static void Main(string[] args)
    {
        //Console.WriteLine("Hello World!");
        RunAsync().Wait();
    }

    static async Task RunAsync()
    {
            var client = new RestTest();

        // await client.Delete("3");

        var excursies = await client.GetAll();

        //for (int i = 0; i < excursies.Length; i++)
        //{
        //    Console.WriteLine(excursies[i].ToString());
        //}

        var excursie = await client.GetById(1);

        var newexcursie = new Excursie(100,"Maramures", "TCP", "12:00:00", 100, 100, 50);
        var ex = await client.Create(newexcursie);


        newexcursie.NumeTransport = "Autobuz";
        await client.Update(newexcursie);

        await client.Delete("100");


        // var createdexcursie = await client.Create(newexcursie);
        // Console.WriteLine(createdexcursie.ToString());

        // await client.Update(newexcursie);

    }


}


