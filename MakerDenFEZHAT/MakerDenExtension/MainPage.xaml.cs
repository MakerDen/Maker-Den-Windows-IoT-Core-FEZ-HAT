using GHIElectronics.UWP.Shields;
using IotServices;
using System;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace MakerDenExtension
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        #region Expand to view global variables

        FEZHAT hat;

        TransformGroup myTransformGroup = new TransformGroup();
        TranslateTransform myTranslate = new TranslateTransform();

        double screenWidth = Window.Current.Bounds.Width;
        double screenHeight = Window.Current.Bounds.Height;

        Color computedColour = Colors.Yellow;
        double x, y, z;

        DispatcherTimer timer;

        DmxLight dmxLight = new DmxLight(MqttBrokerAddress, new uint[] { FixtureId });

        #endregion

        const string MqttBrokerAddress = "MQTT Broker Address";
        const uint FixtureId = 1;

        public MainPage()
        {
            this.InitializeComponent();

            Setup();
        }

        private async void Setup()
        {
            #region Lab5a Code to go between the #region and #endregion tags


            #endregion
        }

        private void UpdateOrb(object sender, object e)
        {
            #region Lab5b, Lab6 and Lab7 Code to go between the #region and #endregion tags
 

            #endregion
        }

        void ComputeOrbPosition(double x, double y)
        {
            var newX = (screenWidth - orb.Width) / 2 * -y;
            var newY = (screenHeight - orb.Height) / 2 * -x;

            if (double.IsNaN(newX) || double.IsNaN(newY)) { return; }

            myTranslate.X = newX;
            myTranslate.Y = newY;

        }

        FEZHAT.Color FezColour(Color colour)
        {
            return new FEZHAT.Color(colour.R, colour.G, colour.B);
        }

        Color ComputeColour(double x, double y, double z)
        {
            return new Color() { R = (byte)((1 - Math.Abs(x)) * 255), G = (byte)((1 - Math.Abs(y)) * 255), B = 0, A = 255 };
        }
    }
}