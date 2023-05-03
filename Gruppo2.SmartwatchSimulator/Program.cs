using Microsoft.Extensions.CommandLineUtils;
using System;
using System.Diagnostics.Metrics;
using System.Threading;

namespace app
{
    class Program
    {
        static void Main()
        {
            int pulseRate = 0;
            pulseRate = InitPulseRate();

            Console.WriteLine(pulseRate);

            while (true)
            {
                Thread.Sleep(100);

                if (pulseRate > 30 && pulseRate < 200)
                {
                    Console.WriteLine($"\n{pulseRate}");
                    ModifyPulseRate(ref pulseRate);
                }
                else
                {
                    Console.WriteLine($"\n{pulseRate} - NO");
                    ReturnToNormalPulseRate(ref pulseRate);
                }
            }

        }


        #region PulseRate

        static int InitPulseRate()
        {
            Random nRandom = new Random();
            int pulseRate = nRandom.Next(40, 190);        //40 a 190

            return pulseRate;
        }

        static int ModifyPulseRate(ref int pulseRate)
        {
            Random nRandom = new Random();
            int incrDecrease = nRandom.Next(-7, 8);
            //Console.WriteLine(incrDecrease);

            pulseRate += incrDecrease;

            return pulseRate;
        }

        static int ReturnToNormalPulseRate(ref int pulseRate)
        {
            Random nRandom = new Random();
            int incrDecrease = 0;

            switch (pulseRate)
            {
                case <= 24:
                    incrDecrease = nRandom.Next(3, 5);
                    pulseRate += incrDecrease;
                    break;

                case 25:
                case 26:
                case 27:
                    incrDecrease = nRandom.Next(2, 4);
                    pulseRate += incrDecrease;
                    break;

                case 28:
                case 29:
                case 30:
                    incrDecrease = nRandom.Next(1, 3);
                    pulseRate += incrDecrease;
                    break;

                case 200:
                case 201:
                case 202:
                    incrDecrease = nRandom.Next(-3, -1);
                    pulseRate += incrDecrease;
                    break;

                case 203:
                case 204:
                case 205:
                    incrDecrease = nRandom.Next(-4, -2);
                    pulseRate += incrDecrease;
                    break;

                case >= 206:
                    incrDecrease = nRandom.Next(-5, -3);
                    pulseRate += incrDecrease;
                    break;
            }

            //Console.WriteLine(incrDecrease);
            return pulseRate;
        }

        #endregion

    }
}
