using Microsoft.VisualStudio.TestTools.UnitTesting;
using HairSalon.Models;
using System.Collections.Generic;
using System;

namespace HairSalon.Tests
{
    [TestClass]
    public class ClientTest : IDisposable
    {
        public void Dispose()
        {
            Client.ClearAll();
        }

        public ClientTest()
        {
            DBConfiguration.ConnectionString = "server=localhost;user id=root;password=Hellsing1;port=3306;database=joe_barnes_test;";
        }

        [TestMethod]
        public void ClientConstructor_CreatesInstanceOfClient_Item()
        {
            Client newClient = new Client("test",1,1);
            Assert.AreEqual(typeof(Client), newClient.GetType());
        }

        [TestMethod]
        public void GetName_ReturnsName_String()
        {
            string name = "test";
            int id = 0;
            int secondId = 0;
            Client newClient = new Client(name, id, secondId);

            string result = newClient.GetName();

            Assert.AreEqual(name, result);
        }

        [TestMethod]
        public void SetName_SetsName_String()
        {
            string name = "test";
            int id = 0;
            int secondId = 0;
            Client newClient = new Client(name, id, secondId);

            string updatedName = "test2";
            newClient.SetName(updatedName);
            string result = newClient.GetName();

            Assert.AreEqual(updatedName, result);
        }

        [TestMethod]
        public void GetAll_ReturnsEmptyListFromDatabase_ClientList()
        {
            List<Client> newList = new List<Client> {};

            List<Client> result = Client.GetAll();

            CollectionAssert.AreEqual(newList, result);
        }

        [TestMethod]
        public void GetAll_ReturnsClients_ClientList()
        {
            string name01 = "test";
            string name02 = "test2";
            int id01 = 0;
            int id02 = 0;
            int styleInt = 0;
            int styleInt2 = 0;
            Client newClient1 = new Client(name01, styleInt, id01);
            newClient1.Save();
            Client newClient2 = new Client(name02, styleInt2, id02);
            newClient2.Save();
            List<Client> newClient = new List<Client> {newClient1, newClient2};

            List<Client> result = Client.GetAll();

            CollectionAssert.AreEqual(newClient, result);
        }

        [TestMethod]
        public void Save_AssingsIdToObject_Id()
        {
            Client newClient = new Client("joe",1,1);

            newClient.Save();
            Client savedClient = Client.GetAll()[0];

            int result = savedClient.GetId();
            int clientId = newClient.GetId();

            Assert.AreEqual(clientId, result);
        }

        [TestMethod]
        public void Find_ReturnsCorrectClientFromDatabase_Client()
        {
            Client testClient = new Client("John",1,1);
            testClient.Save();

            Client foundClient = Client.Find(testClient.GetId());

            Assert.AreEqual(testClient, foundClient);
        }

        [TestMethod]
        public void Save_SavesToDatabase_ClientList()
        {
            Client testClient = new Client("Jimmy",1,1);

            testClient.Save();
            List<Client> result = Client.GetAll();
            List<Client> testList = new List<Client>{testClient};

            CollectionAssert.AreEqual(testList, result);
        }

        [TestMethod]
        public void Edit_UpdatesItemDatabase_String()
        {
            string name01 = "Joe";
            Client testClient = new Client(name01, 1,1);
            testClient.Save();
            string name02 = "jimmy";

            testClient.Edit(name02);
            string result = Client.Find(testClient.GetId()).GetName();

            Assert.AreEqual(name02, result);
        }

        [TestMethod]
        public void GetStylistId_ReturnsClientParentStylistId_int()
        {
            Stylist newStylist = new Stylist("Ted",1);
            Client newClient = new Client("blerp",1,newStylist.GetId());

            int result = newClient.GetstylistId();

            Assert.AreEqual(newStylist.GetId(), result);
        }

        [TestMethod]
        public void Delete_DeleteClientFromDatabase_List()
        {
            Stylist newStylist = new Stylist("adam",1);
            Client newClient01 = new Client("name1", 1, newStylist.GetId());
            Client newClient02  = new Client("name2", 1, newStylist.GetId());
            newClient01.Save();
            newClient02.Save();

            newClient01.Delete();
            List<Client> result = Client.GetAll();
            List<Client> newList = new List<Client> {newClient02};

            CollectionAssert.AreEqual(newList, result);
        }

        

    }
}
