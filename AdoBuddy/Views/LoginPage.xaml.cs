using AdoBuddy.ViewModels;

namespace AdoBuddy.Views
{
    public partial class LoginPage : ContentPage
    {
        public LoginPage(LoginViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
            viewModel.PropertyChanged += async (s, e) =>
            {
                if (e.PropertyName == nameof(LoginViewModel.IsLoginSuccessful) && viewModel.IsLoginSuccessful)
                    await Shell.Current.GoToAsync("//ProjectsPage");
            };
        }
    }
}
