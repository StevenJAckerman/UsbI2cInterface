//////////////////////////////////////////////////////////////////////////
// Adafruit16ChannelServo.cs
//////////////////////////////////////////////////////////////////////////
// Interface to adafruit 16 channel servo via I2C bus using U2bI2CInterface 
// wrapper and ACS USB I2C Interface module.
//////////////////////////////////////////////////////////////////////////
// written by:  Steven J. Ackerman, Consultant
//              ACS, Sarasota, Florida
//              mailto:steve@acscontrol.com
//              http://www.acscontrol.com 
//////////////////////////////////////////////////////////////////////////

using System.Threading;
using UsbI2CInterface;

//
// Adafruit 16-Channel 12-bit PWM/Servo Driver - I2C interface - PCA9685
//
// https://www.adafruit.com/products/815
//
namespace TestUsbI2cInterface
{
    public class Adafruit16ChannelServo
    {
        private readonly UsbI2C _usbI2C;
        private readonly PCA9685 _pca9685;

        private readonly PCA9685.Reg[] _servoBaseRegs =
        {
            PCA9685.Reg.LED0_ON_L,
            PCA9685.Reg.LED1_ON_L,
            PCA9685.Reg.LED2_ON_L,
            PCA9685.Reg.LED3_ON_L,
            PCA9685.Reg.LED4_ON_L,
            PCA9685.Reg.LED5_ON_L,
            PCA9685.Reg.LED6_ON_L,
            PCA9685.Reg.LED7_ON_L,
            PCA9685.Reg.LED8_ON_L,
            PCA9685.Reg.LED9_ON_L,
            PCA9685.Reg.LED10_ON_L,
            PCA9685.Reg.LED11_ON_L,
            PCA9685.Reg.LED12_ON_L,
            PCA9685.Reg.LED13_ON_L,
            PCA9685.Reg.LED14_ON_L,
            PCA9685.Reg.LED15_ON_L
        };

        /// <summary>
        /// Construct an instance of the Adafruit16ChannelServo using an opened UsbI2C using specified address
        /// </summary>
        /// <param name="usbI2C">Open()'ed and Init()'ed UsbI2C</param>
        /// <param name="addressInputs">PCA9685 address inputs</param>
        public Adafruit16ChannelServo(UsbI2C usbI2C, byte addressInputs)
        {
            this._usbI2C = usbI2C;
            _pca9685 = new PCA9685(usbI2C, addressInputs);
        }

        // write on/off period values to servo's pwm registers (requires PCA9685 MODE1 AI bit previously set)
        private UsbI2C.StatusResults setServoPWM(byte servo, ushort onPeriod, ushort offPeriod)
        {
            UsbI2C.StatusResults statusResults;
            byte[] pwmValues = {0, 0, 0, 0};

            pwmValues[0] = (byte) (onPeriod & 0x00ff);
            pwmValues[1] = (byte) (onPeriod >> 8);
            pwmValues[2] = (byte) (offPeriod & 0x00ff);
            pwmValues[3] = (byte) (offPeriod >> 8);

            statusResults = _pca9685.Write(_servoBaseRegs[servo & 0x0f], pwmValues);

            return statusResults;
        }

        /// <summary>
        /// Initialize the Adafruit16ChannelServo PCA9685 chip
        /// </summary>
        /// <returns>StatusResults(FT260_STATUS, FT260_CONTROLLER_STATUS)</returns>
        public UsbI2C.StatusResults Init()
        {
            UsbI2C.StatusResults statusResults;

            // init PCA9685...

            // go to sleep
            byte mode = 0x10;
            if ((statusResults = _pca9685.Write(PCA9685.Reg.MODE1, mode)).HasErrors())
            {
                return statusResults;
            }

            // outputs change on ACK
            byte mode2 = 0x0c;  // OCH = 1, OUTDRV = 1
            if ((statusResults = _pca9685.Write(PCA9685.Reg.MODE2, mode2)).HasErrors())
            {
                return statusResults;
            }

            // setup prescaler
            if ((statusResults = _pca9685.Write(PCA9685.Reg.PRE_SCALE, 0x6e)).HasErrors())
            {
                return statusResults;
            }

            // turn off sleep
            mode &= 0xef;
            if ((statusResults = _pca9685.Write(PCA9685.Reg.MODE1, mode)).HasErrors())
            {
                return statusResults;
            }

            Thread.Sleep(1);

            // turn on restart and auto address increment for later setServoPWM() writes
            mode |= 0xa0;
            if ((statusResults = _pca9685.Write(PCA9685.Reg.MODE1, mode)).HasErrors())
            {
                return statusResults;
            }

            return statusResults;
        }

        /// <summary>
        /// Sets the servo to the position
        /// </summary>
        /// <param name="servo">servo number 0 - 15</param>
        /// <param name="position">position 0 - 4095 (may by limited by physical servo)</param>
        /// <returns>StatusResults(FT260_STATUS, FT260_CONTROLLER_STATUS)</returns>
        public UsbI2C.StatusResults Position(byte servo, ushort position)
        {
            if (position > 4095) position = 4096;

            if (position == 4095)
            {
                return setServoPWM(servo, 4096, 0);
            }
            else if (position == 0)
            {
                return setServoPWM(servo, 0, 4096);
            }
            else
            {
                return setServoPWM(servo, 0, position);
            }
        }
    }
}
