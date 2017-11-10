using System;
using System.Management;

namespace ConsoleApp_NetFx
{
    class Program
    {
        static void Main(string[] args)
        {
            var mo = new ManagementObject("Win32_Share.Name=\"C$\"");
            mo.Get();

            foreach (var p in mo.Properties)
                Logger.Log($"{p.Name}={p.Value}");
        }
    }
}
