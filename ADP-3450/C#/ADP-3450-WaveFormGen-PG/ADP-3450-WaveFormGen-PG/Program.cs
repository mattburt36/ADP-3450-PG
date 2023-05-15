/*
 * ------------------------------------------------------------------------------
 * Code for creating a sweeping sine wave that fluctuates in speed
 * ------------------------------------------------------------------------------
*/
int hdwf;
int idxChannel = 0;
double hzStart = 1e3;
double hzStop = 1e5;
double secSweep = 1;

// Open Device
// Opens a device identified by the enumeration index and retrieves a handle. To automatically
//enumerate all connected devices and open the first discovered device, use index - 1.
dwf.FDwfDeviceOpen(-1, out hdwf);

// enable first channel
dwf.FDwfAnalogOutNodeEnableSet(hdwf, idxChannel, dwf.AnalogOutNodeCarrier, 1);
// set sine carrier
dwf.FDwfAnalogOutNodeFunctionSet(hdwf, idxChannel, dwf.AnalogOutNodeCarrier, dwf.funcSine);
// 1V amplitude, 2V pk2pk
dwf.FDwfAnalogOutNodeAmplitudeSet(hdwf, idxChannel, dwf.AnalogOutNodeCarrier, 1.0);
// 10kHz carrier frequency
dwf.FDwfAnalogOutNodeFrequencySet(hdwf, idxChannel, dwf.AnalogOutNodeCarrier, (hzStart + hzStop) / 2);
// enable frequency modulation
dwf.FDwfAnalogOutNodeEnableSet(hdwf, idxChannel, dwf.AnalogOutNodeFM, 1);
// linear sweep with ramp up symmetry 100%
dwf.FDwfAnalogOutNodeFunctionSet(hdwf, idxChannel, dwf.AnalogOutNodeFM, dwf.funcRampUp);
dwf.FDwfAnalogOutNodeSymmetrySet(hdwf, idxChannel, dwf.AnalogOutNodeFM, 100);
// modulation index
dwf.FDwfAnalogOutNodeAmplitudeSet(hdwf, idxChannel, dwf.AnalogOutNodeFM, 100.0 * (hzStop - hzStart) / (hzStart + hzStop));
// FM frequency = 1/sweep time
dwf.FDwfAnalogOutNodeFrequencySet(hdwf, idxChannel, dwf.AnalogOutNodeFM, 1.0 / secSweep);

// by default the offset is 0V
// start signal generation
dwf.FDwfAnalogOutConfigure(hdwf, idxChannel, 1);
// it will run until stopped or device closed
Console.WriteLine("done\n");

// on close device is stopped and configuration lost
dwf.FDwfDeviceCloseAll();
