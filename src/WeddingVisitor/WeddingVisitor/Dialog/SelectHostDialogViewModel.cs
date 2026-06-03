using Prism.Commands;
using ReactiveUI;

namespace WeddingVisitor.Dialog
{
    public class SelectHostDialogViewModel : ReactiveObject
    {
        private readonly SelectHostDialog _dialog;

        public DelegateCommand SelectGroomCommand { get; }
        public DelegateCommand SelectBrideCommand { get; }

        public SelectHostDialogViewModel(SelectHostDialog dialog)
        {
            _dialog = dialog;

            SelectGroomCommand = new DelegateCommand(() => Select(eHost.Groom));
            SelectBrideCommand = new DelegateCommand(() => Select(eHost.Bride));
        }

        private void Select(eHost host)
        {
            _dialog.Host = host;
            _dialog.DialogResult = true;
            _dialog.Close();
        }
    }
}
