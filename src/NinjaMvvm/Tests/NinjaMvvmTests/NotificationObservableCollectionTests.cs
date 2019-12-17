using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FluentAssertions;

namespace NinjaMvvmTests
{
    [TestClass]
    public class NotificationObservableCollectionTests
    {
        [TestMethod]
        public void ItemPropertyChangedEvent_fires_on_constructor_initialized_list()
        {

            //*************  arrange  ******************
            string propThatchanged = null;
            var lst = new List<TestViewModel>();

            lst.Add(new TestViewModel());


            var ob = new NinjaMvvm.NotificationObservableCollection<TestViewModel>(lst);

            ob.ItemPropertyChangedEvent += (sender, e) => propThatchanged = e.PropertyName;

            //*************    act    ******************
            lst[0].SimpleTestProperty = "pizza";

            //*************  assert   ******************
            propThatchanged.Should().Be("SimpleTestProperty");

        }

        [TestMethod]
        public void ItemPropertyChangedEvent_fires_on_added_item()
        {

            //*************  arrange  ******************
            string propThatchanged = null;
            var ob = new NinjaMvvm.NotificationObservableCollection<TestViewModel>();

            ob.Add(new TestViewModel());

            ob.ItemPropertyChangedEvent += (sender, e) => propThatchanged = e.PropertyName;

            //*************    act    ******************
            ob[0].SimpleTestProperty = "pizza";

            //*************  assert   ******************
            propThatchanged.Should().Be("SimpleTestProperty");

        }


        [TestMethod]
        public void ItemPropertyChangedEvent_does_not_fire_on_removed_item()
        {

            //*************  arrange  ******************
            string propThatchanged = null;
            var ob = new NinjaMvvm.NotificationObservableCollection<TestViewModel>();

            var vm = new TestViewModel();
            ob.Add(vm);

            ob.ItemPropertyChangedEvent += (sender, e) => propThatchanged = e.PropertyName;

            ob.Remove(vm);
            //*************    act    ******************
            vm.SimpleTestProperty = "pizza";

            //*************  assert   ******************
            propThatchanged.Should().BeNull();

        }
    }
}
