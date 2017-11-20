using System.Collections.Generic;
using Newtonsoft.Json;

namespace Lassie
{
    /// <summary>
    /// This class represents an application in Congress.
    /// </summary>
    public class Application
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Lassie.Application"/> class.
        /// </summary>
        public Application()
        {
            this.ApplicationEUI = "";
            this.Tags = new Dictionary<string, string>();
        }

        /// <summary>
        /// Gets or sets the application eui.
        /// </summary>
        /// <value>The application eui.</value>
        [JsonProperty("applicationEUI")]
        public string ApplicationEUI { get; set; }

        /// <summary>
        /// Gets or sets the tags.
        /// </summary>
        /// <value>The tags.</value>
        [JsonProperty("tags")]
        public Dictionary<string, string> Tags { get; set; }
    }

    // A list of applications
    public class ApplicationList 
    {
        [JsonProperty("applications")]
        public Application[] Applications { get; set;  }    
    }
}
