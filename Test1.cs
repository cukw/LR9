using Library.ViewModels;
using Mono.Cecil.Cil;

namespace TestLib;

[TestClass]
public sealed class Test1
{
    [TestMethod]
    public void Constructor_InitializesWithFourItems()
    {
        // Arrange & Act
        var vm = new MainWindowViewModel();

        // Assert
        Assert.AreEqual(4, vm.Items.Count);
        Assert.AreEqual("Всего: 4", vm.TotalText);
    }

    [TestMethod]
    public void AddNumber()
    {
        var vm = new MainWindowViewModel();

        vm.Name = "test";
        vm.Phone = "111-11-15";

        vm.AddCommand.Execute().Subscribe();

        Assert.AreEqual(5, vm.Items.Count);
        Assert.AreEqual("Всего: 5", vm.TotalText);

        var added = vm.Items.Last();
        Assert.AreEqual("test", added.Name);
        Assert.AreEqual("111-11-15", added.Phone);
    }

    [TestMethod]
    public void AddExistNum()
    {
        var vm = new MainWindowViewModel();
        vm.Name = "Валентина";
        vm.Phone = "999-99-99";

        vm.AddCommand.Execute().Subscribe();

        Assert.AreEqual(4, vm.Items.Count);
        Assert.AreEqual("Всего: 4", vm.TotalText);

        var upd = vm.Items.First(x => x.Name == "Валентина");

        Assert.AreEqual("999-99-99", upd.Phone);
    }

    [TestMethod]
    public void Delete()
    {
        var vm = new MainWindowViewModel();

        vm.SelectedItem = vm.Items[0];
        string name = vm.Items[0].Name;

        vm.DeleteCommand.Execute().Subscribe();
        Assert.IsNull(vm.SelectedItem);
        Assert.IsFalse(vm.Items.Any(x=>x.Name == name));
    }


}
