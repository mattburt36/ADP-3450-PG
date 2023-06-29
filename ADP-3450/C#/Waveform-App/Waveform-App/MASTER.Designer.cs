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
            connectButton = new Button();
            updateWritingButton = new Button();
            updateReadingButton = new Button();
            textBox1 = new TextBox();
            SuspendLayout();
            // 
            // formsPlot1
            // 
            formsPlot1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            formsPlot1.Location = new Point(0, 0);
            formsPlot1.Margin = new Padding(4, 3, 4, 3);
            formsPlot1.Name = "formsPlot1";
            formsPlot1.Size = new Size(846, 496);
            formsPlot1.TabIndex = 0;
            // 
            // timer1
            // 
            timer1.Tick += timer1_Tick;
            // 
            // connectButton
            // 
            connectButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            connectButton.Location = new Point(899, 459);
            connectButton.Name = "connectButton";
            connectButton.Size = new Size(75, 23);
            connectButton.TabIndex = 1;
            connectButton.Text = "Connect";
            connectButton.UseVisualStyleBackColor = true;
            connectButton.Click += connectButton_Click;
            // 
            // updateWritingButton
            // 
            updateWritingButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            updateWritingButton.Location = new Point(874, 23);
            updateWritingButton.Name = "updateWritingButton";
            updateWritingButton.Size = new Size(114, 23);
            updateWritingButton.TabIndex = 2;
            updateWritingButton.Text = "Update Writing";
            updateWritingButton.UseVisualStyleBackColor = true;
            updateWritingButton.Click += updateWritingButton_Click;
            // 
            // updateReadingButton
            // 
            updateReadingButton.Anchor = AnchorStyles.Right;
            updateReadingButton.Location = new Point(888, 183);
            updateReadingButton.Name = "updateReadingButton";
            updateReadingButton.Size = new Size(114, 23);
            updateReadingButton.TabIndex = 3;
            updateReadingButton.Text = "Update Reading";
            updateReadingButton.UseVisualStyleBackColor = true;
            updateReadingButton.Click += updateReadingButton_Click;
            // 
            // textBox1
            // 
            textBox1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            textBox1.Location = new Point(888, 52);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(100, 23);
            textBox1.TabIndex = 4;
            // 
            // MASTER
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1014, 494);
            Controls.Add(textBox1);
            Controls.Add(updateReadingButton);
            Controls.Add(updateWritingButton);
            Controls.Add(connectButton);
            Controls.Add(formsPlot1);
            Name = "MASTER";
            Text = "Wave Form Listener Example App";
            FormClosing += MASTER_FormClosing;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ScottPlot.FormsPlot formsPlot1;
        private System.Windows.Forms.Timer timer1;
        private Button connectButton;
        private Button updateWritingButton;
        private Button updateReadingButton;
        private TextBox textBox1;
    }
}