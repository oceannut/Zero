using System;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTest.Zero.Domain__
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {

            Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity("zsp"), new string[] { "admin" });
            IPrincipal principal = Thread.CurrentPrincipal;
            Console.WriteLine(principal.Identity.Name);
            Task.Factory.StartNew(() =>
            {
                principal = Thread.CurrentPrincipal;
                Console.WriteLine(principal.Identity.Name);
            });

        }
    }
}
