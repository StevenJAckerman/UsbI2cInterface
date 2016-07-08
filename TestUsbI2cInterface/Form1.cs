//////////////////////////////////////////////////////////////////////////
// Form1.cs
//////////////////////////////////////////////////////////////////////////
// Winforms application written to test ACS USB I2C Interface module.
// Communicates with FTDI LibFT260.dll to talk to embedded FT260Q chip.
//////////////////////////////////////////////////////////////////////////
// written by:  Steven J. Ackerman, Consultant
//              ACS, Sarasota, Florida
//              mailto:steve@acscontrol.com
//              http://www.acscontrol.com 
//////////////////////////////////////////////////////////////////////////

using System;
using System.ComponentModel;
using System.Globalization;
using System.Threading;
using System.Windows.Forms;
using UsbI2CInterface;

namespace TestUsbI2cInterface
{
    public partial class Form1 : Form
    {
        private UsbI2C _usbI2C;
        private BackgroundWorker _backgroundWorkerAdc;

        private readonly byte _ere_I2C_AI418ML_address = 0x68;
        private readonly byte _ere_I2C_RL8xxM_config = 0;
        private readonly byte _adafruit_7Segment_address = 0x70;
        private readonly byte _adafruit_16Servo_config = 0x10;

        private Adafruit16ChannelServo _servo;

        internal class ReportAdcProgressWithStatus
        {
            private AdcMode _mode;
            public AdcMode Mode
            {
                get { return _mode; }
                set { _mode = value; }
            }

            private uint _reading;
            public uint Reading
            {
                get { return _reading; }
                set { _reading = value; }
            }

            private double _voltage;
            public double Voltage
            {
                get { return _voltage; }
                set { _voltage = value; }
            }

            private double _current;
            public double Current
            {
                get { return _current; }
                set { _current = value; }
            }

            private readonly UsbI2CInterface.UsbI2C.StatusResults _statusResults;

            public UsbI2CInterface.UsbI2C.StatusResults StatusResults
            {
                get { return _statusResults; }
            }

            internal ReportAdcProgressWithStatus(AdcMode mode, uint reading, double voltage, double current, UsbI2CInterface.UsbI2C.StatusResults statusResults)
            {
                _mode = mode;
                _reading = reading;
                _voltage = voltage;
                _current = current;
                _statusResults = statusResults;
            }
        }

        public enum AdcMode
        {
            Raw,
            Voltage,
            Current
        }

        internal class AdcBackgroundWorkerArgs
        {
            private readonly AdcMode _mode;
            public AdcMode Mode
            {
                get { return _mode; }
            }

            private readonly MCP3424.AdcChannel _channel;
            public MCP3424.AdcChannel Channel
            {
                get { return _channel; }
            }

            private readonly MCP3424.AdcSize _size;
            public MCP3424.AdcSize Size
            {
                get { return _size; }
            }

            private readonly MCP3424.AdcPga _pga;
            public MCP3424.AdcPga Pga
            {
                get { return _pga; }
            }

            internal AdcBackgroundWorkerArgs(AdcMode mode, MCP3424.AdcChannel channel, MCP3424.AdcSize size, MCP3424.AdcPga pga)
            {
                this._mode = mode;
                this._channel = channel;
                this._size = size;
                this._pga = pga;
            }
        }

        private void _displayError(UsbI2C.StatusResults statusResults)
        {
            string errorMessage = "";

            if (statusResults.HidStatus != UsbI2C.FT260_STATUS.FT260_OK)
            {
                errorMessage = "USB: " + statusResults.HidStatus.ToString();
            }
            else if (statusResults.ControllerStatus != UsbI2C.FT260_CONTROLLER_STATUS.idle)
            {
                errorMessage = "I2C: " + statusResults.ControllerStatus.ToString();
            }

            labelUsbI2cStatus.Text = errorMessage.ToString();
        }

        //
        // ere I2C-AI418ML...
        //

        private void BackgroundWorkerAdcProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            ReportAdcProgressWithStatus adcProgress = (ReportAdcProgressWithStatus) e.UserState;

            switch (adcProgress.Mode)
            {
                case AdcMode.Raw:
                    textBoxAdcValue.Text = string.Format("{0}", adcProgress.Reading);
                    break;
                case AdcMode.Voltage:
                    textBoxAdcValue.Text = string.Format("{0:F3}v", adcProgress.Voltage);
                    break;
                case AdcMode.Current:
                    textBoxAdcValue.Text = string.Format("{0:F3}mA", adcProgress.Current);
                    break;
            }
            if (adcProgress.StatusResults.HasErrors())
            {
                _displayError(adcProgress.StatusResults);

                checkBoxReadVoltage.Checked = false;
            }
        }

        private void BackgroundWorkerAdcDoWork(object sender, DoWorkEventArgs e)
        {
            var adc = new ere_I2C_AI418ML(_usbI2C, _ere_I2C_AI418ML_address);
            UsbI2C.StatusResults statusResults = new UsbI2C.StatusResults();
            AdcBackgroundWorkerArgs args = e.Argument as AdcBackgroundWorkerArgs;
            while (true)
            {
                uint reading = 0;
                double voltage = 0.0;
                double current = 0.0;
                ReportAdcProgressWithStatus adcProgress = new ReportAdcProgressWithStatus(args.Mode, reading, voltage, current, statusResults);

                switch (args.Mode)
                {
                    case AdcMode.Raw:
                        statusResults = adc.ReadAdc(args.Channel, args.Size, args.Pga, out reading);
                        adcProgress.Reading = reading;
                        break;
                    case AdcMode.Voltage:
                        statusResults = adc.ReadVoltage(args.Channel, args.Size, args.Pga, out voltage);
                        adcProgress.Voltage = voltage;
                        break;
                    case AdcMode.Current:
                        statusResults = adc.ReadCurrent(args.Channel, args.Size, args.Pga, out voltage);
                        adcProgress.Voltage = voltage;
                        break;
                }

                _backgroundWorkerAdc.ReportProgress(0, adcProgress);

                Thread.Sleep(100);

                if (statusResults.HasErrors() || _backgroundWorkerAdc.CancellationPending)
                {
                    e.Cancel = true;
                    return;
                }
            }
        }

        private void checkBoxReadVoltage_CheckedChanged(object sender, EventArgs e)
        {
            if (_usbI2C.IsOpen())
            {
                if (checkBoxReadVoltage.Checked)
                {
                    checkBoxReadVoltage.Text = "Stop";
                    comboBoxAdcMode.Enabled = false;
                    comboBoxChannel.Enabled = false;
                    comboBoxSize.Enabled = false;
                    comboBoxGain.Enabled = false;

                    _backgroundWorkerAdc = new BackgroundWorker();
                    _backgroundWorkerAdc.DoWork += BackgroundWorkerAdcDoWork;
                    _backgroundWorkerAdc.ProgressChanged += BackgroundWorkerAdcProgressChanged;
                    _backgroundWorkerAdc.WorkerReportsProgress = true;
                    _backgroundWorkerAdc.WorkerSupportsCancellation = true;

                    AdcMode mode = AdcMode.Raw;
                    MCP3424.AdcChannel channel = MCP3424.AdcChannel.Channel0;
                    MCP3424.AdcSize size = MCP3424.AdcSize.Size12Bit;
                    MCP3424.AdcPga pga = MCP3424.AdcPga.Gain1;
                    try
                    {
                        Enum.TryParse<AdcMode>(comboBoxAdcMode.SelectedValue.ToString(), out mode);
                        Enum.TryParse<MCP3424.AdcChannel>(comboBoxChannel.SelectedValue.ToString(), out channel);
                        Enum.TryParse<MCP3424.AdcSize>(comboBoxSize.SelectedValue.ToString(), out size);
                        Enum.TryParse<MCP3424.AdcPga>(comboBoxGain.SelectedValue.ToString(), out pga);
                    }
                    catch (Exception ex)
                    {
                        string error = ex.Message;
                    }

                    AdcBackgroundWorkerArgs args = new AdcBackgroundWorkerArgs(mode, channel, size, pga);
                    _backgroundWorkerAdc.RunWorkerAsync(args);
                }
                else
                {
                    checkBoxReadVoltage.Text = "Start";
                    comboBoxAdcMode.Enabled = true;
                    comboBoxChannel.Enabled = true;
                    comboBoxSize.Enabled = true;
                    comboBoxGain.Enabled = true;

                    if (_backgroundWorkerAdc != null && _backgroundWorkerAdc.IsBusy)
                    {
                        _backgroundWorkerAdc.CancelAsync();
                    }

                    Thread.Sleep(1000);
                }
            }
        }

        private void comboBoxAdcMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBoxAdcValue.Text = "";
        }

        private void comboBoxChannel_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBoxAdcValue.Text = "";
        }

        private void comboBoxSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBoxAdcValue.Text = "";
        }

        private void comboBoxGain_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBoxAdcValue.Text = "";
        }

        //
        // adafruit 0.56" LED Backpack...
        //

        private void buttonDisplayNumber_Click(object sender, EventArgs e)
        {
            double value;
            if (double.TryParse(textBoxDisplayNumber.Text, NumberStyles.Number, CultureInfo.CurrentCulture, out value))
            {
                if (_usbI2C.IsOpen())
                {
                    UsbI2C.StatusResults statusResults;

                    var ledDisplay = new Adafruit7SegmentLedBackpack(_usbI2C, _adafruit_7Segment_address);
                    if ((statusResults = ledDisplay.Enable(true, 0)).HasErrors())
                    {
                        _displayError(statusResults);
                        return;
                    }
                    if ((statusResults = ledDisplay.Brightness(15)).HasErrors())
                    {
                        _displayError(statusResults);
                        return;
                    }

                    if ((statusResults = ledDisplay.Display(value)).HasErrors())
                    {
                        _displayError(statusResults);
                        return;
                    }

                    _displayError(statusResults);
                }
            }
        }

        //
        // ere I2C RL8xxM...
        // 

        private void _setRelayState(ere_I2C_RL8xxM.Relay relay, bool state)
        {
            if (_usbI2C.IsOpen())
            {
                var relays = new ere_I2C_RL8xxM(_usbI2C, _ere_I2C_RL8xxM_config);

                UsbI2C.StatusResults statusResults;

                if (state)
                {
                    statusResults = relays.On(relay);
                }
                else
                {
                    statusResults = relays.Off(relay);
                }

                _displayError(statusResults);
            }
        }

        private void checkBoxK1_CheckedChanged(object sender, EventArgs e)
        {
            _setRelayState(ere_I2C_RL8xxM.Relay.K1, checkBoxK1.Checked);
        }

        private void checkBoxK2_CheckedChanged(object sender, EventArgs e)
        {
            _setRelayState(ere_I2C_RL8xxM.Relay.K2, checkBoxK2.Checked);
        }

        private void checkBoxK3_CheckedChanged(object sender, EventArgs e)
        {
            _setRelayState(ere_I2C_RL8xxM.Relay.K3, checkBoxK3.Checked);
        }

        private void checkBoxK4_CheckedChanged(object sender, EventArgs e)
        {
            _setRelayState(ere_I2C_RL8xxM.Relay.K4, checkBoxK4.Checked);
        }

        private void checkBoxK5_CheckedChanged(object sender, EventArgs e)
        {
            _setRelayState(ere_I2C_RL8xxM.Relay.K5, checkBoxK5.Checked);
        }

        private void checkBoxK6_CheckedChanged(object sender, EventArgs e)
        {
            _setRelayState(ere_I2C_RL8xxM.Relay.K6, checkBoxK6.Checked);
        }

        private void checkBoxK7_CheckedChanged(object sender, EventArgs e)
        {
            _setRelayState(ere_I2C_RL8xxM.Relay.K7, checkBoxK7.Checked);
        }

        private void checkBoxK8_CheckedChanged(object sender, EventArgs e)
        {
            _setRelayState(ere_I2C_RL8xxM.Relay.K8, checkBoxK8.Checked);
        }

        private void checkBoxAll_CheckedChanged(object sender, EventArgs e)
        {
            if (_usbI2C.IsOpen())
            {
                var relays = new ere_I2C_RL8xxM(_usbI2C, 0);

                if (checkBoxAll.Checked)
                {
                    _setRelayState(ere_I2C_RL8xxM.Relay.ALL, checkBoxAll.Checked);
                    checkBoxK1.Checked = true;
                    checkBoxK2.Checked = true;
                    checkBoxK3.Checked = true;
                    checkBoxK4.Checked = true;
                    checkBoxK5.Checked = true;
                    checkBoxK6.Checked = true;
                    checkBoxK7.Checked = true;
                    checkBoxK8.Checked = true;
                }
                else
                {
                    _setRelayState(ere_I2C_RL8xxM.Relay.ALL, checkBoxAll.Checked);
                    checkBoxK1.Checked = false;
                    checkBoxK2.Checked = false;
                    checkBoxK3.Checked = false;
                    checkBoxK4.Checked = false;
                    checkBoxK5.Checked = false;
                    checkBoxK6.Checked = false;
                    checkBoxK7.Checked = false;
                    checkBoxK8.Checked = false;
                }
            }
        }

        //
        // adafruit 16 channel servo...
        //

        private void trackBarServo_Scroll(object sender, EventArgs e)
        {
            if (_usbI2C.IsOpen())
            {
                UsbI2C.StatusResults statusResults;

                if ((statusResults = _servo.Position((byte)comboBoxServo.SelectedIndex, (ushort)trackBarServo.Value)).HasErrors())
                {
                    _displayError(statusResults);
                    trackBarServo.Value = trackBarServo.Minimum;
                    checkBoxServo.Checked = false;
                }
            }
        }

        private void checkBoxServo_CheckedChanged(object sender, EventArgs e)
        {
            if (_usbI2C.IsOpen())
            {
                if (checkBoxServo.Checked)
                {
                    UsbI2C.StatusResults statusResults;

                    _servo = new Adafruit16ChannelServo(_usbI2C, _adafruit_16Servo_config);
                    if ((statusResults = _servo.Init()).HasErrors())
                    {
                        _displayError(statusResults);
                        checkBoxServo.Checked = false;
                        comboBoxServo.Enabled = false;
                    }
                    else
                    {
                        checkBoxServo.Text = "Disable";
                        comboBoxServo.Enabled = true;
                        trackBarServo.Enabled = true;
                    }
                }
                else
                {
                    checkBoxServo.Text = "Enable";
                    comboBoxServo.Enabled = false;
                    trackBarServo.Enabled = false;
                }
            }
        }

        //
        // the form...
        //

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            comboBoxAdcMode.DataSource = EnumExtensions.Values<AdcMode>();
            comboBoxAdcMode.DisplayMember = "name";
            comboBoxChannel.DataSource = EnumExtensions.Values<MCP3424.AdcChannel>();
            comboBoxChannel.DisplayMember = "name";
            comboBoxSize.DataSource = EnumExtensions.Values < MCP3424.AdcSize>();
            comboBoxSize.DisplayMember = "name";
            comboBoxGain.DataSource = EnumExtensions.Values<MCP3424.AdcPga>();
            comboBoxGain.DisplayMember = "name";

            comboBoxServo.SelectedIndex = 0;
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            UsbI2CInterface.UsbI2C.StatusResults statusResults;

            _usbI2C = new UsbI2CInterface.UsbI2C();

            if ((statusResults = _usbI2C.Open()).HasErrors())
            {
                _displayError(statusResults);
                return;
            }

            if ((statusResults = _usbI2C.Init(100)).HasErrors())
            {
                _displayError(statusResults);
                return;
            }
        }

        private void buttonExit_Click(object sender, EventArgs e)
        {
            if (_backgroundWorkerAdc != null && _backgroundWorkerAdc.IsBusy)
            {
                _backgroundWorkerAdc.CancelAsync();
            }

            Thread.Sleep(1000);

            if (_usbI2C.IsOpen())
            {
                _usbI2C.Close();
            }

            Close();
        }
    }
}
