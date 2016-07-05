//file name: StartupTask.cs

using GHIElectronics.UWP.Shields;
using IotServices;
using Microsoft.Azure.Devices.Client;
using System;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;

namespace MakerDenFEZHAT
{
    public sealed class StartupTask : IBackgroundTask
    {
        const string ConnectionString = "Connection String";
        const double Light_Threshold = 85d;

        #region Expand to view global variables
        BackgroundTaskDeferral deferral;
        DeviceClient deviceClient;
        Telemetry telemetry;
        FEZHAT.Color publishColor = FEZHAT.Color.Green;
        FEZHAT hat;

        public static string ConnectionString1
        {
            get
            {
                return ConnectionString;
            }
        }
        #endregion

        public async void Run(IBackgroundTaskInstance taskInstance)
        {
            #region Expand to view variable initialisation
            deferral = taskInstance.GetDeferral();
            hat = await FEZHAT.CreateAsync();
            deviceClient = ConnectionString.StartsWith("Connection") ? null : DeviceClient.CreateFromConnectionString(ConnectionString);
            telemetry = new Telemetry("Sydney", Publish, 5);
            Command_Processing();
            #endregion

            #region Code snippets for labs 1 & 2 go between the #region and #endregion tags

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
            #region Lab 6 - Publish to Azure IoTHub

            #endregion
        }

        private async void Command_Processing()
        {
            #region Lab 7 - IoT Hub Command Support
           
            #endregion
        }
    }
}
