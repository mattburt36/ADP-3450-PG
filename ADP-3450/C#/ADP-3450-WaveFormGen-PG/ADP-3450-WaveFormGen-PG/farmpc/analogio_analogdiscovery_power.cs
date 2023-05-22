/* Author: Matthew Burton 23/05/23
 * ------------------------------------------------------------------------------
 * This code opens an analog input/output device, enables positive and negative 
 * supply channels, calculates the total supply power and load percentage for 60 
 * iterations, and handles over-current conditions by re-enabling the supplies 
 * when necessary before closing the device.
 * ------------------------------------------------------------------------------
*/
int hdwf;
double sts;
double vSupply, aSupply, wSupply;
int prcSupply;
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
    dwf.FDwfAnalogIOChannelNodeStatus(hdwf, 3, 0, out vSupply);
    dwf.FDwfAnalogIOChannelNodeStatus(hdwf, 3, 1, out aSupply);
    wSupply = vSupply * aSupply;

    prcSupply = (int)(100 * (aSupply / 0.2));
    Console.WriteLine("{0:F3} W \t{1}%\n", wSupply, prcSupply);

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