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
using SkypeTest.Engine.Models;
using System.Collections.ObjectModel;
using SkypeTest.Engine.Enablers;
using System.Collections.Generic;
using SkypeTest.Data;
using System.Diagnostics;
using System.Threading;
using SkypeTest.Engine.Listeners;

namespace SkypeTest.ViewModels
{
    /// <summary>
    /// The viewmodel responsible for providing data for the contacts page.
    /// </summary>
    public class PeopleViewModel : ViewModelBase
    {
        private ContactsUpdateListner _updater = null;
        private bool _startUpdates = false;

        /// <summary>
        /// A collection for Contact objects. This will be used to populate the contacts for the user
        /// </summary>
        public ObservableCollection<ContactsInGroup> GroupedContacts { get; set; }
        public Dictionary<string, ContactsInGroup> Groups;
        
        private bool _dataReady;
        /// <summary>
        /// This property indicates that the grouped contacts have been sorted and ready for viewing
        /// </summary>
        public bool DataReady
        {
            get
            {
                return _dataReady;
            }
            set
            {
                if (value != _dataReady)
                {
                    _dataReady = value;
                    NotifyPropertyChanged("DataReady");
                }
            }
        }

        public PeopleViewModel()
        {
            this.GroupedContacts = new ObservableCollection<ContactsInGroup>();
            this.Groups = new Dictionary<string, ContactsInGroup>();
        }

        /// <summary>
        /// Asynchronously fetches contacts data from the Skype backend servers
        /// </summary>
        /// <param name="subsectionResult"></param>
        public async void LoadData(ContactsResult contactsResult)
        {
            if (contactsResult == null)
            {
                NotifyStateChanged(CurrentState.Loading);   // this should show the waiting state

                DataFetcher dataFetcher = null;
#if USING_LIVE_DATA
                // TODO: write the code for requesting live content using a REST api
#else
                dataFetcher = DataFetcher.CreateDataFetcher(Protocol.LOCAL);
                dataFetcher.address = "/SkypeTest;component/SampleData/samplecontacts.txt";
#endif
                contactsResult = await BackendEnabler.Default.StartDataRequest<ContactsResult>(dataFetcher);
            }

            if (contactsResult == null ||
                contactsResult.list == null ||
                contactsResult.list.Count == 0)
            {
                NotifyStateChanged(CurrentState.Error);   // this should display the error message as the download was unsuccessful
            }
            else
            {
                OrderByGroup(contactsResult.list);
                this.IsDataLoaded = true;
                NotifyStateChanged(CurrentState.EndLoading);   // this should get rid of the waiting state and show the landing page

                // This is test code for starting the listener
                _updater = new ContactsUpdateListner(contactsResult.list);
                if (_startUpdates)
                {
                    _updater.ContactChanged -= new ContactChangedEventHandler(_updater_ContactChanged);
                    _updater.ContactChanged += new ContactChangedEventHandler(_updater_ContactChanged);
                    _updater.StartRandomUpdates();  // start the simulation of random status updates
                }
            }
        }

        /// <summary>
        /// This is a test method for starting the simulation of random contact updates
        /// </summary>
        public void StartUpdates()
        {
            _startUpdates = true;
        }

        void _updater_ContactChanged(object sender, ContactChangedEventArgs e)
        {
            UpdateContact(e.Contact);
        }

        /// <summary>
        /// Find a contact in the list by id and update its properties. Changing the properties
        /// of the Contact model will automatically update the view
        /// </summary>
        /// <param name="updatedcontact">The contact to update</param>
        private void UpdateContact(Contact updatedcontact)
        {
            ContactsInGroup g;
            Groups.TryGetValue(updatedcontact.displayname[0].ToString().ToLower(), out g);
            if (g != null)
            {
                foreach (var c in (g as ObservableCollection<Contact>))
                {
                    if (c.id == updatedcontact.id)
                    {
                        c.status = updatedcontact.status;
                        Debug.WriteLine("Status updated for contact id :" + updatedcontact.id + "    displayname: " + updatedcontact.displayname);
                    }
                }
            }
        }

        private void OrderByGroup(List<Contact> contacts)
        {
            contacts.Sort(CompareByName);

            foreach (var contact in contacts)
            {
                string key = GetFirstNameKey(contact);

                if (!Groups.ContainsKey(key))
                {
                    ContactsInGroup group = new ContactsInGroup(key);
                    GroupedContacts.Add(group);
                    Groups[key] = group;
                }
                Groups[key].Add(contact);
            }
            DataReady = true;
        }

        private string GetFirstNameKey(Contact contact)
        {
            if (String.IsNullOrWhiteSpace(contact.displayname))
                return "#";

            char key = char.ToLower(contact.displayname[0]);

            if (key < 'a' || key > 'z')
            {
                key = '#';
            }

            return key.ToString();
        }

        private int CompareByName(Contact con1, Contact con2)
        {
            return con1.displayname.CompareTo(con2.displayname);
        }
    }
}
