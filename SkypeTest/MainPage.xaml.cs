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
using Microsoft.Phone.Shell;
using SkypeTest.Resources;
using SkypeTest.ViewModels;
using System.ComponentModel;
using System.Windows.Controls.Primitives;
using System.IO.IsolatedStorage;
using SkypeTest.Templates;

namespace SkypeTest
{
    public partial class MainPage : PhoneApplicationPage
    {
        private PeopleViewModel _viewModel = null;
        private LongListSelector currentSelector;
        private Popup _signinPopup;

        // Constructor
        public MainPage()
        {
            InitializeComponent();

            _viewModel = new PeopleViewModel();
            this.DataContext = _viewModel;
            _viewModel.StateChanged += new StateChangedEventHandler(_viewModel_StateChanged);

            this.Loaded += new RoutedEventHandler(MainPage_Loaded);
            _viewModel.PropertyChanged += new PropertyChangedEventHandler(PropertyChanged_event);
        }

        private void PropertyChanged_event(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "DataReady")
            {
                ContactsListBox.ItemsSource = _viewModel.GroupedContacts;
            }
        }

        void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            if (!_viewModel.IsDataLoaded)
            {
                IsolatedStorageSettings settings = IsolatedStorageSettings.ApplicationSettings;
                if (!settings.Contains("Username") || !settings.Contains("Password"))
                    return; // user hasn't been verified yet

                _viewModel.LoadData(null);
                _viewModel.StartUpdates();
            }
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            IsolatedStorageSettings settings = IsolatedStorageSettings.ApplicationSettings;
            if (!settings.Contains("Username") || !settings.Contains("Password"))
            {
                // user hasn't been verified yet
                _signinPopup = new Popup();
                _signinPopup.Child = new LoginPage();
                (_signinPopup.Child as LoginPage).LoggedIn += new LoginPage.DoneEventHandler(MainPage_LoggedIn);
                _signinPopup.IsOpen = true;
            }
        }

        void MainPage_LoggedIn()
        {
            IsolatedStorageSettings settings = IsolatedStorageSettings.ApplicationSettings;
            if (!settings.Contains("Username") || !settings.Contains("Password"))
                return;

            this.Dispatcher.BeginInvoke(() =>
            {
                if (_signinPopup != null && _signinPopup.IsOpen)
                {
                    MainPage_Loaded(this, new RoutedEventArgs());
                    _signinPopup.IsOpen = false;
                }
            });
        }

        /// <summary>
        /// Dynamically change the state of the page depending on the outcome of the viewmodels request
        /// </summary>
        /// <param name="sender">The viewmodel</param>
        /// <param name="e">The the viewmodel is in</param>
        private void _viewModel_StateChanged(object sender, StateChangedEventArgs e)
        {
            switch (e.State)
            {
                case CurrentState.Loading:
                    VisualStateManager.GoToState(this, "LoadingState", false);
                    break;
                case CurrentState.EndLoading:
                    VisualStateManager.GoToState(this, "EndLoadingState", false);
                    ApplicationBar.IsVisible = true;
                    break;
                case CurrentState.Error:
                    VisualStateManager.GoToState(this, "ErrorState", false);
                    break;
                default:
                    break;
            }
        }

        private void Appbar_Click(object sender, EventArgs e)
        {
            MessageBox.Show(Labels.NotImplemented);
        }

        private void ApplicationBar_StateChanged(object sender, Microsoft.Phone.Shell.ApplicationBarStateChangedEventArgs e)
        {
            if (sender != null && sender is ApplicationBar)
                ((ApplicationBar)sender).Opacity = e.IsMenuVisible ? 1.0 : 0.5;
        }

        // This code is from the LongListSelector sample
        private void LongListSelector_GroupViewOpened(object sender, GroupViewOpenedEventArgs e)
        {
            //Hold a reference to the active long list selector.
            currentSelector = sender as LongListSelector;

            //Construct and begin a swivel animation to pop in the group view.
            IEasingFunction quadraticEase = new QuadraticEase { EasingMode = EasingMode.EaseOut };
            Storyboard _swivelShow = new Storyboard();
            ItemsControl groupItems = e.ItemsControl;

            foreach (var item in groupItems.Items)
            {
                UIElement container = groupItems.ItemContainerGenerator.ContainerFromItem(item) as UIElement;
                if (container != null)
                {
                    Border content = VisualTreeHelper.GetChild(container, 0) as Border;
                    if (content != null)
                    {
                        DoubleAnimationUsingKeyFrames showAnimation = new DoubleAnimationUsingKeyFrames();

                        EasingDoubleKeyFrame showKeyFrame1 = new EasingDoubleKeyFrame();
                        showKeyFrame1.KeyTime = TimeSpan.FromMilliseconds(0);
                        showKeyFrame1.Value = -60;
                        showKeyFrame1.EasingFunction = quadraticEase;

                        EasingDoubleKeyFrame showKeyFrame2 = new EasingDoubleKeyFrame();
                        showKeyFrame2.KeyTime = TimeSpan.FromMilliseconds(85);
                        showKeyFrame2.Value = 0;
                        showKeyFrame2.EasingFunction = quadraticEase;

                        showAnimation.KeyFrames.Add(showKeyFrame1);
                        showAnimation.KeyFrames.Add(showKeyFrame2);

                        Storyboard.SetTargetProperty(showAnimation, new PropertyPath(PlaneProjection.RotationXProperty));
                        Storyboard.SetTarget(showAnimation, content.Projection);

                        _swivelShow.Children.Add(showAnimation);
                    }
                }
            }

            _swivelShow.Begin();
        }

        // This code is from the LongListSelector sample
        private void LongListSelector_GroupViewClosing(object sender, GroupViewClosingEventArgs e)
        {
            //Cancelling automatic closing and scrolling to do it manually.
            e.Cancel = true;
            if (e.SelectedGroup != null)
            {
                currentSelector.ScrollToGroup(e.SelectedGroup);
            }

            //Dispatch the swivel animation for performance on the UI thread.
            Dispatcher.BeginInvoke(() =>
            {
                //Construct and begin a swivel animation to pop out the group view.
                IEasingFunction quadraticEase = new QuadraticEase { EasingMode = EasingMode.EaseOut };
                Storyboard _swivelHide = new Storyboard();
                ItemsControl groupItems = e.ItemsControl;

                foreach (var item in groupItems.Items)
                {
                    UIElement container = groupItems.ItemContainerGenerator.ContainerFromItem(item) as UIElement;
                    if (container != null)
                    {
                        Border content = VisualTreeHelper.GetChild(container, 0) as Border;
                        if (content != null)
                        {
                            DoubleAnimationUsingKeyFrames showAnimation = new DoubleAnimationUsingKeyFrames();

                            EasingDoubleKeyFrame showKeyFrame1 = new EasingDoubleKeyFrame();
                            showKeyFrame1.KeyTime = TimeSpan.FromMilliseconds(0);
                            showKeyFrame1.Value = 0;
                            showKeyFrame1.EasingFunction = quadraticEase;

                            EasingDoubleKeyFrame showKeyFrame2 = new EasingDoubleKeyFrame();
                            showKeyFrame2.KeyTime = TimeSpan.FromMilliseconds(125);
                            showKeyFrame2.Value = 90;
                            showKeyFrame2.EasingFunction = quadraticEase;

                            showAnimation.KeyFrames.Add(showKeyFrame1);
                            showAnimation.KeyFrames.Add(showKeyFrame2);

                            Storyboard.SetTargetProperty(showAnimation, new PropertyPath(PlaneProjection.RotationXProperty));
                            Storyboard.SetTarget(showAnimation, content.Projection);

                            _swivelHide.Children.Add(showAnimation);
                        }
                    }
                }

                _swivelHide.Completed += _swivelHide_Completed;
                _swivelHide.Begin();

            });
        }

        // This code is from the LongListSelector sample
        private void _swivelHide_Completed(object sender, EventArgs e)
        {
            //Close group view.
            if (currentSelector != null)
            {
                currentSelector.CloseGroupView();
                currentSelector = null;
            }
        }
    }
}