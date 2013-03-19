using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Silverlight.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SkypeTest.ViewModels;
using System.Threading.Tasks;
using System.Collections.Generic;
using SkypeTest.Engine.Models;
using SkypeTest.Engine.Enablers;

namespace SkypeTest.Backend.UnitTests
{
    /// <summary>
    /// This TestClass is for testing the different viewmodels.
    /// </summary>
    [TestClass]
    public class ViewModelTests : SilverlightTest
    {
        [TestMethod]
        [Description("Tests the result from the contacts load request without any wait")]
        public void ContactsRequestWithoutWait()
        {
            PeopleViewModel vm = new PeopleViewModel();
            vm.LoadData(null);
            // This is supposed to be empty since LoadData hasn't completed the task yet
            Assert.AreEqual(0, vm.GroupedContacts.Count, "GroupedContacts list is not empty");
            vm.GroupedContacts.Clear();
            vm.Groups.Clear();
        }

        [TestMethod]
        [Description("Tests contacts LoadData with different ContactsResult parameters")]
        public void ContactsRequestWithParams()
        {
            PeopleViewModel vm = new PeopleViewModel();
            vm.LoadData(new Engine.Models.ContactsResult());
            Assert.AreEqual(0, vm.GroupedContacts.Count, "GroupedContacts list is not empty");
            vm.GroupedContacts.Clear();
            vm.Groups.Clear();
            List<Contact> l = new List<Contact>();
            l.Add(new Contact { displayname = "albert" });
            vm.LoadData(new Engine.Models.ContactsResult { list = l});
            Assert.AreEqual(1, vm.GroupedContacts.Count, "GroupContacts list is more than one");
            Assert.AreEqual("a", vm.GroupedContacts[0].Key, "Group has invalid key. key should be \"a\"");
            vm.GroupedContacts.Clear();
            vm.Groups.Clear();
            List<Contact> l2 = new List<Contact>();
            l2.Add(new Contact { displayname = "007" });
            vm.LoadData(new Engine.Models.ContactsResult { list = l2 });
            Assert.AreEqual(1, vm.GroupedContacts.Count, "GroupContacts list is not one");
            Assert.AreEqual("#", vm.GroupedContacts[0].Key, "Group has invalid key. key should be \"#\"");
            vm.GroupedContacts.Clear();
            vm.Groups.Clear();
            List<Contact> l3 = new List<Contact>();
            l3.Add(new Contact { displayname = "" });
            vm.LoadData(new Engine.Models.ContactsResult { list = l3 });
            Assert.AreEqual(1, vm.GroupedContacts.Count, "GroupContacts list is not one");
            Assert.AreEqual("#", vm.GroupedContacts[0].Key, "Group has invalid key. key should be \"#\"");
            vm.GroupedContacts.Clear();
            vm.Groups.Clear();
            List<Contact> l4 = new List<Contact>();
            l4.Add(new Contact { displayname = "   " });
            vm.LoadData(new Engine.Models.ContactsResult { list = l4 });
            Assert.AreEqual(1, vm.GroupedContacts.Count, "GroupContacts list is not one");
            Assert.AreEqual("#", vm.GroupedContacts[0].Key, "Group has invalid key. key should be \"#\"");
            vm.GroupedContacts.Clear();
            vm.Groups.Clear();
            List<Contact> l5 = new List<Contact>();
            l5.Add(new Contact { displayname = "¬!\"£$%^&*(+_)(*&" });
            l5.Add(new Contact { displayname = "1234567890" });
            vm.LoadData(new Engine.Models.ContactsResult { list = l5 });
            Assert.AreEqual(1, vm.GroupedContacts.Count, "GroupContacts list is not one");
            Assert.AreEqual("#", vm.GroupedContacts[0].Key, "Group has invalid key. key should be \"#\"");
            Assert.AreEqual(2, vm.GroupedContacts[0].Count, "The group doesn't contain 2 elements");
            vm.GroupedContacts.Clear();
            vm.Groups.Clear();
        }

        [TestMethod]
        [Asynchronous]
        [Description("Tests the result from the contacts load request after waiting fiew seconds")]
        public async void ContactsRequest()
        {
            PeopleViewModel vm = new PeopleViewModel();
            vm.LoadData(null);
            await TaskEx.WhenAny(TaskEx.Delay(TimeSpan.FromSeconds(4)));    // we know the local task will complete in 3 seconds

            Assert.AreNotEqual(0, vm.GroupedContacts.Count, "GroupContacts list is empty");
            Assert.AreEqual(7, vm.GroupedContacts.Count, "GroupContacts list contains invalid number of groups");
            foreach (var group in vm.GroupedContacts)
            {
                Assert.AreNotEqual(0, group.HasItems, "Group is empty");
                foreach (var c in group)
                {
                    Assert.IsFalse(String.IsNullOrWhiteSpace(c.displayname), "displayname is invalid");
                    Assert.IsFalse(String.IsNullOrWhiteSpace(c.message), "message is invalid");
                    Assert.IsFalse(String.IsNullOrWhiteSpace(c.skypename), "skypename is invalid");
                    Assert.IsFalse(String.IsNullOrWhiteSpace(c.status), "status is empty");
                    Assert.IsTrue(c.status == "online" || c.status == "offline" || c.status == "away", "status is invalid");
                    Assert.IsInstanceOfType(c.displayname, typeof(String), "displayname is invalid type");
                    Assert.IsInstanceOfType(c.message, typeof(String), "message is invalid type");
                    Assert.IsInstanceOfType(c.skypename, typeof(String), "skypename is invalid type");
                    Assert.IsInstanceOfType(c.status, typeof(String), "status is invalid type");
                    Assert.IsInstanceOfType(c.id, typeof(int), "id is invalid type");
                }
            }
            vm.GroupedContacts.Clear();
            vm.Groups.Clear();
            this.EnqueueTestComplete();
        }

        [TestMethod]
        [Asynchronous]
        [Description("Tests the VerifyCrednetials with different inputs")]
        public async void VerifyCrednetials()
        {
            LoginViewModel vm = new LoginViewModel();
            vm.VerifyCrednetials(new Credentials( "", "" ));
            await TaskEx.WhenAny(TaskEx.Delay(TimeSpan.FromSeconds(4)));    // we know the local task will complete in 3 seconds
            Assert.IsFalse(vm.Verified, "Verified wrong credentials");
            vm.VerifyCrednetials(new Credentials("Tarek", "tarek"));
            await TaskEx.WhenAny(TaskEx.Delay(TimeSpan.FromSeconds(4)));
            Assert.IsFalse(vm.Verified, "Verified wrong credentials");
            vm.VerifyCrednetials(new Credentials("tarek", "Tarek"));
            await TaskEx.WhenAny(TaskEx.Delay(TimeSpan.FromSeconds(4)));
            Assert.IsFalse(vm.Verified, "Verified wrong credentials");
            vm.VerifyCrednetials(new Credentials("tarek", "tarek"));
            await TaskEx.WhenAny(TaskEx.Delay(TimeSpan.FromSeconds(4)));
            Assert.IsTrue(vm.Verified, "Could not verify credentials");

            this.EnqueueTestComplete();
        }
    }

    /// <summary>
    /// This TestClass for testing the engine apis directly.
    /// </summary>
    [TestClass]
    public class BackendEnablerTests : SilverlightTest
    {
        [TestMethod]
        [Asynchronous]
        [Description("Tests the StartDataRequest api with local protocol")]
        public async void StartDataRequestLocal()
        {
            // Test with the valid JSON file
            DataFetcher dataFetcher = DataFetcher.CreateDataFetcher(Protocol.LOCAL);
            dataFetcher.address = "/SkypeTest;component/SampleData/samplecontacts.txt";
            ContactsResult contactsResult = await BackendEnabler.Default.StartDataRequest<ContactsResult>(dataFetcher);
            Assert.IsNotNull(contactsResult, "ContactsResult is null");
            Assert.IsNotNull(contactsResult.list, "ContactsResult.list is null");
            Assert.AreEqual(10, contactsResult.list.Count, "ContactsResult.list is the wrong length");

            // Test with an invalid JSON file
            dataFetcher.address = "/SkypeTest;component/SampleData/samplecorruptjson.txt";
            ContactsResult defaultContactsResult = await BackendEnabler.Default.StartDataRequest<ContactsResult>(dataFetcher);
            Assert.IsNull(defaultContactsResult, "ContactsResult is not null");

            // Test with an invalid url
            dataFetcher.address = "/SkypeTest;component/SampleData/sample.txt";
            defaultContactsResult = await BackendEnabler.Default.StartDataRequest<ContactsResult>(dataFetcher);
            Assert.IsNull(defaultContactsResult, "ContactsResult is not null");

            // Test other models here directly by replacing the template parameter
            this.EnqueueTestComplete();
        }

        [TestMethod]
        [Description("Tests the StartDataRequest api with remote protocol")]
        public void StartDataRequestRemote()
        {
            // no real protocols are currently supported. write tests here once the implementation is ready
        }

        [TestMethod]
        [Asynchronous]
        [Description("Tests the StartLoginVerification local protocol")]
        public async void StartLoginVerificationLocal()
        {
            LoginVerifier verifier = LoginVerifier.CreateLoginVerifier(Protocol.LOCAL);
            verifier.credentials = new Credentials("tarek", "tarek");
            bool ret = await BackendEnabler.Default.StartLoginVerification(verifier);
            Assert.IsTrue(ret, "Could not verify credentials");

            this.EnqueueTestComplete();
        }

        [TestMethod]
        [Description("Tests the StartLoginVerification remote protocol")]
        public void StartLoginVerificationRemote()
        {
            // no real protocols are currently supported. write tests here once the implementation is ready
        }
    }
}
