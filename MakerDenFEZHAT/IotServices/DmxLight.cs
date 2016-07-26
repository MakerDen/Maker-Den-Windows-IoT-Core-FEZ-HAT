using IotServices.Fixtures;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using uPLibrary.Networking.M2Mqtt;
using Windows.UI;

namespace IotServices
{
    public class DmxLight
    {
        IFixture rgbwLight = new GenericRGBW();

        MqttClient client;
        const string MqttTopic = "dmx/data/";
        string MqttBroker;

        const int publishCycleTime = 250;
        AutoResetEvent publishEvent = new AutoResetEvent(false);
        private bool isConnected => client != null && client.IsConnected;


        public DmxLight(string mqttBroker, uint[] fixtureIds)
        {
            rgbwLight.id = fixtureIds;
            this.MqttBroker = mqttBroker;

            Task.Run(new Action(Publish));
        }

        public DmxLight(uint fixtureId)
        {
            rgbwLight.id = new uint[] { fixtureId };
            Task.Run(new Action(Publish));
        }

        public void Update(Color c)
        {
            rgbwLight.SetRgb(c.R, c.G, c.B);
            publishEvent.Set();
        }

        public void Update(byte red, byte green, byte blue)
        {
            if (!rgbwLight.SetRgb(red, green, blue)) { publishEvent.Set(); }
        }

        async void Publish()  //run async
        {
            while (true)
            {
                await Task.Delay(publishCycleTime);
                publishEvent.WaitOne();

                if (client == null || !client.IsConnected)
                {
                    try
                    {
                        if (client == null)
                        {
                            client = new MqttClient(MqttBroker);
                        }

                        client.Connect(Guid.NewGuid().ToString().Substring(0, 20));

                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine($"Failed connection to broker: exception: {ex.Message}");
                    }
                }

                if (!isConnected)
                {
                    continue;
                }


                try
                {
                   
                    var json = rgbwLight.ToJson();

                    client.Publish($"{MqttTopic}", json);
                    

                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Failed to send to client {ex.Message}");
                }
            }
        }
    }
}
