using FacturilaAPI.Exceptions;
using FacturilaAPI.Models.Dto;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Diagnostics;

namespace FacturilaAPI.Services.Impl
{
    public class FirmService : IFirmService
    {
        private readonly HttpClient _httpClient;
        private readonly String _apiUrl;

        public FirmService(IConfiguration config)
        {
            _httpClient = new HttpClient();
            _apiUrl = config.GetSection("AppSettings")["AnafApiUrl"];
        }

        public async Task<FirmDataDto> GetFirmDataFromAnaf(string cui)
        {
            string currentDate = DateTime.Now.ToString("yyyy-MM-dd");

            var requestBody = new[]
            {
                new
                {
                    cui = cui,
                    data = currentDate
                }
            };

            var response = await _httpClient.PostAsJsonAsync(_apiUrl, requestBody);

            if (response.IsSuccessStatusCode)
            {
                var responseString = await response.Content.ReadAsStringAsync();
                JObject json = JObject.Parse(responseString);

                if (json["found"].HasValues && json["found"][0]?["date_generale"] != null)
                {
                    var dateGenerale = json["found"][0]["date_generale"];

                    return new FirmDataDto
                    {
                        Name = dateGenerale["denumire"].ToString(),
                        CUI = dateGenerale["cui"].ToString(),
                        RegCom = dateGenerale["nrRegCom"].ToString(),
                        Adress = dateGenerale["adresa"].ToString()
                    };
                };
            }

            throw new AnafFirmNotFoundException(cui);
        }
    }
}
