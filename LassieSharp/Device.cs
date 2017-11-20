using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Lassie
{
    /// <summary>
    /// Device.
    /// </summary>
    public class Device
    {
        /// <summary>
        /// Over-the-air-activated device type
        /// </summary>
        public const string OTAA = "OTAA";

        /// <summary>
        /// Acivation By Personalization device type.
        /// </summary>
        public const string ABP = "ABP";

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Lassie.Device"/> class.
        /// </summary>
        public Device()
        {
            Tags = new Dictionary<string, string>();
        }

        /// <summary>
        /// Gets or sets the device eui.
        /// </summary>
        /// <value>The device eui.</value>
        [JsonProperty("deviceEUI")]
        public string DeviceEUI { get; set; }

        /// <summary>
        /// Gets or sets the device address.
        /// </summary>
        /// <value>The device address.</value>
        [JsonProperty("DevAddr")]
        public string DeviceAddress { get; set; }

        /// <summary>
        /// Gets or sets the application session key.
        /// </summary>
        /// <value>The application session key.</value>
        [JsonProperty("appSKey")]
        public string ApplicationSessionKey { get; set; }

        /// <summary>
        /// Gets or sets the application key.
        /// </summary>
        /// <value>The application key.</value>
        [JsonProperty("appKey")]
        public string ApplicationKey { get; set; }

        /// <summary>
        /// Gets or sets the network session key.
        /// </summary>
        /// <value>The network session key.</value>
        [JsonProperty("nwkSKey")]
        public string NetworkSessionKey { get; set; }

        /// <summary>
        /// Gets or sets the frame counter up.
        /// </summary>
        /// <value>The frame counter up.</value>
        [JsonProperty("fCntUp")]
        public UInt16 FrameCounterUp { get; set; }

        /// <summary>
        /// Gets or sets the frame counter down.
        /// </summary>
        /// <value>The frame counter down.</value>
        [JsonProperty("fCntDn")]
        public UInt16 FrameCounterDown { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="T:Lassie.Device"/> relaxed counter.
        /// </summary>
        /// <value><c>true</c> if relaxed counter; otherwise, <c>false</c>.</value>
        [JsonProperty("relaxedCounter")]
        public bool RelaxedCounter { get; set; }

        /// <summary>
        /// Gets or sets the type of the device.
        /// </summary>
        /// <value>The type of the device.</value>
        [JsonProperty("deviceType")]
        public string DeviceType { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="T:Lassie.Device"/> key warning.
        /// </summary>
        /// <value><c>true</c> if key warning; otherwise, <c>false</c>.</value>
        [JsonProperty("keyWarning")]
        public bool KeyWarning { get; set; }

        /// <summary>
        /// Gets or sets the tags.
        /// </summary>
        /// <value>The tags.</value>
        [JsonProperty("tags")]
        public Dictionary<string, string> Tags { get; set; }
    }

    public class DeviceList
    {
        [JsonProperty("devices")]
        public Device[] Devices { get; set; }
    }
}
