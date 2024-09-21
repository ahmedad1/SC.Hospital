using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RepositoryPattern.EfCore;
using RepositoryPatternUOW.Core.DTOs;
using RepositoryPatternUOW.Core.OptionPattern;
using System.Net.Http.Json;
using System.Net.Mime;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace RepositoryPatternWithUOW.EfCore.InitPayService
{
    public class InitPayService(IOptions<PaymobInitOptions> payInitOptions, IHttpClientFactory httpClientFactory, AppDbContext context) : IInitPaymentService
    {



        record FirstResult(string Token);
        record SecondResult(int Id);
        record ThirdResult(string Token);
        record RedirectUrl(string redirect_url);
        class Item
        {
            public string name { get; set; }
            public string amount_cents { get; set; }
            public string description { get; set; }
            public string quantity { get; set; }
        }

        async Task<FirstResult?> FirstStep()
        {
            var strings = $"{{\"api_key\": \"{{{{{payInitOptions.Value.ApiKey}}}}}\"}}";
            var stringContent = new StringContent(strings, Encoding.UTF8, MediaTypeNames.Application.Json);
            var httpClient = httpClientFactory.CreateClient();
            using var response = await httpClient.PostAsync($@"https://accept.paymob.com/api/auth/tokens", stringContent);
            if (response.IsSuccessStatusCode)
                return await response.Content.ReadFromJsonAsync<FirstResult>();
            return null;

        }
        async Task<SecondResult?> SecondStep(string token, string pName, float pPrice, string pDescription)
        {

            //string json = $"{{\"auth_token\":\"{token}\",\"amount_cents\":\"{pPrice}\",\"delivery_needed\":\"false\",\"currency\":\"EGP\",\"items\":[{{\"name\":\"{pName}\",\"amount_cents\":\"{pPrice}\",\"description\":\"{pDescription}\",\"quantity\":\"1\"}}]}}";
            var json = new
            {
                auth_token = token,
                amount_cents = pPrice.ToString(),//must be string
                delivery_needed = "false",
                currency = "EGP",
                items = new List<Item>{new Item {
                    name = pName,
                    amount_cents = pPrice.ToString(),//must be string
                    description = pDescription,
                    quantity = "1"
                }
            }
            };
            var content = new StringContent(JsonConvert.SerializeObject(json), Encoding.UTF8, "application/json");
            var httpClient = httpClientFactory.CreateClient();
            using var response = await httpClient.PostAsync("https://accept.paymob.com/api/ecommerce/orders", content);
            var result = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
                return await response.Content.ReadFromJsonAsync<SecondResult>();


            return null;


        }
        async Task<ThirdResult?> ThirdStep(string token, int orderId, float pPrice, string firstName, string lastName, string emailAddress, string phoneNumber, float integrationId)
        {
            var json = new
            {
                expiration = 3600,
                auth_token = token,
                currency = "EGP",
                amount_cents = pPrice,
                order_id = orderId,
                integration_id = integrationId,
                billing_data = new
                {
                    apartment = "6",
                    first_name = firstName,
                    last_name = lastName,
                    city = "fayoum",
                    street = "938, Al-Jadeed Bldg",
                    building = "939",
                    phone_number = phoneNumber,
                    country = "OMN",
                    email = emailAddress,
                    floor = "1",
                    state = "Alkhuwair"

                }
            };




            var stringContent = new StringContent(JsonConvert.SerializeObject(json), Encoding.UTF8, MediaTypeNames.Application.Json);
            var httpClient = httpClientFactory.CreateClient();
            using var response = await httpClient.PostAsync("https://accept.paymob.com/api/acceptance/payment_keys", stringContent);
            if (response.IsSuccessStatusCode)
                return await response.Content.ReadFromJsonAsync<ThirdResult>();

            return null;
        }
        public async Task<string?> InitPay(string firstName, string lastName, string email, int doctorId, string pDescription,int patientId)
        {
            var doctor = await context.Doctors.FirstOrDefaultAsync(x => x.Id == doctorId);
            if (doctor == null)
                return null;
            try
            {
                ServiceDescriptionPaymob? descriptionPaymob = JsonConvert.DeserializeObject<ServiceDescriptionPaymob>(pDescription);
                if (descriptionPaymob is null) return null;
                if (!Enum.TryParse(descriptionPaymob.Date, true, out DateOnly date))
                    return null;
                if (!await context.Schedules.AnyAsync(x => x.Day == date.DayOfWeek)) return null;
                if (descriptionPaymob == null) return null;
                descriptionPaymob.PatientId = patientId;

                var res1 = await FirstStep();
                if (res1 is null) return null;
                var res2 = await SecondStep(res1.Token, doctorId.ToString(), doctor.Price * 100, pDescription);
                if (res2 is null) return null;
                var res3 = await ThirdStep(res1.Token, res2.Id, doctor.Price * 100, firstName, lastName, email, "01010101010", payInitOptions.Value.CardIntId);
                if (res3 == null) return null;

                return "https://accept.paymob.com/api/acceptance/iframes/834278?payment_token=" + res3.Token;
            }
            catch
            {
                return null;
            }

        }


    }
}
