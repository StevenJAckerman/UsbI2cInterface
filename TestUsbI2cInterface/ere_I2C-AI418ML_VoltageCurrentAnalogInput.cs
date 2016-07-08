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
        private readonly byte _configurationJumpers;
        private readonly MCP3424 _mcp3424;

        /// <summary>
        /// Construct an instance of ere_I2C_AI418ML using opened UsbI2C and I2C address
        /// </summary>
        /// <param name="usbI2C">Open()'ed and Init()'ed UsbI2C</param>
        /// <param name="jumpers">I2C address</param>
        public ere_I2C_AI418ML(UsbI2C usbI2C, byte jumpers)
        {
            _usbI2C = usbI2C;
            _configurationJumpers = (byte)(jumpers & 0x07);
            _mcp3424 = new MCP3424(_usbI2C, _configurationJumpers);
        }

        /// <summary>
        /// Get a raw ADC reading
        /// </summary>
        /// <param name="channel">MCP3424.AdcChannel</param>
        /// <param name="size">MCP3424.AdcSize</param>
        /// <param name="pga">MCP3424.AdcPga</param>
        /// <param name="reading">value of ADC channel read at size & gain</param>
        /// <returns>StatusResults(FT260_STATUS, FT260_CONTROLLER_STATUS)</returns>
        public UsbI2C.StatusResults ReadAdc(MCP3424.AdcChannel channel, MCP3424.AdcSize size, MCP3424.AdcPga pga,
            out uint reading)
        {
            return _mcp3424.ReadAdc(channel, size, pga, out reading);
        }

        /// <summary>
        /// Get an ADC voltage reading (requires voltage connection to I2C-AI418ML)
        /// </summary>
        /// <param name="channel">MCP3424.AdcChannel</param>
        /// <param name="size">MCP3424.AdcSize</param>
        /// <param name="pga">MCP3424.AdcPga</param>
        /// <param name="voltage">voltage of ADC channel read at size & gain</param>
        /// <returns>StatusResults(FT260_STATUS, FT260_CONTROLLER_STATUS)</returns>
        public UsbI2C.StatusResults ReadVoltage(MCP3424.AdcChannel channel, MCP3424.AdcSize size, MCP3424.AdcPga pga, out double voltage)
        {
            UsbI2C.StatusResults statusResults;
            voltage = 0.0;
            uint reading = 0;

            if ((statusResults = _mcp3424.ReadAdc(channel, size, pga, out reading)).HasErrors())
            {
                return statusResults;
            }

            voltage = (((reading * MCP3424.VRef) / (33 * _mcp3424.Gains[(int)pga])) * 180) /
                _mcp3424.MaxReadings[(int) size];

            return statusResults;
        }

        /// <summary>
        /// Get an ADC current reading (requires current connection to I2C-AI418ML)
        /// </summary>
        /// <param name="channel">MCP3424.AdcChannel</param>
        /// <param name="size">MCP3424.AdcSize</param>
        /// <param name="pga">MCP3424.AdcPga</param>
        /// <param name="current">current of ADC channel read at size & gain (voltage / 249R)</param>
        /// <returns>StatusResults(FT260_STATUS, FT260_CONTROLLER_STATUS)</returns>
        public UsbI2C.StatusResults ReadCurrent(MCP3424.AdcChannel channel, MCP3424.AdcSize size, MCP3424.AdcPga pga, out double current)
        {
            UsbI2C.StatusResults statusResults;
            double voltage = 0.0;
            current = 0.0;

            if ((statusResults = ReadVoltage(channel, size, pga, out voltage)).HasErrors())
            {
                return statusResults;
            }

            current = voltage  /249;

            return statusResults;
        }
    }
}
