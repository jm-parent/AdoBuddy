using System.Collections.ObjectModel;
using AdoBuddy.Models;
using AdoBuddy.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace AdoBuddy.ViewModels
{
    public partial class PipelinesViewModel : BaseViewModel
    {
        private readonly IAzureDevOpsService _service;

        public ObservableCollection<PipelineRun> PipelineRuns { get; } = new();

        [ObservableProperty]
        public partial string SelectedProject { get; set; }

        public PipelinesViewModel(IAzureDevOpsService service)
        {
            _service = service;
            Title = "Pipelines";
            SelectedProject = string.Empty;
        }

        [RelayCommand]
        private async Task LoadPipelinesAsync()
        {
            if (IsBusy) return;
            IsBusy = true;
            try
            {
                var runs = await _service.GetPipelineRunsAsync(SelectedProject);
                PipelineRuns.Clear();
                foreach (var run in runs)
                    PipelineRuns.Add(run);
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
