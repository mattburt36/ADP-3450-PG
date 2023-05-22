/* Author: Matthew Burton 23/05/23
 * ------------------------------------------------------------------------------
 * This code opens an analog output device, generates a sine waveform with 
 * specific settings such as frequency, amplitude, and offset, configures the 
 * analog output, starts the signal generation, and then closes the device.
 * ------------------------------------------------------------------------------
*/
int hdwf;

Console.WriteLine("Open automatically the first available device");
dwf.FDwfDeviceOpen(-1, out hdwf);

Console.WriteLine("Generating waveform for 5 seconds...");
// enable first channel
dwf.FDwfAnalogOutNodeEnableSet(hdwf, 0, dwf.AnalogOutNodeCarrier, 1);
// set sine function
dwf.FDwfAnalogOutNodeFunctionSet(hdwf, 0, dwf.AnalogOutNodeCarrier, dwf.funcSine);
// 10kHz
dwf.FDwfAnalogOutNodeFrequencySet(hdwf, 0, dwf.AnalogOutNodeCarrier, 10000.0);
// 1.41V amplitude (1Vrms), 2.82V pk2pk
dwf.FDwfAnalogOutNodeAmplitudeSet(hdwf, 0, dwf.AnalogOutNodeCarrier, 1.41);
// 1.41V offset
dwf.FDwfAnalogOutNodeOffsetSet(hdwf, 0, dwf.AnalogOutNodeCarrier, 1.41);
// start signal generation
dwf.FDwfAnalogOutConfigure(hdwf, 0, 1);
// it will run until stopped, reset, parameter changed or device closed

Console.WriteLine("done\n");

// close the device
dwf.FDwfDeviceClose(hdwf);