using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zero.Service.Rest
{
    public class TestRestServiceImpl : ITestRestService
    {

        public string GetServicePath()
        {
            return Environment.CurrentDirectory;
        }

    }
}
