using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TDF.Core.Configuration;
using TDF.Core.Ioc;
using Autofac;

namespace TDF.Core.Test
{
    [TestClass]
    public class ConfigTest
    {
        [TestMethod]
        public void TestOverridConfig()
        {
            InitializerContext.Instance.IocInitialize(x=>x.Builder.RegisterType<MyConfigs>().As<Configs>().SingleInstance()).ExecuteInit();
            Assert.AreEqual(Configs.Instance.TokenExpireMinute,10);
        }
    }

    public class MyConfigs : Configs
    {
        public override int TokenExpireMinute =>10;
    }
}
