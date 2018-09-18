using System.ComponentModel;
using System.Windows.Ink;

namespace WpfApp3
{
    class ViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private string meaInfo;
        public string MeaInfo
        {
            get => meaInfo;
            set
            {
                meaInfo = value;
                OnPropertyChanged("MeaInfo");
            }
        }

        private StrokeCollection inkStrokes;
        public StrokeCollection InkStrokes
        {
            get => inkStrokes;
            set
            {
                inkStrokes = value;
                OnPropertyChanged("InkStrokes");
            }
        }
    }
}