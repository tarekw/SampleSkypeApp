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

namespace SkypeTest.Resources
{
    public class LabelsManager : INotifyPropertyChanged
    {
        public Labels Labels { get; set; }
        public LabelsManager()
        {
            Labels = new Labels();
        }

        public void ResetResources()
        {
            OnPropertyChanged("Labels");
        }


        #region INotifyPropertyChanged region
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string property)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }
        #endregion
    }
}
