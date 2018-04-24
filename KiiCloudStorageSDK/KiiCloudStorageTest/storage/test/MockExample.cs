// ------------------------------------------------------------------------------
//  <autogenerated>
//      This code was generated by a tool.
//      Mono Runtime Version: 4.0.30319.1
// 
//      Changes to this file may cause incorrect behavior and will be lost if 
//      the code is regenerated.
//  </autogenerated>
// ------------------------------------------------------------------------------
using System;
using NUnit.Framework;
using Moq;
using Moq.Language;
using Moq.Protected;

namespace KiiCorp.Cloud.Storage
{
    public class PersonOperation
    {
        IPersonManager personManager;
        public PersonOperation (IPersonManager manager)
        {
            this.personManager = manager;
        }

        public Person getPersonById(int id) {
            return personManager.getPersonById (id);
        }
    }

    public interface IPersonManager
    {
        Person getPersonById(int id);
    }

    public class Person
    {
         public string name { get; set; }
         public int id { get; set; }
    }

    [TestFixture()]
    public class MockExample
    {
        [Test]
        public void Test_PersonById () 
        {
            // mock setup
            var mockManager = new Mock<IPersonManager> ();
            mockManager.Setup (m => m.getPersonById(1)).Returns(new Person{name = "test", id = 1});
            var op = new PersonOperation (mockManager.Object);

            // act
            var person = op.getPersonById (1);

            // assert
            Assert.IsNotNull (person);
            Assert.AreEqual ("test", person.name);
            Assert.AreEqual (1, person.id);
        }

    }
}

