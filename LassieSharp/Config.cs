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
using System.IO;

namespace Lassie
{
    /// <summary>
    /// This is the default endpoint to use.
    /// </summary>
    public static class Const
    {
        public const string DefaultEndpoint = "https://api.lora.telenor.io";
        public const string DefaultToken = "";
    }
    /// <summary>
    /// Configuration interface. Provides two properties; Endpoint and Token.
    /// </summary>
    public interface Configuration
    {
        /// <summary>
        /// Gets the API endpoint for Lassie/Congress
        /// </summary>
        /// <value>The endpoint.</value>
        string Endpoint { get; }

        /// <summary>
        /// The API token for Congress.
        /// </summary>
        /// <value>The token.</value>
        string Token { get; }
    }

    /// <summary>
    /// File configuration. Gets configuration from a configuration file stored
    /// in the user's home directory.
    /// </summary>
    public class ConfigFile : Configuration
    {
        public const string ConfigFileName = "lassie.cfg";

        string endpoint = Const.DefaultEndpoint;
        string token = Const.DefaultToken;

        public ConfigFile()
        {
            var fileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ConfigFileName);
            if (File.Exists(fileName))
            {
                foreach (string line in File.ReadAllLines(fileName))
                {
                    var elements = line.ToLower().Split('=');
                    if (elements.Length == 2)
                    {
                        if (elements[0] == "endpoint")
                        {
                            endpoint = elements[1];
                        }
                        if (elements[0] == "token")
                        {
                            token = elements[1];
                        }
                    }
                }
            }
        }

        public string Endpoint { get { return endpoint; } }
        public string Token { get { return token; } }
    }

    /// <summary>
    /// Environment config.
    /// </summary>
    public class EnvironmentConfig : Configuration
    {
        public string Endpoint
        {
            get
            {
                var env = Environment.GetEnvironmentVariable("LASSIE_ENDPOINT");
                if (env == null)
                {
                    return Const.DefaultEndpoint;
                }
                return env;
            }
        }
        public string Token
        {
            get
            {
                var env = Environment.GetEnvironmentVariable("LASSIE_TOKEN");
                if (env == null)
                {
                    return Const.DefaultToken;
                }
                return env;
            }
        }
    }

    /// <summary>
    /// Combo configuration that first checks for a file config, then an
    /// environment config, ie the environment overrides the file. If neither
    /// a file or environment variable is found the default is returned;
    /// </summary>
    public class ComboConfig : Configuration
    {


        readonly ConfigFile file = new ConfigFile();
        readonly EnvironmentConfig env = new EnvironmentConfig();

        public string Endpoint
        {
            get
            {
                if (env.Endpoint != Const.DefaultEndpoint)
                {
                    return env.Endpoint;
                }
                return file.Endpoint;
            }
        }

        public string Token
        {
            get
            {
                if (env.Token != Const.DefaultToken)
                {
                    return env.Token;
                }
                return file.Token;
            }
        }
    }
}
