using CommunityToolkit.Mvvm.Input;

namespace AdoBuddy.ViewModels
{
    public partial class MainViewModel : BaseViewModel
    {
        public int Count { get; private set; }

        public MainViewModel()
        {
            Title = "Click me";
        }

        [RelayCommand]
        private void CounterClicked()
        {
            Count++;
            Title = Count == 1 ? $"Clicked {Count} time" : $"Clicked {Count} times";
#if ANDROID || IOS || MACCATALYST || WINDOWS
            SemanticScreenReader.Announce(Title);
#endif
        }
    }
}
