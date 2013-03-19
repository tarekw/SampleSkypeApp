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
using Microsoft.Phone.Controls;
using Microsoft.Silverlight.Testing;
using SkypeTest.Backend.UnitTests;

namespace SkypeTest.UnitTest
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(MainPage_Loaded);
        }

        void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            var testSettings = UnitTestSystem.CreateDefaultSettings();
            testSettings.TestAssemblies.Add(typeof(ViewModelTests).Assembly);
            var testPage = UnitTestSystem.CreateTestPage(testSettings) as IMobileTestPage;
            this.BackKeyPress += (s, arg) => arg.Cancel = testPage.NavigateBack();
            (Application.Current.RootVisual as PhoneApplicationFrame).Content = testPage;
        }
    }
}