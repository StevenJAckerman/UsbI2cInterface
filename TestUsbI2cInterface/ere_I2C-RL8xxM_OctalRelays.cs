//////////////////////////////////////////////////////////////////////////
// ere_I2C-RL8xxM_OctalRelays.cs
//////////////////////////////////////////////////////////////////////////
// Interface to ere I2C-RL8xxM via I2C bus using U2bI2CInterface wrapper 
// and ACS USB I2C Interface module.
//////////////////////////////////////////////////////////////////////////
// written by:  Steven J. Ackerman, Consultant
//              ACS, Sarasota, Florida
//              mailto:steve@acscontrol.com
//              http://www.acscontrol.com 
//////////////////////////////////////////////////////////////////////////

using System;
using UsbI2CInterface;

//
// I2C Relay 12V 10A PCF8574 - I2C-RL812M
//
// https://www.ereshop.com/shop/index.php?main_page=product_info&cPath=143&products_id=739
//
namespace TestUsbI2cInterface
{
    public class ere_I2C_RL8xxM
    {
        private readonly UsbI2C _usbI2C;
        private readonly byte _configurationJumpers;
        private readonly PCF8574 _pcf8574;

        [Flags]
        public enum Relay : byte
        {
            K1 = 0x01,
            K2 = 0x02,
            K3 = 0x04,
            K4 = 0x08,
            K5 = 0x10,
            K6 = 0x20,
            K7 = 0x40,
            K8 = 0x80,
            ALL = 0xff
        }

        /// <summary>
        /// Construct an instance of ere_I2C_RL8xxM using an opened usbI2C and jumpers
        /// </summary>
        /// <param name="usbI2C">Open()'ed and Init()'ed UsbI2C</param>
        /// <param name="jumpers">jumper inputs</param>
        public ere_I2C_RL8xxM(UsbI2C usbI2C, byte jumpers)
        {
            _usbI2C = usbI2C;
            _configurationJumpers = (byte) (jumpers & 0x07);
            _pcf8574 = new PCF8574(_usbI2C, _configurationJumpers);
        }

        /// <summary>
        /// turn on one or more relays
        /// </summary>
        /// <param name="relays">K1, - ,ALL</param>
        /// <returns>StatusResults(FT260_STATUS, FT260_CONTROLLER_STATUS)</returns>
        public UsbI2C.StatusResults On(Relay relays)
        {
            UsbI2C.StatusResults statusResults;

            byte newPins = (byte) relays;

            byte currentPins;

            statusResults = _pcf8574.Read(out currentPins);

            currentPins = (byte) ~currentPins;
            currentPins |= (byte) newPins;
            currentPins = (byte)~currentPins;

            statusResults = _pcf8574.Write((byte) currentPins);

            return statusResults;
        }

        /// <summary>
        /// turn off one or more relays
        /// </summary>
        /// <param name="relays">K1, - ,ALL</param>
        /// <returns>StatusResults(FT260_STATUS, FT260_CONTROLLER_STATUS)</returns>
        public UsbI2C.StatusResults Off(Relay relays)
        {
            UsbI2C.StatusResults statusResults;

            byte newPins = (byte)relays;

            byte currentPins;

            statusResults = _pcf8574.Read(out currentPins);

            currentPins = (byte)~currentPins;
            currentPins &= (byte) ~newPins;
            currentPins = (byte)~currentPins;

            statusResults = _pcf8574.Write((byte)currentPins);

            return statusResults;
        }

        /// <summary>
        /// obtain the current status of the relays
        /// </summary>
        /// <param name="relays">K1, - , ALL</param>
        /// <returns>StatusResults(FT260_STATUS, FT260_CONTROLLER_STATUS)</returns>
        public UsbI2C.StatusResults Status(out Relay relays)
        {
            UsbI2C.StatusResults statusResults;

            byte currentPins;

            statusResults = _pcf8574.Read(out currentPins);

            currentPins = (byte)~currentPins;
            relays = (Relay) currentPins;

            return statusResults;;
        }
    }
}
