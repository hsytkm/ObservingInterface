using Prism.Mvvm;

namespace ObservingInterface.Models
{
    class Person : BindableBase, IPerson
    {
        private string _name;
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        private bool _isInput;
        public bool IsInput
        {
            get => _isInput;
            set => SetProperty(ref _isInput, value);
        }

        public void SetName(string name)
        {
            this.Name = name;
            this.IsInput = GetIsInput();
        }

        public bool GetIsInput()
        {
            return !string.IsNullOrWhiteSpace(this.Name);
        }
    }
}
