/* Author: Matthew Burton 23/05/23
 * ------------------------------------------------------------------------------
 * This code opens the first available device, enables output on 8 LSB IO pins, 
 * sets a specific value on those pins, reads the state of all pins (regardless 
 * of output enable), and prints the hexadecimal value of the digital IO pins. 
 * Finally, it closes the device.
 * ------------------------------------------------------------------------------
*/
int hdwf;
int sts;
uint dwRead;

Console.WriteLine("Open automatically the first available device");
dwf.FDwfDeviceOpen(-1, out hdwf);

// enable output/mask on 8 LSB IO pins, from DIO 0 to 7
dwf.FDwfDigitalIOOutputEnableSet(hdwf, 0x00FF);
// set value on enabled IO pins
dwf.FDwfDigitalIOOutputSet(hdwf, 0x12);
// fetch digital IO information from the device 
dwf.FDwfDigitalIOStatus(hdwf);
// read state of all pins, regardless of output enable
dwf.FDwfDigitalIOInputStatus(hdwf, out dwRead);

Console.WriteLine("Digital IO Pins:  0x{0:X8}\n", dwRead);

// close the device
dwf.FDwfDeviceClose(hdwf);