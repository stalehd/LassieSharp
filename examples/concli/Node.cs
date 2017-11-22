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
using System.Collections.Generic;
namespace concli
{
    public delegate List<Node> ChildFunc();
    public delegate List<string> AttributeFunc();

    /// <summary>
    /// This is one node in the resource tree.
    /// </summary>
    public class Node
    {
        public ChildFunc Children;

        public string Name { get; set; }
        public AttributeFunc Attributes;

        public Node Parent { get; set; }

        string AncestorPath(string child)
        {
            if (Parent == null)
            {
                return child;
            }
            return Parent.AncestorPath(("/" + Name + child));
        }
        public string FullPath { get { return AncestorPath(""); } }
    }

    public class NodeTree
    {
        Lassie.Client client;

        readonly Node Root = new Node
        {
            Parent = null,
            Name = ""
        };

        Node DataNodeFromDevice(string appEUI, Lassie.Device device, Node parent)
        {
            var dataNode = new Node { Parent = parent, Name = "messages" };
            dataNode.Children = () => new List<Node>();
            dataNode.Attributes = () =>
            {
                var attrs = new List<string>();
                var data = client.GetDeviceDataAsync(appEUI, device.DeviceEUI);
                data.Wait();
                attrs.Add(String.Format("{0:-40} {1:-20} {2:-10} {3:-10}", "Data", "RSSI", "SNR", "Freq"));
                foreach (Lassie.DeviceData d in data.Result.Messages)
                {
                    attrs.Add(String.Format("{0:-40} {1:-20} {2:-10} {3:-10}", d.HexData, d.RSSI, d.SNR, d.Frequency));
                }
                return attrs;
            };
            return dataNode;
        }

        Node DeviceNodeFromDevice(string appEUI, Lassie.Device device, Node parent)
        {
            var deviceNode = new Node { Parent = parent, Name = device.DeviceEUI };
            deviceNode.Children = () =>
            {
                var ret = new List<Node>();
                ret.Add(DataNodeFromDevice(appEUI, device, deviceNode));
                return ret;
            };
            deviceNode.Attributes = () =>
            {
                var attrs = new List<string>();
                attrs.Add(String.Format("DeviceType:     {0}", device.DeviceType));
                attrs.Add(String.Format("DeviceEUI:      {0}", device.DeviceAddress));
                attrs.Add(String.Format("DevAddr:        {0}", device.DeviceAddress));
                attrs.Add(String.Format("AppSKey:        {0}", device.ApplicationSessionKey));
                attrs.Add(String.Format("NwkSKey:        {0}", device.NetworkSessionKey));
                attrs.Add(String.Format("AppKey:         {0}", device.ApplicationKey));
                attrs.Add(String.Format("FCntUp:         {0}", device.FrameCounterUp));
                attrs.Add(String.Format("FCntDn:         {0}", device.FrameCounterDown));
                attrs.Add(String.Format("RelaxedCounter: {0}", device.RelaxedCounter));
                attrs.Add(String.Format("KeyWarning:     {0}", device.KeyWarning));
                return attrs;
            };
            return deviceNode;
        }

        List<Node> ListDevices(string appEUI, Node parent)
        {
            var ret = new List<Node>();
            var dl = client.ListDevicesAsync(appEUI);
            dl.Wait();
            foreach (Lassie.Device d in dl.Result.Devices)
            {
                ret.Add(DeviceNodeFromDevice(appEUI, d, parent));
            }
            return ret;
        }

        Node AppNodeFromApplication(Lassie.Application app, Node parent)
        {
            var appEntry = new Node
            {
                Parent = parent,
                Name = app.ApplicationEUI
            };
            appEntry.Children = () =>
                {
                    var ret = new List<Node>();
                    var deviceListEntry = new Node { Parent = appEntry, Name = "devices" };
                    deviceListEntry.Children = () => ListDevices(app.ApplicationEUI, deviceListEntry);
                    deviceListEntry.Attributes = () => new List<string>();
                    ret.Add(deviceListEntry);
                    return ret;
                };
            appEntry.Attributes = () =>
            {
                var ret = new List<string>();
                ret.Add(String.Format("applicationEUI: {0}", app.ApplicationEUI));
                ret.Add("Tags:");
                foreach (string k in app.Tags.Keys)
                {
                    ret.Add(String.Format("{0} = {1}", k, app.Tags[k]));
                }
                return ret;
            };
            return appEntry;
        }

        Node GwNodeFromGateway(Lassie.Gateway gw, Node parent)
        {
            var gwEntry = new Node
            {
                Parent = parent,
                Name = gw.GatewayEUI
            };
            gwEntry.Children = () => new List<Node>();
            gwEntry.Attributes = () =>
            {
                var ret = new List<string>();
                ret.Add(String.Format("GatewayEUI: {0}", gw.GatewayEUI));
                ret.Add(String.Format("IP: {0}", gw.IP));
                ret.Add(String.Format("StrictIP: {0}", gw.StrictIP));
                ret.Add(String.Format("Latitude: {0}", gw.Latitude));
                ret.Add(String.Format("Longitude: {0}", gw.Longitude));
                ret.Add(String.Format("Altitude: {0}", gw.Altitude));
                ret.Add("Tags:");
                foreach (string k in gw.Tags.Keys)
                {
                    ret.Add(String.Format("{0} = {1}", k, gw.Tags[k]));
                }
                return ret;
            };
            return gwEntry;
        }

        public NodeTree(Lassie.Client c)
        {
            client = c;
            Path = "/";
            CurrentEntry = Root;

            var apps = new Node { Parent = Root, Name = "applications" };
            var gateways = new Node { Parent = Root, Name = "gateways" };
            Root.Children = () =>
            {
                var ret = new List<Node>();
                ret.Add(apps);
                ret.Add(gateways);
                return ret;
            };
            Root.Attributes = () => new List<string>();
            apps.Children = () =>
            {
                var a = new List<Node>();
                var al = client.ListApplicationsAsync();
                al.Wait();
                foreach (Lassie.Application app in al.Result.Applications)
                {
                    a.Add(AppNodeFromApplication(app, apps));
                }
                return a;
            };
            gateways.Children = () =>
            {
                var g = new List<Node>();
                var gl = client.ListGatewaysAsync();
                gl.Wait();
                foreach (Lassie.Gateway gw in gl.Result.Gateways)
                {
                    g.Add(GwNodeFromGateway(gw, gateways));
                }
                return g;
            };
            apps.Attributes = () => new List<string>();
            gateways.Attributes = () => new List<string>();
        }

        public Node CurrentEntry { get; private set; }

        public string Path { get; private set; }

        public Node ChangeEntry(string pathSpec)
        {
            var tmpEntry = CurrentEntry;
            foreach (string element in pathSpec.Split('/', StringSplitOptions.RemoveEmptyEntries))
            {
                if (element.Trim() == "..")
                {
                    if (CurrentEntry.Parent == null)
                    {
                        // Error: Can't cd below root
                        return null;
                    }
                    tmpEntry = tmpEntry.Parent;
                    continue;
                }
                else
                {
                    var found = false;
                    foreach (Node e in tmpEntry.Children())
                    {
                        if (e.Name == element.Trim())
                        {
                            found = true;
                            tmpEntry = e;
                            break;
                        }
                    }
                    if (!found)
                    {
                        return null;
                    }
                }

            }
            CurrentEntry = tmpEntry;
            return CurrentEntry;
        }
    }
}
