/* Author: Matthew Burton 23/05/23
 * ------------------------------------------------------------------------------
 * This code sets up the digital input acquisition for a specified number of 
 * samples after a trigger, configures the device with sample rate and trigger 
 * settings, retrieves the acquired samples in a byte array, and then copies them 
 * to the main array for further processing, ensuring proper acquisition and error 
 * handling before closing the device.
 * ------------------------------------------------------------------------------
*/
int hdwf;
byte sts;
double hzSys;
uint nSamples = 100000;
byte[] rgwSamples = new byte[nSamples];
int cSamples = 0, cAvailable, cLost, cCorrupt;
bool fLost = false, fCorrupt = false;
string szError;

Console.WriteLine("Open automatically the first available device");
dwf.FDwfDeviceOpen(-1, out hdwf);

// in record mode samples after trigger are acquired only
dwf.FDwfDigitalInAcquisitionModeSet(hdwf, dwf.acqmodeRecord);
// sample rate = system frequency / divider, 100MHz/1000 = 100kHz
dwf.FDwfDigitalInDividerSet(hdwf, 1000);
// 16bit per sample format
dwf.FDwfDigitalInSampleFormatSet(hdwf, 16);
// number of samples after trigger
dwf.FDwfDigitalInTriggerPositionSet(hdwf, nSamples);
// trigger when all digital pins are low
dwf.FDwfDigitalInTriggerSourceSet(hdwf, dwf.trigsrcDetectorDigitalIn);
// trigger detector mask: low &  height & ( rising | falling )
dwf.FDwfDigitalInTriggerSet(hdwf, 0xFFFF, 0, 0, 0);

// begin acquisition
dwf.FDwfDigitalInConfigure(hdwf, 0, 1);

Console.WriteLine("Starting record...");

while (cSamples < nSamples)
{
    dwf.FDwfDigitalInStatus(hdwf, 1, out sts);
    if (cSamples == 0 && (sts == dwf.DwfStateConfig || sts == dwf.DwfStatePrefill || sts == dwf.DwfStateArmed))
    {
        // acquisition not yet started.
        continue;
    }

    dwf.FDwfDigitalInStatusRecord(hdwf, out cAvailable, out cLost, out cCorrupt);

    cSamples += cLost;

    // samples lost due to device FIFO overflow
    if (cLost > 0)
    {
        fLost = true;
    }
    // samples could be corrupted by FIFO overflow
    if (cCorrupt > 0)
    {
        fCorrupt = true;
    }
    if (cAvailable == 0)
    {
        continue;
    }
    if (cSamples + cAvailable > nSamples)
    {
        cAvailable = (int)(nSamples - cSamples);
    }
    // create a byte array to receive the samples
    byte[] samplesBuffer = new byte[2 * cAvailable];

    // get samples
    dwf.FDwfDigitalInStatusData(hdwf, samplesBuffer, 2 * cAvailable);

    // copy the received samples to the main array
    Array.Copy(samplesBuffer, 0, rgwSamples, cSamples, 2 * cAvailable);

    cSamples += cAvailable;
}
Console.WriteLine("done\n");

// close the device
dwf.FDwfDeviceClose(hdwf);