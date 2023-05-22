/* Author: Matthew Burton 23/05/23
 * ------------------------------------------------------------------------------
 * This code opens an analog input/output device, monitors the USB supply voltage, 
 * current, AUX supply voltage, current, and device temperature for 60 iterations, 
 * and then closes the device.
 * ------------------------------------------------------------------------------
*/
int hdwf;
double vUSB, aUSB, vAUX, aAUX, degDevice;

Console.WriteLine("Open automatically the first available device");
dwf.FDwfDeviceOpen(-1, out hdwf);


// monitor voltage, current, temperature
Console.WriteLine("Device USB supply voltage, current and device temperature: \n");
for (int i = 0; i < 60; i++)
{
    // fetch analog IO status from device
    dwf.FDwfAnalogIOStatus(hdwf);
    // get system monitor readings
    dwf.FDwfAnalogIOChannelNodeStatus(hdwf, 2, 0, out vUSB);
    dwf.FDwfAnalogIOChannelNodeStatus(hdwf, 2, 1, out aUSB);
    dwf.FDwfAnalogIOChannelNodeStatus(hdwf, 2, 2, out degDevice);
    dwf.FDwfAnalogIOChannelNodeStatus(hdwf, 3, 0, out vAUX);
    dwf.FDwfAnalogIOChannelNodeStatus(hdwf, 3, 1, out aAUX);
    Console.WriteLine("USB: %.3lf V \t%.3lf A \tAUX: %.3lf V \t%.3lf A  \t%.2lf degC\n", vUSB, aUSB, vAUX, aAUX, degDevice);
}

// close the device
dwf.FDwfDeviceClose(hdwf);