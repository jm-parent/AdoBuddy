using AdoBuddy.ViewModels;

namespace AdoBuddy.Views
{
    public partial class PullRequestsPage : ContentPage
    {
        public PullRequestsPage(PullRequestsViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }
    }
}
