using System;
using GameLib;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestApp
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {

            var board = Board.CreateNewGame(3);


            Assert.IsTrue(true);
        }
    }
}
