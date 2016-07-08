//////////////////////////////////////////////////////////////////////////
// MCP3424.cs
//////////////////////////////////////////////////////////////////////////
// Interface to Microship MCP3424 via I2C bus using U2bI2CInterface wrapper 
// and ACS USB I2C Interface module.
//////////////////////////////////////////////////////////////////////////
// written by:  Steven J. Ackerman, Consultant
//              ACS, Sarasota, Florida
//              mailto:steve@acscontrol.com
//              http://www.acscontrol.com 
//////////////////////////////////////////////////////////////////////////

using UsbI2CInterface;

//
// Microchip MCP3424 18-bit multichannel ADC w/ref and I2C
//
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TestUsbI2cInterface
{
    public class MCP3424
    {
        private readonly UsbI2C _usbI2C;

        private readonly byte _i2cAddress;

        private readonly byte [] _configurableI2CAddresses =
        {
            0x68, 0x69, 0x6A, 0x6B, 0x6C, 0x6D, 0x6E, 0x6F
        };

        public enum AdcChannel : byte
        {
            Channel0,
            Channel1,
            Channel2,
            Channel3
        };

        public enum AdcSize : byte
        {
            Size12Bit,
            Size14Bit,
            Size16Bit,
            Size18Bit
        };

        public enum AdcPga : byte
        {
            Gain1,
            Gain2,
            Gain4,
            Gain8
        };

        public const double VRef = 2.048;

        public readonly uint[] MaxReadings =
        {
            2047,
            8191,
            32767,
            131071
        };

        public readonly uint[] Gains = { 1, 2, 4, 8 };

        /// <summary>
        /// Construct an instance of MCP3424 using opened UsbI2C with specified address inputs
        /// </summary>
        /// <param name="usbI2C">Open()'ed and Init()'ed UsbI2C</param>
        /// <param name="addressInputs">PCA9685 address inputs</param>
        public MCP3424(UsbI2C usbI2C, byte addressInputs)
        {
            _usbI2C = usbI2C;
            _i2cAddress = _configurableI2CAddresses[addressInputs & 0x07];
        }

        /// <summary>
        /// Get a raw ADC reading
        /// </summary>
        /// <param name="channel">Channel0 - Channel3</param>
        /// <param name="size">Size12Bit, Size14Bit, Size16Bit, Size18Bit</param>
        /// <param name="pga">Gain1, Gain2, Gain4, Gain8</param>
        /// <param name="reading">value of ADC channel read at size & gain</param>
        /// <returns>StatusResults(FT260_STATUS, FT260_CONTROLLER_STATUS)</returns>
        public UsbI2C.StatusResults ReadAdc(AdcChannel channel, AdcSize size, AdcPga pga, out uint reading)
        {
            UsbI2C.StatusResults statusResults;
            reading = 0;

            byte[] control = { 0 };

            // start one-shot conversion on channel @ size and gain...

            control[0] = (byte)(0x80 | (((int)channel & 3) << 5 | (((int)size & 3) << 2) | ((int)pga & 3)));

            lock (_usbI2C)
            {
                statusResults = _usbI2C.Write(
                    _i2cAddress,
                    UsbI2C.FT260_I2C_FLAG.FT260_I2C_START_AND_STOP,
                    control,
                    (uint)control.Length);
            }

            if (statusResults.HasErrors())
            {
                return statusResults;
            }

            // length of result depends upon size of conversion...

            byte[] result;
            if (size == AdcSize.Size18Bit)
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
                        _i2cAddress,
                        UsbI2C.FT260_I2C_FLAG.FT260_I2C_START_AND_STOP,
                        result,
                        (uint)result.Length);
                }

                if (statusResults.HasErrors())
                {
                    return statusResults;
                }

                Thread.Sleep(1);
            } while ((result[result.Length - 1] & 0x80) == 0x00);

            // assemble result depending upon size of conversion...

            if (size == AdcSize.Size18Bit)
            {
                reading = ((uint)(result[0] & 3) << 16) | ((uint)result[1] << 8) | (uint)result[2];
            }
            else
            {
                reading = ((uint)result[0] << 8) | (uint)result[1];
            }

            return statusResults;
        }


    }
}
