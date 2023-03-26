using Guardian_Utility;
using Guardian_Web.Models.API;
using Guardian_Web.Services.IServices;
using Newtonsoft.Json;
using System.Net;
using System.Text;

namespace Guardian_Web.Services
{
    public class BaseService : IBaseService
    {
        //It is a generic base service, so whatever service will be inheriting or calling the base service that will be added to program.cs
        //to add the implementation of client factory
        public APIResponse responseModel { get; set; }
        public IHttpClientFactory httpClient { get; set; }

        public BaseService(IHttpClientFactory httpClient)
        {
            this.responseModel = new();
            this.httpClient = httpClient;
        }
        public async Task<T> SendAsync<T>(APIResquest apiResquest)
        {
            //It is a generic method where I am calling the API endpoint
            try
            {
                var client = httpClient.CreateClient("GuardianAPI");
                HttpRequestMessage message = new HttpRequestMessage();
                message.Headers.Add("Accept", "application/json");
                message.RequestUri = new Uri(apiResquest.Url);

                //Serealize Data from insert or update
                if (apiResquest.Data != null) //Data will not be null in POST/PUT/ PATCH HTPP Calls
                {
                    message.Content = new StringContent(JsonConvert.SerializeObject(apiResquest.Data), Encoding.UTF8, "application/json");                    
                }

                //Define HTPP type
                switch (apiResquest.ApiType) //ApiType is eNum so we can use switch case
                {
                    case SD.ApiType.POST:
                        message.Method = HttpMethod.Post;
                        break;

                    case SD.ApiType.PUT:
                        message.Method = HttpMethod.Put;
                        break;

                    case SD.ApiType.DELETE:
                        message.Method = HttpMethod.Delete;
                        break;
                    default:
                        message.Method = HttpMethod.Get;
                        break;
                }

                HttpResponseMessage apiResponse = null;
                apiResponse = await client.SendAsync(message); //Consuming API
                var apiContent = await apiResponse.Content.ReadAsStringAsync();

                try
                {
                    APIResponse apiFinalResponse = JsonConvert.DeserializeObject<APIResponse>(apiContent); //API will always retrive APIResponse, that is the reason it is not a type T (generic)
                    if (apiFinalResponse.StatusCode == HttpStatusCode.BadRequest || apiFinalResponse.StatusCode == HttpStatusCode.NotFound)
                    {
                        apiFinalResponse.StatusCode = HttpStatusCode.BadRequest;
                        apiFinalResponse.IsSuccess = false;

                        var response = JsonConvert.SerializeObject(apiResponse);
                        var returnObj = JsonConvert.DeserializeObject<T>(response); //The method expects type<T>
                        return returnObj;
                    }
                }
                catch (Exception e)
                {
                    var exceptionResponse = JsonConvert.DeserializeObject<T>(apiContent);
                    return exceptionResponse;
                }

                var APIResponse = JsonConvert.DeserializeObject<T>(apiContent);
                return APIResponse;
            }
            catch (Exception ex)
            {
                var dto = new APIResponse
                {
                    ErrorMessages = new List<string> { Convert.ToString(ex.Message) },
                    IsSuccess = false,
                };
                var response = JsonConvert.SerializeObject(dto);
                var APIResponse = JsonConvert.DeserializeObject<T>(response); //The method expects type<T>
                return APIResponse;
            }
        }
    }
}
