/* Author: Matthew Burton 23/05/23
 * ------------------------------------------------------------------------------
 * Code to take a reading of voltage of a channel 
 * ------------------------------------------------------------------------------
*/

int hdwf;
double voltage;
byte psts;

Console.WriteLine("Open automatically the first available device");
dwf.FDwfDeviceOpen(-1, out hdwf);

// enable first channel FDwfAnalogInChannelEnableSet(hdwf, 0, true)
// set 0V offset
dwf.FDwfAnalogInChannelOffsetSet(hdwf, 0, 0);
// set 5V pk2pk input range, -2.5V to 2.5V
dwf.FDwfAnalogInChannelRangeSet(hdwf, 0, 5);
// start signal generation
dwf.FDwfAnalogInConfigure(hdwf, 0, 0);

Console.WriteLine("Analog in channel 1 voltage:\n");
for (int i = 0; i < 10; i++)
{
    // fetch analog input information from the device
    dwf.FDwfAnalogInStatus(hdwf, 0, out psts);
    // read voltage input of first channel
    dwf.FDwfAnalogInStatusSample(hdwf, 0, out voltage);
    Console.WriteLine("%.3lf V\n", voltage);
}

// close the device
dwf.FDwfDeviceClose(hdwf);