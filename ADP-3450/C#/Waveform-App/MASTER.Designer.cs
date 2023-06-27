namespace Waveform_App
{
    partial class MASTER
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            formsPlot1 = new ScottPlot.FormsPlot();
            timer1 = new System.Windows.Forms.Timer(components);
            ConnectedLabel = new Label();
            ConnectButton = new Button();
            GenerateButton = new Button();
            WaveTypeComboBox = new ComboBox();
            SuspendLayout();
            // 
            // formsPlot1
            // 
            formsPlot1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            formsPlot1.Location = new Point(0, 0);
            formsPlot1.Margin = new Padding(4, 3, 4, 3);
            formsPlot1.Name = "formsPlot1";
            formsPlot1.Size = new Size(824, 424);
            formsPlot1.TabIndex = 0;
            // 
            // timer1
            // 
            timer1.Enabled = true;
            timer1.Interval = 20;
            timer1.Tick += timer1_Tick;
            // 
            // ConnectedLabel
            // 
            ConnectedLabel.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            ConnectedLabel.AutoSize = true;
            ConnectedLabel.Location = new Point(831, 44);
            ConnectedLabel.Name = "ConnectedLabel";
            ConnectedLabel.Size = new Size(16, 15);
            ConnectedLabel.TabIndex = 1;
            ConnectedLabel.Text = "...";
            // 
            // ConnectButton
            // 
            ConnectButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            ConnectButton.BackColor = SystemColors.ControlLight;
            ConnectButton.Location = new Point(831, 18);
            ConnectButton.Name = "ConnectButton";
            ConnectButton.Size = new Size(75, 23);
            ConnectButton.TabIndex = 2;
            ConnectButton.Text = "Connect";
            ConnectButton.UseVisualStyleBackColor = false;
            ConnectButton.Click += ConnectButton_Click;
            // 
            // GenerateButton
            // 
            GenerateButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            GenerateButton.Enabled = false;
            GenerateButton.Location = new Point(831, 112);
            GenerateButton.Name = "GenerateButton";
            GenerateButton.Size = new Size(75, 23);
            GenerateButton.TabIndex = 3;
            GenerateButton.Text = "Generate Wave";
            GenerateButton.UseVisualStyleBackColor = true;
            // 
            // WaveTypeComboBox
            // 
            WaveTypeComboBox.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            WaveTypeComboBox.Enabled = false;
            WaveTypeComboBox.FormattingEnabled = true;
            WaveTypeComboBox.Items.AddRange(new object[] { "Square", "Sine", "Triangular" });
            WaveTypeComboBox.Location = new Point(831, 83);
            WaveTypeComboBox.Name = "WaveTypeComboBox";
            WaveTypeComboBox.Size = new Size(121, 23);
            WaveTypeComboBox.TabIndex = 4;
            // 
            // MASTER
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(974, 422);
            Controls.Add(WaveTypeComboBox);
            Controls.Add(GenerateButton);
            Controls.Add(ConnectButton);
            Controls.Add(ConnectedLabel);
            Controls.Add(formsPlot1);
            Name = "MASTER";
            Text = "Wave Form Listener";
            FormClosing += MASTER_FormClosing;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private FileSystemWatcher fileSystemWatcher1;
        private ScottPlot.FormsPlot formsPlot1;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Timer timer2;
        private Label ConnectedLabel;
        private Button ConnectButton;
        private Button GenerateButton;
        private ComboBox WaveTypeComboBox;
    }
}