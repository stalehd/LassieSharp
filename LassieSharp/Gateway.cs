using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Lassie
{
    /// <summary>
    /// Gateway.
    /// </summary>
    public class Gateway
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Lassie.Gateway"/> class.
        /// </summary>
        public Gateway() {
            Tags = new Dictionary<string, string>();
        }

        /// <summary>
        /// Gets or sets the gateway eui.
        /// </summary>
        /// <value>The gateway eui.</value>
        [JsonProperty("gatewayEUI")]
        public string GatewayEUI { get; set;  }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="T:Lassie.Gateway"/> strict ip.
        /// </summary>
        /// <value><c>true</c> if strict ip; otherwise, <c>false</c>.</value>
        [JsonProperty("strictIP")]
        public bool StrictIP { get; set;  }

        /// <summary>
        /// Gets or sets the ip.
        /// </summary>
        /// <value>The ip.</value>
        [JsonProperty("ip")]
        public string IP { get; set;  }

        /// <summary>
        /// Gets or sets the latitude.
        /// </summary>
        /// <value>The latitude.</value>
        [JsonProperty("latitude")]
        public float Latitude { get; set;  }

        /// <summary>
        /// Gets or sets the longitude.
        /// </summary>
        /// <value>The longitude.</value>
        [JsonProperty("longitude")]
        public float Longitude { get; set;  }

        /// <summary>
        /// Gets or sets the altitude.
        /// </summary>
        /// <value>The altitude.</value>
        [JsonProperty("altitude")]
        public float Altitude { get; set; }

        /// <summary>
        /// Gets or sets the tags.
        /// </summary>
        /// <value>The tags.</value>
        [JsonProperty("tags")]
        public Dictionary<string, string> Tags { get; set; }
    }

    public class GatewayList {
        [JsonProperty("gateways")]
        public Gateway[] Gateways { get; set; }
    }
}
