/* Author: Matthew Burton 16/05/23
 * ------------------------------------------------------------------------------
 * 
 * ------------------------------------------------------------------------------
*/
int nSamples = 100000;
int hdwf;
byte sts;
double hzAcq = 50000.0;
int cSamples = 0;
int cAvailable, cLost, cCorrupted;
double[] rgdSamples;
bool fLost = false, fCorrupted = false;

Console.WriteLine("Open automatically the first available device");
dwf.FDwfDeviceOpen(-1, out hdwf);

dwf.FDwfAnalogInChannelEnableSet(hdwf, 0, 1);
dwf.FDwfAnalogInChannelRangeSet(hdwf, 0, 5);

// recording rate for more samples than the device buffer is limited by device communication
dwf.FDwfAnalogInAcquisitionModeSet(hdwf, dwf.acqmodeRecord);
dwf.FDwfAnalogInFrequencySet(hdwf, hzAcq);
dwf.FDwfAnalogInRecordLengthSet(hdwf, 1.0 * nSamples / hzAcq);

// start
dwf.FDwfAnalogInConfigure(hdwf, 0, 1);

Console.WriteLine("Recording...\n");

while (cSamples < nSamples)
{
    dwf.FDwfAnalogInStatus(hdwf, 1, out sts);

    if (sts == dwf.stsError)
    {
        Console.WriteLine("error");
    }
    if (cSamples == 0 && (sts == dwf.stsCfg || sts == dwf.stsPrefill || sts == dwf.stsArm))
    {
        // Acquisition not yet started.
        continue;
    }

    dwf.FDwfAnalogInStatusRecord(hdwf, out cAvailable, out cLost, out cCorrupted);

    cSamples += cLost;

    if (cLost > 0) fLost = true;
    if (cCorrupted > 0) fCorrupted = true;

    if (cAvailable == 0) continue;

    // get samples
    rgdSamples = new double[cSamples];
    dwf.FDwfAnalogInStatusData(hdwf, 0, rgdSamples, cAvailable);
    cSamples += cAvailable;
}

Console.WriteLine("done\n");

if (fLost)
{
    Console.WriteLine("Samples were lost! Reduce frequency");
}
else if (fCorrupted)
{
    Console.WriteLine("Samples could be corrupted! Reduce frequency");
}

// close the device
dwf.FDwfDeviceClose(hdwf);
