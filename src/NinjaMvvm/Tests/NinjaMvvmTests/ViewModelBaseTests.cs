using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using FluentAssertions;
using System.Threading.Tasks;

namespace NinjaMvvmTests
{
    [TestClass]
    public class ViewModelBaseTests
    {
        [TestMethod]
        public void calls_loaddesigndata_when_in_design_mode()
        {

            //*************  arrange  ******************
            //*************    act    ******************
            var vm = new DesignModeTestViewModel();

            //*************  assert   ******************
            vm.CalledLoadDesignData.Should().BeTrue();
        }

        [TestMethod]
        public void does_not_call_loaddesigndata_when_not_in_design_mode()
        {
            //*************  arrange  ******************
            //*************    act    ******************
            var vm = new TestViewModel();

            //*************  assert   ******************
            vm.CalledLoadDesignData.Should().BeFalse();
        }

        [TestMethod]
        public void defaults_LoadWhenBound_toTrue()
        {
            //*************  arrange  ******************
            //*************    act    ******************
            var vm = new TestViewModel();

            //*************  assert   ******************
            vm.Exposed_LoadWhenBound.Should().BeTrue();
        }

        [TestMethod]
        public void calls_reload_when_LoadWhenBound_toTrue()
        {
            //*************  arrange  ******************
            var mockvm = new Mock<TestViewModel>();
            mockvm.CallBase = true;
            mockvm.Object.Exposed_LoadWhenBound = true;

            //*************    act    ******************
            var x = mockvm.Object.ViewBound;

            //*************  assert   ******************
            mockvm.Verify(m => m.Exposed_OnReloadDataAsync(It.IsAny<System.Threading.CancellationToken>()), times: Times.Once);
        }

        [TestMethod]
        public void does_not_call_reload_when_LoadWhenBound_false()
        {
            //*************  arrange  ******************
            var mockvm = new Mock<TestViewModel>();
            mockvm.CallBase = true;
            mockvm.Object.Exposed_LoadWhenBound = false;

            //*************    act    ******************
            var x = mockvm.Object.ViewBound;

            //*************  assert   ******************
            mockvm.Verify(m => m.Exposed_OnReloadDataAsync(It.IsAny<System.Threading.CancellationToken>()),
                times: Times.Never);
        }

        [TestMethod]
        public void OnReloadDataAsync_sets_LoadFailed_false()
        {
            //*************  arrange  ******************
            var mockvm = new Mock<TestViewModel>();
            mockvm.CallBase = true;
            mockvm.Setup(m => m.Exposed_OnReloadDataAsync(It.IsAny<System.Threading.CancellationToken>()))
                .Returns(Task.FromResult(true));

            //*************    act    ******************
            var x = mockvm.Object.ViewBound;

            //*************  assert   ******************
            mockvm.Object.LoadFailed.Should().BeFalse();
            mockvm.Object.LoadCancelled.Should().BeFalse();
        }

        [TestMethod]
        public void OnReloadDataAsync_sets_LoadFailed_true()
        {
            //*************  arrange  ******************
            var mockvm = new Mock<TestViewModel>();
            mockvm.CallBase = true;
            mockvm.Setup(m => m.Exposed_OnReloadDataAsync(It.IsAny<System.Threading.CancellationToken>()))
                .Returns(Task.FromResult(false));

            //*************    act    ******************
            var x = mockvm.Object.ViewBound;
            Task.Delay(300).Wait();

            //*************  assert   ******************
            mockvm.Object.LoadFailed.Should().BeTrue();
            mockvm.Object.LoadCancelled.Should().BeFalse();
        }


        [TestMethod]
        public void OnReloadDataAsync_sets_LoadCancelled_true()
        {
            //*************  arrange  ******************
            var mockvm = new Mock<TestViewModel>();
            mockvm.CallBase = true;
            mockvm.Setup(m => m.Exposed_OnReloadDataAsync(It.IsAny<System.Threading.CancellationToken>()))
                .Returns<System.Threading.CancellationToken>(async (c) =>
                {
                    try { await Task.Delay(5000, c); } catch { }
                    return false;
                });

            //*************    act    ******************
            var x = mockvm.Object.ViewBound;
            Task.Delay(300).Wait();
            mockvm.Object.Exposed_CancelReloadDataAsync();
            Task.Delay(100).Wait();

            //*************  assert   ******************
            mockvm.Object.LoadFailed.Should().BeFalse();
            mockvm.Object.LoadCancelled.Should().BeTrue();
        }

        [TestMethod]
        public void OnReloadDataAsync_is_called_on_main_thread()
        {
            //*************  arrange  ******************
            if (System.Threading.Thread.CurrentThread.GetApartmentState() != System.Threading.ApartmentState.STA)
            {
                throw new ApplicationException("The current threads apartment state is not STA");
            }

            bool fired = false;
            bool isSTA = false;
            var mockvm = new Mock<TestViewModel>();
            mockvm.CallBase = true;
            mockvm.Setup(m => m.Exposed_OnReloadDataAsync(It.IsAny<System.Threading.CancellationToken>()))
                .Returns<System.Threading.CancellationToken>((c) =>
                {
                    fired = true;
                    isSTA = (System.Threading.Thread.CurrentThread.GetApartmentState() == System.Threading.ApartmentState.STA);
                    return Task.FromResult(true);
                });

            //*************    act    ******************
            mockvm.Object.Exposed_ReloadDataAsync();
            Task.Delay(300).Wait();

            //*************  assert   ******************
            fired.Should().BeTrue();
            isSTA.Should().BeTrue();

        }
    }
}
