using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using SkypeTest.Resources;
using System.IO.IsolatedStorage;
using SkypeTest.Engine.Enablers;
using SkypeTest.ViewModels;
using SkypeTest.Engine.Models;

namespace SkypeTest.Templates
{
    public partial class LoginPage : UserControl
    {
        private LoginViewModel _viewModel = null;

        public delegate void DoneEventHandler();

        public LoginPage()
        {
            InitializeComponent();

            _viewModel = new LoginViewModel();
            this.DataContext = _viewModel;
            _viewModel.StateChanged += new StateChangedEventHandler(_viewModel_StateChanged);
        }

        private void _viewModel_StateChanged(object sender, StateChangedEventArgs e)
        {
            switch (e.State)
            {
                case CurrentState.Loading:
                    Overlay.Visibility = System.Windows.Visibility.Visible;
                    ProgressBar.Visibility = System.Windows.Visibility.Visible;
                    break;
                case CurrentState.EndLoading:
                    IsolatedStorageSettings settings = IsolatedStorageSettings.ApplicationSettings;
                    settings["Username"] = usernamebox.Text;
                    settings["Password"] = pwdbox.Password;
                    NotifyLoggedIn();
                    break;
                case CurrentState.Error:
                    Overlay.Visibility = System.Windows.Visibility.Collapsed;
                    ProgressBar.Visibility = System.Windows.Visibility.Collapsed;
                    MessageBox.Show(Labels.LoginError);
                    break;
                default:
                    break;
            }
        }

        // Doing some very simple validation here
        private void textbox_SelectionChanged(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrWhiteSpace(usernamebox.Text) || String.IsNullOrWhiteSpace(pwdbox.Password))
                signinbutton.IsEnabled = false;
            else
                signinbutton.IsEnabled = true;
        }

        private void signinbutton_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.VerifyCrednetials(new Credentials(usernamebox.Text, pwdbox.Password));
        }

        public event DoneEventHandler LoggedIn;
        private void NotifyLoggedIn()
        {
            DoneEventHandler handler = LoggedIn;
            if (null != handler)
            {
                handler();
            }
        }
    }
}
