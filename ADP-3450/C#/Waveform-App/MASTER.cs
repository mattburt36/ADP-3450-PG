namespace Waveform_App
{
    public partial class MASTER : Form
    {
        #region Handy Links
        // https://scottplot.net/cookbook/4.1/
        // https://github.com/mattburt36/ADP-3450-PG
        // W:\Matt-Sloan-Work\DOCS-Drivers\WaveForms SDK Reference Manual.pdf
        #endregion

        #region Variables

        #region Generator vars
        int outChannel = 0;
        double outVoltAmplitude = 1.41;
        double outFrequency = 10000.0;
        #endregion

        #region Oscilloscope vars
        int inChannel = 0;
        double inVoltRange = 5;
        double inFrequency = 300000.0;
        int minBufferSize;
        int maxBufferSize;
        int bufferSize;
        double[] buffer;
        byte status;
        double triggerVoltLevel = 1.0;
        int triggerTimeOut = 10;
        #endregion

        #region General vars
        double hzIndx = 0;
        double vrtIndx = 0;
        int devHandle;
        #endregion

        #endregion

        #region Functions

        #region Events
        public MASTER()
        {
            InitializeComponent();

            // Open device on startup 
            Open();

            // Do this once, the ADP will repeat until changes are made 
            Write();
            Read();

            //Trigger clock to start 
            timer1.Interval = 1000; //1 second update intervals == 1000
            timer1.Start();         //Start Timer

            // Add axis lines
            formsPlot1.Plot.AddHorizontalLine(hzIndx);
            formsPlot1.Plot.AddVerticalLine(vrtIndx);
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            // TODO: 
            // Make this update more efficiently with multiple threads  

            // Update every half second
            Read();
            formsPlot1.Render();
        }

        private void ConnectButton_Click(object sender, EventArgs e)
        {

        }

        private void MASTER_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Close devices on shutdown of app 
            CloseAll();
        }
        #endregion

        #region Logic 
        //-----------------------------------------------------------------------------------------------
        // Function to set the ADP3450 output signal up to write data to 
        //
        // TODO:
        // Consider adding values to pass to write function to adjust the settings being written to the
        // WFG, object? class? same same?
        //-----------------------------------------------------------------------------------------------
        public void Write()
        {
            #region Create signal
            // enable first channel
            dwf.FDwfAnalogOutNodeEnableSet(devHandle, outChannel, dwf.AnalogOutNodeCarrier, 1);
            // set sine function
            dwf.FDwfAnalogOutNodeFunctionSet(devHandle, outChannel, dwf.AnalogOutNodeCarrier, dwf.funcSine);
            // 10kHz
            dwf.FDwfAnalogOutNodeFrequencySet(devHandle, outChannel, dwf.AnalogOutNodeCarrier, outFrequency);
            // 1.41V amplitude (1Vrms), 2.82V pk2pk
            dwf.FDwfAnalogOutNodeAmplitudeSet(devHandle, outChannel, dwf.AnalogOutNodeCarrier, outVoltAmplitude);
            // 1.41V offset
            dwf.FDwfAnalogOutNodeOffsetSet(devHandle, outChannel, dwf.AnalogOutNodeCarrier, outVoltAmplitude);
            // start signal generation
            dwf.FDwfAnalogOutConfigure(devHandle, outChannel, 1);
            // will run until stopped, reset, parameter changed or device closed
            #endregion
        }

        //-----------------------------------------------------------------------------------------------
        // Function to read data on the ADP3450 oscilloscope 
        //-----------------------------------------------------------------------------------------------
        public void Read()
        {
            #region Listen to signal 

            #region Configure
            //Enable the analog in channel on the device 
            dwf.FDwfAnalogInChannelEnableSet(devHandle, inChannel, 1);

            //Set the peak to peak volt range for the channel 
            dwf.FDwfAnalogInChannelRangeSet(devHandle, inChannel, inVoltRange);

            //Set the frequency sample rate 
            dwf.FDwfAnalogInFrequencySet(devHandle, inFrequency);

            //Get the maximum buffer size
            dwf.FDwfAnalogInBufferSizeInfo(devHandle, out minBufferSize, out maxBufferSize);

            //Assign buffer size to appropriately named var
            bufferSize = maxBufferSize;
            //Create buffer array to store data at max size of buffer expected 
            buffer = new double[bufferSize];

            //Set the expected buffer size to maximum
            dwf.FDwfAnalogInBufferSizeSet(devHandle, bufferSize);

            //Configure the trigger for the analog in 
            dwf.FDwfAnalogInTriggerSourceSet(devHandle, dwf.trigsrcDetectorAnalogIn);
            dwf.FDwfAnalogInTriggerAutoTimeoutSet(devHandle, triggerTimeOut);
            dwf.FDwfAnalogInTriggerChannelSet(devHandle, inChannel);
            dwf.FDwfAnalogInTriggerTypeSet(devHandle, dwf.trigtypeEdge);
            dwf.FDwfAnalogInTriggerLevelSet(devHandle, triggerVoltLevel);
            dwf.FDwfAnalogInTriggerConditionSet(devHandle, dwf.trigcondRisingPositive);

            #endregion

            #region Read
            // start
            dwf.FDwfAnalogInConfigure(devHandle, 0, 1);

            //Read from the device until the status is done
            do
            {
                dwf.FDwfAnalogInStatus(devHandle, 1, out status);
            } while (status != dwf.stsDone);

            //Retrieve the data
            dwf.FDwfAnalogInStatusData(devHandle, inChannel, buffer, bufferSize);

            #endregion

            // Add a signal connection to the buffer of data coming in from the oscilloscope 
            formsPlot1.Plot.AddSignal(buffer);

            #endregion
        }

        //-----------------------------------------------------------------------------------------------
        // Opens the first ADP3450 device found connected to the machine 
        //-----------------------------------------------------------------------------------------------
        public void Open()
        {
            //Open a device 
            dwf.FDwfDeviceOpen(-1, out devHandle);
        }

        //-----------------------------------------------------------------------------------------------
        // Closes all ADP3450 devices connected to the machine 
        //-----------------------------------------------------------------------------------------------
        public void CloseAll()
        {
            dwf.FDwfDeviceCloseAll();
        }
        #endregion

        #endregion
    }
}