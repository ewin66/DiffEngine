﻿using System;
using System.Linq;

namespace DiffEngine
{
    public static class ContinuousTestingDetector
    {
        static ContinuousTestingDetector()
        {
            if (AppDomain.CurrentDomain.GetAssemblies()
                .Any(a => a.FullName.StartsWith("Microsoft.CodeAnalysis.LiveUnitTesting.Runtime")))
            {
                Detected = true;
                return;
            }

            if (Environment.GetEnvironmentVariable("NCRUNCH") != null)
            {
                Detected = true;
                return;
            }
        }

        public static bool Detected { get; set; }
    }
}