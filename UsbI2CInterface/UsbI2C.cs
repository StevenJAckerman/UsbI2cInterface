//////////////////////////////////////////////////////////////////////////
// UsbI2C.cs
//////////////////////////////////////////////////////////////////////////
// USB I2C Interface Class Library (wrapper for FTDI LibFT260.dll)
//////////////////////////////////////////////////////////////////////////
// written by:  Steven J. Ackerman, Consultant
//              ACS, Sarasota, Florida
//              mailto:steve@acscontrol.com
//              http://www.acscontrol.com 
//////////////////////////////////////////////////////////////////////////

//////////////////////////////////////////////////////////////////////////
// NOTE: When using .NET 4.0 or later the following entry is required
// in app.config:
//
//     <runtime>
//        <NetFx40_PInvokeStackResilience enabled = "1" />
//    </ runtime >
//
// This is due to a mismatch between what is declared in the LibFT260.dll
// exports and what is actually required - no response from FTDI on this
// anamoly to date.
//////////////////////////////////////////////////////////////////////////

using System;
using System.Runtime.InteropServices;

//
// FTDI FT260Q USB HID I2C Interface:
//
// http://www.ftdichip.com/Products/ICs/FT260.html 
//
namespace UsbI2CInterface
{
    public class UsbI2C : IDisposable
    {
        private uint numDevs = 0;

        private readonly ushort ft260VID = 0x0403;
        private readonly ushort ft260PID = 0x6030;
        private readonly uint ft260I2C_DEVICE = 0;

        private IntPtr ft260Handle = IntPtr.Zero;

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                }

                // free unmanaged resources (unmanaged objects) and override a finalizer below.
                if (ft260Handle != IntPtr.Zero)
                {
                    FT260_Close(ft260Handle);
                    ft260Handle = IntPtr.Zero;
                }

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        ~UsbI2C()
        {
           // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
           Dispose(false);
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            GC.SuppressFinalize(this);
        }
        #endregion

        public enum FT260_STATUS
        {
            FT260_OK,
            FT260_INVALID_HANDLE,
            FT260_DEVICE_NOT_FOUND,
            FT260_DEVICE_NOT_OPENED,
            FT260_DEVICE_OPEN_FAIL,
            FT260_DEVICE_CLOSE_FAIL,
            FT260_INCORRECT_INTERFACE,
            FT260_INCORRECT_CHIP_MODE,
            FT260_DEVICE_MANAGER_ERROR,
            FT260_IO_ERROR,
            FT260_INVALID_PARAMETER,
            FT260_NULL_BUFFER_POINTER,
            FT260_BUFFER_SIZE_ERROR,
            FT260_UART_SET_FAIL,
            FT260_RX_NO_DATA,
            FT260_GPIO_WRONG_DIRECTION,
            FT260_INVALID_DEVICE,
            FT260_OTHER_ERROR
        }

        public enum FT260_I2C_FLAG : byte
        {
            FT260_I2C_NONE = 0,
            FT260_I2C_START = 0x02,
            FT260_I2C_REPEATED_START = 0x03,
            FT260_I2C_STOP = 0x04,
            FT260_I2C_START_AND_STOP = 0x06,
            FT260_I2C_REPEATED_START_AND_STOP = 0x07
        }

        [Flags]
        public enum FT260_CONTROLLER_STATUS : byte
        {
            unknown = 0x00,
            busy = 0x01,
            error = 0x02,
            address_nack = 0x04,
            data_nack = 0x08,
            arbitration_lost = 0x10,
            idle = 0x20,
            bus_busy = 0x40
        }

        public class StatusResults
        {
            private readonly FT260_STATUS _hidStatus;
            public FT260_STATUS HidStatus
            {
                get { return _hidStatus; }
            }

            private readonly FT260_CONTROLLER_STATUS _controllerStatus;
            public FT260_CONTROLLER_STATUS ControllerStatus
            {
                get { return _controllerStatus; }
            }

            public StatusResults()
            {
                _hidStatus = FT260_STATUS.FT260_OK;
                _controllerStatus = FT260_CONTROLLER_STATUS.unknown;
            }

            public StatusResults(FT260_STATUS status, FT260_CONTROLLER_STATUS controllerStatus) : this()
            {
                _hidStatus = status;
                _controllerStatus = controllerStatus;
            }

            public bool HasErrors()
            {
                return HidStatus != FT260_STATUS.FT260_OK 
                    || ControllerStatus.HasFlag(FT260_CONTROLLER_STATUS.error);
            }
        }

// >dumpbin /exports LibFT260.dll
// Microsoft (R) COFF/PE Dumper Version 14.00.23918.0
// Copyright(C) Microsoft Corporation.All rights reserved.

// Dump of file LibFT260.dll

// File Type: DLL

// Section contains the following exports for LibFT260.dll

//    00000000 characteristics
//    56EA04B5 time date stamp Wed Mar 16 21:13:25 2016
//        0.00 version
//           1 ordinal base
//          47 number of functions
//          47 number of names

//   ordinal hint RVA      name

//          1    0 0000E760 _FT260_CleanInterruptFlag@8
//          2    1 0000EA40 _FT260_Close@4
//          3    2 0000DFD0 _FT260_CreateDeviceList@4
//          4    3 0000ECC0 _FT260_EnableDcdRiPin@8
//          5    4 0000EC80 _FT260_EnableI2CPin@8
//          6    5 00010C30 _FT260_GPIO_Get@8
//          7    6 00010E00 _FT260_GPIO_Read@12
//          8    7 00010B60 _FT260_GPIO_Set@12
//          9    8 00010D00 _FT260_GPIO_SetDir@12
//         10    9 00010F50 _FT260_GPIO_Write@12
//         11    A 0000E5C0 _FT260_GetChipVersion@8
//         12    B 0000C340 _FT260_GetDevicePath@12
//         13    C 0000E6D0 _FT260_GetInterruptFlag@8
//         14    D 0000B460 _FT260_GetLibVersion@4
//         15    E 0000F8B0 _FT260_I2CMaster_GetStatus@8
//         16    F 0000F240 _FT260_I2CMaster_Init@8
//         17   10 0000F3D0 _FT260_I2CMaster_Read@24
//         18   11 0000F990 _FT260_I2CMaster_Reset@4
//         19   12 0000F5C0 _FT260_I2CMaster_Write@24
//         20   13 0000E270 _FT260_Open@8
//         21   14 0000E510 _FT260_OpenByDevicePath@8
//         22   15 0000E380 _FT260_OpenByVidPid@16
//         23   16 0000EB00 _FT260_SelectGpio2Function@8
//         24   17 0000EB40 _FT260_SelectGpioAFunction@8
//         25   18 0000EB80 _FT260_SelectGpioGFunction@8
//         26   19 0000EA70 _FT260_SetClock@8
//         27   1A 0000EAC0 _FT260_SetInterruptTriggerType@12
//         28   1B 0000EC40 _FT260_SetParam_U16@12
//         29   1C 0000EBF0 _FT260_SetParam_U8@12
//         30   1D 0000EBC0 _FT260_SetSuspendOutPolarity@8
//         31   1E 0000ECA0 _FT260_SetUartToGPIOPin@4
//         32   1F 0000EAA0 _FT260_SetWakeupInterrupt@8
//         33   20 00010B10 _FT260_UART_EnableRiWakeup@8
//         34   21 00010230 _FT260_UART_GetConfig@8
//         35   22 00010890 _FT260_UART_GetDcdRiStatus@8
//         36   23 00010310 _FT260_UART_GetQueueStatus@8
//         37   24 000100B0 _FT260_UART_Init@4
//         38   25 000103A0 _FT260_UART_Read@20
//         39   26 00010AF0 _FT260_UART_Reset@4
//         40   27 00010580 _FT260_UART_SetBaudRate@8
//         41   28 00010AD0 _FT260_UART_SetBreakOff@4
//         42   29 00010AB0 _FT260_UART_SetBreakOn@4
//         43   2A 00010670 _FT260_UART_SetDataCharacteristics@16
//         44   2B 00010A80 _FT260_UART_SetFlowControl@8
//         45   2C 00010B30 _FT260_UART_SetRiWakeupConfig@8
//         46   2D 000107B0 _FT260_UART_SetXonXoffChar@12
//         47   2E 00010470 _FT260_UART_Write@20


// Summary

//        2000 .data
//        8000 .rdata
//        3000 .reloc
//        1000 .rsrc
//       21000 .text
//        1000 .tls

        const string dllLocation = "LibFT260.dll";

        [DllImport(dllLocation, EntryPoint = "_FT260_CreateDeviceList@4", CallingConvention = CallingConvention.Cdecl)]
        public static extern FT260_STATUS FT260_CreateDeviceList(out uint lpdwNumDevs);

        [DllImport(dllLocation, EntryPoint = "_FT260_OpenByVidPid@16", CallingConvention = CallingConvention.Cdecl)]
        public static extern FT260_STATUS FT260_OpenByVidPid(ushort vid, ushort pid, uint deviceIndex, out IntPtr pFt260Handle);

        [DllImport(dllLocation, EntryPoint = "_FT260_Close@4", CallingConvention = CallingConvention.Cdecl)]
        public static extern FT260_STATUS FT260_Close(IntPtr ft260Handle);

        [DllImport(dllLocation, EntryPoint = "_FT260_I2CMaster_Init@8", CallingConvention = CallingConvention.Cdecl)]
        public static extern FT260_STATUS FT260_I2CMaster_Init(IntPtr ft260Handle, uint kbps);

        [DllImport(dllLocation, EntryPoint = "_FT260_I2CMaster_Read@24", CallingConvention = CallingConvention.Cdecl)]
        public static extern FT260_STATUS FTFT260_I2CMaster_Read(IntPtr ft260Handle, byte deviceAddress, FT260_I2C_FLAG flag, IntPtr lpBuffer, uint dwBytesToRead, out uint lpdwBytesReturned);

        [DllImport(dllLocation, EntryPoint = "_FT260_I2CMaster_Write@24", CallingConvention = CallingConvention.Cdecl)]
        public static extern FT260_STATUS FT260_I2CMaster_Write(IntPtr ft260Handle, byte deviceAddress, FT260_I2C_FLAG flag, IntPtr lpBuffer, uint dwBytesToWrite, out uint lpdwBytesWritten);

        [DllImport(dllLocation, EntryPoint = "_FT260_I2CMaster_GetStatus@8", CallingConvention = CallingConvention.Cdecl)]
        public static extern FT260_STATUS FT260_I2CMaster_GetStatus(IntPtr ft260Handle, out byte status);

        [DllImport(dllLocation, EntryPoint = "_FT260_I2CMaster_Reset@4", CallingConvention = CallingConvention.Cdecl)]
        public static extern FT260_STATUS FT260_I2CMaster_Reset(IntPtr ft260Handle);

        /// <summary>
        /// Opens FT260Q using the default VID, PID
        /// </summary>
        /// <returns>StatusResults(FT260_STATUS, FT260_CONTROLLER_STATUS.idle)</returns>
        public StatusResults Open()
        {
            var ftStatus = FT260_STATUS.FT260_OTHER_ERROR;

            if (numDevs == 0)
            {
                ftStatus = FT260_CreateDeviceList(out numDevs);
            }

            ftStatus = FT260_OpenByVidPid(ft260VID, ft260PID, ft260I2C_DEVICE, out ft260Handle);

            return new StatusResults(ftStatus, FT260_CONTROLLER_STATUS.idle);
        }

        /// <summary>
        /// Answers whether Open() has been performed
        /// </summary>
        /// <returns>true = Open() has been called, false - no</returns>
        public bool IsOpen()
        {
            return ft260Handle != IntPtr.Zero;
        }

        /// <summary>
        /// Closes the FT260Q
        /// </summary>
        /// <returns>StatusResults(FT260_STATUS, FT260_CONTROLLER_STATUS.idle)</returns>
        public StatusResults Close()
        {
            var ftStatus = FT260_STATUS.FT260_OTHER_ERROR;

            ftStatus = FT260_Close(ft260Handle);

            ft260Handle = IntPtr.Zero;

            return new StatusResults(ftStatus, FT260_CONTROLLER_STATUS.idle);
        }

        /// <summary>
        /// Initializes the opened FT160Q as an I2C Master at the provided bit rate
        /// </summary>
        /// <param name="kbps"></param>
        /// <returns>StatusResults(FT260_STATUS, FT260_CONTROLLER_STATUS.idle)</returns>
        public StatusResults Init(uint kbps)
        {
            var ftStatus = FT260_STATUS.FT260_OTHER_ERROR;

            ftStatus = FT260_I2CMaster_Init(ft260Handle, kbps);

            return new StatusResults(ftStatus, FT260_CONTROLLER_STATUS.idle);
        }

        /// <summary>
        /// Reads bytes from I2C device into provided buffer
        /// </summary>
        /// <param name="deviceAddress">7-bit address of the I2C device</param>
        /// <param name="flag">FT260_I2C_FLAG</param>
        /// <param name="buffer">byte array to read into</param>
        /// <param name="bytesToRead">number of bytes to read into the buffer</param>
        /// <returns>StatusResults(FT260_STATUS, FT260_CONTROLLER_STATUS)</returns>
        public StatusResults Read(byte deviceAddress, FT260_I2C_FLAG flag, byte[] buffer, uint bytesToRead)
        {
            var ftStatus = FT260_STATUS.FT260_OTHER_ERROR;
            uint bytesRead = 0;
            var status = FT260_CONTROLLER_STATUS.idle;

            var size = (int) (Marshal.SizeOf(buffer[0])*bytesToRead);
            var pntr = Marshal.AllocHGlobal(size);

            if ((ftStatus = FTFT260_I2CMaster_Read(ft260Handle, deviceAddress, flag, pntr, bytesToRead, out bytesRead)) ==
                FT260_STATUS.FT260_OK)
            {
                try
                {
                    Marshal.Copy(pntr, buffer, 0, buffer.Length);
                }
                finally
                {
                    //Marshal.FreeHGlobal(pntr);
                }

                if (bytesRead != bytesToRead)
                {
                    ftStatus = FT260_STATUS.FT260_OTHER_ERROR;
                }
            }

            byte controllerStatus = 0;
            if (FT260_I2CMaster_GetStatus(ft260Handle, out controllerStatus) == FT260_STATUS.FT260_OK)
            {
                status = (FT260_CONTROLLER_STATUS)controllerStatus;
            }

            Marshal.FreeHGlobal(pntr);
            return new StatusResults(ftStatus, status);
        }

        /// <summary>
        /// Writes bytes to I2C device from provided buffer
        /// </summary>
        /// <param name="deviceAddress">7-bit address of the I2C device</param>
        /// <param name="flag">FT260_I2C_FLAG</param>
        /// <param name="buffer">byte array to write from</param>
        /// <param name="bytesToWrite">number of bytes to write from the buffer</param>
        /// <returns>StatusResults(FT260_STATUS, FT260_CONTROLLER_STATUS)</returns>
        public StatusResults Write(byte deviceAddress, FT260_I2C_FLAG flag, byte[] buffer, uint bytesToWrite)
        {
            var ftStatus = FT260_STATUS.FT260_OTHER_ERROR;
            uint bytesWritten = 0;
            var status = FT260_CONTROLLER_STATUS.idle;

            var size = (int) (Marshal.SizeOf(buffer[0]) * bytesToWrite);
            var pntr = Marshal.AllocHGlobal(size);
            try
            {
                Marshal.Copy(buffer, 0, pntr, (int)bytesToWrite);
            }
            finally
            {
                //Marshal.FreeHGlobal(pntr);
            }

            if (
                (ftStatus =
                    FT260_I2CMaster_Write(ft260Handle, deviceAddress, flag, pntr, bytesToWrite, out bytesWritten)) == FT260_STATUS.FT260_OK)
            {
                if (bytesWritten != bytesToWrite)
                {
                    ftStatus = FT260_STATUS.FT260_OTHER_ERROR;
                }
            }

            byte controllerStatus = 0;
            if (FT260_I2CMaster_GetStatus(ft260Handle, out controllerStatus) == FT260_STATUS.FT260_OK)
            {
                status = (FT260_CONTROLLER_STATUS) controllerStatus;
            }

            Marshal.FreeHGlobal(pntr);
            return new StatusResults(ftStatus, status);
        }

        /// <summary>
        /// Read the status of the FT260Q I2C controller
        /// </summary>
        /// <returns>StatusResults(FT260_STATUS, FT260_CONTROLLER_STATUS)</returns>
        public StatusResults GetStatus()
        {
            var ftStatus = FT260_STATUS.FT260_OTHER_ERROR;
            byte status = 0;
            ftStatus = FT260_I2CMaster_GetStatus(ft260Handle, out status);

            return new StatusResults(ftStatus, (FT260_CONTROLLER_STATUS)status);
        }

        /// <summary>
        /// Reset the FT260Q I2C controller
        /// </summary>
        /// <returns>StatusResults(FT260_STATUS, FT260_CONTROLLER_STATUS)</returns>
        public StatusResults Reset()
        {
            var ftStatus = FT260_STATUS.FT260_OTHER_ERROR;
            var status = FT260_CONTROLLER_STATUS.idle;
            ftStatus = FT260_I2CMaster_Reset(ft260Handle);

            byte controllerStatus = 0;
            if ((ftStatus = FT260_I2CMaster_GetStatus(ft260Handle, out controllerStatus)) == FT260_STATUS.FT260_OK)
            {
                status = (FT260_CONTROLLER_STATUS)controllerStatus;
            }

            return new StatusResults(ftStatus, status);
        }
    }
}
