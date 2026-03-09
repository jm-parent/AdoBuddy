using CommunityToolkit.Mvvm.Input;

namespace AdoBuddy.ViewModels
{
    public partial class MainViewModel : BaseViewModel
    {
        public int Count { get; private set; }

        [RelayCommand]
        private void CounterClicked()
        {
            Count++;
            Title = Count == 1 ? $"Clicked {Count} time" : $"Clicked {Count} times";
        }
    }
}
