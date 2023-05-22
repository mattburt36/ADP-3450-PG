/* Author: Matthew Burton 23/05/23
 * ------------------------------------------------------------------------------
 * This code opens an analog output and analog input device, configures and starts 
 * the analog output channel to generate a sine wave, configures the analog input 
 * settings including frequency, channel range, and buffer size, waits for the analog 
 * input offset to stabilize, starts the analog input acquisition, waits for the 
 * acquisition to finish, reads the acquired data into an array, performs some 
 * operations on the acquired data, and finally closes the device.
 * ------------------------------------------------------------------------------
*/
int hdwf;
byte sts;
const int nSamples = 1000;
double[] rgdSamples = new double[nSamples];

Console.WriteLine("Open automatically the first available device");
dwf.FDwfDeviceOpen(-1, out hdwf);

Console.WriteLine("Configure and start first analog out channel\n");
dwf.FDwfAnalogOutEnableSet(hdwf, 0, 1);
// 1 = Sine wave
dwf.FDwfAnalogOutFunctionSet(hdwf, 0, 1);
dwf.FDwfAnalogOutFrequencySet(hdwf, 0, 1);
dwf.FDwfAnalogOutConfigure(hdwf, 9, 1);

Console.WriteLine("Configure analog in\n");
dwf.FDwfAnalogInFrequencySet(hdwf, 1000000);
// set range for all channels
dwf.FDwfAnalogInChannelRangeSet(hdwf, -1, 4);
dwf.FDwfAnalogInBufferSizeSet(hdwf, nSamples);

Console.WriteLine("Wait after first device opening the analog in offset to stabilize\n");

Console.WriteLine("Starting acquisition\n");
dwf.FDwfAnalogInConfigure(hdwf, 1, 1);

Console.WriteLine("Waiting to finish... ");
while (true)
{
    dwf.FDwfAnalogInStatus(hdwf, 1, out sts);
    if (sts == dwf.DwfStateDone)
    {
        break;
    }
}
Console.WriteLine("done\n");

Console.WriteLine("Reading acquisition data\n");
dwf.FDwfAnalogInStatusData(hdwf, 0, rgdSamples, nSamples);
// do something with the data in rgdSample

// close the device
dwf.FDwfDeviceClose(hdwf);