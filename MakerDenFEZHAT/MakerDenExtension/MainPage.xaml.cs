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

        #endregion

        DmxLight dmxLight = new DmxLight("mqtt broker address", new uint[] { 1 });

        public MainPage()
        {
            this.InitializeComponent();

            Setup();
        }

        private async void Setup()
        {
            #region Lab4b Code to go between the #region and #endregion tags

            myTransformGroup.Children.Add(myTranslate);
            orb.RenderTransform = myTransformGroup;

            this.hat = await FEZHAT.CreateAsync();

            timer = new DispatcherTimer();
            this.timer.Interval = TimeSpan.FromMilliseconds(100);
            this.timer.Tick += this.UpdateOrb;
            this.timer.Start();

            #endregion
        }

        private void UpdateOrb(object sender, object e)
        {
            #region Lab4c and Lab4d Code to go between the #region and #endregion tags

            Temperature.Text = $"The temperature is {hat.GetTemperature().ToString("n2")}";
            Light.Text = $"The light level is {hat.GetLightLevel().ToString("n4")}";

            hat.GetAcceleration(out x, out y, out z);

            computedColour = ComputeColour(x, y, z);

            orb.Fill = new SolidColorBrush(computedColour);

            ComputeOrbPosition(x, y);

            orb.UpdateLayout(); // update the orb with the new colour and position

            hat.D2.Color = hat.D3.Color = FezColour(computedColour); // set both Fez Hat RGB LEDs

            dmxLight.Update(computedColour); // set colour on DMX RGB Light    

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