/* Author: Matthew Burton 23/05/23
 * ------------------------------------------------------------------------------
 * This code opens the first available device, retrieves the internal clock 
 * information, generates a binary counter using the digital output pins, 
 * configures and starts the counter, and then closes the device.
 * ------------------------------------------------------------------------------
*/
int hdwf;
double hzSys;

Console.WriteLine("Open automatically the first available device");
dwf.FDwfDeviceOpen(-1, out hdwf);

dwf.FDwfDigitalOutInternalClockInfo(hdwf, out hzSys);

Console.WriteLine("Generating binary counter...");
for (int i = 0; i < 16; i++)
{
    dwf.FDwfDigitalOutEnableSet(hdwf, i, 1);
    // increase by 2 the period of successive bits
    dwf.FDwfDigitalOutDividerSet(hdwf, i, (uint)(1 << i));
    // 100kHz coutner rate, SystemFrequency/100kHz
    dwf.FDwfDigitalOutCounterSet(hdwf, i, (uint)(hzSys / 1e5), (uint)(hzSys / 1e5));
}

dwf.FDwfDigitalOutConfigure(hdwf, 1);

// it will run until stopped, reset, parameter changed or device closed
dwf.FDwfDigitalOutReset(hdwf);
Console.WriteLine("done\n");

// close the device
dwf.FDwfDeviceClose(hdwf);