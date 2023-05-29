/* Author: Matthew Burton 23/05/23
 * ------------------------------------------------------------------------------
 * This code generates digital signals on different IO pins of a device, 
 * including a 1kHz pulse, a 1kHz 25% duty pulse, a 2kHz random signal, and a 
 * 1kHz custom signal, and then closes the device.
 * ------------------------------------------------------------------------------
*/
int hdwf;
double hzSys;
string szError;

Console.WriteLine("Open automatically the first available device");
dwf.FDwfDeviceOpen(-1, out hdwf);

Console.WriteLine("Generating digital signals...");
dwf.FDwfDigitalOutInternalClockInfo(hdwf, out hzSys);

// 1kHz pulse on IO pin 0
dwf.FDwfDigitalOutEnableSet(hdwf, 0, 1);
// prescaler to 2kHz, SystemFrequency/1kHz/2
dwf.FDwfDigitalOutDividerSet(hdwf, 0, (uint)(hzSys / 1e3 / 2));
// 1 tick low, 1 tick high
dwf.FDwfDigitalOutCounterSet(hdwf, 0, 1, 1);

// 1kHz 25% duty pulse on IO pin 1
dwf.FDwfDigitalOutEnableSet(hdwf, 1, 1);
// prescaler to 4kHz SystemFrequency/1kHz/2
dwf.FDwfDigitalOutDividerSet(hdwf, 1, (uint)(hzSys / 1e3 / 4));
// 3 ticks low, 1 tick high
dwf.FDwfDigitalOutCounterSet(hdwf, 1, 3, 1);

// 2kHz random on IO pin 2
dwf.FDwfDigitalOutEnableSet(hdwf, 2, 1);
dwf.FDwfDigitalOutTypeSet(hdwf, 2, dwf.DwfDigitalOutTypeRandom);
dwf.FDwfDigitalOutDividerSet(hdwf, 2, (uint)(hzSys / 2e3));

byte[] rgcustom = { 0x00, 0xAA, 0x66, 0xFF };
// 1kHz custom on IO pin 3
dwf.FDwfDigitalOutEnableSet(hdwf, 3, 1);
dwf.FDwfDigitalOutTypeSet(hdwf, 3, dwf.DwfDigitalOutTypeCustom);
dwf.FDwfDigitalOutDividerSet(hdwf, 3, (uint)(hzSys / 1e3));
dwf.FDwfDigitalOutDataSet(hdwf, 3, rgcustom, 4 * 8);

dwf.FDwfDigitalOutConfigure(hdwf, 1);

// it will run until stopped, reset, parameter changed or device closed
Console.WriteLine("done\n");

dwf.FDwfDigitalOutReset(hdwf);

// close the device
dwf.FDwfDeviceClose(hdwf);