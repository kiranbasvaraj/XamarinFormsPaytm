using CCDLocationsV2.Helpers;
using CCDLocationsV2.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace CCDLocationsV2.Services
{
    public sealed class RestClient : IRestClient
    {
       private static RestClient _restClientInstance = null;
            public static HttpStatusCode HttpStatus;

            public static RestClient RestClientInstance
            {
                get
                {
                    if (_restClientInstance == null)
                    {
                        _restClientInstance = new RestClient();
                    }
                    return _restClientInstance;
                }

            }
            private HttpClient client;
            private RestClient()
            {

            }


            private HttpClient GetHttpClient(string username = "", string password = "", bool shouldUseDefaultAuthValue = true)
            {
                string authData = string.Empty;
                try
                {
                    client = new HttpClient();
                    client.MaxResponseContentBufferSize = 1024000;
                    if (shouldUseDefaultAuthValue == true)
                    {
                        authData = string.Format("{0}:{1}", "", "");
                    }
                    else
                    {
                        authData = string.Format("{0}:{1}", username, password);
                    }

                    string authHeaderValue = Convert.ToBase64String(Encoding.UTF8.GetBytes(authData));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authHeaderValue);
                    return client;
                }
                catch (Exception ex)
                {

                    throw new Exception(ex.StackTrace);
                }
                finally
                {

                }


            }

        private readonly Action<HttpStatusCode, string> _defaultErrorHandler = (statusCode, body) =>
            {
                if (statusCode < HttpStatusCode.OK || statusCode >= HttpStatusCode.BadRequest)
                {
                    Debug.WriteLine(string.Format("Request responded with status code={0}, response={1}", statusCode, body));
                    throw new RestClientException(statusCode, body);
                }
            };


            private void HandleIfErrorResponse(HttpStatusCode statusCode, string content, Action<HttpStatusCode, string> errorHandler = null)
            {
                if (errorHandler != null)
                {
                    errorHandler(statusCode, content);
                }
                else
                {
                    _defaultErrorHandler(statusCode, content);
                }
            }


            private T GetValue<T>(String value)
            {
                return (T)Convert.ChangeType(value, typeof(T));
            }

            private HttpContent GetHttpContent(object payload)
            {
                try
                {
                    var json = JsonConvert.SerializeObject(payload);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    return content;
                }
                catch (Exception ex)
                {

                    throw new Exception("Problem in Serializing Object");
                }
            }

            public string ResposeCode;
            #region HttpMethods
            public async Task<T> GetAsync<T>(string Url, bool SetAuth = false)
            {
                HttpClient client = null;
                try
                {
                    if (SetAuth == false)
                    {
                        client = GetHttpClient();
                    }
                    else
                    {
                        client = GetHttpClient("", "", false);
                    }

                    var response = await client.GetAsync(Url);
                    ResposeCode = response.StatusCode.ToString();
                    var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    HttpStatus = response.StatusCode;
                    HandleIfErrorResponse(response.StatusCode, json);
                    if (typeof(T) == typeof(string))
                    {
                        return GetValue<T>(json);
                    }
                    var x = JsonConvert.DeserializeObject<T>(json);
                    return x;
                }
                catch (System.Net.WebException ex)
                {

                    throw ex;
                }
                catch (HttpRequestException ex)
                {
                    Debug.WriteLine("Error in GET Request :" + ex.Message);
                }
                return default(T);
            }



            public async Task<T> PostAsync<T>(string Url, bool SetAuth = false, object payload = null)
            {
                HttpClient client = null;
                try
                {

                    if (!SetAuth)
                    {
                        client = GetHttpClient();
                    }
                    else
                    {
                        client = GetHttpClient("", "", false);
                    }
                    var response = await client.PostAsync(Url, GetHttpContent(payload)).ConfigureAwait(false);
                    var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    HttpStatus = response.StatusCode;
                    HandleIfErrorResponse(response.StatusCode, json);
                    if (typeof(T) == typeof(string))
                    {
                        return GetValue<T>(json);
                    }
                    var x = JsonConvert.DeserializeObject<T>(json);
                    return x;
                }
                catch (System.Net.WebException ex)
                {

                    throw ex;
                }
                catch (HttpRequestException ex)
                {
                    Debug.WriteLine("Error in POST Request :" + ex.Message);
                }
                return default(T);
            }



            public async Task<T> PutAsync<T>(string Url, bool SetAuth = false, object payload = null)
            {
                try
                {
                    var client = GetHttpClient();
                    var response = await client.PutAsync(Url, GetHttpContent(payload));
                    var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    HandleIfErrorResponse(response.StatusCode, json);
                    HttpStatus = response.StatusCode;
                    if (typeof(T) == typeof(string))
                    {
                        return GetValue<T>(json);
                    }
                    var x = JsonConvert.DeserializeObject<T>(json);
                    return x;
                }
                catch (System.Net.WebException ex)
                {

                    throw ex;
                }
                catch (HttpRequestException ex)
                {
                    Debug.WriteLine("Error in POST Request :" + ex.Message);
                }
                return default(T);
            }



            public async Task<T> DeleteAsync<T>(string Url, bool SetAuth = false)
            {
                try
                {
                    var client = GetHttpClient();
                    var response = await client.DeleteAsync(Url);
                    var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    HandleIfErrorResponse(response.StatusCode, json);
                    HttpStatus = response.StatusCode;
                    if (typeof(T) == typeof(string))
                    {
                        return GetValue<T>(json);
                    }
                    var x = JsonConvert.DeserializeObject<T>(json);
                    return x;
                }
                catch (System.Net.WebException ex)
                {

                    throw ex;
                }
                catch (HttpRequestException ex)
                {
                    Debug.WriteLine("Error in Delete Request :" + ex.Message);
                }
                return default(T);
            }
            #endregion
        }
    }
