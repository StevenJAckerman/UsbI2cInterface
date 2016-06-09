namespace TestUsbI2cInterface
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.comboBoxAdcMode = new System.Windows.Forms.ComboBox();
            this.comboBoxGain = new System.Windows.Forms.ComboBox();
            this.comboBoxSize = new System.Windows.Forms.ComboBox();
            this.comboBoxChannel = new System.Windows.Forms.ComboBox();
            this.textBoxAdcValue = new System.Windows.Forms.TextBox();
            this.checkBoxReadVoltage = new System.Windows.Forms.CheckBox();
            this.button1 = new System.Windows.Forms.Button();
            this.buttonDisplayNumber = new System.Windows.Forms.Button();
            this.textBoxDisplayNumber = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.checkBoxAll = new System.Windows.Forms.CheckBox();
            this.checkBoxK1 = new System.Windows.Forms.CheckBox();
            this.checkBoxK5 = new System.Windows.Forms.CheckBox();
            this.checkBoxK3 = new System.Windows.Forms.CheckBox();
            this.checkBoxK2 = new System.Windows.Forms.CheckBox();
            this.checkBoxK7 = new System.Windows.Forms.CheckBox();
            this.checkBoxK4 = new System.Windows.Forms.CheckBox();
            this.checkBoxK6 = new System.Windows.Forms.CheckBox();
            this.checkBoxK8 = new System.Windows.Forms.CheckBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.labelUsbI2cStatus = new System.Windows.Forms.Label();
            this.adafruit16ChannelServoBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.checkBoxServo = new System.Windows.Forms.CheckBox();
            this.trackBarServo = new System.Windows.Forms.TrackBar();
            this.comboBoxServo = new System.Windows.Forms.ComboBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.adafruit16ChannelServoBindingSource)).BeginInit();
            this.groupBox4.SuspendLayout();
            this.groupBox5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarServo)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.comboBoxAdcMode);
            this.groupBox1.Controls.Add(this.comboBoxGain);
            this.groupBox1.Controls.Add(this.comboBoxSize);
            this.groupBox1.Controls.Add(this.comboBoxChannel);
            this.groupBox1.Controls.Add(this.textBoxAdcValue);
            this.groupBox1.Controls.Add(this.checkBoxReadVoltage);
            this.groupBox1.Location = new System.Drawing.Point(12, 55);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(459, 49);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "ere I2C AI418ML Quad Analog Input";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(346, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(19, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "=>";
            // 
            // comboBoxAdcMode
            // 
            this.comboBoxAdcMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxAdcMode.FormattingEnabled = true;
            this.comboBoxAdcMode.Location = new System.Drawing.Point(51, 22);
            this.comboBoxAdcMode.Name = "comboBoxAdcMode";
            this.comboBoxAdcMode.Size = new System.Drawing.Size(64, 21);
            this.comboBoxAdcMode.TabIndex = 1;
            this.comboBoxAdcMode.SelectedIndexChanged += new System.EventHandler(this.comboBoxAdcMode_SelectedIndexChanged);
            // 
            // comboBoxGain
            // 
            this.comboBoxGain.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxGain.FormattingEnabled = true;
            this.comboBoxGain.Location = new System.Drawing.Point(273, 22);
            this.comboBoxGain.Name = "comboBoxGain";
            this.comboBoxGain.Size = new System.Drawing.Size(64, 21);
            this.comboBoxGain.TabIndex = 4;
            this.comboBoxGain.SelectedIndexChanged += new System.EventHandler(this.comboBoxGain_SelectedIndexChanged);
            // 
            // comboBoxSize
            // 
            this.comboBoxSize.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxSize.FormattingEnabled = true;
            this.comboBoxSize.Location = new System.Drawing.Point(197, 22);
            this.comboBoxSize.Name = "comboBoxSize";
            this.comboBoxSize.Size = new System.Drawing.Size(70, 21);
            this.comboBoxSize.TabIndex = 3;
            this.comboBoxSize.SelectedIndexChanged += new System.EventHandler(this.comboBoxSize_SelectedIndexChanged);
            // 
            // comboBoxChannel
            // 
            this.comboBoxChannel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxChannel.FormattingEnabled = true;
            this.comboBoxChannel.Location = new System.Drawing.Point(121, 22);
            this.comboBoxChannel.Name = "comboBoxChannel";
            this.comboBoxChannel.Size = new System.Drawing.Size(70, 21);
            this.comboBoxChannel.TabIndex = 2;
            this.comboBoxChannel.SelectedIndexChanged += new System.EventHandler(this.comboBoxChannel_SelectedIndexChanged);
            // 
            // textBoxAdcValue
            // 
            this.textBoxAdcValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxAdcValue.Location = new System.Drawing.Point(371, 21);
            this.textBoxAdcValue.Name = "textBoxAdcValue";
            this.textBoxAdcValue.ReadOnly = true;
            this.textBoxAdcValue.Size = new System.Drawing.Size(82, 22);
            this.textBoxAdcValue.TabIndex = 6;
            this.textBoxAdcValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // checkBoxReadVoltage
            // 
            this.checkBoxReadVoltage.Appearance = System.Windows.Forms.Appearance.Button;
            this.checkBoxReadVoltage.AutoSize = true;
            this.checkBoxReadVoltage.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.checkBoxReadVoltage.Location = new System.Drawing.Point(7, 20);
            this.checkBoxReadVoltage.Name = "checkBoxReadVoltage";
            this.checkBoxReadVoltage.Size = new System.Drawing.Size(39, 23);
            this.checkBoxReadVoltage.TabIndex = 0;
            this.checkBoxReadVoltage.Text = "Start";
            this.checkBoxReadVoltage.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.checkBoxReadVoltage.UseVisualStyleBackColor = true;
            this.checkBoxReadVoltage.CheckedChanged += new System.EventHandler(this.checkBoxReadVoltage_CheckedChanged);
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.button1.Location = new System.Drawing.Point(396, 23);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "Exit";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.buttonExit_Click);
            // 
            // buttonDisplayNumber
            // 
            this.buttonDisplayNumber.Location = new System.Drawing.Point(7, 16);
            this.buttonDisplayNumber.Name = "buttonDisplayNumber";
            this.buttonDisplayNumber.Size = new System.Drawing.Size(75, 23);
            this.buttonDisplayNumber.TabIndex = 0;
            this.buttonDisplayNumber.Text = "Display";
            this.buttonDisplayNumber.UseVisualStyleBackColor = true;
            this.buttonDisplayNumber.Click += new System.EventHandler(this.buttonDisplayNumber_Click);
            // 
            // textBoxDisplayNumber
            // 
            this.textBoxDisplayNumber.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxDisplayNumber.Location = new System.Drawing.Point(88, 16);
            this.textBoxDisplayNumber.Name = "textBoxDisplayNumber";
            this.textBoxDisplayNumber.Size = new System.Drawing.Size(100, 22);
            this.textBoxDisplayNumber.TabIndex = 1;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.checkBoxAll);
            this.groupBox2.Controls.Add(this.checkBoxK1);
            this.groupBox2.Controls.Add(this.checkBoxK5);
            this.groupBox2.Controls.Add(this.checkBoxK3);
            this.groupBox2.Controls.Add(this.checkBoxK2);
            this.groupBox2.Controls.Add(this.checkBoxK7);
            this.groupBox2.Controls.Add(this.checkBoxK4);
            this.groupBox2.Controls.Add(this.checkBoxK6);
            this.groupBox2.Controls.Add(this.checkBoxK8);
            this.groupBox2.Location = new System.Drawing.Point(12, 110);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(459, 49);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "ere I2C RL8xxM Octal Relay";
            // 
            // checkBoxAll
            // 
            this.checkBoxAll.AutoSize = true;
            this.checkBoxAll.Location = new System.Drawing.Point(362, 18);
            this.checkBoxAll.Name = "checkBoxAll";
            this.checkBoxAll.Size = new System.Drawing.Size(45, 17);
            this.checkBoxAll.TabIndex = 8;
            this.checkBoxAll.Text = "ALL";
            this.checkBoxAll.UseVisualStyleBackColor = true;
            this.checkBoxAll.CheckedChanged += new System.EventHandler(this.checkBoxAll_CheckedChanged);
            // 
            // checkBoxK1
            // 
            this.checkBoxK1.AutoSize = true;
            this.checkBoxK1.Location = new System.Drawing.Point(317, 18);
            this.checkBoxK1.Name = "checkBoxK1";
            this.checkBoxK1.Size = new System.Drawing.Size(39, 17);
            this.checkBoxK1.TabIndex = 7;
            this.checkBoxK1.Text = "K1";
            this.checkBoxK1.UseVisualStyleBackColor = true;
            this.checkBoxK1.CheckedChanged += new System.EventHandler(this.checkBoxK1_CheckedChanged);
            // 
            // checkBoxK5
            // 
            this.checkBoxK5.AutoSize = true;
            this.checkBoxK5.Location = new System.Drawing.Point(140, 19);
            this.checkBoxK5.Name = "checkBoxK5";
            this.checkBoxK5.Size = new System.Drawing.Size(39, 17);
            this.checkBoxK5.TabIndex = 3;
            this.checkBoxK5.Text = "K5";
            this.checkBoxK5.UseVisualStyleBackColor = true;
            this.checkBoxK5.CheckedChanged += new System.EventHandler(this.checkBoxK5_CheckedChanged);
            // 
            // checkBoxK3
            // 
            this.checkBoxK3.AutoSize = true;
            this.checkBoxK3.Location = new System.Drawing.Point(228, 19);
            this.checkBoxK3.Name = "checkBoxK3";
            this.checkBoxK3.Size = new System.Drawing.Size(39, 17);
            this.checkBoxK3.TabIndex = 5;
            this.checkBoxK3.Text = "K3";
            this.checkBoxK3.UseVisualStyleBackColor = true;
            this.checkBoxK3.CheckedChanged += new System.EventHandler(this.checkBoxK3_CheckedChanged);
            // 
            // checkBoxK2
            // 
            this.checkBoxK2.AutoSize = true;
            this.checkBoxK2.Location = new System.Drawing.Point(273, 18);
            this.checkBoxK2.Name = "checkBoxK2";
            this.checkBoxK2.Size = new System.Drawing.Size(39, 17);
            this.checkBoxK2.TabIndex = 6;
            this.checkBoxK2.Text = "K2";
            this.checkBoxK2.UseVisualStyleBackColor = true;
            this.checkBoxK2.CheckedChanged += new System.EventHandler(this.checkBoxK2_CheckedChanged);
            // 
            // checkBoxK7
            // 
            this.checkBoxK7.AutoSize = true;
            this.checkBoxK7.Location = new System.Drawing.Point(51, 20);
            this.checkBoxK7.Name = "checkBoxK7";
            this.checkBoxK7.Size = new System.Drawing.Size(39, 17);
            this.checkBoxK7.TabIndex = 1;
            this.checkBoxK7.Text = "K7";
            this.checkBoxK7.UseVisualStyleBackColor = true;
            this.checkBoxK7.CheckedChanged += new System.EventHandler(this.checkBoxK7_CheckedChanged);
            // 
            // checkBoxK4
            // 
            this.checkBoxK4.AutoSize = true;
            this.checkBoxK4.Location = new System.Drawing.Point(184, 19);
            this.checkBoxK4.Name = "checkBoxK4";
            this.checkBoxK4.Size = new System.Drawing.Size(39, 17);
            this.checkBoxK4.TabIndex = 4;
            this.checkBoxK4.Text = "K4";
            this.checkBoxK4.UseVisualStyleBackColor = true;
            this.checkBoxK4.CheckedChanged += new System.EventHandler(this.checkBoxK4_CheckedChanged);
            // 
            // checkBoxK6
            // 
            this.checkBoxK6.AutoSize = true;
            this.checkBoxK6.Location = new System.Drawing.Point(96, 19);
            this.checkBoxK6.Name = "checkBoxK6";
            this.checkBoxK6.Size = new System.Drawing.Size(39, 17);
            this.checkBoxK6.TabIndex = 2;
            this.checkBoxK6.Text = "K6";
            this.checkBoxK6.UseVisualStyleBackColor = true;
            this.checkBoxK6.CheckedChanged += new System.EventHandler(this.checkBoxK6_CheckedChanged);
            // 
            // checkBoxK8
            // 
            this.checkBoxK8.AutoSize = true;
            this.checkBoxK8.Location = new System.Drawing.Point(7, 20);
            this.checkBoxK8.Name = "checkBoxK8";
            this.checkBoxK8.Size = new System.Drawing.Size(39, 17);
            this.checkBoxK8.TabIndex = 0;
            this.checkBoxK8.Text = "K8";
            this.checkBoxK8.UseVisualStyleBackColor = true;
            this.checkBoxK8.CheckedChanged += new System.EventHandler(this.checkBoxK8_CheckedChanged);
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox3.Controls.Add(this.labelUsbI2cStatus);
            this.groupBox3.Location = new System.Drawing.Point(12, 12);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(378, 37);
            this.groupBox3.TabIndex = 5;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "USB I2C Interface Status";
            // 
            // labelUsbI2cStatus
            // 
            this.labelUsbI2cStatus.AutoSize = true;
            this.labelUsbI2cStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelUsbI2cStatus.Location = new System.Drawing.Point(6, 16);
            this.labelUsbI2cStatus.Name = "labelUsbI2cStatus";
            this.labelUsbI2cStatus.Size = new System.Drawing.Size(0, 13);
            this.labelUsbI2cStatus.TabIndex = 0;
            this.labelUsbI2cStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.buttonDisplayNumber);
            this.groupBox4.Controls.Add(this.textBoxDisplayNumber);
            this.groupBox4.Location = new System.Drawing.Point(12, 165);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(459, 45);
            this.groupBox4.TabIndex = 3;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "adafruit 0.56 LED Display";
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.comboBoxServo);
            this.groupBox5.Controls.Add(this.checkBoxServo);
            this.groupBox5.Controls.Add(this.trackBarServo);
            this.groupBox5.Location = new System.Drawing.Point(13, 217);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(458, 76);
            this.groupBox5.TabIndex = 4;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "adafruit 16 channel servo";
            // 
            // checkBoxServo
            // 
            this.checkBoxServo.Appearance = System.Windows.Forms.Appearance.Button;
            this.checkBoxServo.AutoSize = true;
            this.checkBoxServo.Location = new System.Drawing.Point(8, 30);
            this.checkBoxServo.Name = "checkBoxServo";
            this.checkBoxServo.Size = new System.Drawing.Size(50, 23);
            this.checkBoxServo.TabIndex = 0;
            this.checkBoxServo.Text = "Enable";
            this.checkBoxServo.UseVisualStyleBackColor = true;
            this.checkBoxServo.CheckedChanged += new System.EventHandler(this.checkBoxServo_CheckedChanged);
            // 
            // trackBarServo
            // 
            this.trackBarServo.Location = new System.Drawing.Point(112, 19);
            this.trackBarServo.Maximum = 650;
            this.trackBarServo.Minimum = 150;
            this.trackBarServo.Name = "trackBarServo";
            this.trackBarServo.Size = new System.Drawing.Size(340, 45);
            this.trackBarServo.TabIndex = 2;
            this.trackBarServo.TickFrequency = 10;
            this.trackBarServo.TickStyle = System.Windows.Forms.TickStyle.Both;
            this.trackBarServo.Value = 150;
            this.trackBarServo.Scroll += new System.EventHandler(this.trackBarServo_Scroll);
            // 
            // comboBoxServo
            // 
            this.comboBoxServo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxServo.FormattingEnabled = true;
            this.comboBoxServo.Items.AddRange(new object[] {
            "0",
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9",
            "10",
            "11",
            "12",
            "13",
            "14",
            "15"});
            this.comboBoxServo.Location = new System.Drawing.Point(64, 32);
            this.comboBoxServo.Name = "comboBoxServo";
            this.comboBoxServo.Size = new System.Drawing.Size(42, 21);
            this.comboBoxServo.TabIndex = 1;
            // 
            // Form1
            // 
            this.ClientSize = new System.Drawing.Size(483, 305);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.groupBox1);
            this.Name = "Form1";
            this.Text = "USB I2C Interface";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Shown += new System.EventHandler(this.Form1_Shown);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.adafruit16ChannelServoBindingSource)).EndInit();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarServo)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox textBoxAdcValue;
        private System.Windows.Forms.CheckBox checkBoxReadVoltage;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ComboBox comboBoxChannel;
        private System.Windows.Forms.BindingSource adafruit16ChannelServoBindingSource;
        private System.Windows.Forms.ComboBox comboBoxSize;
        private System.Windows.Forms.ComboBox comboBoxGain;
        private System.Windows.Forms.ComboBox comboBoxAdcMode;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button buttonDisplayNumber;
        private System.Windows.Forms.TextBox textBoxDisplayNumber;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox checkBoxK1;
        private System.Windows.Forms.CheckBox checkBoxK5;
        private System.Windows.Forms.CheckBox checkBoxK3;
        private System.Windows.Forms.CheckBox checkBoxK2;
        private System.Windows.Forms.CheckBox checkBoxK7;
        private System.Windows.Forms.CheckBox checkBoxK4;
        private System.Windows.Forms.CheckBox checkBoxK6;
        private System.Windows.Forms.CheckBox checkBoxK8;
        private System.Windows.Forms.CheckBox checkBoxAll;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label labelUsbI2cStatus;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.TrackBar trackBarServo;
        private System.Windows.Forms.CheckBox checkBoxServo;
        private System.Windows.Forms.ComboBox comboBoxServo;
    }
}

