using System;
using System.ComponentModel;

namespace Mills.Models
{
    public class NotifyPropertyModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void NotifyPropertyChanged(string propertyName)
        {
            if (string.IsNullOrEmpty(propertyName))
                throw new ArgumentException("Argument 'propertyName' cannot be null or empty.");

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
