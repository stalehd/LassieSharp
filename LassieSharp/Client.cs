/*
 Copyright 2017 Telenor Digital AS

   Licensed under the Apache License, Version 2.0 (the "License");
   you may not use this file except in compliance with the License.
   You may obtain a copy of the License at

       http://www.apache.org/licenses/LICENSE-2.0

   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
   See the License for the specific language governing permissions and
   limitations under the License.
*/
using System;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Lassie
{
    public class Client
    {
        internal string endpoint;
        internal readonly HttpClient restClient = new HttpClient();

        /// <summary>
        /// Initializes a new instance of the <see cref="T:LassieSharp.Client"/> class.
        /// </summary>
        public Client()
        {
            var config = new ComboConfig();
            NewClient(config.Endpoint, config.Token);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Lassie.Client"/> class with
        /// the specified endpoint or token.
        /// </summary>
        /// <param name="endpoint">Endpoint.</param>
        /// <param name="token">Token.</param>
        public Client(string endpoint, string token)
        {
            NewClient(endpoint, token);
        }

        /// <summary>
        /// Create a new client and initialise the rest client.
        /// </summary>
        /// <param name="ep">Endpoint.</param>
        /// <param name="token">Token.</param>
        void NewClient(string ep, string token)
        {
            this.endpoint = ep;
            restClient.DefaultRequestHeaders.Add("X-API-Token", new string[] { token });
        }

        /// <summary>
        /// Throws if not status.
        /// </summary>
        /// <param name="code">Code.</param>
        /// <param name="response">Response.</param>
        async Task ThrowIfNotStatus(HttpStatusCode code, HttpResponseMessage response)
        {
            if (response.StatusCode != code)
            {
                throw new LassieException(response.StatusCode, await response.Content.ReadAsStringAsync());
            }
        }

        /// <summary>
        /// Creates the request.
        /// </summary>
        /// <returns>The request.</returns>
        /// <param name="method">Method.</param>
        /// <param name="path">Path.</param>
        /// <param name="parameter">Parameter.</param>
        HttpRequestMessage CreateRequest(HttpMethod method, string path, object parameter)
        {
            var request = new HttpRequestMessage();
            request.RequestUri = new Uri(endpoint + path);
            if (parameter != null)
            {
                request.Content = new StringContent(
                    JsonConvert.SerializeObject(parameter),
                    Encoding.UTF8, "application/json");
            }
            request.Method = method;
            return request;
        }

        /// <summary>
        /// Generic create method. POSTs to the resource and returns the serialized type.
        /// </summary>
        /// <returns>The create.</returns>
        /// <param name="template">Template.</param>
        /// <param name="path">Path.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        async Task<T> Create<T>(T template, string path)
        {
            var response = await restClient.SendAsync(CreateRequest(HttpMethod.Post, path, template));
            await ThrowIfNotStatus(HttpStatusCode.Created, response);
            return JsonConvert.DeserializeObject<T>(await response.Content.ReadAsStringAsync());
        }

        /// <summary>
        /// Generic PUT method for entitis. PUTs to the resource and returns the updated type.
        /// </summary>
        /// <returns>The update.</returns>
        /// <param name="updated">Updated.</param>
        /// <param name="path">Path.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        async Task<T> Update<T>(T updated, string path)
        {
            var response = await restClient.SendAsync(CreateRequest(HttpMethod.Put, path, updated));
            await ThrowIfNotStatus(HttpStatusCode.OK, response);
            return JsonConvert.DeserializeObject<T>(await response.Content.ReadAsStringAsync());
        }


        /// <summary>
        /// Generic DELETE method for entities. DELETEs the specified resource.
        /// </summary>
        /// <returns>The delete.</returns>
        /// <param name="path">Path.</param>
        async Task Delete(string path)
        {
            await ThrowIfNotStatus(HttpStatusCode.NoContent,
                 await restClient.SendAsync(
                     CreateRequest(HttpMethod.Delete, path, null)));
        }

        /// <summary>
        /// Generic GET wrapper.
        /// </summary>
        /// <returns>The get.</returns>
        /// <param name="path">Path.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        async Task<T> Get<T>(string path)
        {
            var response = await restClient.SendAsync(CreateRequest(HttpMethod.Get, path, null));
            await ThrowIfNotStatus(HttpStatusCode.OK, response);
            return JsonConvert.DeserializeObject<T>(await response.Content.ReadAsStringAsync());
        }

        /// <summary>
        /// Creates the application async.
        /// </summary>
        /// <returns>The application async.</returns>
        /// <param name="template">Template.</param>
        public async Task<Application> CreateApplicationAsync(Application template)
        {
            return await Create(template, "/applications");
        }

        /// <summary>
        /// Updates the application async.
        /// </summary>
        /// <returns>The application async.</returns>
        /// <param name="updated">Updated.</param>
        public async Task<Application> UpdateApplicationAsync(Application updated)
        {
            return await Update(updated, "/applications/" + updated.ApplicationEUI);
        }

        /// <summary>
        /// Deletes the application async.
        /// </summary>
        /// <returns>The application async.</returns>
        /// <param name="applicationEUI">Application eui.</param>
        public async Task DeleteApplicationAsync(string applicationEUI)
        {
            await Delete("/applications/" + applicationEUI);
        }

        /// <summary>
        /// List all applications
        /// </summary>
        /// <returns>The applications async.</returns>
        public async Task<ApplicationList> ListApplicationsAsync()
        {
            return await Get<ApplicationList>("/applications");
        }

        /// <summary>
        /// Retrieve a single application.
        /// </summary>
        /// <returns>The application async.</returns>
        /// <param name="applicationEUI">Application eui.</param>
        public async Task<Application> GetApplicationAsync(string applicationEUI)
        {
            return await Get<Application>("/applications/" + applicationEUI);
        }

        /// <summary>
        /// Creates the gateway async.
        /// </summary>
        /// <returns>The gateway async.</returns>
        /// <param name="template">Template.</param>
        public async Task<Gateway> CreateGatewayAsync(Gateway template)
        {
            return await Create(template, "/gateways");
        }

        /// <summary>
        /// Updates the gateway async.
        /// </summary>
        /// <returns>The gateway async.</returns>
        /// <param name="updated">Updated.</param>
        public async Task<Gateway> UpdateGatewayAsync(Gateway updated)
        {
            return await Update(updated, "/gateways/" + updated.GatewayEUI);
        }

        /// <summary>
        /// Deletes the gateway.
        /// </summary>
        /// <returns>The gateway.</returns>
        /// <param name="gatewayEUI">Gateway eui.</param>
        public async Task DeleteGateway(string gatewayEUI)
        {
            await Delete("/gateways/" + gatewayEUI);
        }

        /// <summary>
        /// List all gateways
        /// </summary>
        /// <returns>The gateways async.</returns>
        public async Task<GatewayList> ListGatewaysAsync()
        {
            return await Get<GatewayList>("/gateways");
        }

        /// <summary>
        /// Retrive a single gateway
        /// </summary>
        /// <returns>The gateway async.</returns>
        /// <param name="gatewayEUI">Gateway eui.</param>
        public async Task<Gateway> GetGatewayAsync(string gatewayEUI)
        {
            return await Get<Gateway>("/gateways/" + gatewayEUI);
        }

        /// <summary>
        /// Creates the device async.
        /// </summary>
        /// <returns>The device async.</returns>
        /// <param name="applicationEUI">Application eui.</param>
        /// <param name="template">Template.</param>
        public async Task<Device> CreateDeviceAsync(string applicationEUI, Device template)
        {
            return await Create(template, "/applications/" + applicationEUI + "/devices");
        }

        /// <summary>
        /// Updates the device async.
        /// </summary>
        /// <returns>The device async.</returns>
        /// <param name="applicationEUI">Application eui.</param>
        /// <param name="updated">Updated.</param>
        public async Task<Device> UpdateDeviceAsync(string applicationEUI, Device updated)
        {
            return await Update(updated, "/applications/" + applicationEUI + "/devices/" + updated.DeviceEUI);
        }

        /// <summary>
        /// Deletes the device async.
        /// </summary>
        /// <returns>The device async.</returns>
        /// <param name="applicationEUI">Application eui.</param>
        /// <param name="deviceEUI">Device eui.</param>
        public async Task DeleteDeviceAsync(string applicationEUI, string deviceEUI)
        {
            await Delete("/applications/" + applicationEUI + "/devices/" + deviceEUI);
        }

        /// <summary>
        /// List devices in application
        /// </summary>
        /// <returns>The devices async.</returns>
        /// <param name="applicationEUI">Application eui.</param>
        public async Task<DeviceList> ListDevicesAsync(string applicationEUI)
        {
            return await Get<DeviceList>("/applications/" + applicationEUI + "/devices");
        }

        /// <summary>
        /// Retrieve a single device
        /// </summary>
        /// <returns>The device async.</returns>
        /// <param name="applicationEUI">Application eui.</param>
        /// <param name="deviceEUI">Device eui.</param>
        public async Task<Device> GetDeviceAsync(string applicationEUI, string deviceEUI)
        {
            return await Get<Device>("/applications/" + applicationEUI + "/devices/" + deviceEUI);
        }


        /// <summary>
        /// Retrieve data send by the device.
        /// </summary>
        /// <returns>The device data.</returns>
        /// <param name="applicationEUI">Application eui.</param>
        /// <param name="deviceEUI">Device eui.</param>
        public async Task<DeviceDataList> GetDeviceDataAsync(string applicationEUI, string deviceEUI)
        {
            return await Get<DeviceDataList>("/applications/" + applicationEUI + "/devices/" + deviceEUI + "/data");
        }
    }
}
