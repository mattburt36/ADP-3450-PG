/* Author: Matthew Burton 23/05/23
 * ------------------------------------------------------------------------------
 * This code opens an analog output device, generates a custom amplitude-modulated 
 * waveform for 5 seconds with specific settings such as carrier frequency, 
 * amplitude, modulation index, and waveform samples, configures the analog output 
 * for amplitude modulation, and then closes the device.
 * ------------------------------------------------------------------------------
*/
int hdwf;
double[] rgdSamples = new double[1024];
int idxChannel = 0;

Console.WriteLine("Open automatically the first available device");
dwf.FDwfDeviceOpen(-1, out hdwf);


Console.WriteLine("Generating custom amplitude modulated waveform for 5 seconds...");

// needed for EExplorer, don't care for ADiscovery
dwf.FDwfAnalogOutCustomAMFMEnableSet(hdwf, idxChannel, 1);
// enable first channel
dwf.FDwfAnalogOutNodeEnableSet(hdwf, idxChannel, dwf.AnalogOutNodeCarrier, 1);
// set sine carrier
dwf.FDwfAnalogOutNodeFunctionSet(hdwf, idxChannel, dwf.AnalogOutNodeCarrier, dwf.funcSine);
// 1V amplitude, 2V pk2pk
dwf.FDwfAnalogOutNodeAmplitudeSet(hdwf, idxChannel, dwf.AnalogOutNodeCarrier, 1.0);
// 10kHz carrier frequency
dwf.FDwfAnalogOutNodeFrequencySet(hdwf, idxChannel, dwf.AnalogOutNodeCarrier, 1000.0);
// enable amplitude modulation
dwf.FDwfAnalogOutNodeEnableSet(hdwf, idxChannel, dwf.AnalogOutNodeAM, 1);
// set custom AM
dwf.FDwfAnalogOutNodeFunctionSet(hdwf, idxChannel, dwf.AnalogOutNodeAM, dwf.funcCustom) ;
// +-100% modulation index, will result with 1V amplitude carrier, 0V to 2V
dwf.FDwfAnalogOutNodeAmplitudeSet(hdwf, idxChannel, dwf.AnalogOutNodeAM, 100);
// 10Hz AM frequency
dwf.FDwfAnalogOutNodeFrequencySet(hdwf, idxChannel, dwf.AnalogOutNodeAM, 10.0);
// set custom waveform samples
// normalized to ±1 values
dwf.FDwfAnalogOutNodeDataSet(hdwf, idxChannel, dwf.AnalogOutNodeAM, rgdSamples, 1024);
// by default the offset is 0V
// start signal generation
dwf.FDwfAnalogOutConfigure(hdwf, idxChannel, 1);
// it will run until stopped or device closed

Console.WriteLine("done\n");

// close the device
dwf.FDwfDeviceClose(hdwf);