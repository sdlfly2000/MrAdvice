﻿#region Weavisor
// Arx One Aspects
// A simple post build weaving package
// https://github.com/ArxOne/Weavisor
// Released under MIT license http://opensource.org/licenses/mit-license.php
#endregion
namespace MethodLevelTest
{
    using System;
    using System.Linq.Expressions;
    using System.Reflection;
    using Advices;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    public class MethodAdvisedCtorClass
    {
        [RecordCall]
        public MethodAdvisedCtorClass()
        {
        }
    }

    [TestClass]
    public class BasicTests
    {
        [TestMethod]
        [TestCategory("Weaving")]
        public void MethodTest()
        {
            new EmptyAdvisedClass().MethodTest();
        }

        [TestMethod]
        [TestCategory("Weaving")]
        public void StaticMethodTest()
        {
            EmptyAdvisedClass.StaticMethodTest();
        }

        [TestMethod]
        [TestCategory("Weaving")]
        public void MethodWithParameterTest()
        {
            new EmptyAdvisedClass().MethodWithParameterTest(2);
        }

        [TestMethod]
        [TestCategory("Weaving")]
        public void StaticMethodWithParameterTest()
        {
            EmptyAdvisedClass.StaticMethodWithParameterTest(3);
        }

        [TestMethod]
        [TestCategory("Weaving")]
        public void PropertyAtMethodLevelTest()
        {
            var c = new EmptyAdvisedClass();
            c.Property++; // which calls a getter and a setter
        }

        [TestMethod]
        [TestCategory("Weaving")]
        public void PropertyAtPropertyLevelTest()
        {
            var c = new EmptyAdvisedClass();
            c.Property2++; // which calls a getter and a setter
        }

        [TestMethod]
        [TestCategory("Interception")]
        public void ResolveOverloadTest()
        {
            var r = EmptyAdvisedClass.Overload(2);
            Assert.AreEqual(2, r);
        }

        [TestMethod]
        [TestCategory("Constructor")]
        public void EmptyAdvisedWithMethodCtorTest()
        {
            var count = RecordCall.Count;
            var instance = new MethodAdvisedCtorClass();
            Assert.AreEqual(count + 1, RecordCall.Count);
        }

        [TestMethod]
        [TestCategory("Weaving")]
        public void RefParameterTest()
        {
            var c = new EmptyAdvisedClass();
            int b = 10;
            var r = c.UsesRef(3, ref b);
            Assert.AreEqual(r, b);
            Assert.AreEqual(13, b);
        }

        [TestMethod]
        [TestCategory("Weaving")]
        public void OutParameterTest()
        {
            var c = new EmptyAdvisedClass();
            int b;
            c.UsesOut(4, out b);
            Assert.AreEqual(4, b);
        }

        //[TestMethod]
        //public void NonGenericExpression()
        //{
        //    Z(new Action(NonGenericExpression));
        //    //Expression<Action> e = () => NonGenericExpression();
        //    //var m = (MethodInfo)MethodBase.GetCurrentMethod();
        //    //var f = Expression.Call(m,Expression.Constant(this));
        //}

        [TestMethod]
        [TestCategory("Weaving")]
        public void TryBlockUnusedTest()
        {
            new EmptyAdvisedClass().TryBlockUnused();
        }

        [TestMethod]
        [TestCategory("Weaving")]
        public void TryBlockUsedTest()
        {
            new EmptyAdvisedClass().TryBlockUsed();
        }

        [TestMethod]
        [TestCategory("Interception")]
        public void AsyncTest()
        {
            var c = new AdvisedClass();
            Assert.IsTrue(c.LaunchAsyncMethod());
        }


        private void Z(Delegate d)
        { }

        ////[TestMethod]
        ////[TestCategory("Weaving")]
        ////public void MethodWithGenericParameterTest()
        ////{
        ////    new EmptyAdvisedClass().MethodWithGenericParameterTest(6);
        ////}
    }
}