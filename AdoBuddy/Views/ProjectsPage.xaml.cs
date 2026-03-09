using AdoBuddy.Models;
using AdoBuddy.ViewModels;

namespace AdoBuddy.Views
{
    public partial class ProjectsPage : ContentPage
    {
        public ProjectsPage(ProjectsViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (BindingContext is ProjectsViewModel vm)
                vm.LoadProjectsCommand.Execute(null);
        }

        private async void OnProjectSelected(object? sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection.FirstOrDefault() is AzureDevOpsProject proj)
            {
                // Reset selection
                ((CollectionView)sender).SelectedItem = null;
                await Shell.Current.GoToAsync(
                    $"//ProjectShell?projectId={Uri.EscapeDataString(proj.Id)}&projectName={Uri.EscapeDataString(proj.Name)}");
            }
        }
    }
}
