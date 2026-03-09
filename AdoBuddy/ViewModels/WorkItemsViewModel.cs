using System.Collections.ObjectModel;
using AdoBuddy.Models;
using AdoBuddy.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace AdoBuddy.ViewModels
{
    public partial class WorkItemsViewModel : BaseViewModel
    {
        private readonly IAzureDevOpsService _service;

        public ObservableCollection<WorkItem> WorkItems { get; } = [];

        [ObservableProperty]
        public partial string ProjectName { get; set; }

        [ObservableProperty]
        public partial string ProjectId { get; set; }

        [ObservableProperty]
        public partial string ErrorMessage { get; set; }

        public WorkItemsViewModel(IAzureDevOpsService service)
        {
            _service = service;
            ProjectName = string.Empty;
            ProjectId = string.Empty;
            ErrorMessage = string.Empty;
            Title = "Backlog";
        }

        partial void OnProjectNameChanged(string value)
        {
            if (!string.IsNullOrWhiteSpace(value))
                LoadWorkItemsCommand.Execute(null);
        }

        [RelayCommand]
        private async Task LoadWorkItemsAsync()
        {
            if (IsBusy) return;
            IsBusy = true;
            ErrorMessage = string.Empty;
            WorkItems.Clear();
            try
            {
                var items = await _service.GetWorkItemsAsync(ProjectName);
                foreach (var item in items)
                    WorkItems.Add(item);
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Failed to load work items: {ex.Message}";
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
