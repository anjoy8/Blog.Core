using Blog.Core.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Core.Common.HttpPolly
{
    public class HttpPollyHelper : IHttpPollyHelper
    {
        private readonly IHttpClientFactory _clientFactory;

        public HttpPollyHelper(IHttpClientFactory httpClientFactory)
        {
            _clientFactory = httpClientFactory;
        }

        public async Task<T> PostAsync<T, R>(HttpEnum httpEnum, string url, R request, Dictionary<string, string> headers = null)
        {
            try
            {
                var client = _clientFactory.CreateClient(httpEnum.ToString());
                if (headers != null)
                {
                    foreach (var header in headers)
                    {
                        if (!client.DefaultRequestHeaders.Contains(header.Key))
                        {
                            client.DefaultRequestHeaders.Add(header.Key, header.Value);
                        }
                    }
                }

                var stringContent = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
                var response = await client.PostAsync(url, stringContent);
                
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    string result = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<T>(result);
                }
                else
                {
                    throw new Exception($"Http Error StatusCode:{response.StatusCode}");
                }
            }
            catch (Exception)
            {

                throw;
            }

        }

        public async Task<T> PostAsync<T>(HttpEnum httpEnum, string url, string request, Dictionary<string, string> headers = null)
        {
            try
            {
                var client = _clientFactory.CreateClient(httpEnum.ToString());
                if (headers != null)
                {
                    foreach (var header in headers)
                    {
                        if (!client.DefaultRequestHeaders.Contains(header.Key))
                        {
                            client.DefaultRequestHeaders.Add(header.Key, header.Value);
                        }
                    }
                }

                var stringContent = new StringContent(request, Encoding.UTF8, "application/json");
                var response = await client.PostAsync(url, stringContent);
                
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    string result = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<T>(result);
                }
                else
                {
                    throw new Exception($"Http Error StatusCode:{response.StatusCode}");
                }

            }
            catch (Exception)
            {

                throw;
            }

        }

        public async Task<string> PostAsync<R>(HttpEnum httpEnum, string url, R request, Dictionary<string, string> headers = null)
        {
            try
            {
                var client = _clientFactory.CreateClient(httpEnum.ToString());
                if (headers != null)
                {
                    foreach (var header in headers)
                    {
                        if (!client.DefaultRequestHeaders.Contains(header.Key))
                        {
                            client.DefaultRequestHeaders.Add(header.Key, header.Value);
                        }
                    }
                }

                var stringContent = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
                var response = await client.PostAsync(url, stringContent);
                
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    return await response.Content.ReadAsStringAsync();
                }
                else
                {
                    throw new Exception($"Http Error StatusCode:{response.StatusCode}");
                }
            }
            catch (Exception)
            {

                throw;
            }

        }

        public async Task<string> PostAsync(HttpEnum httpEnum, string url, string request, Dictionary<string, string> headers = null)
        {
            try
            {
                var client = _clientFactory.CreateClient(httpEnum.ToString());
                if (headers != null)
                {
                    foreach (var header in headers)
                    {
                        if (!client.DefaultRequestHeaders.Contains(header.Key))
                        {
                            client.DefaultRequestHeaders.Add(header.Key, header.Value);
                        }
                    }
                }

                var stringContent = new StringContent(request, Encoding.UTF8, "application/json");
                var response = await client.PostAsync(url, stringContent);
                
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    return await response.Content.ReadAsStringAsync();
                }
                else
                {
                    throw new Exception($"Http Error StatusCode:{response.StatusCode}");
                }

            }
            catch (Exception)
            {

                throw;
            }

        }

        public async Task<T> GetAsync<T>(HttpEnum httpEnum, string url, Dictionary<string, string> headers = null)
        {
            try
            {
                var client = _clientFactory.CreateClient(httpEnum.ToString());
                if (headers != null)
                {
                    foreach (var header in headers)
                    {
                        if (!client.DefaultRequestHeaders.Contains(header.Key))
                        {
                            client.DefaultRequestHeaders.Add(header.Key, header.Value);
                        }
                    }
                }

                var response = await client.GetAsync(url);
                
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    string result = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<T>(result);
                }
                else
                {
                    throw new Exception($"Http Error StatusCode:{response.StatusCode}");
                }

            }
            catch (Exception)
            {

                throw;
            }

        }

        public async Task<string> GetAsync(HttpEnum httpEnum, string url, Dictionary<string, string> headers = null)
        {
            try
            {
                var client = _clientFactory.CreateClient(httpEnum.ToString());
                if (headers != null)
                {
                    foreach (var header in headers)
                    {
                        if (!client.DefaultRequestHeaders.Contains(header.Key))
                        {
                            client.DefaultRequestHeaders.Add(header.Key, header.Value);
                        }
                    }
                }

                var response = await client.GetAsync(url);
                
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    return await response.Content.ReadAsStringAsync(); ;
                }
                else
                {
                    throw new Exception($"Http Error StatusCode:{response.StatusCode}");
                }

            }
            catch (Exception)
            {

                throw;
            }

        }

        public async Task<T> PutAsync<T, R>(HttpEnum httpEnum, string url, R request, Dictionary<string, string> headers = null)
        {
            try
            {
                var client = _clientFactory.CreateClient(httpEnum.ToString());
                if (headers != null)
                {
                    foreach (var header in headers)
                    {
                        if (!client.DefaultRequestHeaders.Contains(header.Key))
                        {
                            client.DefaultRequestHeaders.Add(header.Key, header.Value);
                        }
                    }
                }

                var stringContent = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
                var response = await client.PutAsync(url, stringContent);
                
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    string result = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<T>(result);
                }
                else
                {
                    throw new Exception($"Http Error StatusCode:{response.StatusCode}");
                }

            }
            catch (Exception)
            {

                throw;
            }

        }

        public async Task<T> PutAsync<T>(HttpEnum httpEnum, string url, string request, Dictionary<string, string> headers = null)
        {
            try
            {
                var client = _clientFactory.CreateClient(httpEnum.ToString());
                if (headers != null)
                {
                    foreach (var header in headers)
                    {
                        if (!client.DefaultRequestHeaders.Contains(header.Key))
                        {
                            client.DefaultRequestHeaders.Add(header.Key, header.Value);
                        }
                    }
                }

                var stringContent = new StringContent(request, Encoding.UTF8, "application/json");
                var response = await client.PutAsync(url, stringContent);
                
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    string result = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<T>(result);
                }
                else
                {
                    throw new Exception($"Http Error StatusCode:{response.StatusCode}");
                }

            }
            catch (Exception)
            {

                throw;
            }

        }

        public async Task<T> DeleteAsync<T>(HttpEnum httpEnum, string url, Dictionary<string, string> headers = null)
        {
            try
            {
                var client = _clientFactory.CreateClient(httpEnum.ToString());
                if (headers != null)
                {
                    foreach (var header in headers)
                    {
                        if (!client.DefaultRequestHeaders.Contains(header.Key))
                        {
                            client.DefaultRequestHeaders.Add(header.Key, header.Value);
                        }
                    }
                }

                var response = await client.DeleteAsync(url);
                
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    string result = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<T>(result);
                }
                else
                {
                    throw new Exception($"Http Error StatusCode:{response.StatusCode}");
                }

            }
            catch (Exception)
            {

                throw;
            }

        }
    }
}
