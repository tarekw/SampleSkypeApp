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
using System.Collections.Generic;
using SkypeTest.Engine.Models;
using System.Threading;
using System.Diagnostics;

namespace SkypeTest.Engine.Listeners
{
    public class ContactChangedEventArgs : EventArgs
    {
        private Contact _contact;

        public ContactChangedEventArgs(Contact contact)
        {
            this._contact = contact;
        }

        public Contact Contact
        {
            get { return _contact; }
        }
    }

    // Delegate declaration.
    public delegate void ContactChangedEventHandler(object sender, ContactChangedEventArgs e);

    /// <summary>
    /// This is a class for simulating random contact status updates. It cycles through the available contacts and
    /// and changes the status of each contact. In reality we would be listening for RawNotification updates from
    /// the Microsoft Push Notification Service
    /// </summary>
    public class ContactsUpdateListner
    {
        private List<Contact> _contacts;
        private Timer _timer;
        private int _index = 0;
        private Random _rand;

        public event ContactChangedEventHandler ContactChanged;

        public ContactsUpdateListner(List<Contact> contacts)
        {
            _contacts = contacts;
            _rand = new Random();
            _timer = new Timer(UpdateContact);
        }

        public void StartRandomUpdates()
        {
            _timer.Change(_rand.Next(5, 10) * 1000, Timeout.Infinite);
        }

        private void UpdateContact(object state)
        {
            _index = _index == _contacts.Count ? 0 : _index;
            Contact c = _contacts[_index++];
            // updatedContact is the new contact data received from the push notification
            Contact updatedContact = new Contact { id = c.id, displayname = c.displayname, avataruri = c.avataruri, message = c.message, skypename = c.skypename };

            // cycle through different states for each contact
            if (c.status == "online")
                updatedContact.status = "away";
            else if (c.status == "away")
                updatedContact.status = "offline";
            else
                updatedContact.status = "online";

            NotifyContactChanged(updatedContact);
            _timer.Change(_rand.Next(5, 10) * 1000, Timeout.Infinite);
        }

        private void NotifyContactChanged(Contact contact)
        {
            ContactChangedEventHandler handler = ContactChanged;
            if (null != handler)
            {
                handler(this, new ContactChangedEventArgs(contact));
            }
        }
    }
}
