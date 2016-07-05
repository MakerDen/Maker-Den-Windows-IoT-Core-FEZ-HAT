using System.Threading;
using System.Threading.Tasks;

namespace IotServices
{
    public delegate void MeasureMethod();
    public class Scheduler
    {
        Timer timer;
        bool publishing = false;
        MeasureMethod measureMethod;
        int sampleRateInSeconds = 10;  // defaults to sample every 10 seconds

        public int SampleRateInSeconds {
            get { return sampleRateInSeconds; }
            set {
                sampleRateInSeconds = value > 0 ? value : sampleRateInSeconds;
                timer?.Change(0, sampleRateInSeconds * 1000);
            }
        }

        public Scheduler(MeasureMethod measureMethod, int sampleRateInSeconds) {
            if (measureMethod == null) { return; }

            this.measureMethod = measureMethod;
            this.sampleRateInSeconds = sampleRateInSeconds;

            timer = new Timer(ActionTimer, null, 0, SampleRateInSeconds * 1000);
        }

        void ActionTimer(object state) {
            if (!publishing) {
                publishing = true;
                measureMethod();
                publishing = false;
            }
        }

        public bool SetSampleRateInSeconds(string cmd) {
            ushort newSampleRateInSeconds = 0;
            if (ushort.TryParse(cmd, out newSampleRateInSeconds)) {
                SampleRateInSeconds = newSampleRateInSeconds;
                return true;
            }
            return false;
        }
    }
}
