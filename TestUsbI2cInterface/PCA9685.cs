//////////////////////////////////////////////////////////////////////////
// PCA9685.cs
//////////////////////////////////////////////////////////////////////////
// Interface to Nxp PCA9685 via I2C bus using U2bI2CInterface wrapper 
// and ACS USB I2C Interface module.
//////////////////////////////////////////////////////////////////////////
// written by:  Steven J. Ackerman, Consultant
//              ACS, Sarasota, Florida
//              mailto:steve@acscontrol.com
//              http://www.acscontrol.com 
//////////////////////////////////////////////////////////////////////////

using UsbI2CInterface;

//
// NXP PCA9685 16-channel, 12-bit PWM Fm+ I2C-bus LED controller
//
namespace TestUsbI2cInterface
{
    public class PCA9685
    {
        private readonly UsbI2C _usbI2C;

        private readonly byte _i2cAddress;

        private readonly byte[] _configurableAddresses =
        {
            0x40, 0x41, 0x42, 0x43, 0x44, 0x45, 0x46, 0x47, 0x48, 0x49, 0x4a, 0x4b, 0x4c, 0x4d, 0x4e, 0x4f,
            0x50, 0x51, 0x52, 0x53, 0x54, 0x55, 0x56, 0x57, 0x58, 0x59, 0x5a, 0x5b, 0x5c, 0x5d, 0x5e, 0x5f,
            0x60, 0x61, 0x62, 0x63, 0x64, 0x65, 0x66, 0x67, 0x68, 0x69, 0x6a, 0x6b, 0x6c, 0x6d, 0x6e, 0x6f,
            0x70, 0x71, 0x72, 0x73, 0x74, 0x75, 0x76, 0x77, 0x78, 0x79, 0x7a, 0x7b, 0x7c, 0x7d, 0x7e, 0x7f
        };

        public enum Reg : byte
        {
            MODE1 = 0, MODE2,
            SUBADR1, SUBADR2, SUBADR3,
            ALLCALLADR,
            LED0_ON_L, LED0_ON_H, LED0_OFF_L, LED0_OFF_H,
            LED1_ON_L, LED1_ON_H, LED1_OFF_L, LED1_OFF_H,
            LED2_ON_L, LED2_ON_H, LED2_OFF_L, LED2_OFF_H,
            LED3_ON_L, LED3_ON_H, LED3_OFF_L, LED3_OFF_H,
            LED4_ON_L, LED4_ON_H, LED4_OFF_L, LED4_OFF_H,
            LED5_ON_L, LED5_ON_H, LED5_OFF_L, LED5_OFF_H,
            LED6_ON_L, LED6_ON_H, LED6_OFF_L, LED6_OFF_H,
            LED7_ON_L, LED7_ON_H, LED7_OFF_L, LED7_OFF_H,
            LED8_ON_L, LED8_ON_H, LED8_OFF_L, LED8_OFF_H,
            LED9_ON_L, LED9_ON_H, LED9_OFF_L, LED9_OFF_H,
            LED10_ON_L, LED10_ON_H, LED10_OFF_L, LED10_OFF_H,
            LED11_ON_L, LED11_ON_H, LED11_OFF_L, LED11_OFF_H,
            LED12_ON_L, LED12_ON_H, LED12_OFF_L, LED12_OFF_H,
            LED13_ON_L, LED13_ON_H, LED13_OFF_L, LED13_OFF_H, 
            LED14_ON_L, LED14_ON_H, LED14_OFF_L, LED14_OFF_H,
            LED15_ON_L, LED15_ON_H, LED15_OFF_L, LED15_OFF_H,
            ALL_LED_ON_L = 0xfa, ALL_LED_ON_H, ALL_LED_OFF_L, ALL_LED_OFF_H,
            PRE_SCALE
        };

        /// <summary>
        /// Construct an instance of PCA9685 using an opened UsbI2C with specified address inputs
        /// </summary>
        /// <param name="usbI2C">Open()'ed and Init()'ed UsbI2C</param>
        /// <param name="addressInputs">PCA9685 address inputs</param>
        public PCA9685(UsbI2C usbI2C, byte addressInputs = 0)
        {
            _usbI2C = usbI2C;
            _i2cAddress = _configurableAddresses[addressInputs & 0x3f];
        }

        /// <summary>
        /// Read from PCA9685 register
        /// </summary>
        /// <param name="reg">PCA9685 register to read from</param>
        /// <param name="value">value read from register</param>
        /// <returns>StatusResults(FT260_STATUS, FT260_CONTROLLER_STATUS)</returns>
        public UsbI2C.StatusResults Read(Reg reg, out byte value)
        {
            UsbI2C.StatusResults statusResults;
            byte[] regValue = { (byte)reg };
            byte[] readBytes = { 0 };

            value = 0;

            lock (_usbI2C)
            {
                if (!(statusResults = _usbI2C.Write(
                    _i2cAddress,
                    UsbI2C.FT260_I2C_FLAG.FT260_I2C_START,
                    regValue,
                    (uint) regValue.Length)).HasErrors())
                {
                    if (!(statusResults = _usbI2C.Read(
                        _i2cAddress, 
                        UsbI2C.FT260_I2C_FLAG.FT260_I2C_REPEATED_START_AND_STOP,
                        readBytes, 
                        (uint) readBytes.Length)).HasErrors())
                    {
                        value = readBytes[0];
                    }
                }
            }

            return statusResults;;
        }

        /// <summary>
        /// Read from sequential PCA9685 registers (requires MODE1 AI bit previously set)
        /// </summary>
        /// <param name="baseReg">PCA9685 Register number to start writing to</param>
        /// <param name="values">values read from sequential registers starting at baseReg</param>
        /// <returns>StatusResults(FT260_STATUS, FT260_CONTROLLER_STATUS)</returns>
        public UsbI2C.StatusResults Read(Reg reg, byte[] values)
        {
            UsbI2C.StatusResults statusResults;
            byte[] regValue = { (byte)reg };
            byte[] readBytes = new byte[values.Length];

            lock (_usbI2C)
            {
                if (!(statusResults = _usbI2C.Write(
                    _i2cAddress,
                    UsbI2C.FT260_I2C_FLAG.FT260_I2C_START,
                    regValue,
                    (uint)regValue.Length)).HasErrors())
                {
                    statusResults = _usbI2C.Read(
                        _i2cAddress,
                        UsbI2C.FT260_I2C_FLAG.FT260_I2C_REPEATED_START_AND_STOP,
                        values,
                        (uint)values.Length);
                }
            }

            return statusResults; ;
        }

        /// <summary>
        /// Write to PCA9685 register
        /// </summary>
        /// <param name="reg">PCA9685 Register number</param>
        /// <param name="value">value to write to register</param>
        /// <returns>StatusResults(FT260_STATUS, FT260_CONTROLLER_STATUS)</returns>
        public UsbI2C.StatusResults Write(Reg reg, byte value)
        {
            UsbI2C.StatusResults statusResults;
            byte[] regValue = { (byte)reg, value };

            lock (_usbI2C)
            {
                statusResults = _usbI2C.Write(
                    _i2cAddress,
                    UsbI2C.FT260_I2C_FLAG.FT260_I2C_START_AND_STOP,
                    regValue,
                    (uint)regValue.Length);
            }

            return statusResults;
        }

        /// <summary>
        /// Write to sequential PCA9685 registers (requires MODE1 AI bit previously set)
        /// </summary>
        /// <param name="baseReg">PCA9685 Register number to start writing to</param>
        /// <param name="values">values to write to sequential registers starting at baseReg</param>
        /// <returns>StatusResults(FT260_STATUS, FT260_CONTROLLER_STATUS)</returns>
        public UsbI2C.StatusResults Write(Reg baseReg, byte[] values)
        {
            UsbI2C.StatusResults statusResults;
            byte[] regValues = new byte[values.Length + 1];

            regValues[0] = (byte) baseReg;
            values.CopyTo(regValues, 1);

            lock (_usbI2C)
            {
                statusResults = _usbI2C.Write(
                    _i2cAddress,
                    UsbI2C.FT260_I2C_FLAG.FT260_I2C_START_AND_STOP,
                    regValues,
                    (uint)regValues.Length);
            }

            return statusResults;
        }
    }
}
