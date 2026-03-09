using AdoBuddy.Services;
using AdoBuddy.ViewModels;

namespace AdoBuddy.Views
{
    public partial class WorkItemsPage : ContentPage, IQueryAttributable
    {
        private readonly WorkItemsViewModel _vm;
        private readonly ProjectContext _projectContext;

        public WorkItemsPage(WorkItemsViewModel viewModel, ProjectContext projectContext)
        {
            InitializeComponent();
            BindingContext = _vm = viewModel;
            _projectContext = projectContext;
        }

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.TryGetValue("projectName", out var name))
            {
                _vm.ProjectName = Uri.UnescapeDataString(name.ToString() ?? string.Empty);
                _projectContext.ProjectName = _vm.ProjectName;
            }
            if (query.TryGetValue("projectId", out var id))
            {
                _vm.ProjectId = Uri.UnescapeDataString(id.ToString() ?? string.Empty);
                _projectContext.ProjectId = _vm.ProjectId;
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (string.IsNullOrWhiteSpace(_vm.ProjectName) && !string.IsNullOrWhiteSpace(_projectContext.ProjectName))
                _vm.ProjectName = _projectContext.ProjectName;
        }
    }
}
