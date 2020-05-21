using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace SignalLib
{
    internal class DASP52032Connector
    {
        // Fundamental System Function

        [DllImport("DASP52032r.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern int DASP52032_QuickInstalled(byte _id, byte _res);

        [DllImport("DASP52032r.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern int DASP52032_Release(byte _id);

        [DllImport("DASP52032r.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern int DASP52032_GetDeviceStatus(byte _id, ref byte _sts);

        [DllImport("DASP52032r.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern int DASP52032_GetDeviceList(Int16 _addrs, ref byte _id, ref byte _sts);

        [DllImport("DASP52032r.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern int DASP52032_GetPciDeviceList(ref byte _id, ref int _cardSN, ref byte _sts);

        [DllImport("DASP52032r.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern int DASP52032_GetDllVersion(ref int _ver);

        // Digital I/O Function

        [DllImport("DASP52032r.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern int DASP52032_ReadGpio(byte _id, ref Int16 _data);

        [DllImport("DASP52032r.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern int DASP52032_WriteGpio(byte _id, Int16 _data);

        [DllImport("DASP52032r.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern int DASP52032_ReadBackGpio(byte _id, ref Int16 _data);

        public static string GetError(int _err)
        {
            int ErrorCode = _err & 0x0FF;
            if (ErrorCode == 0)
                return (null);
            int SystemID = (_err >> 28) & 0x0F;
            int AuxCode = (_err >> 16) & 0x0FFF;
            int TargetID = (_err >> 8) & 0x0FF;
            Dictionary<int, string> DSystemID = new Dictionary<int, string>()
            {
                {0x01,"I/O Board"},       
                {0x02,"Remote Module"},
                {0x03,"SoftPLC"},  
                {0x04,"Motion"},   
                {0x05,"PID Loop"},
                {0x06,"Network"},   
                {0x07,"Database"}, 
                {0x08,"DAQ"},
                {0x09,"GPE"}
            };
            Dictionary<int, string> DTargetID = new Dictionary<int, string>()
            {
                {0x01,"DASA-51040"},
                {0x02,"DASA-51080"},
                {0x03,"DASA-51062"},
                {0x04,"DASA-51016"},
                {0x05,"DASA-51048"}, 
                {0x06,"DASA-51010"},  
                {0x07,"DASA-51063"}, 
                {0x08,"DASP-52064"}, 
                {0x09,"DASP-52048"}, 
                {0x0A,"DASP-52010"},  
                {0x0B,"DASP-52056"}, 
                {0x0C,"DASA-51016"}, 
                {0x0D,"DASP-52063"}, 
                {0x0E,"DASP-52032"}, 
                {0x0F,"DASP-52096"},     
                {0x11,"DASA-51184"}, 
                {0x12,"DASA-51180"}, 
                {0x13,"DASA-51104"}, 
                {0x14,"DASA-51185"}, 
                {0x17,"DASP-52104"},  
                {0x18,"DASP-52180"}, 
                {0x21,"DASA-51280"}, 
                {0x22,"DASA-51281"}, 
                {0x23,"DASA-51282"}, 
                {0x24,"DASP-52282"},   
                {0x31,"DASA-51308"}, 
                {0x32,"DASA-51309"}, 
                {0x51,"DASA-51502"}, 
                {0x52,"DASA-51504"}, 
                {0x53,"DASA-51505"},  
                {0x54,"DASA-51506"}, 
                {0x55,"DASP-52504"} 
            };
            Dictionary<int, string> DErrorCode = new Dictionary<int, string>()
            {
                {0x01,"InvalidOpenBaseIo"}, 
                {0x02,"InvalidCloseBaseIo"}, 
                {0x03,"InvalidBoardHandle"}, 
                {0x04,"InvalidBoardID"},
                {0x05,"InvalidGetDeviceList"},
                {0x06,"InvalidBoardType"}, 
                {0x07,"InvalidBaseAddress"}, 
                {0x08,"InvalidFunctionCode"}, 
                {0x09,"InvalidSramSpace"}, 
                {0x0A,"InvalidPortNumber"}, 
                {0x0B,"InvalidIoIntChannel"}, 
                {0x0C,"InvalidReadIoPort"}, 
                {0x0D,"InvalidWriteIoPort"}, 
                {0x0E,"InvalidIoController"}, 
                {0x0F,"InvalidValue"},
                {0x10,"InvalidBoardStatus"},
                {0x11,"InvalidIRQNumber"}, 
                {0x12,"InvalidInitInterrupt"}, 
                {0x13,"InvalidCloseInterrupt"}, 
                {0x14,"InvalidReadEeprom"}, 
                {0x15,"InvalidWriteEeprom"}, 
                {0x16,"InvalidInitSram"}, 
                {0x17,"InvalidCloseSram"}, 
                {0x21,"InvalidCtrlWord"}, 
                {0x22,"InvalidADGain"}, 
                {0x23,"InvalidADMode"}, 
                {0x24,"InvalidADChannel"}, 
                {0x25,"InvalidADCalibrate"}, 
                {0x26,"InvalidAIScale"}, 
                {0x27,"InvalidFilterRatio"}, 
                {0x28,"InvalidTCType"}, 
                {0x29,"InvalidADConverter"}, 
                {0x2A,"InvalidCBArrayHandle"}, 
                {0x2B,"InvalidCBArrayIndex"}, 
                {0x2C,"InvalidCBArrayLength"}, 
                {0x31,"InvalidDAChannel"}, 
                {0x32,"InvalidAOScale"}, 
                {0x41,"InvalidPWMChannel"}, 
                {0x42,"InvalidHSCChannel"}, 
                {0x43,"InvalidHSCMode"}, 
                {0x44,"InvalidHSCGainRes"}, 
                {0x45,"InvalidHSCDataFormat"}, 
                {0x51,"InvalidTimerSetting"}, 
                {0x52,"InvalidPICControl"}, 
                {0x61,"ErrorPciDevice"}, 
                {0x62,"ErrorReadPciPcr"}, 
                {0x63,"ErrorReadPciLcr"}, 
                {0x64,"ErrorReadPciEEPROM"}, 
                {0x65,"ErrorPciHaDaqSign"}, 
                {0x66,"ErrorPciSN"}
            };
            string sout;
            string ret = "0x" + SystemID.ToString("X");
            if (DSystemID.TryGetValue(SystemID, out sout))
                ret += ": " + sout;
            ret += ", 0x" + AuxCode.ToString("X");
            ret += ", 0x" + TargetID.ToString("X");
            if (DTargetID.TryGetValue(TargetID, out sout))
                ret += ": " + sout;
            ret += ", 0x" + ErrorCode.ToString("X");
            if (DErrorCode.TryGetValue(TargetID, out sout))
                ret += ": " + sout;
            return (ret);
        }

    }
}
/*
Attribute VB_Name = "DASP52032"
'=========================================================
' Module Name: DASP52032.BAS
' Purpose: The constant and function declaration for DASP52032
' Version: 0, 9, 2, 0
' Date: 08/01/2004
' Copyright (c) 2004 Axiomtek Co. Ltd.
' All rights reserved.
'=========================================================

Option Explicit

'Global Const
Global Const NOINTERRUPT = -1
Global Const ANYVALUE = 1
Global Const BOARD_STATUS_FALSE = &H0        ' Initial value
Global Const BOARD_STATUS_READY = &HFE       ' Pci card ready for use
Global Const BOARD_STATUS_TRUE = &HFF
Global Const MAX_BOARD_NO = 8                ' Max. board Number
' DASP-52032 Board TYPE


'-------------- Fundamental System Function ----------------------
Declare Function DASP52032_QuickInstalled Lib "DASP52032r.dll" _
(ByVal id As Byte, ByVal res As Byte) As Long

Declare Function DASP52032_Release Lib "DASP52032r.dll" _
(ByVal id As Byte) As Long

Declare Function DASP52032_GetDeviceStatus Lib "DASP52032r.dll" _
(ByVal id As Byte, ByRef sts As Byte) As Long

Declare Function DASP52032_GetDeviceList Lib "DASP52032r.dll" _
(ByVal addrs As Integer, ByRef id As Byte, ByRef sts As Byte) As Long

Declare Function DASP52032_GetPciDeviceList Lib "DASP52032r.dll" _
(ByRef id As Byte, ByRef cardSN As Long, ByRef sts As Byte) As Long

Declare Function DASP52032_GetDllVersion Lib "DASP52032r.dll" _
(ByRef ver As Long) As Long

'-------------- Digital I/O Function -----------------------------
Declare Function DASP52032_ReadGpio Lib "DASP52032r.dll" _
(ByVal id As Byte, ByRef data As Long) As Long

Declare Function DASP52032_WriteGpio Lib "DASP52032r.dll" _
(ByVal id As Byte, ByVal data As Long) As Long

Declare Function DASP52032_ReadBackGpio Lib "DASP52032r.dll" _
(ByVal id As Byte, ByRef data As Long) As Long
*/