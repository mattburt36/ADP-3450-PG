/* Author: Matthew Burton 23/05/23
 * ------------------------------------------------------------------------------
 * This code opens the first available device, configures it for digital input, 
 * sets the desired sample rate to 1MHz, triggers on any pin transition, waits 
 * for the acquisition to complete, retrieves the digital input samples, converts 
 * them from a byte array to a uint array, and finally closes the device.
 * ------------------------------------------------------------------------------
*/
int hdwf;
byte sts;
double hzSys;
uint[] rgwSamples;
int cSamples;
string szError;

Console.WriteLine("Open automatically the first available device");
dwf.FDwfDeviceOpen(-1, out hdwf);

dwf.FDwfDigitalInInternalClockInfo(hdwf, out hzSys);

// Calculate the desired sample rate
uint desiredSampleRate = (uint)(hzSys / 1e6);

// Set the sample rate to 1MHz
dwf.FDwfDigitalInDividerSet(hdwf, desiredSampleRate);

// 16bit WORD format
dwf.FDwfDigitalInSampleFormatSet(hdwf, 16);

// get the maximum buffer size
dwf.FDwfDigitalInBufferSizeInfo(hdwf, out cSamples);
// default buffer size is set to maximum
//FDwfDigitalInBufferSizeSet(hdwf, cSamples);
rgwSamples = new uint[cSamples];

// set trigger position to the middle 
dwf.FDwfDigitalInTriggerPositionSet(hdwf, (uint)(cSamples / 2));

// trigger on any pin transition
dwf.FDwfDigitalInTriggerSourceSet(hdwf, dwf.trigsrcDetectorDigitalIn);
dwf.FDwfDigitalInTriggerAutoTimeoutSet(hdwf, 10.0);
dwf.FDwfDigitalInTriggerSet(hdwf, 0, 0, 0xFFFF, 0xFFFF);

// start
dwf.FDwfDigitalInConfigure(hdwf, 0, 1);

Console.WriteLine("Waiting for triggered or auto acquisition...");
do
{
    if (dwf.FDwfDigitalInStatus(hdwf, 1, out sts) == 0) return;
} while (sts != dwf.stsDone);

// Create a byte array to receive the samples
byte[] rgSamplesBytes = new byte[cSamples * sizeof(uint)];

// get the samples
dwf.FDwfDigitalInStatusData(hdwf, rgSamplesBytes, cSamples * sizeof(uint));

// Convert the byte array to uint array
Buffer.BlockCopy(rgSamplesBytes, 0, rgwSamples, 0, rgSamplesBytes.Length);

Console.WriteLine("done\n");

// close the device
dwf.FDwfDeviceClose(hdwf);