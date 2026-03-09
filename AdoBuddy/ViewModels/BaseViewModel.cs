using CommunityToolkit.Mvvm.ComponentModel;

namespace AdoBuddy.ViewModels
{
    public partial class BaseViewModel : ObservableObject
    {
        [ObservableProperty]
        public partial bool IsBusy { get; set; }

        [ObservableProperty]
        public partial string Title { get; set; }

        protected BaseViewModel()
        {
            Title = string.Empty;
        }
    }
}
