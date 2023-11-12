using FacturilaAPI.Exceptions;
using FacturilaAPI.Models.Dto;
using Newtonsoft.Json.Linq;

namespace FacturilaAPI.Services.Impl
{
    public class FirmService : IFirmService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiUrl;

        public FirmService(IConfiguration config)
        {
            _httpClient = new HttpClient();
            _apiUrl = config.GetSection("AppSettings")?["AnafApiUrl"] ?? throw new ArgumentNullException("AnafApiUrl is not configured");
        }

        public async Task<FirmDto> GetFirmDataFromAnaf(string cui)
        {
            try
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

                    var dateGenerale = json["found"]?[0]?["date_generale"];
                    if (dateGenerale != null)
                    {
                        if (dateGenerale != null)
                        {
                            string? name = dateGenerale["denumire"]?.ToString();
                            string? cuiValue = dateGenerale["cui"]?.ToString();
                            string? regCom = dateGenerale["nrRegCom"]?.ToString();
                            string? address = dateGenerale["adresa"]?.ToString();

                            if (name != null && cuiValue != null && regCom != null && address != null)
                            {
                                return new FirmDto
                                {
                                    Name = name,
                                    CUI = cuiValue,
                                    RegCom = regCom,
                                    Adress = address
                                };
                            }
                        }
                    }
                }
                throw new AnafFirmNotFoundException(cui);
            }
            catch (Exception)
            {
                throw new AnafFirmNotFoundException(cui);
            }
        }
    }
}
