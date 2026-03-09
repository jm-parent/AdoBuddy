using System.Collections.ObjectModel;
using AdoBuddy.Models;
using AdoBuddy.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace AdoBuddy.ViewModels
{
#if ANDROID || IOS || MACCATALYST || WINDOWS
    [Microsoft.Maui.Controls.QueryProperty(nameof(ProjectName), "projectName")]
    [Microsoft.Maui.Controls.QueryProperty(nameof(ProjectId), "projectId")]
#endif
    public partial class PipelinesViewModel : BaseViewModel
    {
        private readonly IAzureDevOpsService _service;

        public ObservableCollection<PipelineRun> PipelineRuns { get; } = new();

        [ObservableProperty]
        public partial string ErrorMessage { get; set; }

        public bool HasError => !string.IsNullOrEmpty(ErrorMessage);

        partial void OnErrorMessageChanged(string value) => OnPropertyChanged(nameof(HasError));

        private string _projectName = string.Empty;
        public string ProjectName
        {
            get => _projectName;
            set
            {
                _projectName = value;
                Title = value;
                if (!string.IsNullOrEmpty(value))
                    LoadPipelinesCommand.Execute(null);
            }
        }

        [ObservableProperty]
        public partial string ProjectId { get; set; }

        public PipelinesViewModel(IAzureDevOpsService service)
        {
            _service = service;
            Title = "Pipelines";
            ProjectId = string.Empty;
            ErrorMessage = string.Empty;
        }

        [RelayCommand]
        private async Task LoadPipelinesAsync()
        {
            if (IsBusy) return;
            IsBusy = true;
            ErrorMessage = string.Empty;
            try
            {
                var runs = await _service.GetPipelineRunsAsync(ProjectName);
                PipelineRuns.Clear();
                foreach (var run in runs)
                    PipelineRuns.Add(run);
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Failed to load pipelines: {ex.Message}";
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}

