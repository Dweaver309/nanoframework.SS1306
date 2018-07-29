
//**************
// I2C1 
//Pin 18 Data 
//Pin 19 Clock

// 12C2
//Pin 25 Data
//Pin 26 Clock
//*************

//**************
// Device Address 
// 0x54
// Up to 8 EEprom microchips on bus
// Address 0x50..0x57
//*************

//**************
// Power Supply
// 5 Volts
//**************

//**************
// Page Size
// Page size is 64 bits
// Max stings size 60 (2 Address, 2 Length)
// *************

//*************
// Thanks to Adrain Soundy
// Who helped create much of the code below
//************

using System;
using Windows.Devices.I2c;
using System.Threading;


namespace nanoframework.i2c.eeprom
{ 

public class EEprom24LC256
{
        
        readonly public I2cDevice EEprom;

        /// <summary>
        /// Structure to put addresses to write to and read from
        /// </summary>
        public struct Address
         {
        public static int FirstString = 64;
        public static int SecondString = 256;
        public static int ThirdString = 192;
         }

        // i2c bus can use I2C1 and or I2C2
        public struct DeviceConnectionSting
        {
            public static string I2C1 = "I2C1";
            public static string I2C2 = "I2C2";

        }

        // Maximum bytes to write
        // Some EEprom microchips allow 128 
        const int PAGESIZE = 64;


        // Constructor for i2c bus
        // Example: EEprom24LC256 eeprom = new EEprom24LC256(EEprom24LC256.DeviceConnectionSting.I2C1, 0x54);
        public EEprom24LC256(string device, byte DeviceAddress)
        {
           
            EEprom = I2cDevice.FromId(device, new I2cConnectionSettings(DeviceAddress) { BusSpeed = I2cBusSpeed.StandardMode, SharingMode = I2cSharingMode.Shared });
            
        }

        /// <summary>
        /// Write string to address
        /// </summary>
        public void Write(int address, string str)
    {
        try
        {
         
            string strlength = String.Empty;

            if (str.Length < 10)
            {

                    strlength = "0" + str.Length.ToString();
            }
            else
            {

                    strlength = str.Length.ToString();
            }

            // The addess in buffer 0 and 1
            // Length in buffer 3 and 4
            str = "00" + strlength + str;

            Console.WriteLine("String to encode: " + str);

            byte[] buffer = System.Text.Encoding.UTF8.GetBytes(str);

            // Set Write address as first 2 bytes, High + low 
            buffer[0] = (Byte)(address >> 8);

            buffer[1] = (Byte)(address & 0xFF);
                         
           // Get offset in write page
           int offset = address % PAGESIZE;

                // Can only write within 1 page, if you write more than pagesize 
                // then it will just wrap around in the same page so give ArgumentOutOfRangeException
                // so we know something is wrong
                if (offset + buffer.Length > PAGESIZE)
                {

                    throw new ArgumentOutOfRangeException();
                }

           EEprom.Write(buffer);
                         
           WaitReady();

        }

        catch (Exception)
        {

            Console.WriteLine("Error writing to eeprom");

        }

    }

    private  void WaitReady()
    {
        int count = 0;

        // Wait for Acknowledge from eeprom by sending 0 length write until acknowledged or other error
        // End anyway if slave not acknowledging, normaly slave starts to Ack after 4 or 5 count which means
        // previous write is complete
        while (count < 20)
        {

                I2cTransferResult res = EEprom.WritePartial(null);

               
            if (res.Status != I2cTransferStatus.SlaveAddressNotAcknowledged) break;
                {
                    count++;

                    Thread.Sleep(1);  // Give time to other threads

                }
        }
    }

        /// <summary>
        /// Read data lengh bytes  
        /// Return the saved string
        // </summary>
        public String Read(int address, int datalength = PAGESIZE)
        {
            try
            {

                // Avoid wrap around issues
                if (datalength > PAGESIZE)
                {

                    // 60 max string size
                    datalength = PAGESIZE - 4;

                }


                var Data = new byte[datalength];

                EEprom.Write(new[] { (Byte)(address >> 8), (Byte)(address & 0xFF) });

                EEprom.Read(Data);

                //Get the first char in the two byte length
                char flb = Convert.ToChar(Data[0]);

                Console.WriteLine("First length char " + flb.ToString());

                // Get the second char in length
                char slb = Convert.ToChar(Data[1]);

                Console.WriteLine("Second length char " + slb.ToString());

                //make the string
                string sl = flb.ToString() + slb.ToString();

                //Convert to integer
                int length = Convert.ToInt32(sl);
              
                Console.WriteLine("Length " + length);

                string rs = string.Empty;

                //Start reading after the two byte saved length
                for (int i = 2; i < length + 2; i++)
                {

                    char c = Convert.ToChar(Data[i]);

                    rs = rs + c.ToString();

                    Console.WriteLine("Read Adddress  " + address + " Char Read " + c.ToString());

                    address += 1;

                }

                return rs;

            }

            catch (Exception)
            {

                return "Error reading eeprom";

            }
        }


        /// <summary>
        /// If there is no length char the data has not been saved
        /// </summary>
        ///     ''' <returns>True or False</returns>
        ///     ''' <remarks>Use before calling the Read function</remarks>
        public bool Exist(int address)
            {

            // address, address, length 1, length 2
            var Data = new byte[4];

            EEprom.Write(new[] { (Byte)(address >> 8), (Byte)(address & 0xFF) });
                  
            EEprom.Read(Data);
          
            // Get the second char in length
            char slb = Convert.ToChar(Data[1]);

            Console.WriteLine("Second length char " + slb.ToString());

            string Str = slb.ToString();

         switch (Str)
         {
            case "0":
            case "1":
            case "2":
            case "3":
            case "4":
            case "5":
            case "6":
            case "7":
            case "8":
            case "9":
                {
                    return true;

                }

            default:
                {
                    return false;

                }
        }
    }

}

}

