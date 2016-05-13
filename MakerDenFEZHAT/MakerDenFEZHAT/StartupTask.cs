//file name: StartupTask.cs

using GHIElectronics.UWP.Shields;
using IotServices;
using Microsoft.Azure.Devices.Client;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;

namespace MakerDenFEZHAT
{
    public sealed class StartupTask : IBackgroundTask
    {
        #region Expand to view global variables
        BackgroundTaskDeferral deferral;
        DeviceClient deviceClient;
        IoTHubCommand<String> iotHubCommand;
        Telemetry telemetry;
        
        FEZHAT.Color publishColor = FEZHAT.Color.Green;
        FEZHAT hat;

        const double LIGHT_THRESHOLD = 85d;
        #endregion
        

        public async void Run(IBackgroundTaskInstance taskInstance)
        {
            #region Expand to view variable initialisation
            deferral = taskInstance.GetDeferral();
            hat = await FEZHAT.CreateAsync();
            telemetry = new Telemetry("Sydney", "RPiFez", Publish, 10);
            iotHubCommand = new IoTHubCommand<string>(deviceClient, telemetry);
            iotHubCommand.CommandReceived += Commanding_CommandReceived;
            #endregion

            deviceClient = DeviceClient.CreateFromConnectionString("device connection string");

            #region Code snippets to go between the #region and #endregion tags

            while (true)
            {
                var level = hat.GetLightLevel() * 100;

                if (level > LIGHT_THRESHOLD)
                {
                    hat.D2.Color = FEZHAT.Color.Green;
                }
                else
                {
                    hat.D2.Color = FEZHAT.Color.Red;
                }

                await Task.Delay(500);
            }

            #endregion
        }

        async void Publish()
        {
            #region Publish to Azure IoTHub

            try  // Exception handling if problem streaming telemetry to Azure IoT Hub
            {
                hat.D3.Color = publishColor;      // turn on publish indicator LED

                var temperature = hat.GetTemperature(); // read temperature from the FEZ HAT
                var light = hat.GetLightLevel();        // read light level from the FEZ HAT
                var json = telemetry.ToJson(temperature, light, 0, 0);  //serialise to JSON

                var content = new Message(json);
                await deviceClient.SendEventAsync(content); //Send telemetry data to IoT Hub
            }
            catch { telemetry.Exceptions++; }
            finally { hat.D3.TurnOff(); }

            #endregion
        }

        private void Commanding_CommandReceived(object sender, CommandEventArgs<string> e)
        {
            #region IoT Hub Command Support

            char cmd = e.Item.Length > 0 ? e.Item.ToUpper()[0] : ' ';  // get command character sent from IoT Hub

            switch (cmd)
            {
                case 'R':
                    publishColor = FEZHAT.Color.Red;
                    break;
                case 'G':
                    publishColor = FEZHAT.Color.Green;
                    break;
                case 'B':
                    publishColor = FEZHAT.Color.Blue;
                    break;
                case 'Y':
                    publishColor = FEZHAT.Color.Yellow;
                    break;
                case 'M':
                    publishColor = FEZHAT.Color.Magneta;
                    break;
                default:
                    System.Diagnostics.Debug.WriteLine("Unrecognized command: {0}", e.Item);
                    break;
            }

            hat.D3.Color = publishColor;

            #endregion
        }
    }
}
