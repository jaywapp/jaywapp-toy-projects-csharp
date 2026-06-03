using Prism.Commands;
using System.Windows;
using System.Windows.Controls;

namespace WeddingVisitor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            DataContext = new MainWindowViewModel(this);
            Loaded += MainWindow_Loaded;
            InitializeComponent();
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if (!(DataContext is MainWindowViewModel vm))
                return;

            vm.Authenticate();
        }

        private void StackPanel_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (!(DataContext is MainWindowViewModel vm))
                return;

            if (e.Key == System.Windows.Input.Key.Enter)
                vm.RegisterCommand.Execute();
            else if (e.Key == System.Windows.Input.Key.Escape)
                vm.CancelCommand.Execute();
        }

        private void ListViewItem_PreviewMouseRightButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (!(DataContext is MainWindowViewModel vm))
                return;
            if (!(sender is ListViewItem item))
                return;

            var contextMenu = new ContextMenu();

            contextMenu.Items.Add(new MenuItem()
            {
                Header = "삭제",
                Command = new DelegateCommand(() =>
                {
                    vm.RemoveCommand.Execute(item.DataContext);
                })
            }); ;

            contextMenu.IsOpen = true;
        }
    }
}
