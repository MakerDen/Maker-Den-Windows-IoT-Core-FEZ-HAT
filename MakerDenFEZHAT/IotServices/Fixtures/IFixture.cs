namespace IotServices.Fixtures
{
    interface IFixture
    {
        byte[] data { get; }
        uint[] id { get; set; }
        bool SetChannel(int channel, byte value);
        bool SetRgb(byte red, byte green, byte blue, byte white = 0);
        byte[] ToJson();
    }
}