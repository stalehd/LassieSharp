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

namespace Lassie
{
    public class DeviceData
    {
        [JsonProperty("deviceEUI")]
        public string DeviceEUI { get; set; }

        [JsonProperty("devAddr")]
        public string DeviceAddress { get; set; }

        [JsonProperty("gatewayEUI")]
        public string GatewayEUI { get; set; }

        [JsonProperty("appEUI")]
        public string ApplicationEUI { get; set; }

        [JsonProperty("timestamp")]
        public Int64 Timestamp { get; set; }

        [JsonProperty("data")]
        public string HexData { get; set; }

        [JsonProperty("frequency")]
        public float Frequency { get; set; }

        [JsonProperty("dataRate")]
        public string DataRate { get; set; }

        [JsonProperty("rssi")]
        public int RSSI { get; set; }

        [JsonProperty("snr")]
        public float SNR { get; set; }
    }

    public class DeviceDataList
    {
        [JsonProperty("messages")]
        public DeviceData[] Messages { get; set; }
    }
}
