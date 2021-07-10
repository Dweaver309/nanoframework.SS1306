
//Uncomment for FeatherWind 
//#define FeatherWing
using System.Threading;
using System.Diagnostics;

namespace NF.SSD1306.i2c
{
    public class Program
    {
        public static void Main()
        {
            Debug.WriteLine("Hello from SSD1306.");

#if FeatherWing

            OLED oled = new(OLED.DisplayType.OLED128x32, 23, 22);

            oled.ClearScreen();
        
            oled.Write(0, 0, "Adafruit FeatherWing");

            oled.Write(0, 3, "Display size 128x32",false);
#else
            OLED oled = new(OLED.DisplayType.OLED128x64);

            oled.ClearScreen();
        
            oled.Write(0, 0, "SSD1306 Display i2c");

            oled.Write(0, 3, "Display size 128x64",false);

#endif



            Thread.Sleep(Timeout.Infinite);

            // Browse our samples repository: https://github.com/nanoframework/samples
            // Check our documentation online: https://docs.nanoframework.net/
            // Join our lively Discord community: https://discord.gg/gCyBu8T
        }
    }
}
