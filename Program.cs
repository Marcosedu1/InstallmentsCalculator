using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckInstallments
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome To The Ultimate Installments Calculator Ever Made!\n");
            Console.WriteLine("Please, Enter an intial date (DD/MM/YYYY)");
            string dateString = Console.ReadLine();
                        
            DateTime date;
            if (dateString == "")
            {
                Console.WriteLine("Ending Program");
            }
            else
            {
                while (!DateTime.TryParse(dateString, out date))
                {
                    Console.WriteLine("Please, enter a valid date!");
                    dateString = Console.ReadLine();
                    if (dateString == "")
                    {
                        break;
                    }
                }
                if(dateString != "")
                {
                var checkingDate = new InstallmentsCalculator(date);

                checkingDate.CheckInstallments();
                }
            }
        }
    }
}