using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using RshCSharpWrapper;
using RshCSharpWrapper.RshDevice;

using Share;
using UPAR;
using UPAR.TS.TSDef;

namespace Defect.GSPF052PCI
{
    public class GSPF : IGSPF052
    {
        public GSPF(DOnPr _OnPr = null)
        {
            if (_OnPr != null)
                OnPr += _OnPr;
        }

        public event DOnPr OnPr = null;
        Device device = null;
        public string Start(SGWorkPars _tsDefSG)
        {
            if (device != null)
                return ("Устройство уже запущено");
            // Служебное имя устройства, с которым будет работать программа.
            const string BOARD_NAME = "GSPF052PCI";

            //частота генерируемого синуса в Герцах
            double signalFrequency = _tsDefSG.Frequency * 1000;
            if (signalFrequency <= 0)
                return ("signalFrequency <=0");
            //амплитуда генерируемого синуса в вольтах 
            double signalVoltage = _tsDefSG.Voltage;
            if (signalVoltage <= 0)
                return ("signalVoltage <=0");
            // Код выполнения операции.
            RSH_API st;

            //Создание экземпляра класса и подключение к библиотеке абстракции устройства
            device = new Device(BOARD_NAME);

            pr("--> Sinus Generator <--");

            //=================== ИНФОРМАЦИЯ О ЗАГРУЖЕННОЙ БИБЛИОТЕКЕ ====================== 
            string libVersion, libname, libCoreVersion, libCoreName;

            st = device.Get(RSH_GET.LIBRARY_VERSION_STR, out libVersion);
            if (st != RSH_API.SUCCESS) return (SayGoodBye(st));

            st = device.Get(RSH_GET.CORELIB_VERSION_STR, out libCoreVersion);
            st = device.Get(RSH_GET.CORELIB_FILENAME, out libCoreName);
            st = device.Get(RSH_GET.LIBRARY_FILENAME, out libname);

            pr("Library Name: " + libname);
            pr("Library Version: " + libVersion);
            pr("Core Library Name: " + libCoreName);
            pr("Core Library Version: " + libCoreVersion);

            //===================== ПРОВЕРКА СОВМЕСТИМОСТИ =================================  

            uint caps = (uint)RSH_CAPS.SOFT_GENERATION_IS_AVAILABLE;
            //Проверим, поддерживает ли устройство функцию генерации сигнала.
            st = device.Get(RSH_GET.DEVICE_IS_CAPABLE, ref caps);
            if (st != RSH_API.SUCCESS) return (SayGoodBye(st));

            //========================== ИНИЦИАЛИЗАЦИЯ =====================================        

            // Подключаемся к устройству. Нумерация устройств начинается с 1.
            st = device.Connect(Convert.ToUInt32(ParAll.SG.GSPF.DevNum));
            if (st != RSH_API.SUCCESS) return (SayGoodBye(st));

            /*
		    Для подключения к устройству по заводскому номеру.
		    uint serialNumber = 11111;
		    st = device.Connect(serialNumber, RSH_CONNECT_MODE.SERIAL_NUMBER); 
		    if (st != RSH_API.SUCCESS)
				return SayGoodBye(st);
		    */

            //структура инициализации
            RshInitGSPF initStr = new RshInitGSPF();

            //запуск генерации - программный
            initStr.startType = ParAll.SG.GSPF.GetEGSPFStart();
            //циклическое проигрывание сигнала
            initStr.control = ParAll.SG.GSPF.GetEGSPFSPlay();

            //Определим частоту ЦАП, которую будем использовать для генерации
            initStr.frequency = signalFrequency < 1000 ? 12500000 : 100000000;

            //определим максимальный размер буфера
            uint bufferSize = 0;
            device.Get(RSH_GET.DEVICE_MEMORY_SIZE, ref bufferSize);

            //Подгоняем частоту дискретизации устройства, чтобы буфер поместился в память устройства
            while (initStr.frequency / signalFrequency > bufferSize)
                initStr.frequency /= 2;

            //вычисляем размер  буфера (сколько отсчетов займет один период синуса)	
            int bufSize = Convert.ToInt32(initStr.frequency / signalFrequency);

            //Число слов в буфере должно быть чётным (аппаратные особенности устройств серии ГСПФ)
            if (bufSize % 2 != 0)
                bufSize++;

            //Определяем подходящий коэффициент ослабления
            if (signalVoltage > 5.1)
                initStr.attenuator = RshInitGSPF.AttenuatorBit.AttenuationOff;
            else if (signalVoltage > 2.6)
                initStr.attenuator = RshInitGSPF.AttenuatorBit.Attenuation6dB;
            else if (signalVoltage > 1.26)
                initStr.attenuator = RshInitGSPF.AttenuatorBit.Attenuation12db;
            else if (signalVoltage > 0.626)
                initStr.attenuator = RshInitGSPF.AttenuatorBit.Attenuation18dB;
            else if (signalVoltage > 0.313)
                initStr.attenuator = RshInitGSPF.AttenuatorBit.Attenuation24dB;
            else if (signalVoltage > 0.157)
                initStr.attenuator = RshInitGSPF.AttenuatorBit.Attenuation30dB;
            else if (signalVoltage > 0.078)
                initStr.attenuator = RshInitGSPF.AttenuatorBit.Attenuation36dB;
            else
                initStr.attenuator = RshInitGSPF.AttenuatorBit.Attenuation42dB;

            //Вычисляем амплитуду
            double range = 0;
            //Максимальная амплитуда в вольтах
            device.Get(RSH_GET.DEVICE_OUTPUT_RANGE_VOLTS, ref range);

            //Можно пересчитать на установленную нагрузку
            double OutR = 1000000.0f;
            range = OutR * (range / (OutR + 50.0));
            double amplitude = (signalVoltage / range) * (double)Math.Pow(2, (double)initStr.attenuator); ;

            short[] dataBuffer = new short[bufSize];
            // формируем синус
            for (int i = 0; i < bufSize; i++)
                dataBuffer[i] = (short)(Convert.ToInt16(amplitude * 8190 * Math.Sin(i * (2 * Math.PI * signalFrequency) / initStr.frequency)) << 2);

            st = device.Init(initStr);
            if (st != RSH_API.SUCCESS) return (SayGoodBye(st));

            pr("=============================================================");
            pr(string.Format("Signal Frequency: {0} Hz Signal Range: {1} V",
                signalFrequency.ToString(), signalVoltage.ToString()));
            pr("=============================================================");
            pr("Loading buffer into device . . .");
            st = device.SetData(dataBuffer);
            if (st != RSH_API.SUCCESS) return (SayGoodBye(st));

            st = device.Start();
            if (st != RSH_API.SUCCESS) return (SayGoodBye(st));

            // Ожидаем окончания проигрывания буфера. (для GSPF052PCI)
            st = device.Get(RSH_GET.WAIT_BUFFER_READY_EVENT);
            if (st != RSH_API.SUCCESS)
                SayGoodBye(st);
            else
            {
                pr("Interrupt has taken place!");
                pr("Which means that GSPF had generated loaded buffer completely.");
            }
            return (null);
        }
        public string Stop()
        {
            if (device == null)
                return ("Устройство ещё не запущено");
            RSH_API st;
            st = device.Stop();
            if (st != RSH_API.SUCCESS) return (SayGoodBye(st));
            device = null;
            return (null);
        }
        public void Dispose()
        {
            if (device != null)
                device.Stop();
        }
        public string SayGoodBye(RSH_API statusCode)
        {
            string errorMessage;
            Device.RshGetErrorDescription(statusCode, out errorMessage, RSH_LANGUAGE.RUSSIAN);
            string ret = statusCode.ToString() + ": " + errorMessage;
            pr(ret);
            return (ret);
        }
        void pr(string _msg)
        {
            if (OnPr != null)
                OnPr(_msg);
        }
    }
}
