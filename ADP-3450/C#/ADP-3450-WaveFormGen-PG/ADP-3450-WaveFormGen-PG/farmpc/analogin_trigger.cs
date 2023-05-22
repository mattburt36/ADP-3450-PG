/* Author: Matthew Burton 23/05/23
 * ------------------------------------------------------------------------------
 * This code uses the DWF library to control an analog input device, perform 
 * repeated acquisitions, and calculate the average voltage from the acquired 
 * samples.
 * ------------------------------------------------------------------------------
*/
int hdwf;
byte sts;
int cSamples = 8192;
double[] rgdSamples;

Console.WriteLine("Open automatically the first available device");
dwf.FDwfDeviceOpen(-1, out hdwf);

dwf.FDwfAnalogInFrequencySet(hdwf, 20000000.0);
dwf.FDwfAnalogInBufferSizeSet(hdwf, 8192);
dwf.FDwfAnalogInChannelEnableSet(hdwf, 0, 1);
dwf.FDwfAnalogInChannelRangeSet(hdwf, 0, 5.0);

// set up trigger
// disable auto trigger
dwf.FDwfAnalogInTriggerAutoTimeoutSet(hdwf, 0);
// one of the analog in channels
dwf.FDwfAnalogInTriggerSourceSet(hdwf, dwf.trigsrcDetectorAnalogIn);
dwf.FDwfAnalogInTriggerTypeSet(hdwf, dwf.trigtypeEdge);
// first channel
dwf.FDwfAnalogInTriggerChannelSet(hdwf, 0);
dwf.FDwfAnalogInTriggerLevelSet(hdwf, 0.5);
dwf.FDwfAnalogInTriggerConditionSet(hdwf, dwf.trigcondRisingPositive);

dwf.FDwfAnalogInConfigure(hdwf, 0, 1);

Console.WriteLine("Starting repeated acquisitions:\n");
for (int iTrigger = 0; iTrigger < 100; iTrigger++)
{

    while (true)
    {
        dwf.FDwfAnalogInStatus(hdwf, 1, out sts);
        if (sts == dwf.DwfStateDone)
        {
            break;
        }
    }

    rgdSamples = new double[cSamples];

    dwf.FDwfAnalogInStatusData(hdwf, 0, rgdSamples, cSamples);
    double vAvg = 0;
    for (int i = 0; i < cSamples; i++)
    {
        vAvg += rgdSamples[i];
    }
    vAvg /= cSamples;
    Console.WriteLine(" #%i average: %.3lf V\n", iTrigger + 1, vAvg);
}

// close the device
dwf.FDwfDeviceClose(hdwf);