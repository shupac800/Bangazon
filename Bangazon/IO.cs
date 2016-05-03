using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bangazon
{
    public class IO
    {
        public static void displayMenu(List<string> linesToDisplay)
        {
            int index = 1;
            foreach (string line in linesToDisplay)
            {
                Console.WriteLine("{0}. {1}", index++, line);
            }
        }

        public static int getChoice()
        {
            Console.Write("> ");
            string choice = Console.ReadLine();
            // future improvement: validate input!
            return Int32.Parse(choice) - 1;
        }

    }
}
