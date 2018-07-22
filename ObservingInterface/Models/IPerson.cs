using System.ComponentModel;

namespace ObservingInterface.Models
{
    interface IPerson : INotifyPropertyChanged
    {
        bool IsInput { get; set; }
        void SetName(string name);
        bool GetIsInput();
    }
}
