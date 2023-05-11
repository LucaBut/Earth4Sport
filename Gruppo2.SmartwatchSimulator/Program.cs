﻿using System.Runtime.InteropServices;

namespace app
{
    class Program
    {
        static void Main()
        {
            bool moving = true;     // true quando sta avanzando, false quando torna indietro (andamento nella piscina)

            string geoCoordinates = "";
            geoCoordinates = InitGeoCoordinates();
            Console.WriteLine($"ciao {geoCoordinates}");

            Random nRandom = new Random();
            int randLatLong = nRandom.Next(0, 2);   // 0 viene modificata la latitudine (↔) (spostamento vert.)
                                                    // 1 viene modificate la longitudine (↕) (spostamento orizz.)

            Console.WriteLine(randLatLong.ToString());

            Thread.Sleep(5000);
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();

            int minValMovement = 0, maxValMovement = 0;
            LimitsForMovement(geoCoordinates, randLatLong, ref minValMovement, ref maxValMovement);
            
            int pulseRate = 0;
            pulseRate = InitPulseRate();

            //Console.WriteLine(pulseRate);

            while (true)
            {
                Thread.Sleep(5000);

                ModifyGeoCoordinates(ref geoCoordinates, randLatLong, minValMovement, maxValMovement, ref moving);
                

                //if (pulseRate > 30 && pulseRate < 200)
                //{
                //    Console.WriteLine($"\n{pulseRate}");
                //    ModifyPulseRate(ref pulseRate);
                //}
                //else
                //{
                //    Console.WriteLine($"\n{pulseRate} - NO");
                //    ReturnToNormalPulseRate(ref pulseRate);
                //}
            }

        }

        #region GeographicCoordinates
        static string InitGeoCoordinates()
        {
            Random nRandom = new Random();

            int latitudeInt = nRandom.Next(-90, 90);
            int longitudeInt = nRandom.Next(-180, 180);

            int latitudeDecimals = nRandom.Next(501, 999499);       //da 500 a (999999-500) perché dovrei gestire nel caso in cui i movimenti
            int longitudeDecimals = nRandom.Next(501, 999499);      //causino anche una modifica della parte intera delle coord.

            string geoCoordinates = latitudeInt.ToString() + "." + latitudeDecimals.ToString().PadLeft(6, '0') + "," + 
                                    longitudeInt.ToString() + "." + longitudeDecimals.ToString().PadLeft(6, '0');

            return geoCoordinates;
        }

        // 45.951548, 12.679304
        // 45.951548, 12.679304
        // in orizzontale varia la longitudine di 0.000650 circa => spostamento di 50 metri, corrispondente a una vasca

        // 45.953165, 12.688238
        // 45.953605, 12.688238
        // in verticale varia la latitudine  di 0.000440 circa => spostamento di 50 metri, corrispondente a una vasca
        //              0.000500
        static void ModifyGeoCoordinates(ref string geoCoordinates, int randLatLong, int minValMovement, int maxValMovement, ref bool moving)
        {
            int latitudeDecimals = 0, longitudeDecimals = 0, latitudeInt = 0, longitudeInt = 0;
            SplitCoordinates(geoCoordinates, ref latitudeInt, ref latitudeDecimals, ref longitudeInt, ref longitudeDecimals);
            
            Random nRandom = new Random();
            int movement = nRandom.Next(80, 110);
            Console.WriteLine(movement.ToString());
            
            if (randLatLong == 0)
            {
                // SPOSTAMENTO LATITUDINE: mi sposto verticalmente, longitudine rimane uguale, modifico latitudine

                if (moving == true)
                {
                    //somma il nRandom
                    if ((latitudeDecimals + movement) > maxValMovement)
                    {
                        moving = false;

                        int diff = (latitudeDecimals + movement) - maxValMovement;
                        latitudeDecimals = maxValMovement - diff;
                    }
                    else
                    {
                        latitudeDecimals += movement;
                    }
                }
                else
                {
                    //diminuisci del nRandom
                    if ((latitudeDecimals - movement) < minValMovement)
                    {
                        moving = true;

                        int diff = minValMovement - (latitudeDecimals - movement);
                        latitudeDecimals = minValMovement + diff;
                    }
                    else
                    {
                        latitudeDecimals -= movement;
                    }
                }
            }
            else
            {
                // SPOSTAMENTO LONGITUDINE: mi sposto orizzontalmente, latitudine rimane uguale, modifico longitudine

                if (moving)
                {
                    //somma il nRandom
                    if ((longitudeDecimals + movement) > maxValMovement)
                    {
                        moving = false;

                        int diff = (longitudeDecimals + movement) - maxValMovement;
                        longitudeDecimals = maxValMovement - diff;
                    }
                    else
                    {
                        longitudeDecimals += movement;
                    }
                }
                else
                {
                    //diminuisci del nRandom
                    if ((longitudeDecimals - movement) < minValMovement)
                    {
                        moving = true;

                        int diff = (longitudeDecimals - movement) - minValMovement;
                        longitudeDecimals = minValMovement - diff;
                    }
                    else
                    {
                        longitudeDecimals -= movement;
                    }
                }

            }

            //funzione per ricomporre la stringa delle coordinate completa
            geoCoordinates = ComponingStringCoordinates(geoCoordinates, latitudeInt, latitudeDecimals, longitudeInt, longitudeDecimals);

            Console.WriteLine(geoCoordinates);
        }

        static string ComponingStringCoordinates(string geoCoordinates, int latitudeInt, int latitudeDecimals,
                                          int longitudeInt, int longitudeDecimals)
        {
            geoCoordinates = latitudeInt.ToString() + "." + latitudeDecimals.ToString().PadLeft(6, '0') + "," +
                             longitudeInt.ToString() + "." + longitudeDecimals.ToString().PadLeft(6, '0');

            return geoCoordinates;
        }

        static void LimitsForMovement(string geoCoordinates, int randLatLong, ref int minValMovement, ref int maxValMovement)
        {
            int latitudeDecimals = 0, longitudeDecimals = 0, latitudeInt = 0, longitudeInt = 0;

            if (randLatLong == 0)
            {
                // modifico solo latitudine
                SplitCoordinates(geoCoordinates, ref latitudeInt, ref latitudeDecimals, ref longitudeInt, ref longitudeDecimals);

                minValMovement = latitudeDecimals;
                maxValMovement = latitudeDecimals + 500;
            }
            else
            {
                // modifico solo longitudine
                SplitCoordinates(geoCoordinates, ref latitudeInt, ref latitudeDecimals, ref longitudeInt, ref longitudeDecimals);

                minValMovement = longitudeDecimals;
                maxValMovement = longitudeDecimals + 500;
            }
        }

        static void SplitCoordinates(string geoCoordinates, ref int latitudeInt, ref int latitudeDecimals,
                              ref int longitudeInt, ref int longitudeDecimals)
        {
            string[] splittedCoordinates = geoCoordinates.Split(new char[] { ',' });

            string[] onlyLatitude = splittedCoordinates[0].Split(new char[] { '.' });
            latitudeInt = Int32.Parse(onlyLatitude[0]);
            latitudeDecimals = Int32.Parse(onlyLatitude[1]);
            
            string[] onlyLongitude = splittedCoordinates[1].Split(new char[] { '.' });
            longitudeInt = Int32.Parse(onlyLongitude[0]);
            longitudeDecimals = Int32.Parse(onlyLongitude[1]);
            
            //Console.WriteLine($"{latitudeDecimals} ..... {longitudeDecimals}");
        }



        #endregion


        #region PulseRate

        static int InitPulseRate()
        {
            Random nRandom = new Random();
            int pulseRate = nRandom.Next(40, 190);        //40 a 190

            return pulseRate;
        }

        static void ModifyPulseRate(ref int pulseRate)
        {
            Random nRandom = new Random();
            int incrDecrease = nRandom.Next(-7, 8);

            pulseRate += incrDecrease;
        }

        static void ReturnToNormalPulseRate(ref int pulseRate)
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
        }

        #endregion

    }
}
