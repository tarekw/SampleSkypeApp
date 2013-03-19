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
using System.ComponentModel;

namespace SkypeTest.ViewModels
{
    /// <summary>
    /// Each page in the application has the following three states.
    /// Loading - when the page is in the process of communicating with the server
    /// EndLoading - when data is has been retrieved and available for display
    /// Error - there was an error (e.g a network error)
    /// </summary>
    public enum CurrentState
    {
        Loading = 0,
        EndLoading,
        Error
    };

    public class StateChangedEventArgs : EventArgs
    {
        private CurrentState _state;

        public StateChangedEventArgs(CurrentState state)
        {
            this._state = state;
        }

        public CurrentState State
        {
            get { return _state; }
        }
    }

    public delegate void StateChangedEventHandler(object sender, StateChangedEventArgs e);

    /// <summary>
    /// A base class for all viewmodels. It contains a minimal set of properties and events
    /// that are applicable to all viewmodels in this application
    /// </summary>
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        public bool IsDataLoaded
        {
            get;
            protected set;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void NotifyPropertyChanged(String propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public event StateChangedEventHandler StateChanged;
        protected virtual void NotifyStateChanged(CurrentState state)
        {
            StateChangedEventHandler handler = StateChanged;
            if (null != handler)
            {
                handler(this, new StateChangedEventArgs(state));
            }
        }
    }
}
