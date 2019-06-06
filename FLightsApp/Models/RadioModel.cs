using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace FLightsApp.Models
{
	public class RadioModel: INotifyPropertyChanged
    {
        public string Title { get; set; }
        private bool _isSelected { get; set; }
        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                if (value != _isSelected)
                {
                    this._isSelected = value;
                    NotifyPropertyChanged();
                }
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            if (PropertyChanged != null)
            {      
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
