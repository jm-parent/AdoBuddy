using System.Collections.ObjectModel;
using AdoBuddy.Models;
using AdoBuddy.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace AdoBuddy.ViewModels
{
    public partial class ProjectsViewModel : BaseViewModel
    {
        private readonly IAzureDevOpsService _service;

        public ObservableCollection<AzureDevOpsProject> Projects { get; } = [];

        [ObservableProperty]
        public partial string ErrorMessage { get; set; }

        public bool IsNotBusy => !IsBusy;

        /// <summary>Set by the view when user taps a project; code-behind handles navigation.</summary>
        [ObservableProperty]
        public partial AzureDevOpsProject? SelectedProject { get; set; }

        public ProjectsViewModel(IAzureDevOpsService service)
        {
            _service = service;
            ErrorMessage = string.Empty;
            Title = "Select a Project";
            PropertyChanged += (_, e) =>
            {
                if (e.PropertyName == nameof(IsBusy))
                    OnPropertyChanged(nameof(IsNotBusy));
            };
        }

        [RelayCommand]
        private async Task LoadProjectsAsync()
        {
            if (IsBusy) return;
            IsBusy = true;
            ErrorMessage = string.Empty;
            Projects.Clear();
            try
            {
                var projects = await _service.GetProjectsAsync();
                foreach (var project in projects)
                    Projects.Add(project);
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Failed to load projects: {ex.Message}";
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}

