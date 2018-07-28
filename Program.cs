//************
// EEprom references
// https://github.com/Dweaver309/nanoframework.EEprom
// https://www.hackster.io/dweaver309/eeprom-i2c-driver-for-nanoframework-718669
//************

//************
// SS1306 Oled display references
// https://github.com/Dweaver309/nanoframework.SS1306
// https://www.hackster.io/dweaver309/oled-display-driver-for-nanoframework-8744e7
//************


using System;
using nanoframework.i2c.eeprom;
using nanoframework.i2c.SS1306;

namespace nanoframework.I2C.driver
{
    public class Program
    {
        public static void Main()
        {

            //Using EEprom
            Boolean Connect_EEprom = true;

            //Using SS1306 OLED display
            Boolean Connect_OLED = true;

            if (Connect_EEprom)
            {
                EEprom24LC256 eeprom = new EEprom24LC256(EEprom24LC256.DeviceConnectionSting.I2C1, 0x54);

                String str = "This is an EEprom test for ESP32.";

                eeprom.Write(EEprom24LC256.Address.SecondString, str);

                str = eeprom.Read(EEprom24LC256.Address.SecondString);

                Console.WriteLine("\r\nReturned " + str);

                // If the string exist read it 
                if (eeprom.Exist(EEprom24LC256.Address.ThirdString))
                {
                    str = eeprom.Read(EEprom24LC256.Address.ThirdString);

                    Console.WriteLine("\r\nReturned " + str);

                }

                else
                {

                    // String did not exist create it
                    string ts = "This is the thrid string we just created.";

                    eeprom.Write(EEprom24LC256.Address.ThirdString, ts);

                    ts = eeprom.Read(EEprom24LC256.Address.ThirdString);

                    Console.WriteLine("\r\nReturned " + str);
                   
                }

            }

            if(Connect_OLED)
            {

                OLED oled = new OLED(OLED.DeviceConnectionSting.I2C1, 0x3C);

                oled.Initialize();

                oled.Write(0, 2, "Hello ESP32");

                oled.Write(0, 4, "Connected to I2C1");

            }

        }
    }
}