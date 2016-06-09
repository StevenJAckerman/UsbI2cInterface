//////////////////////////////////////////////////////////////////////////
// PCF8574.cs
//////////////////////////////////////////////////////////////////////////
// Interface to Nxp PCF8574 via I2C bus using U2bI2CInterface wrapper and
// ACS USB I2C Interface module.
//////////////////////////////////////////////////////////////////////////
// written by:  Steven J. Ackerman, Consultant
//              ACS, Sarasota, Florida
//              mailto:steve@acscontrol.com
//              http://www.acscontrol.com 
//////////////////////////////////////////////////////////////////////////

using UsbI2CInterface;

//
// NXP PCF8574 I2C 8-bit I/O Exander
//
namespace TestUsbI2cInterface
{
    public class PCF8574
    {
        private readonly UsbI2C _usbI2C;

        private readonly byte _i2cAddress;

        private readonly byte[] _configurableI2cAddresses =
        {
            0x20, 0x21, 0x22, 0x23, 0x24, 0x25, 0x26, 0x27
        };

        /// <summary>
        /// Construct an instance of PCF8574 using an opened UsbI2C with specified address inputs
        /// </summary>
        /// <param name="usbI2C">Open()'ed and Init()'ed UsbI2C</param>
        /// <param name="addressInputs">PCA9685 address inputs</param>
        public PCF8574(UsbI2C usbI2C, byte addressInputs = 0)
        {
            _usbI2C = usbI2C;
            _i2cAddress = _configurableI2cAddresses[addressInputs & 0x07];
        }

        /// <summary>
        /// Read from PCF8574 port pins
        /// </summary>
        /// <param name="pins">value read from port pins</param>
        /// <returns>StatusResults(FT260_STATUS, FT260_CONTROLLER_STATUS)</returns>
        public UsbI2C.StatusResults Read(out byte pins)
        {
            UsbI2C.StatusResults statusResults;

            byte[] result = new byte[1];

            lock (_usbI2C)
            {
                statusResults = _usbI2C.Read(_i2cAddress,
                    UsbI2C.FT260_I2C_FLAG.FT260_I2C_START_AND_STOP,
                    result,
                    (uint) result.Length);
            }

            pins = result[0];

            return statusResults;
        }

        /// <summary>
        /// Write to PCF8574 port pins
        /// </summary>
        /// <param name="pins">value to write to port pins</param>
        /// <returns>StatusResults(FT260_STATUS, FT260_CONTROLLER_STATUS)</returns>
        public UsbI2C.StatusResults Write(byte pins)
        {
            UsbI2C.StatusResults statusResults;

            byte[] buffer = new byte[] {pins};

            lock (_usbI2C)
            {
                statusResults = _usbI2C.Write(
                    _i2cAddress,
                    UsbI2C.FT260_I2C_FLAG.FT260_I2C_START_AND_STOP,
                    buffer,
                    (uint) buffer.Length);
            }

            return statusResults;
        }
    }
}
