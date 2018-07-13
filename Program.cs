using System;
using System.Threading;
using System.Diagnostics;
using SS1306;

namespace SS1306.Driver
{
    public class Program
    {
        public static void Main()
        {
           
                OLED.Initialize();

                OLED.Write(1, 2, "Hello from ESP32!");


                while (true)

                {
                                               
                    Thread.Sleep(1000);
                 
                }
            }
            
        }
    }
