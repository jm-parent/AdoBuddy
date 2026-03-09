using AdoBuddy.ViewModels;

namespace AdoBuddy.Views
{
    public partial class ProjectsPage : ContentPage
    {
        public ProjectsPage(ProjectsViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
            viewModel.PropertyChanged += async (s, e) =>
            {
                if (e.PropertyName == nameof(ProjectsViewModel.SelectedProject) && viewModel.SelectedProject != null)
                {
                    var proj = viewModel.SelectedProject;
                    viewModel.SelectedProject = null; // reset for next selection
                    await Shell.Current.GoToAsync(
                        $"//ProjectShell?projectId={Uri.EscapeDataString(proj.Id)}&projectName={Uri.EscapeDataString(proj.Name)}");
                }
            };
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (BindingContext is ProjectsViewModel vm)
                vm.LoadProjectsCommand.Execute(null);
        }
    }
}
