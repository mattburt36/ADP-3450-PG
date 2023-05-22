/* Author: Matthew Burton 23/05/23
 * ------------------------------------------------------------------------------
 * This code opens an analog output device, generates synchronized waveforms on 
 * two channels with specific settings such as frequency, amplitude, and phase, 
 * configures the channels, starts the signal generation on the master channel, 
 * which in turn starts the slave channel, and then closes the device.
 * ------------------------------------------------------------------------------
*/
int hdwf;

Console.WriteLine("Open automatically the first available device");
dwf.FDwfDeviceOpen(-1, out hdwf);

Console.WriteLine("Generating synchronized waveforms for 5 seconds...");
// enable two channels
dwf.FDwfAnalogOutNodeEnableSet(hdwf, 0, dwf.AnalogOutNodeCarrier, 1);
dwf.FDwfAnalogOutNodeEnableSet(hdwf, 1, dwf.AnalogOutNodeCarrier, 1);
// for second channel set master the first channel
dwf.FDwfAnalogOutMasterSet(hdwf, 1, 1);
// slave channel is controlled by the master channel
// it is enough to set trigger, wait, run and repeat parameters for master channel
// when using different frequencies it might need periodical resynchronization
// to do so set limited runtime and repeat infinite

// configure enabled channels
dwf.FDwfAnalogOutNodeFunctionSet(hdwf, -1, dwf.AnalogOutNodeCarrier, dwf.funcSine);
dwf.FDwfAnalogOutNodeFrequencySet(hdwf, -1, dwf.AnalogOutNodeCarrier, 10000.0);
dwf.FDwfAnalogOutNodeAmplitudeSet(hdwf, -1, dwf.AnalogOutNodeCarrier, 1.0);
// set phase for second channel
dwf.FDwfAnalogOutNodePhaseSet(hdwf, 1, dwf.AnalogOutNodeCarrier, 180.0);

// slave channel will only be initialized
dwf.FDwfAnalogOutConfigure(hdwf, 1, 1);
// starting master will start slave channels too
dwf.FDwfAnalogOutConfigure(hdwf, 0, 1);
// it will run until stopped, reset, parameter changed or device closed

Console.WriteLine("done\n");

// close the device
dwf.FDwfDeviceClose(hdwf);