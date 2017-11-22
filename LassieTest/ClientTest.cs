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
using Xunit;
using System.Threading.Tasks;

namespace LassieTest
{
    public class ClientTest
    {
        string NewRandomEUI()
        {
            var bytes = new byte[8];
            var r = new Random();
            r.NextBytes(bytes);
            return String.Format("{0:X2}-{1:X2}-{2:X2}-{3:X2}-{4:X2}-{5:X2}-{6:X2}-{7:X2}", bytes[0], bytes[1], bytes[2], bytes[3], bytes[4], bytes[5], bytes[6], bytes[7]);
        }

        [Fact]
        public void TestNewClient() => new Lassie.Client();


        [Fact]
        public async Task TestCreateAndDeleteApplicationAsync()
        {
            var c = new Lassie.Client();
            var app = await c.CreateApplicationAsync(new Lassie.Application());
            Assert.NotNull((app));
            await c.DeleteApplicationAsync(app.ApplicationEUI);
        }

        [Fact]
        public async Task TestUpdateApplicationAsync()
        {
            var c = new Lassie.Client();
            var app = await c.CreateApplicationAsync(new Lassie.Application());
            app.Tags["name"] = "foo";
            var updated = await c.UpdateApplicationAsync(app);
            Assert.Equal(app.ApplicationEUI, updated.ApplicationEUI);
            Assert.Equal("foo", updated.Tags["name"]);
            await c.DeleteApplicationAsync(updated.ApplicationEUI);
        }

        [Fact]
        public async Task TestCreateAndDeleteGatewayAsync()
        {
            var c = new Lassie.Client();
            var template = new Lassie.Gateway
            {
                GatewayEUI = NewRandomEUI().ToString(),
                IP = "127.0.0.1",
                StrictIP = false,
            };
            var gw = await c.CreateGatewayAsync(template);
            Assert.NotNull((gw));
            Assert.Equal(gw.GatewayEUI.ToLower(), template.GatewayEUI.ToLower());
            Assert.Equal(gw.IP, template.IP);

            await c.DeleteGateway(gw.GatewayEUI);
        }

        [Fact]
        public async Task TestUpdateGatewayAsync()
        {
            var c = new Lassie.Client();
            var template = new Lassie.Gateway
            {
                GatewayEUI = NewRandomEUI().ToString(),
                IP = "127.0.0.1",
                StrictIP = false,
            };
            var gw = await c.CreateGatewayAsync(template);
            gw.Tags["name"] = "bar";
            gw.Tags["foo"] = "bar";

            gw.Latitude = 1.0f;
            gw.Longitude = 2.0f;
            gw.Altitude = 3.0f;

            var updated = await c.UpdateGatewayAsync(gw);
            Assert.NotNull(updated);
            Assert.Equal(updated.Tags["name"], "bar");
            Assert.Equal(updated.Tags["foo"], "bar");

            Assert.Equal(updated.Latitude, 1.0f);
            Assert.Equal(updated.Longitude, 2.0f);
            Assert.Equal(updated.Altitude, 3.0f);
            await c.DeleteGateway(gw.GatewayEUI);
        }

        [Fact]
        public async Task TestAddRemoveDevice()
        {
            var c = new Lassie.Client();
            var app = await c.CreateApplicationAsync(new Lassie.Application());
            var device = await c.CreateDeviceAsync(app.ApplicationEUI, new Lassie.Device { DeviceType = Lassie.Device.OTAA });

            Assert.NotNull(device);

            await c.DeleteDeviceAsync(app.ApplicationEUI, device.DeviceEUI);

        }

        [Fact]
        public async Task TestUpdateDevice()
        {
            var c = new Lassie.Client();
            var app = await c.CreateApplicationAsync(new Lassie.Application());
            var device = await c.CreateDeviceAsync(app.ApplicationEUI, new Lassie.Device { DeviceType = Lassie.Device.ABP });

            Assert.NotNull(device);
            Assert.Equal(Lassie.Device.ABP, device.DeviceType);

            device.Tags["name"] = "foo";
            device.RelaxedCounter = true;
            device.ApplicationKey = "01020304aabbccdd01020304aabbccdd";
            var updated = await c.UpdateDeviceAsync(app.ApplicationEUI, device);
            Assert.Equal(device.RelaxedCounter, updated.RelaxedCounter);
            Assert.Equal(device.ApplicationKey, updated.ApplicationKey);
            Assert.Equal("foo", device.Tags["name"]);

            var dd = await c.GetDeviceDataAsync(app.ApplicationEUI, device.DeviceEUI);
            Assert.Equal(0, dd.Messages.Length);
            await c.DeleteDeviceAsync(app.ApplicationEUI, device.DeviceEUI);

        }
    }
}
