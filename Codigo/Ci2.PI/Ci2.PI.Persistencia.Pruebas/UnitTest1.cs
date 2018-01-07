using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Ci2.PI.Persistencia.Pruebas
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            for (int i = 0; i < 3; i++)
            {
                System.Console.WriteLine(Guid.NewGuid());
            }
        }
    }
}
