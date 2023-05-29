/* Author: Matthew Burton 23 / 05 / 23
* ------------------------------------------------------------------------------
* This code generates phased pulses using digital output pins on a device, where 
* different pins are configured with specific phase relationships and counter 
* settings, and then the device is closed.
* ------------------------------------------------------------------------------
*/
int hdwf;
string szError;

Console.WriteLine("Open automatically the first available device");
dwf.FDwfDeviceOpen(-1, out hdwf);

Console.WriteLine("Generating phased pulses:\n");

// 2 pin phase start low and high
for (int i = 0; i < 2; i++)
{
    dwf.FDwfDigitalOutEnableSet(hdwf, i, 1);
    dwf.FDwfDigitalOutCounterInitSet(hdwf, i, (i == 1 ? 1 : 0), 0); // Convert bool to int using the ternary operator
    // 100MHz base freq /(50+50) = 1MHz
    dwf.FDwfDigitalOutCounterSet(hdwf, i, 50, 50);
}
dwf.FDwfDigitalOutConfigure(hdwf, 1);

// 3 pin phase
dwf.FDwfDigitalOutCounterInitSet(hdwf, 0, 1, 0);
dwf.FDwfDigitalOutCounterInitSet(hdwf, 1, 0, 20);
dwf.FDwfDigitalOutCounterInitSet(hdwf, 2, 1, 10);
for (int i = 0; i < 3; i++)
{
    dwf.FDwfDigitalOutEnableSet(hdwf, i, 1);
    // 100MHz base freq /(30+30) = 1.67 MHz
    dwf.FDwfDigitalOutCounterSet(hdwf, i, 30, 30);
}
dwf.FDwfDigitalOutConfigure(hdwf, 1);

// 4 pin phase starting: low & 25, low & 50, high & 25, high & 50 
for (int i = 0; i < 4; i++)
{
    dwf.FDwfDigitalOutEnableSet(hdwf, i, 1);
    dwf.FDwfDigitalOutCounterInitSet(hdwf, i, ((i == 2) || (i == 3) ? 1 : 0), (uint)((i == 0 || i == 2) ? 25 : 50)); // Convert bool to int using the ternary operator
    dwf.FDwfDigitalOutCounterSet(hdwf, i, 50, 50);
}
dwf.FDwfDigitalOutConfigure(hdwf, 1);

Console.WriteLine("done\n");

// close the device
dwf.FDwfDeviceClose(hdwf);
