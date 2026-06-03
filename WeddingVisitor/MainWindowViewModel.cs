using Prism.Commands;
using ReactiveUI;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Windows;
using WeddingVisitor.Dialog;
using WeddingVisitor.Model;
using WeddingVisitor.Service;

namespace WeddingVisitor
{
    public class MainWindowViewModel : ReactiveObject
    {
        #region Internal Field
        private readonly MainWindow _window;
        private eHost _defaultHost;

        private eHost _inputHost;
        private string _inputName;
        private int _inputMoney;
        private int _inputAdultTicketCount;
        private int _inputChildTicketCount;

        private int _totalGuest = 0;
        private int _totalAdultTicket = 0;
        private int _totalChildTicket = 0;

        private ObservableCollection<Guest> _guests = new ObservableCollection<Guest>();
        #endregion

        #region Properties
        public SheetManager SheetManager { get; private set; }

        public eHost InputHost
        {
            get => _inputHost;
            set => this.RaiseAndSetIfChanged(ref _inputHost, value);
        }

        public string InputName
        {
            get => _inputName;
            set => this.RaiseAndSetIfChanged(ref _inputName, value);
        }

        public int InputMoney
        {
            get => _inputMoney;
            set => this.RaiseAndSetIfChanged(ref _inputMoney, value);
        }

        public int InputAdultTicketCount
        {
            get => _inputAdultTicketCount;
            set => this.RaiseAndSetIfChanged(ref _inputAdultTicketCount, value);
        }

        public int InputChildTicketCount
        {
            get => _inputChildTicketCount;
            set => this.RaiseAndSetIfChanged(ref _inputChildTicketCount, value);
        }

        public ObservableCollection<Guest> Guests
        {
            get => _guests;
            set => this.RaiseAndSetIfChanged(ref _guests, value);
        }

        public int TotalGuest
        {
            get => _totalGuest;
            set => this.RaiseAndSetIfChanged(ref _totalGuest, value);
        }

        public int TotalAdultTicket
        {
            get => _totalAdultTicket;
            set => this.RaiseAndSetIfChanged(ref _totalAdultTicket, value);
        }

        public int TotalChildTicket
        {
            get => _totalChildTicket;
            set => this.RaiseAndSetIfChanged(ref _totalChildTicket, value);
        }
        #endregion

        #region Commands
        public DelegateCommand RegisterCommand { get; }
        public DelegateCommand CancelCommand { get; }
        public DelegateCommand<object> RemoveCommand { get; }
        #endregion

        #region Constructor
        public MainWindowViewModel(MainWindow window)
        {
            _window = window;

            RegisterCommand = new DelegateCommand(Register);
            CancelCommand = new DelegateCommand(Cancel);
            RemoveCommand = new DelegateCommand<object>(Remove);

            var dialog = new SelectHostDialog()
            {
                WindowStartupLocation = WindowStartupLocation.CenterScreen,
            };

            if (dialog.ShowDialog() == true)
            {
                _defaultHost = dialog.Host;
                _inputHost = dialog.Host;
            }
        }
        #endregion

        #region Functions
        private async void Remove(object obj)
        {
            if (!(obj is Guest guest))
                return;

            Guests.Remove(guest);

            await Task.Run(() =>
            {
                SheetManager.Clear();
                SheetManager.Sync(Guests);
            });

            UpdateCount();
        }

        private void UpdateCount()
        {
            TotalGuest = Guests.Count;
            TotalAdultTicket = Guests.Sum(g => g?.AdultTicketCount ?? 0);
            TotalChildTicket = Guests.Sum(g => g?.ChildTicketCount ?? 0);
        }

        private void Cancel()
        {
            InputName = "";
            InputMoney = 0;
            InputAdultTicketCount = 1;
            InputChildTicketCount = 0;
            InputHost = _defaultHost;

            _window._nameTextBox.Focus();
        }

        private async void Register()
        {
            var guest = new Guest()
            {
                Name = InputName,
                Money = InputMoney,
                AdultTicketCount = InputAdultTicketCount,
                ChildTicketCount = InputChildTicketCount,
            };

            Guests.Add(guest);

            await Task.Run(() =>
            {
                SheetManager.Sync(Guests);
            });
            
            Cancel();
            UpdateCount();
        }

        internal async void Authenticate()
        {
            var sheetName = _defaultHost == eHost.Groom
                ? "신랑하객명단" : "신부하객명단";

            SheetManager = await Task.Run(() => new SheetManager(Keys.API_KEY, Keys.APPLICATION_NAME, Keys.SPREAD_ID, sheetName));
            Guests = new ObservableCollection<Guest>(SheetManager.Load());

            UpdateCount();
        }
        #endregion
    }
}
