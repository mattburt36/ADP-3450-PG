/* Author: Matthew Burton 23/05/23
 * ------------------------------------------------------------------------------
 * This code opens an analog input/output device, enables positive and negative 
 * supply channels, monitors the USB supply voltage and current, as well as the 
 * AUX supply voltage and current for 60 iterations, and re-enables the supplies 
 * if there is an over-current condition before closing the device.
 * ------------------------------------------------------------------------------
*/
int hdwf;
double vUSB, aUSB, vAUX, aAUX;
int fOn;

Console.WriteLine("Open automatically the first available device");
dwf.FDwfDeviceOpen(-1, out hdwf);

// set up analog IO channel nodes
// enable positive supply
dwf.FDwfAnalogIOChannelNodeSet(hdwf, 0, 0, 1);
// enable negative supply
dwf.FDwfAnalogIOChannelNodeSet(hdwf, 1, 0, 1);
// master enable
dwf.FDwfAnalogIOEnableSet(hdwf, 1);

Console.WriteLine("Total supply power and load percentage:\n");
for (int i = 0; i < 60; i++)
{
    // fetch analogIO status from device
    dwf.FDwfAnalogIOStatus(hdwf);

    // supply monitor
    dwf.FDwfAnalogIOChannelNodeStatus(hdwf, 2, 0, out vUSB);
    dwf.FDwfAnalogIOChannelNodeStatus(hdwf, 2, 1, out aUSB);
    dwf.FDwfAnalogIOChannelNodeStatus(hdwf, 3, 0, out vAUX);
    dwf.FDwfAnalogIOChannelNodeStatus(hdwf, 3, 1, out aAUX);

    Console.WriteLine("USB: {0:F3} V \t{1:F3} A \tAUX: {2:F3} V \t{3:F3} A  \n", vUSB, aUSB, vAUX, aAUX);

    // in case of over-current condition the supplies are disabled
    dwf.FDwfAnalogIOEnableStatus(hdwf, out fOn);
    if (fOn >= 0)
    {
        // re-enable supplies
        dwf.FDwfAnalogIOEnableSet(hdwf, 0);
        dwf.FDwfAnalogIOEnableSet(hdwf, 1);
    }
}
// close the device
dwf.FDwfDeviceClose(hdwf);