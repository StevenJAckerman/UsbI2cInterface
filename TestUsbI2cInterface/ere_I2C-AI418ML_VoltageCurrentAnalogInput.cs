//////////////////////////////////////////////////////////////////////////
// ere_I2C-AI418ML_VoltageCurrentAnalogInput.cs
//////////////////////////////////////////////////////////////////////////
// Interface to ere I2C-AI418ML via I2C bus using U2bI2CInterface wrapper 
// and ACS USB I2C Interface module.
//////////////////////////////////////////////////////////////////////////
// written by:  Steven J. Ackerman, Consultant
//              ACS, Sarasota, Florida
//              mailto:steve@acscontrol.com
//              http://www.acscontrol.com 
//////////////////////////////////////////////////////////////////////////

using System.Threading;
using UsbI2CInterface;

//
// I2C 4-20ma & 0-10v ADC TVS - I2C-AI418ML
//
// https://www.ereshop.com/shop/index.php?main_page=product_info&cPath=143&products_id=826
//
namespace TestUsbI2cInterface
{
    public class ere_I2C_AI418ML
    {
        private readonly UsbI2C _usbI2C;
        private readonly byte _i2CAdc;

        public enum Channel : byte
        {
            Channel0,
            Channel1,
            Channel2,
            Channel3
        };

        public enum Size : byte
        {
            Size12Bit,
            Size14Bit,
            Size16Bit,
            Size18Bit
        };

        public enum Pga : byte
        {
            Gain1,
            Gain2,
            Gain4,
            Gain8
        };
        
        private readonly uint[] maxReadings =
        {
            2047,
            8191,
            32767,
            131071
        };

        private readonly uint[] gains = {1, 2, 4, 8};

        /// <summary>
        /// Construct an instance of ere_I2C_AI418ML using opened UsbI2C and I2C address
        /// </summary>
        /// <param name="usbI2C">Open()'ed and Init()'ed UsbI2C</param>
        /// <param name="i2CAddress">I2C address</param>
        public ere_I2C_AI418ML(UsbI2C usbI2C, byte i2CAddress)
        {
            _usbI2C = usbI2C;
            _i2CAdc = i2CAddress;
        }

        /// <summary>
        /// Get a raw ADC reading
        /// </summary>
        /// <param name="channel">Channel0 - Channel3</param>
        /// <param name="size">Size12Bit, Size14Bit, Size16Bit, Size18Bit</param>
        /// <param name="pga">Gain1, Gain2, Gain4, Gain8</param>
        /// <param name="reading">value of ADC channel read at size & gain</param>
        /// <returns>StatusResults(FT260_STATUS, FT260_CONTROLLER_STATUS)</returns>
        public UsbI2C.StatusResults ReadAdc(Channel channel, Size size, Pga pga, out uint reading)
        {
            UsbI2C.StatusResults statusResults;
            reading = 0;

            byte[] control = {0};

            // start one-shot conversion on channel @ size and gain...

            control[0] = (byte) (0x80 | (((int)channel & 3) << 5 | (((int)size & 3) << 2) | ((int)pga & 3)));

            lock (_usbI2C)
            {
                statusResults = _usbI2C.Write(
                    _i2CAdc,
                    UsbI2C.FT260_I2C_FLAG.FT260_I2C_START_AND_STOP,
                    control,
                    (uint) control.Length);
            }

            if (statusResults.HasErrors())
            {
                return statusResults;
            }

            // length of result depends upon size of conversion...

            byte[] result;
            if (size == Size.Size18Bit)
            {
                result = new byte[4];
            }
            else
            {
                result = new byte[3];
            }

            // continuously read until conversion complete (/RDY bit = 1)...

            do
            {
                lock (_usbI2C)
                {
                    statusResults = _usbI2C.Read(
                        _i2CAdc,
                        UsbI2C.FT260_I2C_FLAG.FT260_I2C_START_AND_STOP,
                        result,
                        (uint) result.Length);
                }

                if (statusResults.HasErrors())
                {
                    return statusResults;
                }

                Thread.Sleep(1);
            } while ((result[result.Length - 1] & 0x80) == 0x00);

            // assemble result depending upon size of conversion...

            if (size == Size.Size18Bit)
            {
                reading = ((uint)(result[0] & 3) << 16) | ((uint)result[1] << 8) | (uint)result[2];
            }
            else
            {
                reading = ((uint)result[0] << 8) | (uint)result[1];
            }

            return statusResults;
        }

        /// <summary>
        /// Get an ADC voltage reading (requires voltage connection to I2C-AI418ML)
        /// </summary>
        /// <param name="channel">Channel0 - Channel3</param>
        /// <param name="size">Size12Bit, Size14Bit, Size16Bit, Size18Bit</param>
        /// <param name="pga">Gain1, Gain2, Gain4, Gain8</param>
        /// <param name="voltage">voltage of ADC channel read at size & gain</param>
        /// <returns>StatusResults(FT260_STATUS, FT260_CONTROLLER_STATUS)</returns>
        public UsbI2C.StatusResults ReadVoltage(Channel channel, Size size, Pga pga, out double voltage)
        {
            UsbI2C.StatusResults statusResults;
            voltage = 0.0;
            uint reading = 0;

            if ((statusResults = ReadAdc(channel, size, pga, out reading)).HasErrors())
            {
                return statusResults;
            }

            voltage = (((reading * 2.048) / (33 * gains[(int)pga])) * 180) /
                maxReadings[(int) size];

            return statusResults;
        }

        /// <summary>
        /// Get an ADC current reading (requires current connection to I2C-AI418ML)
        /// </summary>
        /// <param name="channel">Channel0 - Channel3</param>
        /// <param name="size">Size12Bit, Size14Bit, Size16Bit, Size18Bit</param>
        /// <param name="pga">Gain1, Gain2, Gain4, Gain8</param>
        /// <param name="current">current of ADC channel read at size & gain (voltage / 249R)</param>
        /// <returns>StatusResults(FT260_STATUS, FT260_CONTROLLER_STATUS)</returns>
        public UsbI2C.StatusResults ReadCurrent(Channel channel, Size size, Pga pga, out double current)
        {
            UsbI2C.StatusResults statusResults;
            double voltage = 0.0;
            current = 0.0;

            if ((statusResults = ReadVoltage(channel, size, pga, out voltage)).HasErrors())
            {
                return statusResults;
            }

            current = voltage/249;

            return statusResults;
        }
    }
}
