/* Author: Matthew Burton 23/05/23
 * ------------------------------------------------------------------------------
 * This code opens the first available device, generates digital signals with 
 * 10-bit walking 1 pattern, configures the digital output, runs it until stopped 
 * or device closed, and then closes the device.
 * ------------------------------------------------------------------------------
*/
int hdwf;
double hzSys;
string szError;

Console.WriteLine("Open automatically the first available device");
dwf.FDwfDeviceOpen(-1, out hdwf);

Console.WriteLine("Generating digital signals...");
dwf.FDwfDigitalOutInternalClockInfo(hdwf, out hzSys);

// 10 bit walking 1
for (int i = 0; i < 10; i++)
{
    dwf.FDwfDigitalOutEnableSet(hdwf, i, 1);
    // divide system frequency down to 1kHz
    dwf.FDwfDigitalOutDividerSet(hdwf, i, (uint)(hzSys / 1e3));
    // all pins will be 9 ticks low and 1 high
    dwf.FDwfDigitalOutCounterSet(hdwf, i, 9, 1);
    // first bit will start high others low with increasing phase
    dwf.FDwfDigitalOutCounterInitSet(hdwf, i, (i == 0 ? 1 : 0), (uint)(i == 0 ? 0 : i)); // Convert bool to int using the ternary operator
}

// start digital out
dwf.FDwfDigitalOutConfigure(hdwf, 1);

// it will run until stopped, reset, parameter changed or device closed
dwf.FDwfDigitalOutReset(hdwf);
Console.WriteLine("done\n");

// close the device
dwf.FDwfDeviceClose(hdwf);
