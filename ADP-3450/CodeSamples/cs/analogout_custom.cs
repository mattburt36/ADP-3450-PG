/* Author: Matthew Burton 23/05/23
 * ------------------------------------------------------------------------------
 * This code opens an analog output device, generates a custom waveform for 5 
 * seconds with specific settings such as waveform samples, frequency, amplitude, 
 * and offset, configures the analog output, and then closes the device.
 * ------------------------------------------------------------------------------
*/
int hdwf;
double[] rgdSamples = new double[4096];

Console.WriteLine("Open automatically the first available device");
dwf.FDwfDeviceOpen(-1, out hdwf);


Console.WriteLine("Generating custom waveform for 5 seconds...");
// enable first channel
dwf.FDwfAnalogOutNodeEnableSet(hdwf, 0, dwf.AnalogOutNodeCarrier, 1);
// set custom function
dwf.FDwfAnalogOutNodeFunctionSet(hdwf, 0, dwf.AnalogOutNodeCarrier, dwf.funcCustom);
// set custom waveform samples
// normalized to ±1 values
dwf.FDwfAnalogOutNodeDataSet(hdwf, 0, dwf.AnalogOutNodeCarrier, rgdSamples, 4096);
// 10kHz waveform frequency
dwf.FDwfAnalogOutNodeFrequencySet(hdwf, 0, dwf.AnalogOutNodeCarrier, 10000.0);
// 2V amplitude, 4V pk2pk, for sample value -1 will output -2V, for 1 +2V
dwf.FDwfAnalogOutNodeAmplitudeSet(hdwf, 0, dwf.AnalogOutNodeCarrier, 2);
// by default the offset is 0V
// start signal generation
dwf.FDwfAnalogOutConfigure(hdwf, 0, 1);
// it will run until stopped or device closed

Console.WriteLine("done\n");

// close the device
dwf.FDwfDeviceClose(hdwf);