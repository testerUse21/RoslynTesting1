using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Xsl;

namespace RoslynWalker_DotNet5_Sample
{
	class TestV2PR_2_CSharp
	{
		public void test()
		{
            try
            {
                // do something
            }
            catch
            {
            }
        }
    }

    class MyClass
    {
        private readonly object myLockObj = new object();
        public void MyMethod()
        {
            Monitor.Enter(myLockObj); //Noncompliant
        }
    }
}
