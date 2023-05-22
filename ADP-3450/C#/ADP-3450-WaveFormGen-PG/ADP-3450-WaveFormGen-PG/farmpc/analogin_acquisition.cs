/* Author: Matthew Burton 16/05/23
 * ------------------------------------------------------------------------------
 * Code for acquisitioning data on oscilloscope 
 * ------------------------------------------------------------------------------
*/
int hdwf;
byte sts;
double[] rgdSamples;
int cSamples;
int cChannel;
int throwAway;

// Open Device
// Opens a device identified by the enumeration index and retrieves a handle. To automatically
//enumerate all connected devices and open the first discovered device, use index - 1.
dwf.FDwfDeviceOpen(-1, out hdwf);

// get the number of analog in channels
dwf.FDwfAnalogInChannelCount(hdwf, out cChannel);

// enable channels
for (int c = 0; c < cChannel; c++)
{
    dwf.FDwfAnalogInChannelEnableSet(hdwf, c, 1);
}
// set 5V pk2pk input range for all channels
dwf.FDwfAnalogInChannelRangeSet(hdwf, -1, 5);

// 20MHz sample rate
dwf.FDwfAnalogInFrequencySet(hdwf, 20000000.0);

// get the maximum buffer size
dwf.FDwfAnalogInBufferSizeInfo(hdwf, out throwAway, out cSamples);
dwf.FDwfAnalogInBufferSizeSet(hdwf, cSamples);

rgdSamples = new double[cSamples];

// configure trigger
dwf.FDwfAnalogInTriggerSourceSet(hdwf, dwf.trigsrcDetectorAnalogIn);
dwf.FDwfAnalogInTriggerAutoTimeoutSet(hdwf, 10.0);
dwf.FDwfAnalogInTriggerChannelSet(hdwf, 0);
dwf.FDwfAnalogInTriggerTypeSet(hdwf, dwf.trigtypeEdge);
dwf.FDwfAnalogInTriggerLevelSet(hdwf, 1.0);
dwf.FDwfAnalogInTriggerConditionSet(hdwf, dwf.trigcondRisingPositive);

// start
dwf.FDwfAnalogInConfigure(hdwf, 0, 1);

Console.WriteLine("Waiting for triggered or auto acquisition\n");
do
{
    dwf.FDwfAnalogInStatus(hdwf, 1, out sts);
} while (sts != dwf.stsDone);

// get the samples for each channel
for (int c = 0; c < cChannel; c++)
{
    dwf.FDwfAnalogInStatusData(hdwf, c, rgdSamples, cSamples);
    // do something with it
}

Console.WriteLine("done\n");

// close the device
dwf.FDwfDeviceClose(hdwf);