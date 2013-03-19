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
using System.ComponentModel;

namespace SkypeTest.Engine.Models
{
    /// <summary>
    /// A model class for encapsulating the contact data received from the server.
    /// This class inherits from INotifyPropertyChanged so that any views displaying
    /// this model automatically get updated with a property changes
    /// </summary>
    public class Contact : INotifyPropertyChanged
    {
        private int _id;
        public int id
        {
            get
            {
                return _id;
            }
            set
            {
                if (value != _id)
                {
                    _id = value;
                    NotifyPropertyChanged("id");
                }
            }
        }

        private string _skypename;
        public string skypename
        {
            get
            {
                return _skypename;
            }
            set
            {
                if (value != _skypename)
                {
                    _skypename = value;
                    NotifyPropertyChanged("skypename");
                }
            }
        }

        private string _displayname;
        public string displayname
        {
            get
            {
                return _displayname;
            }
            set
            {
                if (value != _displayname)
                {
                    _displayname = value;
                    NotifyPropertyChanged("displayname");
                }
            }
        }

        private string _message;
        public string message
        {
            get
            {
                return _message;
            }
            set
            {
                if (value != _message)
                {
                    _message = value;
                    NotifyPropertyChanged("message");
                }
            }
        }

        private string _avataruri;
        public string avataruri
        {
            get
            {
                return _avataruri;
            }
            set
            {
                if (value != _avataruri)
                {
                    _avataruri = value;
                    NotifyPropertyChanged("avataruri");
                }
            }
        }

        private string _status;
        public string status
        {
            get
            {
                return _status;
            }
            set
            {
                if (value != _status)
                {
                    _status = value;
                    NotifyPropertyChanged("status");
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        // This method is called by the Set accessor of each property. 
        private void NotifyPropertyChanged(String propertyName)
        {
            Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    if (PropertyChanged != null)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
                    }
                });
        }
    }

    /// <summary>
    /// The list of contact objects received form the server
    /// </summary>
    public class ContactsResult
    {
        public List<Contact> list { get; set; }
    }
}
