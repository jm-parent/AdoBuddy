using AdoBuddy.ViewModels;

namespace AdoBuddy.Views
{
    public partial class PipelinesPage : ContentPage
    {
        public PipelinesPage(PipelinesViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }
    }
}
