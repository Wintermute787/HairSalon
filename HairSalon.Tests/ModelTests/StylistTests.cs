using Microsoft.VisualStudio.TestTools.UnitTesting;
using HairSalon.Models;
using System.Collections.Generic;
using System;

namespace HairSalon.Tests
{
    [TestClass]
    public class StylistTest : IDisposable
    {
        public void Dispose()
        {
            Stylist.ClearAll();
        }
         public StylistTest()
        {
            DBConfiguration.ConnectionString = "server=localhost;user id=root;password=Hellsing1;port=3306;database=joe_barnes_test;";
        }

        [TestMethod]
        public void StylistConstructor_CreatesInstanceOfStylist_Stylist()
        {
            Stylist newStylist = new Stylist("test",1);
            Assert.AreEqual(typeof(Stylist), newStylist.GetType());
        }

        [TestMethod]
        public void GetAll_StylistEmptyAtFirst_0()
        {
            int result = Stylist.GetAll().Count;

            Assert.AreEqual(0, result);
        }

        // [TestMethod]
        // public void Equals_ReturnsTrueIfNamesAreTheSame_Stylist()
        // {
        //     Stylist firstStylist = new Stylist("doe",1);
        //     Stylist secondStylist = new Stylist("john",1);

        //     Assert.AreEqual(firstStylist,secondStylist);
        // }

        [TestMethod]
        public void Save_SaveStylistToDatabase_StylistList()
        {
            Stylist testStylist = new Stylist("Jim",1);
            testStylist.Save();

            List<Stylist> result = Stylist.GetAll();
            List<Stylist> testList = new List<Stylist>{testStylist};

            CollectionAssert.AreEqual(testList, result);
        }
  [TestMethod]
        public void Save_DatabaseAssignsIdToStylist_Id()
        {
           
            Stylist testStylist = new Stylist("name",1);
            testStylist.Save();

            
            Stylist savedStylist = Stylist.GetAll()[0];

            int result = savedStylist.GetId();
            int testId = testStylist.GetId();

           
            Assert.AreEqual(testId, result);
        }

        [TestMethod]
        public void Find_FindsStylistInDatabase_Stylist()
        {
            
            Stylist testStylist = new Stylist("name",1);
            testStylist.Save();

            
            Stylist foundStylist = Stylist.Find(testStylist.GetId());

           
            Assert.AreEqual(testStylist, foundStylist);
        }

        [TestMethod]
        public void GetClients_RetrievesAllClientsWithStylists_ClientList()
        {
            
         
            Stylist testStylist = new Stylist("name",1);
            testStylist.Save();
            Client client01 = new Client("joe", testStylist.GetId());
            client01.Save();
            Client client02 = new Client("barnes", testStylist.GetId());
            client02.Save();
            List<Client> testClientList = new List<Client> {client01, client02};
            List<Client> resultClientList = testStylist.GetClients();

            //Assert
            CollectionAssert.AreEqual(testClientList, resultClientList);
        }

    }
}