using Microsoft.VisualStudio.TestTools.UnitTesting;
using Demo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Tests
{
    [TestClass()]
    public class ProgramTests
    {
        [TestMethod()]
        public void Test1Test()
        {
            Program.Test1(false);
        }
    }
}