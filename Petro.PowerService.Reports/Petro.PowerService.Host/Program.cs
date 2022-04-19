using System;

namespace Petro.PowerService.Host
{
    internal class Program
    {
        static void Main(string[] args)
        {
            try
            {
                ServiceConfigurator.Configure();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                Console.Read();
            }
        }
    }
}