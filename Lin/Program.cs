﻿using System;

namespace Lin
{

    using System.Threading;

    class Program
    {
        static void Main(string[] args)
        {
            Thread t = new Thread(delegate ()
            {
                // replace the IP with your system IP Address...
                Server myserver = new Server("192.168.1.59", 13000);
            });
            t.Start();

            Console.WriteLine("Server Started...!");

        }



    }
}
