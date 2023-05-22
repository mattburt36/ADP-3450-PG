/* Author: Matthew Burton 23/05/23
 * ------------------------------------------------------------------------------
 * 
 * ------------------------------------------------------------------------------
*/
int cDevice;
int cChannel;
double hzFreq;
char[] szDeviceName = new char[32];
char[] szSN = new char[32];
bool fIsInUse;
int hdwf;
string szError;

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
    FDwfEnumDeviceName(i, szDeviceName);
    FDwfEnumSN(i, szSN);
    printf("\nDevice: %d name: %s %s\n", i + 1, szDeviceName, szSN);
    // before opening, check if the device isn’t already opened by other application, like: WaveForms
    FDwfEnumDeviceIsOpened(i, &fIsInUse);
    if (!fIsInUse)
    {
        if (!FDwfDeviceOpen(i, &hdwf))
        {
            FDwfGetLastErrorMsg(szError);
            printf("FDwfDeviceOpen: %s\n", szError);
            continue;
        }
        FDwfAnalogInChannelCount(hdwf, &cChannel);
        FDwfAnalogInFrequencyInfo(hdwf, NULL, &hzFreq);
        printf("number of analog input channels: %d maximum freq.: %.0f Hz\n", cChannel, hzFreq);
        FDwfDeviceClose(hdwf);
        hdwf = hdwfNone;
    }
}

// close the device
dwf.FDwfDeviceClose(hdwf);