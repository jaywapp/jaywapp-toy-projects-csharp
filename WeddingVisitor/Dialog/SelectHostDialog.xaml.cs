using System.Windows;

namespace WeddingVisitor.Dialog
{
    /// <summary>
    /// SelectHostDialog.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class SelectHostDialog : Window
    {
        public eHost Host { get; set; }

        public SelectHostDialog()
        {
            DataContext = new SelectHostDialogViewModel(this);
            InitializeComponent();
        }
    }
}
