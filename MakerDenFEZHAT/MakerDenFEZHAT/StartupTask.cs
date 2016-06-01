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
        DeviceClient deviceClient = DeviceClient.CreateFromConnectionString("Connection String");

        #region Expand to view global variables
        BackgroundTaskDeferral deferral;
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
            telemetry = new Telemetry("Sydney", Publish);
            iotHubCommand = new IoTHubCommand<string>(deviceClient, telemetry);
            iotHubCommand.CommandReceived += Commanding_CommandReceived;
            #endregion


            #region Code snippets to go between the #region and #endregion tags

            while (true)
            {
                hat.D2.Color = FEZHAT.Color.Red;
                await Task.Delay(500);

                hat.D2.Color = FEZHAT.Color.Green;
                await Task.Delay(500);

                hat.D2.Color = FEZHAT.Color.Blue;
                await Task.Delay(500);
            }

            #endregion
        }

        async void Publish()
        {
            #region Publish to Azure IoTHub



            #endregion
        }

        private void Commanding_CommandReceived(object sender, CommandEventArgs<string> e)
        {
            #region IoT Hub Command Support

            

            #endregion
        }
    }
}
