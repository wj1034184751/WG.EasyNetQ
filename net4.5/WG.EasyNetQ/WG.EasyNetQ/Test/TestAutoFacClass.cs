using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WG.EasyNetQ.Test
{
    public interface  ITestAutoFacClass
    {
        void Print(string typeName, string version);
    }

    public class TestAutoFacClass: ITestAutoFacClass
    {
        public string TypeName { get; set; }

        public int Version { get; set; }

        public void Print(string typeName,string version)
        {
            Console.WriteLine($"{typeName}:{version}");
        }
    }
}
