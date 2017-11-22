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
        public Application[] Applications { get; set; }
    }
}
