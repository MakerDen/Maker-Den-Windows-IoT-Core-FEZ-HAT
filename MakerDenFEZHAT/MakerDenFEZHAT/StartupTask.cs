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
        #endregion

        public async void Run(IBackgroundTaskInstance taskInstance)
        {
            #region Expand to view variable initialisation
            deferral = taskInstance.GetDeferral();
            hat = await FEZHAT.CreateAsync();
            deviceClient = ConnectionString.StartsWith("Connection") ? null : DeviceClient.CreateFromConnectionString(ConnectionString);
            telemetry = new Telemetry("Sydney", Publish, 10);
            Command_Processing();
            #endregion

            #region Code snippets lab2 and lab3 to go between the #region and #endregion tags

            DmxLight dmxLight = new DmxLight("192.168.1.117", new uint[] { 2, 3, 4, 5, 6, 7, 8, 9 });
            double x, y, z = 0;

            while (true)
            {
                hat.GetAcceleration(out x, out y, out z);

                dmxLight.Update((byte)(Math.Abs(x * 255)), (byte)(Math.Abs(y * 255)), (byte)(Math.Abs(z * 255)));

                await Task.Delay(150);
            }

            #endregion
        }

        async void Publish()
        {
            #region Snippet lab6 - Publish to Azure IoT Hub

            #endregion
        }

        private async void Command_Processing()
        {
            #region Snippet lab9 - IoT Hub Command Support

            #endregion
        }
    }
}
