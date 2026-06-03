using System.Collections.ObjectModel;
using AICodeReviewRequester.Interfaces;
using AICodeReviewRequester.Services;
using Prism.Commands;
using ReactiveUI;

namespace AICodeReviewRequester
{
    public class MainWindowViewModel : ReactiveObject
    {
        private bool _isBusy;
        private string _busyText;
        private string _prompt;
        private IAIModel _selectedModel;
        private string _postResult;

        public bool IsBusy
        {
            get => _isBusy;
            set => this.RaiseAndSetIfChanged(ref _isBusy, value);
        }

        public string BusyText
        {
            get => _busyText;
            set => this.RaiseAndSetIfChanged(ref _busyText, value);
        }

        public ObservableCollection<IAIModel> Models { get; } = new ObservableCollection<IAIModel>();

        public string Prompt
        {
            get => _prompt;
            set => this.RaiseAndSetIfChanged(ref _prompt, value);
        }

        public string PostResult
        {
            get => _postResult;
            set=> this.RaiseAndSetIfChanged(ref _postResult, value);
        }

        public IAIModel SelectedModel
        {
            get => _selectedModel;
            set => this.RaiseAndSetIfChanged(ref _selectedModel, value);
        }

        public DelegateCommand PostCommand { get; }

        public MainWindowViewModel()
        {
            PostCommand = new DelegateCommand(Post);

            Models.AddRange(AIFactory.GetModels());
        }

        private async void Post()
        {
            if (SelectedModel == null)
                return;

            if (string.IsNullOrEmpty(Prompt))
                return;

            IsBusy = true;
            BusyText = "Post...";

            var result = await SelectedModel.Post(Prompt);

            BusyText = string.Empty;
            IsBusy = false;

            PostResult = result.Message;
        }
    }
}
