/* Author: Matthew Burton 16/05/23
 * ------------------------------------------------------------------------------
 * Code to generate a sine wave
 * ------------------------------------------------------------------------------
*/
// Parameter to store device handle returned after connecting to reference device
int devHandle;

// Open Device
// Opens a device identified by the enumeration index and retrieves a handle. To automatically
//enumerate all connected devices and open the first discovered device, use index - 1.
dwf.FDwfDeviceOpen(-1, out devHandle);
// enable first channel
dwf.FDwfAnalogOutNodeEnableSet(devHandle, 0, dwf.AnalogOutNodeCarrier, 1);
// set sine function
dwf.FDwfAnalogOutNodeFunctionSet(devHandle, 0, dwf.AnalogOutNodeCarrier, dwf.funcSine);
// 10kHz
dwf.FDwfAnalogOutNodeFrequencySet(devHandle, 0, dwf.AnalogOutNodeCarrier, 10000);
// 1.41V amplitude (1Vrms), 2.82V pk2pk
dwf.FDwfAnalogOutNodeAmplitudeSet(devHandle, 0, dwf.AnalogOutNodeCarrier, 2);
// 1.41V offset
dwf.FDwfAnalogOutNodeOffsetSet(devHandle, 0, dwf.AnalogOutNodeCarrier, 1.41);
// start signal generation
dwf.FDwfAnalogOutConfigure(devHandle, 0, 1);

Console.WriteLine("done\n");

// Close device 
// Closes an interface handle when access to the device is no longer needed. Once the function above
// has returned, the specified interface handle can no longer be used to access the device.
dwf.FDwfDeviceClose(devHandle);
