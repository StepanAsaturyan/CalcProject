using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using CalcProject;

namespace CalcTests
{
    [TestClass]
    public class CalcTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CalcTest_EmptyStringExp_0()
        {
            var calc = new Calc(string.Empty);

            calc.Calculate();
        }

        [TestMethod]
        public void CalcTest_DoubleParent()
        {
            var calc = new Calc("(1 - (2+3) /4  ) * 2");
            double expected = -0.5;

            Assert.AreEqual(expected, calc.Calculate());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CalcTest_WhiteSpaces_0()
        {
            var calc = new Calc("   ");

            calc.Calculate();
        }

        [TestMethod]
        [ExpectedException(typeof(DivideByZeroException))]
        public void CalcTest_DividingByZero_Exception()
        {
            var calc = new Calc("1/0");

            calc.Calculate();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CalcTest_IncorrectExpression_Exception()
        {
            var calc = new Calc("1 - (x/2)");

            calc.Calculate();
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void CalcTest_IncorrectPath_Exception()
        {
            var calcFile = new CalcFile("TestFileX.txt");

            calcFile.Start();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CalcTest_NeedMoreParentheses_Exception()
        {
            var calc = new Calc("(((1 + (13/2)");

            calc.Calculate();
        }

        [TestMethod]
        public void CalcTest_TestFile()
        {
            var calcFile = new CalcFile("TestFile.txt");

            calcFile.Start();

            string[] expectedData = File.ReadAllLines("Answer.txt");
            string[] resultData = File.ReadAllLines("Results.txt");

            CollectionAssert.AreEqual(expectedData, resultData);
        }
    }
}
