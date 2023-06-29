namespace Waveform_App
{
    public partial class MASTER : Form
    {
        #region Handy Links
        // https://scottplot.net/cookbook/4.1/
        // https://scottplot.net/faq/live-data/
        // https://github.com/mattburt36/ADP-3450-PG
        // W:\Matt-Sloan-Work\DOCS-Drivers\WaveForms SDK Reference Manual.pdf
        #endregion

        #region Variables

        #region Generator vars
        private static Thread? _WaveGenThread;
        #endregion

        #region Oscilloscope vars
        int inChannel = 0;
        double inVoltRange = 5;
        double inFrequency = 300000.0;
        int minBufferSize;
        int maxBufferSize;
        int bufferSize;
        byte status;
        double triggerVoltLevel = 1.0;
        int triggerTimeOut = 10;
        private static Thread? _OscilloscopeThread;
        private Queue<double[]> bufferQueue = new Queue<double[]>();
        #endregion

        #region General vars
        double hzIndx = 0;
        double vrtIndx = 0;
        int devHandle;
        bool firstTime = true;
        #endregion

        #endregion

        #region Functions

        #region Events
        public MASTER()
        {
            InitializeComponent();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            // 
            // TODO: Check if still connected
            //

            if (bufferQueue.Count > 1)
            {
                double[] currentBuffer = bufferQueue.Dequeue();

                // Just do this once to allow auto axis scaling to begin with 
                if (firstTime)
                {
                    // Add axis lines with the color set to red, change hzIndx and vrtIndx values to move the axis
                    formsPlot1.Plot.AddHorizontalLine(hzIndx, color: Color.Red);
                    formsPlot1.Plot.AddVerticalLine(vrtIndx, color: Color.Red);

                    formsPlot1.Plot.AddSignal(currentBuffer);
                    formsPlot1.Render();

                    // Cherry popped
                    firstTime = false;
                }

                // Store the current zoom and position
                var xLimits = formsPlot1.Plot.GetAxisLimits(xAxisIndex: 0);
                var yLimits = formsPlot1.Plot.GetAxisLimits(yAxisIndex: 0);

                // Extract the individual limit values
                double xMin = xLimits.XMin;
                double xMax = xLimits.XMax;
                double yMin = yLimits.YMin;
                double yMax = yLimits.YMax;

                // Clear the plot
                formsPlot1.Plot.Clear();

                // Add axis lines with the color set to red, change hzIndx and vrtIndx values to move the axis
                formsPlot1.Plot.AddHorizontalLine(hzIndx, color: Color.Red);
                formsPlot1.Plot.AddVerticalLine(vrtIndx, color: Color.Red);

                // Redraw the plot
                // Restore the previous zoom and position
                formsPlot1.Plot.SetAxisLimits(xMin, xMax, yMin, yMax);
                formsPlot1.Plot.AddSignal(currentBuffer);
                formsPlot1.Render();
            }
        }

        private void connectButton_Click(object sender, EventArgs e)
        {
            // Open ADP3450 device 
            if (Open())
            {
                connectButton.Enabled = false;

                // Do this once, the ADP will repeat until changes are made 
                // Probably doesn't need it's own thread at this point, just doing this for 
                // consistency of dealing with different parts of the ADP
                /*
                 * int outChannel = 0;
                 * double outVoltAmplitude = 1.41;
                 * double outFrequency = 10000.0;
                 */
                _WaveGenThread = new Thread(() =>
                {
                    // Add text box data here 
                    Write(0, 1.41, 10000.0);
                })
                {
                    Priority = ThreadPriority.Normal
                };
                // Start wavegen thread 
                _WaveGenThread.Start();

                // Kick off oscilloscope thread 
                _OscilloscopeThread = new Thread(Read)
                {
                    Priority = ThreadPriority.Highest
                };

                // Start oscilloscope thread 
                _OscilloscopeThread.Start();

                // Trigger clock to start 
                // I have made this update faster for better rendering speed
                timer1.Interval = 100;
                timer1.Start();
            }
            else
            {
                MessageBox.Show("No devices found");
            }
        }

        private void updateWritingButton_Click(object sender, EventArgs e)
        {
            if(_WaveGenThread.IsAlive)
            {
                _WaveGenThread.Abort();
                formsPlot1.Plot.Clear();
            }

            // Restart the writing to ADP3450 parameters 
        }

        private void updateReadingButton_Click(object sender, EventArgs e)
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
        public void Write(int outChannel, double outVoltAmplitude, double outFrequency)
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
        // 
        // TODO:
        // Same as write, maybe make an object to pass input box data to to affect how data is being read
        //-----------------------------------------------------------------------------------------------
        public void Read()
        {
            #region Listen to signal 

            #region Configure
            // Enable the analog in channel on the device 
            dwf.FDwfAnalogInChannelEnableSet(devHandle, inChannel, 1);

            // Set the peak to peak volt range for the channel 
            dwf.FDwfAnalogInChannelRangeSet(devHandle, inChannel, inVoltRange);

            // Set the frequency sample rate 
            dwf.FDwfAnalogInFrequencySet(devHandle, inFrequency);

            // Get the maximum buffer size (NOT NECESSARILY GOOD -> 32,500)
            // dwf.FDwfAnalogInBufferSizeInfo(devHandle, out minBufferSize, out maxBufferSize);

            // Set the amount of readings you want here 
            bufferSize = 500;

            // Set the expected buffer size 
            dwf.FDwfAnalogInBufferSizeSet(devHandle, bufferSize);

            // Configure the trigger for the analog in 
            dwf.FDwfAnalogInTriggerSourceSet(devHandle, dwf.trigsrcDetectorAnalogIn);
            dwf.FDwfAnalogInTriggerAutoTimeoutSet(devHandle, triggerTimeOut);
            dwf.FDwfAnalogInTriggerChannelSet(devHandle, inChannel);
            dwf.FDwfAnalogInTriggerTypeSet(devHandle, dwf.trigtypeEdge);
            dwf.FDwfAnalogInTriggerLevelSet(devHandle, triggerVoltLevel);
            dwf.FDwfAnalogInTriggerConditionSet(devHandle, dwf.trigcondRisingPositive);

            #endregion

            #region Read

            // Just do this forever
            // Fuck the rules and all their friends 
            while (true)
            {
                PullData();
            }

            #endregion

            #endregion
        }

        //-----------------------------------------------------------------------------------------------
        // Method to extract data from the device, the amount of data to extract is controlled by buffer
        // size
        //-----------------------------------------------------------------------------------------------
        private void PullData()
        {
            // Only do this if the bufferQueue is under 10, allowing the GUI thread to catch up 
            if (bufferQueue.Count < 10)
            {
                // Create buffer array to store data
                double[] currentBuffer = new double[bufferSize];

                // Configures and begins reading
                // IMPORTANT API WILL NOT READ WITHOUT EACH TIME DO NOT MOVE 
                dwf.FDwfAnalogInConfigure(devHandle, 0, 1);

                // Read from the device until the status == done
                do
                {
                    dwf.FDwfAnalogInStatus(devHandle, 1, out status);
                } while (status != dwf.stsDone);

                // Retrieve the data
                dwf.FDwfAnalogInStatusData(devHandle, 0, currentBuffer, bufferSize);

                // Add the buffer to the queue
                bufferQueue.Enqueue(currentBuffer);
            }
        }

        //-----------------------------------------------------------------------------------------------
        // Opens the first ADP3450 device found connected to the machine 
        //-----------------------------------------------------------------------------------------------
        public bool Open()
        {
            // Open a device 
            dwf.FDwfDeviceOpen(-1, out devHandle);

            if (devHandle == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        //-----------------------------------------------------------------------------------------------
        // Function to close the ADP3450 device 
        //-----------------------------------------------------------------------------------------------
        public void CloseAll()
        {
            // Close the ADP3450 device 
            dwf.FDwfDeviceCloseAll();
        }

        #endregion

        #endregion
    }
}
