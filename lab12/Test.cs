using lab12;
using Newtonsoft.Json;
using System;
using System.Net.Http.Json;


namespace test
{
    public class RestTest
    {
        private const string URL = "http://localhost:8080/api/Excursie";
        private readonly HttpClient httpClient = new HttpClient(new LoggingHandler(new HttpClientHandler()));

        private async Task<T> Execute<T>(Func<Task<T>> func)
        {
            try
            {
                return await func();
            }
            catch (HttpRequestException ex) when (ex.StatusCode >= System.Net.HttpStatusCode.BadRequest && ex.StatusCode < System.Net.HttpStatusCode.InternalServerError)
            {
                throw new Exception(ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<Excursie[]> GetAll()
        {
#pragma warning disable CS8603 // Possible null reference return.
            return await Execute(async () =>
            {
                HttpResponseMessage response = await httpClient.GetAsync(URL);
                response.EnsureSuccessStatusCode();
                System.Console.WriteLine("Get all: ",response.Content.ToString());
                string responseBody = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<Excursie[]>(responseBody);
            });
#pragma warning restore CS8603 // Possible null reference return.
        }

        public async Task<Excursie> GetById(long id)
        {
            return await Execute(async () =>
            {
                HttpResponseMessage response = await httpClient.GetAsync($"{URL}/{id}");
                response.EnsureSuccessStatusCode();
               
                System.Console.WriteLine(response.StatusCode);
                string responseBody = await response.Content.ReadAsStringAsync();
                System.Console.WriteLine("Get id: ", responseBody);
                return JsonConvert.DeserializeObject<Excursie>(responseBody);
            });
        }

        public async Task<Excursie> Create(Excursie Excursie)
        {
            return await Execute(async () =>
            {
                HttpResponseMessage response = await httpClient.PostAsJsonAsync(URL, Excursie);
                response.EnsureSuccessStatusCode();
                System.Console.WriteLine("Create: ",response.ToString());
                string responseBody = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<Excursie>(responseBody);
            });
        }


        public async Task Update(Excursie Excursie)
        {
            await Execute(async () =>
            {
                HttpResponseMessage response = await httpClient.PutAsJsonAsync(URL, Excursie);
                System.Console.WriteLine("Update: ",response);
                response.EnsureSuccessStatusCode();
                return Task.CompletedTask;
            });
        }

        public async Task Delete(string id)
        {
            await Execute(async () =>
            {
                HttpResponseMessage response = await httpClient.DeleteAsync($"{URL}/{id}");
                response.EnsureSuccessStatusCode();
                System.Console.WriteLine("Delete: ",response.ToString());
                return Task.CompletedTask;
            });
        }
    }


    public class LoggingHandler : DelegatingHandler
    {
        public LoggingHandler(HttpMessageHandler innerHandler)
            : base(innerHandler)
        {
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            Console.WriteLine("Request:");
            Console.WriteLine(request.ToString());
            if (request.Content != null)
            {
                Console.WriteLine(await request.Content.ReadAsStringAsync());
            }
            Console.WriteLine();

            HttpResponseMessage response = await base.SendAsync(request, cancellationToken);

            Console.WriteLine("Response:");
            Console.WriteLine(response.ToString());
            if (response.Content != null)
            {
                Console.WriteLine(await response.Content.ReadAsStringAsync());
            }
            Console.WriteLine();

            return response;
        }
    }
}