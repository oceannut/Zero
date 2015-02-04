using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegrateTest.Zero.Client
{
    class Program
    {
        static void Main(string[] args)
        {
            SignServiceTest test = new SignServiceTest();
            //test.TestIsUsernameExist();
            test.TestSignup();

            Console.WriteLine("Press any key to exit");
            Console.ReadKey();

        }
    }
}
