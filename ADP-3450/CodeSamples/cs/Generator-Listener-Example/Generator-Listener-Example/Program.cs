/* Author: Matthew Burton 27/06/23
 * ------------------------------------------------------------------------------
 * This code generates a signal on a channel of the ADP3450 and then reads a 
 * signal, this is aimed to test the tools and prove they are working as intended
 * ------------------------------------------------------------------------------
*/
#region Variables
#region General vars
using System.Runtime.InteropServices;

int devHandle;
#endregion

# region Wave generator vars
int outChannel = 0;
double outVoltAmplitude = 1.41;
double outFrequency = 10000.0;
#endregion

#region Oscilloscope vars
int inChannel = 0;
double inVoltRange = 5;
double inFrequency = 300000.0;
int minBufferSize;
int maxBufferSize;
int bufferSize;
double[] buffer;
byte status;
double triggerVoltLevel = 1.0;
int triggerTimeOut = 10;
#endregion
#endregion

#region Functions 
//Open a device 
Console.WriteLine("Automatically open the first available device");
dwf.FDwfDeviceOpen(-1, out devHandle);

#region Create signal
Console.WriteLine("Generating waveform");
// enable first channel
dwf.FDwfAnalogOutNodeEnableSet(devHandle, outChannel, dwf.AnalogOutNodeCarrier, 1);
// set sine function
dwf.FDwfAnalogOutNodeFunctionSet(devHandle, outChannel, dwf.AnalogOutNodeCarrier, dwf.funcSine);
// 10kHz
dwf.FDwfAnalogOutNodeFrequencySet(devHandle, outChannel, dwf.AnalogOutNodeCarrier, outFrequency);
// 1.41V amplitude (1Vrms), 2.82V pk2pk
dwf.FDwfAnalogOutNodeAmplitudeSet(devHandle, outChannel, dwf.AnalogOutNodeCarrier, outVoltAmplitude);
// 1.41V offset
dwf.FDwfAnalogOutNodeOffsetSet(devHandle, outChannel, dwf.AnalogOutNodeCarrier, outVoltAmplitude);
// start signal generation
dwf.FDwfAnalogOutConfigure(devHandle, outChannel, 1);
// will run until stopped, reset, parameter changed or device closed
#endregion

#region Listen to signal 

#region Configure
//Enable the analog in channel on the device 
dwf.FDwfAnalogInChannelEnableSet(devHandle, inChannel, 1);

//Set the peak to peak volt range for the channel 
dwf.FDwfAnalogInChannelRangeSet(devHandle, inChannel, inVoltRange);

//Set the frequency sample rate 
dwf.FDwfAnalogInFrequencySet(devHandle, inFrequency);

//Get the maximum buffer size
dwf.FDwfAnalogInBufferSizeInfo(devHandle, out minBufferSize, out maxBufferSize);

//Assign buffer size to appropriately named var
bufferSize = maxBufferSize;
//Create buffer array to store data at max size of buffer expected 
buffer = new double[bufferSize];

//Set the expected buffer size to maximum
dwf.FDwfAnalogInBufferSizeSet(devHandle, bufferSize);

//Configure the trigger for the analog in 
dwf.FDwfAnalogInTriggerSourceSet(devHandle, dwf.trigsrcDetectorAnalogIn);
dwf.FDwfAnalogInTriggerAutoTimeoutSet(devHandle, triggerTimeOut);
dwf.FDwfAnalogInTriggerChannelSet(devHandle, inChannel);
dwf.FDwfAnalogInTriggerTypeSet(devHandle, dwf.trigtypeEdge);
dwf.FDwfAnalogInTriggerLevelSet(devHandle, triggerVoltLevel);
dwf.FDwfAnalogInTriggerConditionSet(devHandle, dwf.trigcondRisingPositive);

#endregion

#region Read
// start
dwf.FDwfAnalogInConfigure(devHandle, 0, 1);

//Read from the device until the status is done
do
{
    dwf.FDwfAnalogInStatus(devHandle, 1, out status);
} while (status != dwf.stsDone);

//Retrieve the data
dwf.FDwfAnalogInStatusData(devHandle, inChannel, buffer, bufferSize);

#endregion

#endregion

#region Print values
foreach (double i in buffer)
{
    Console.Write($"{i},");
}
#endregion
//Close device 
dwf.FDwfDeviceClose(devHandle);
#endregion