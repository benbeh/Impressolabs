using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using ImpressoApp.Exceptions;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using System.Collections.Generic;
using System.Web;
using System.Text;

namespace ImpressoApp.Services.RequestProvider
{
    public class RequestProvider : IRequestProvider
    {
        private JsonSerializerSettings jsonSerializerSettings;
        private HttpClient _httpClient;

        public RequestProvider()
        {
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            jsonSerializerSettings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                DateTimeZoneHandling = DateTimeZoneHandling.Utc,
                NullValueHandling = NullValueHandling.Ignore
            };

            jsonSerializerSettings.Converters.Add(new StringEnumConverter());
        }

        public async Task DeleteAsync(string endpoint)
        {
            var client = GetHttpClient();
            await client.DeleteAsync(endpoint);
        }

        public async Task<TResult> GetAsync<TResult>(string endpoint, List<ReqestParameter> parameters = null)
        {
            var client = GetHttpClient();

            var builder = new UriBuilder(endpoint);
            var query = HttpUtility.ParseQueryString(builder.Query);
            if (parameters != null)
            {
                foreach (var param in parameters)
                {
                    if (param.Value != null)
                    {
                        query[param.Name] = param.Value;
                    }
                    if (param.Values != null)
                    {
                        for (int i = 0; i < param.Values.Count; i++)
                        {
                            query[param.Name + $"[{i}]"] = param.Values[i];
                        }
                    }
                }
            }

            builder.Query = query.ToString();
            string url = builder.ToString();

            var responseMessage = await client.GetAsync(url);
            await HandleResponse(responseMessage);

            string serialized = await responseMessage.Content.ReadAsStringAsync();
            TResult result = await Task.Run(() =>
                                            JsonConvert.DeserializeObject<TResult>(serialized, jsonSerializerSettings));

            return result;
        }

        public async Task<TResult> PostAsync<TResult>(string endpoint, object data)
        {
            var client = GetHttpClient();
            var val = JsonConvert.SerializeObject(data);
            var content = new StringContent(val);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var response = await client.PostAsync(endpoint, content);

            await HandleResponse(response);
            string serialized = await response.Content.ReadAsStringAsync();

            TResult result = await Task.Run(() =>
                                            JsonConvert.DeserializeObject<TResult>(serialized, jsonSerializerSettings));

			return result;
        }

        public async Task<TResult> PostAsync<TResult>(string endpoint, Dictionary<string, string> data)
        {
            var client = GetHttpClient();
            var content = new FormUrlEncodedContent(data);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var response = await client.PostAsync(endpoint, content);

            await HandleResponse(response);
            string serialized = await response.Content.ReadAsStringAsync();

            TResult result = await Task.Run(() =>
                                            JsonConvert.DeserializeObject<TResult>(serialized, jsonSerializerSettings));

            return result;
        }

        public async Task<TResult> PutAsync<TResult>(string endpoint, TResult data)
        {
            var client = GetHttpClient();

            var content = new StringContent(JsonConvert.SerializeObject(data));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var response = await client.PutAsync(endpoint, content);

            await HandleResponse(response);
            string serialized = await response.Content.ReadAsStringAsync();

            TResult result = await Task.Run(() =>
                                            JsonConvert.DeserializeObject<TResult>(serialized, jsonSerializerSettings));

            return result;
        }

        public void InitWithAuthorizationToken(string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        private HttpClient GetHttpClient()
        {
            return _httpClient;
        }

        private async Task HandleResponse(HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();

                if (response.StatusCode == HttpStatusCode.Forbidden ||
                    response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    throw new ServiceAuthenticationException(content);
                }

                throw new RequestExceptionExtension(response.StatusCode, content);
            }
        }
    }

    public class ReqestParameter
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public List<string> Values { get; set; }
    }
}
