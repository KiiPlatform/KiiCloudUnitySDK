using System;
using System.Collections.Generic;
using NUnit.Framework;
using KiiCorp.Cloud.Storage;

namespace KiiCorp.Cloud.ABTesting
{
    [TestFixture()]
    public class TestConversionEvent
    {

        [Test(), KiiUTInfo(
            action = "When we call GetConversionEventByName(name) and method returns ConversionEvent",
            expected = "We can get the ConversionEvent by specified name"
            )]
        public void Test_0000_GetConversionEventByName()
        {
            ConversionEvent e1 = new ConversionEvent("ConversionEvent1");
            ConversionEvent e2 = new ConversionEvent("CONVERSIONEVENT1");
            ConversionEvent e3 = new ConversionEvent("conversionevent1");
            ConversionEvent e4 = new ConversionEvent("ConversionEvent2");
            ConversionEvent e5 = new ConversionEvent("3ConversionEvent");

            ConversionEvent[] array = new ConversionEvent[] {e1, e2, e3, e4, e5};

            Assert.AreEqual(e1, ConversionEvent.GetConversionEventByName("ConversionEvent1", array));
            Assert.AreEqual(e2, ConversionEvent.GetConversionEventByName("CONVERSIONEVENT1", array));
            Assert.AreEqual(e3, ConversionEvent.GetConversionEventByName("conversionevent1", array));
            Assert.AreEqual(e4, ConversionEvent.GetConversionEventByName("ConversionEvent2", array));
            Assert.AreEqual(e5, ConversionEvent.GetConversionEventByName("3ConversionEvent", array));
            Assert.AreEqual(null, ConversionEvent.GetConversionEventByName("hoge", array));
        }
    }
}

