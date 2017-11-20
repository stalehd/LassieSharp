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
