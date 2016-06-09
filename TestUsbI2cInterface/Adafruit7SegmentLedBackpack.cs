//////////////////////////////////////////////////////////////////////////
// Adafruit7SegmentLedBackpack.cs
//////////////////////////////////////////////////////////////////////////
// Interface to adafruit 0.56" LED Backpack via I2C bus using U2bI2CInterface 
// wrapper and ACS USB I2C Interface module.
//////////////////////////////////////////////////////////////////////////
// written by:  Steven J. Ackerman, Consultant
//              ACS, Sarasota, Florida
//              mailto:steve@acscontrol.com
//              http://www.acscontrol.com 
//////////////////////////////////////////////////////////////////////////

using UsbI2CInterface;

//
// Adafruit 0.56" 4-Digit 7-Segment Display w/I2C Backpack - Red
//
// https://www.adafruit.com/products/878
//
namespace TestUsbI2cInterface
{
    public class Adafruit7SegmentLedBackpack
    {
        private readonly byte[] _segments =
        {
            0x3f, // 0
            0x06, // 1
            0x5b, // 2
            0x4f, // 3
            0x66, // 4
            0x6d, // 5
            0x7d, // 6
            0x07, // 7
            0x7f, // 8
            0x6f, // 9
            0x77, // A
            0x7c, // B
            0x39, // C
            0x5e, // D
            0x79, // E
            0x71  // F
        };

        private readonly UsbI2C _usbI2C;
        private readonly byte _i2CDisplay;

        /// <summary>
        /// Construct an instance of the Adafruit_7Segment_LED_Backpack using an opened UsbI2C using specified address
        /// </summary>
        /// <param name="usbI2C">Open()'ed and Init()'ed UsbI2C</param>
        /// <param name="i2CAddress">I2C device address</param>
        public Adafruit7SegmentLedBackpack(UsbI2CInterface.UsbI2C usbI2C, byte i2CAddress)
        {
            _usbI2C = usbI2C;
            _i2CDisplay = i2CAddress;
        }

        /// <summary>
        /// Enable / Disable Adafruit_7Segment_LED_Backpack and control blink rate
        /// </summary>
        /// <param name="on">Enable = true, Disable = false</param>
        /// <param name="blink">Blink rate 0 = none, 7 = max</param>
        /// <returns>StatusResults(FT260_STATUS, FT260_CONTROLLER_STATUS)</returns>
        public UsbI2C.StatusResults Enable(bool on, byte blink)
        {
            UsbI2C.StatusResults statusResults;
            byte[] control = {0x21};

            lock (_usbI2C)
            {
                statusResults = _usbI2C.Write(
                    _i2CDisplay,
                    UsbI2C.FT260_I2C_FLAG.FT260_I2C_START_AND_STOP,
                    control,
                    1);
            }
            if (statusResults.HasErrors())
            {
                return statusResults;
            }

            control[0] = (byte) (0x80 | (((blink & 3) << 1) | (on ? 1 : 0)));

            lock (_usbI2C)
            {
                statusResults = _usbI2C.Write(
                    _i2CDisplay,
                    UsbI2C.FT260_I2C_FLAG.FT260_I2C_START_AND_STOP,
                    control,
                    1);
            }
            if (statusResults.HasErrors())
            {
                return statusResults;
            }

            return statusResults;
        }

        /// <summary>
        /// Set the Adafruit_7Segment_LED_Backpack display brightness
        /// </summary>
        /// <param name="level">Brightness 0 = oFF, 15 = max</param>
        /// <returns>StatusResults(FT260_STATUS, FT260_CONTROLLER_STATUS)</returns>
        public UsbI2C.StatusResults Brightness(byte level)
        {
            UsbI2C.StatusResults statusResults;
            byte[] control = { 0 };
            control[0] = (byte) (0xe0 | (level & 0x0f));

            lock (_usbI2C)
            {
                statusResults = _usbI2C.Write(
                    _i2CDisplay,
                    UsbI2C.FT260_I2C_FLAG.FT260_I2C_START_AND_STOP,
                    control,
                    1);
            }

            return statusResults;
        }

        /// <summary>
        /// Display an unsigned short number
        /// </summary>
        /// <param name="value">Value 0000 - 9999</param>
        /// <returns>StatusResults(FT260_STATUS, FT260_CONTROLLER_STATUS)</returns>
        public UsbI2C.StatusResults Display(ushort value)
        {
            UsbI2C.StatusResults statusResults;

            byte[] displayData = {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0};

            displayData[0] = 0;
            displayData[9] = _segments[value % 10]; value /= 10;
            displayData[7] = _segments[value % 10]; value /= 10;
            displayData[3] = _segments[value % 10]; value /= 10;
            displayData[1] = _segments[value % 10];

            lock (_usbI2C)
            {
                statusResults = _usbI2C.Write(
                    _i2CDisplay,
                    UsbI2C.FT260_I2C_FLAG.FT260_I2C_START_AND_STOP,
                    displayData,
                    (uint) displayData.Length);
            }

            return statusResults;
        }

        /// <summary>
        /// Display an unsigned short number in hexadecimal
        /// </summary>
        /// <param name="value">Value 0000 - FFFF</param>
        /// <returns>StatusResults(FT260_STATUS, FT260_CONTROLLER_STATUS)</returns>
        public UsbI2C.StatusResults DisplayHex(ushort value)
        {
            UsbI2C.StatusResults statusResults;

            byte[] displayData = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

            displayData[0] = 0;
            displayData[9] = _segments[value % 16]; value /= 16;
            displayData[7] = _segments[value % 16]; value /= 16;
            displayData[3] = _segments[value % 16]; value /= 16;
            displayData[1] = _segments[value % 16];

            lock (_usbI2C)
            {
                statusResults = _usbI2C.Write(
                    _i2CDisplay,
                    UsbI2C.FT260_I2C_FLAG.FT260_I2C_START_AND_STOP,
                    displayData,
                    (uint) displayData.Length);
            }

            return statusResults;
        }

        public UsbI2C.StatusResults Display(double value, byte fractionalDigits = 2)
        {
            UsbI2C.StatusResults statusResults;

            byte[] displayData = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

            byte numericDigits = 4;
            bool isNegative = false;

            // Q - is the number negative ?
            if (value < 0.0)
            {
                // yes - remember to draw sign which will take up 1 digit
                isNegative = true;
                --numericDigits;
                value *= -1.0;
            }

            // calc the factor required to shift all fractional digits into the integer part
            double toIntFactor = 1.0;
            for (int i = 0; i < fractionalDigits; ++i)
            {
                toIntFactor *= 10;
            }

            // create integer to display by applying shifting factor and rounding
            uint displayNumber = (uint) (value * toIntFactor + 0.5);

            // calc upper bound on display integer give available digits on display
            uint tooBig = 1;
            for (int i = 0; i < numericDigits; ++i)
            {
                tooBig *= 10;
            }

            // if integer to display is too large try fewer fractional digits
            while (displayNumber >= tooBig)
            {
                --fractionalDigits;
                toIntFactor /= 10;
                displayNumber = (uint) (value * toIntFactor + 0.5);
            }

            // Q - did intFactor shift the decimal off of the display ?
            if (toIntFactor < 1)
            {
                // yes - display error
                displayData[9] = 0x40;
                displayData[7] = 0x40;
                displayData[3] = 0x40;
                displayData[1] = 0x40;
            }
            else
            {
                // no - display the number

                int displayPosition = 9;

                if (displayNumber != 0)
                {
                    for (byte i = 0; (displayNumber != 0) || i <= fractionalDigits; ++i)
                    {
                        bool displayDecimal = (fractionalDigits != 0) && (i == fractionalDigits);
                        displayData[displayPosition] = _segments[displayNumber % 10];
                        if (displayDecimal)
                        {
                            displayData[displayPosition] |= 0x80;
                        }
                        displayPosition -= 2;
                        if (displayPosition == 5) displayPosition -= 2;
                        displayNumber /= 10;
                    }
                }
                else
                {
                    displayData[displayPosition] = _segments[0];
                    displayPosition -= 2;
                }

                if (isNegative)
                {
                    displayData[displayPosition] = 0x40;
                    displayPosition -= 2;
                }
            }

            lock (_usbI2C)
            {
                statusResults = _usbI2C.Write(
                    _i2CDisplay,
                    UsbI2C.FT260_I2C_FLAG.FT260_I2C_START_AND_STOP,
                    displayData,
                    (uint) displayData.Length);
            }

            return statusResults;
        }
    }
}
