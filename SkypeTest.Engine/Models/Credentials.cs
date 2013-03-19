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

namespace SkypeTest.Engine.Models
{
    /// <summary>
    /// A struct for encapsulating the login credentials of a user
    /// </summary>
    public struct Credentials
    {
        private String _username;
        public String Username
        {
            get
            {
                return _username;
            }
            private set
            {
                if (value != _username)
                    _username = value;
            }
        }

        private String _password;
        public String Password
        {
            get
            {
                return _password;
            }
            private set
            {
                if (value != _password)
                    _password = value;
            }
        }

        public Credentials(String username, String password)
        {
            _username = username;
            _password = password;
        }
    }
}
