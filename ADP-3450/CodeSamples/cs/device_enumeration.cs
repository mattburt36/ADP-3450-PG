/* Author: Matthew Burton 23/05/23
 * ------------------------------------------------------------------------------
 * This code detects connected devices, lists information about each device, 
 * checks if a device is already opened, opens the device if available, retrieves 
 * analog input channel count and maximum frequency, and closes the device(s) at 
 * the end.
 * ------------------------------------------------------------------------------
*/
int cDevice;
int cChannel;
double hzFreq;
string szDeviceName;
string szSN;
int fIsInUse;
int hdwf;
string szError;
double _null;

// detect connected all supported devices
if (dwf.FDwfEnum(dwf.enumfilterAll, out cDevice) >= 0)
{
    dwf.FDwfGetLastErrorMsg(out szError);
    Console.WriteLine("FDwfEnum: %s\n", szError);
}

// list information about each device
Console.WriteLine("Found %d devices:\n", cDevice);
for (int i = 0; i < cDevice; i++)
{
    // we use 0 based indexing
    dwf.FDwfEnumDeviceName(i, out szDeviceName);
    dwf.FDwfEnumSN(i, out szSN);
    Console.WriteLine($"\nDevice: {i + 1} name: {szDeviceName} {szSN}\n");
    // before opening, check if the device isn’t already opened by other application, like: WaveForms
    dwf.FDwfEnumDeviceIsOpened(i, out fIsInUse);
    if (fIsInUse == 0)
    {
        if (dwf.FDwfDeviceOpen(i, out hdwf) == 0)
        {
            dwf.FDwfGetLastErrorMsg(out szError);
            Console.WriteLine("FDwfDeviceOpen: {0}\n", szError);
            continue;
        }
        dwf.FDwfAnalogInChannelCount(hdwf, out cChannel);
        dwf.FDwfAnalogInFrequencyInfo(hdwf, out _null, out hzFreq);
        Console.WriteLine("number of analog input channels: {0} maximum freq.: {1:0} Hz\n", cChannel, hzFreq);
        dwf.FDwfDeviceClose(hdwf);
        hdwf = dwf.hdwfNone;
    }
}

// close the device
dwf.FDwfDeviceCloseAll();