using AdoBuddy.ViewModels;

namespace AdoBuddy.Tests.ViewModels
{
    public class LoginViewModelTests
    {
        private static LoginViewModel CreateVm(FakeAzureDevOpsService? svc = null, FakeCredentialStore? store = null) =>
            new(svc ?? new FakeAzureDevOpsService(), store ?? new FakeCredentialStore());

        [Fact]
        public void InitialState_NotBusy_NotSuccessful()
        {
            var vm = CreateVm();
            Assert.False(vm.IsBusy);
            Assert.False(vm.IsLoginSuccessful);
            Assert.Equal(string.Empty, vm.ErrorMessage);
        }

        [Fact]
        public void LoginCommand_CannotExecute_WhenFieldsEmpty()
        {
            var vm = CreateVm();
            vm.OrganizationUrl = string.Empty;
            vm.Pat = string.Empty;
            Assert.False(vm.LoginCommand.CanExecute(null));
        }

        [Fact]
        public void LoginCommand_CanExecute_WhenBothFieldsFilled()
        {
            var vm = CreateVm();
            vm.OrganizationUrl = "https://dev.azure.com/org";
            vm.Pat = "token";
            Assert.True(vm.LoginCommand.CanExecute(null));
        }

        [Fact]
        public async Task LoginCommand_OnSuccess_SetsIsLoginSuccessful()
        {
            var svc = new FakeAzureDevOpsService { ValidateResult = true };
            var vm = CreateVm(svc);
            vm.OrganizationUrl = "https://dev.azure.com/org";
            vm.Pat = "token";

            await vm.LoginCommand.ExecuteAsync(null);

            Assert.True(vm.IsLoginSuccessful);
            Assert.Equal(string.Empty, vm.ErrorMessage);
        }

        [Fact]
        public async Task LoginCommand_OnSuccess_StoresCredentials()
        {
            var svc = new FakeAzureDevOpsService { ValidateResult = true };
            var store = new FakeCredentialStore();
            var vm = CreateVm(svc, store);
            vm.OrganizationUrl = "https://dev.azure.com/org";
            vm.Pat = "secret";

            await vm.LoginCommand.ExecuteAsync(null);

            Assert.Equal("https://dev.azure.com/org", store.StoredOrgUrl);
            Assert.Equal("secret", store.StoredPat);
        }

        [Fact]
        public async Task LoginCommand_OnFailure_SetsErrorMessage()
        {
            var svc = new FakeAzureDevOpsService { ValidateResult = false };
            var vm = CreateVm(svc);
            vm.OrganizationUrl = "https://dev.azure.com/org";
            vm.Pat = "bad";

            await vm.LoginCommand.ExecuteAsync(null);

            Assert.False(vm.IsLoginSuccessful);
            Assert.NotEmpty(vm.ErrorMessage);
        }

        [Fact]
        public async Task LoginCommand_OnException_SetsErrorMessage()
        {
            var svc = new FakeAzureDevOpsService { ShouldThrow = true };
            var vm = CreateVm(svc);
            vm.OrganizationUrl = "https://dev.azure.com/org";
            vm.Pat = "token";

            await vm.LoginCommand.ExecuteAsync(null);

            Assert.False(vm.IsLoginSuccessful);
            Assert.Contains("Network error", vm.ErrorMessage);
        }

        [Fact]
        public async Task LoginCommand_ResetsIsBusy_AfterCompletion()
        {
            var vm = CreateVm();
            vm.OrganizationUrl = "https://dev.azure.com/org";
            vm.Pat = "token";

            await vm.LoginCommand.ExecuteAsync(null);

            Assert.False(vm.IsBusy);
        }
    }
}
