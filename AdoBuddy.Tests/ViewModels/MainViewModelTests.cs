using AdoBuddy.ViewModels;

namespace AdoBuddy.Tests.ViewModels
{
    public class MainViewModelTests
    {
        [Fact]
        public void CounterClicked_IncrementsCount()
        {
            var vm = new MainViewModel();
            Assert.Equal(0, vm.Count);
            vm.CounterClickedCommand.Execute(null);
            Assert.Equal(1, vm.Count);
            vm.CounterClickedCommand.Execute(null);
            Assert.Equal(2, vm.Count);
        }

        [Fact]
        public void CounterClicked_UpdatesTitle_Singular()
        {
            var vm = new MainViewModel();
            vm.CounterClickedCommand.Execute(null);
            Assert.Equal("Clicked 1 time", vm.Title);
        }

        [Fact]
        public void CounterClicked_UpdatesTitle_Plural()
        {
            var vm = new MainViewModel();
            vm.CounterClickedCommand.Execute(null);
            vm.CounterClickedCommand.Execute(null);
            Assert.Equal("Clicked 2 times", vm.Title);
        }
    }
}
