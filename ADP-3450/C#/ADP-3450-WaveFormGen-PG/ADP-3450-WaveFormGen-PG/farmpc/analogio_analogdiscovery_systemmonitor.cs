/* Author: Matthew Burton 23/05/23
 * ------------------------------------------------------------------------------
 * This code opens an analog input/output device, monitors the USB supply 
 * voltage, current, and device temperature for 60 iterations, and then closes 
 * the device.
 * ------------------------------------------------------------------------------
*/
int hdwf;
double vUsb, aUsb, degDevice;

Console.WriteLine("Open automatically the first available device");
dwf.FDwfDeviceOpen(-1, out hdwf);

// monitor voltage, current, temperature
Console.WriteLine("Device USB supply voltage, current and device temperature: \n");
for (int i = 0; i < 60; i++)
{
    // fetch analog IO status from device
    dwf.FDwfAnalogIOStatus(hdwf);
    // get system monitor readings
    dwf.FDwfAnalogIOChannelNodeStatus(hdwf, 2, 0, out vUsb);
    dwf.FDwfAnalogIOChannelNodeStatus(hdwf, 2, 1, out aUsb);
    dwf.FDwfAnalogIOChannelNodeStatus(hdwf, 2, 2, out degDevice);
    Console.WriteLine("{0:F3} V \t{1:F3} A \t{2:F2} degC\n", vUsb, aUsb, degDevice);
}
// close the device
dwf.FDwfDeviceClose(hdwf);