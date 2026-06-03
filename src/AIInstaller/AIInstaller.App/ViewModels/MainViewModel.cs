using System.Collections.ObjectModel;
using System.Windows;
using AIInstaller.App.Models;
using AIInstaller.App.Services;
using Prism.Commands;
using Prism.Mvvm;

namespace AIInstaller.App.ViewModels;

public sealed class MainViewModel : BindableBase
{
    private readonly InstallerDashboardService _dashboardService;
    private ObservableCollection<ModelStatusItem> _modelStatuses;
    private bool _isBusy;
    private string _statusMessage;
    private int _currentStep;
    private string _wizardTitle;
    private string _stepDescription;
    private bool _isPreviousButtonVisible;
    private bool _isNextButtonVisible;
    private bool _isFinishButtonVisible;
    private bool _isStep1Active;
    private bool _isStep2Active;
    private bool _isStep3Active;
    private bool _isStep4Active;
    private bool _isStep1Completed;
    private bool _isStep2Completed;
    private bool _isStep3Completed;
    private bool _isDataGridVisible;
    private bool _isRuleEditorVisible;
    private string _ruleContent;
    private string _ruleDirectoryPath;

    public MainViewModel(InstallerDashboardService dashboardService)
    {
        _dashboardService = dashboardService;
        _modelStatuses = [];
        _statusMessage = "준비되었습니다.";
        _currentStep = 1;
        _wizardTitle = "설치 상태 확인";
        _stepDescription = "시스템에 설치된 AI CLI 도구의 현재 상태를 확인합니다.";
        _isPreviousButtonVisible = false;
        _isNextButtonVisible = true;
        _isFinishButtonVisible = false;
        _isStep1Active = true;
        _isDataGridVisible = true;
        _isRuleEditorVisible = false;
        _ruleContent = string.Empty;
        _ruleDirectoryPath = dashboardService.GetDefaultRuleDirectoryPath();

        PreviousCommand = new DelegateCommand(ExecutePrevious, CanPrevious)
            .ObservesProperty(() => IsBusy)
            .ObservesProperty(() => CurrentStep);

        NextCommand = new DelegateCommand(ExecuteNext, CanNext)
            .ObservesProperty(() => IsBusy)
            .ObservesProperty(() => CurrentStep);

        FinishCommand = new DelegateCommand(ExecuteFinish, CanFinish)
            .ObservesProperty(() => IsBusy)
            .ObservesProperty(() => CurrentStep);

        ExitCommand = new DelegateCommand(ExecuteExit);
        UpdateStepState(_currentStep);
    }

    public DelegateCommand PreviousCommand { get; }

    public DelegateCommand NextCommand { get; }

    public DelegateCommand FinishCommand { get; }

    public DelegateCommand ExitCommand { get; }

    public ObservableCollection<ModelStatusItem> ModelStatuses
    {
        get => _modelStatuses;
        private set => SetProperty(ref _modelStatuses, value);
    }

    public bool IsBusy
    {
        get => _isBusy;
        private set => SetProperty(ref _isBusy, value);
    }

    public string StatusMessage
    {
        get => _statusMessage;
        private set => SetProperty(ref _statusMessage, value);
    }

    public int CurrentStep
    {
        get => _currentStep;
        private set
        {
            if (SetProperty(ref _currentStep, value))
            {
                UpdateStepState(value);
            }
        }
    }

    public string WizardTitle
    {
        get => _wizardTitle;
        private set => SetProperty(ref _wizardTitle, value);
    }

    public string StepDescription
    {
        get => _stepDescription;
        private set => SetProperty(ref _stepDescription, value);
    }

    public bool IsPreviousButtonVisible
    {
        get => _isPreviousButtonVisible;
        private set => SetProperty(ref _isPreviousButtonVisible, value);
    }

    public bool IsNextButtonVisible
    {
        get => _isNextButtonVisible;
        private set => SetProperty(ref _isNextButtonVisible, value);
    }

    public bool IsFinishButtonVisible
    {
        get => _isFinishButtonVisible;
        private set => SetProperty(ref _isFinishButtonVisible, value);
    }

    public bool IsStep1Active
    {
        get => _isStep1Active;
        private set => SetProperty(ref _isStep1Active, value);
    }

    public bool IsStep2Active
    {
        get => _isStep2Active;
        private set => SetProperty(ref _isStep2Active, value);
    }

    public bool IsStep3Active
    {
        get => _isStep3Active;
        private set => SetProperty(ref _isStep3Active, value);
    }

    public bool IsStep4Active
    {
        get => _isStep4Active;
        private set => SetProperty(ref _isStep4Active, value);
    }

    public bool IsStep1Completed
    {
        get => _isStep1Completed;
        private set => SetProperty(ref _isStep1Completed, value);
    }

    public bool IsStep2Completed
    {
        get => _isStep2Completed;
        private set => SetProperty(ref _isStep2Completed, value);
    }

    public bool IsStep3Completed
    {
        get => _isStep3Completed;
        private set => SetProperty(ref _isStep3Completed, value);
    }

    public bool IsDataGridVisible
    {
        get => _isDataGridVisible;
        private set => SetProperty(ref _isDataGridVisible, value);
    }

    public bool IsRuleEditorVisible
    {
        get => _isRuleEditorVisible;
        private set => SetProperty(ref _isRuleEditorVisible, value);
    }

    public string RuleContent
    {
        get => _ruleContent;
        set => SetProperty(ref _ruleContent, value);
    }

    public string RuleDirectoryPath
    {
        get => _ruleDirectoryPath;
        set => SetProperty(ref _ruleDirectoryPath, value);
    }

    public async Task InitializeAsync(CancellationToken cancellationToken)
    {
        await RefreshAsync(cancellationToken).ConfigureAwait(false);
        await Application.Current.Dispatcher.InvokeAsync(() => CurrentStep = 1);
    }

    private bool CanPrevious() => !IsBusy && CurrentStep > 1;

    private bool CanNext() => !IsBusy && CurrentStep < 4;

    private bool CanFinish() => !IsBusy && CurrentStep == 4;

    private void ExecutePrevious()
    {
        if (CurrentStep > 1)
        {
            CurrentStep--;
            StatusMessage = "이전 단계로 이동했습니다.";
        }
    }

    private async void ExecuteNext()
    {
        if (IsBusy)
        {
            return;
        }

        try
        {
            switch (CurrentStep)
            {
                case 1:
                    if (await RefreshAsync(CancellationToken.None).ConfigureAwait(false))
                    {
                        await Application.Current.Dispatcher.InvokeAsync(() =>
                        {
                            CurrentStep = 2;
                            StatusMessage = "상태 확인이 끝났습니다. 필요한 도구 설치와 로그인 연결을 진행하세요.";
                        });
                    }
                    break;
                case 2:
                    if (!await InstallRequiredAsync(CancellationToken.None).ConfigureAwait(false))
                    {
                        return;
                    }

                    if (!await ConnectRequiredAsync(CancellationToken.None).ConfigureAwait(false))
                    {
                        return;
                    }

                    if (!await LoadRuleContentForEditorAsync(CancellationToken.None).ConfigureAwait(false))
                    {
                        return;
                    }

                    await Application.Current.Dispatcher.InvokeAsync(() =>
                    {
                        CurrentStep = 3;
                        StatusMessage = "설치와 연결을 확인했습니다. 규칙 파일을 편집하세요.";
                    });
                    break;
                case 3:
                    if (!await SaveRulesAsync(CancellationToken.None).ConfigureAwait(false))
                    {
                        return;
                    }

                    if (!await RefreshAsync(CancellationToken.None).ConfigureAwait(false))
                    {
                        return;
                    }

                    await Application.Current.Dispatcher.InvokeAsync(() =>
                    {
                        CurrentStep = 4;
                        StatusMessage = "규칙 파일을 저장했습니다. 최종 상태를 확인하세요.";
                    });
                    break;
            }
        }
        catch (Exception ex)
        {
            await Application.Current.Dispatcher.InvokeAsync(() =>
            {
                StatusMessage = $"작업 중 오류가 발생했습니다: {ex.Message}";
                IsBusy = false;
            });
        }
    }

    private async void ExecuteFinish()
    {
        if (IsBusy)
        {
            return;
        }

        try
        {
            if (await RefreshAsync(CancellationToken.None).ConfigureAwait(false))
            {
                await Application.Current.Dispatcher.InvokeAsync(() =>
                {
                    StatusMessage = "AI CLI 설치 마법사를 완료했습니다.";
                });
            }
        }
        catch (Exception ex)
        {
            await Application.Current.Dispatcher.InvokeAsync(() =>
            {
                StatusMessage = $"완료 처리에 실패했습니다: {ex.Message}";
                IsBusy = false;
            });
        }
    }

    private void ExecuteExit()
    {
        Application.Current.Shutdown();
    }

    private async Task<bool> InstallRequiredAsync(CancellationToken cancellationToken)
    {
        try
        {
            IsBusy = true;
            StatusMessage = "필요한 CLI를 설치하는 중입니다.";

            var installResult = await _dashboardService.InstallRequiredAsync(cancellationToken).ConfigureAwait(false);

            await Application.Current.Dispatcher.InvokeAsync(() => StatusMessage = installResult.Message);

            if (!installResult.IsSuccess)
            {
                return false;
            }

            return await RefreshAsync(cancellationToken).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            await Application.Current.Dispatcher.InvokeAsync(() =>
            {
                StatusMessage = $"설치에 실패했습니다: {ex.Message}";
            });
            return false;
        }
        finally
        {
            await Application.Current.Dispatcher.InvokeAsync(() => IsBusy = false);
        }
    }

    private async Task<bool> ConnectRequiredAsync(CancellationToken cancellationToken)
    {
        try
        {
            IsBusy = true;
            StatusMessage = "필요한 CLI 로그인 연결을 확인하는 중입니다. 별도 콘솔 창이 열릴 수 있습니다.";

            var connectResult = await _dashboardService.ConnectRequiredAsync(cancellationToken).ConfigureAwait(false);

            await Application.Current.Dispatcher.InvokeAsync(() => StatusMessage = connectResult.Message);

            if (!connectResult.IsSuccess)
            {
                return false;
            }

            return await RefreshAsync(cancellationToken).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            await Application.Current.Dispatcher.InvokeAsync(() =>
            {
                StatusMessage = $"계정 연결에 실패했습니다: {ex.Message}";
            });
            return false;
        }
        finally
        {
            await Application.Current.Dispatcher.InvokeAsync(() => IsBusy = false);
        }
    }

    private async Task<bool> LoadRuleContentForEditorAsync(CancellationToken cancellationToken)
    {
        try
        {
            string content = await _dashboardService.LoadRuleContentAsync(RuleDirectoryPath, cancellationToken).ConfigureAwait(false);

            await Application.Current.Dispatcher.InvokeAsync(() =>
            {
                RuleContent = content;
                StatusMessage = "규칙 파일 내용을 불러왔습니다.";
            });

            return true;
        }
        catch (Exception ex)
        {
            await Application.Current.Dispatcher.InvokeAsync(() =>
            {
                StatusMessage = $"규칙 파일을 불러오지 못했습니다: {ex.Message}";
            });
            return false;
        }
    }

    private async Task<bool> SaveRulesAsync(CancellationToken cancellationToken)
    {
        try
        {
            IsBusy = true;
            StatusMessage = "규칙 파일을 저장하는 중입니다.";

            string currentContent = string.Empty;
            string currentPath = string.Empty;
            await Application.Current.Dispatcher.InvokeAsync(() =>
            {
                currentContent = RuleContent;
                currentPath = RuleDirectoryPath;
            });

            var result = await _dashboardService.SaveRulesAsync(currentPath, currentContent, cancellationToken).ConfigureAwait(false);

            await Application.Current.Dispatcher.InvokeAsync(() => StatusMessage = result.Message);
            return result.IsSuccess;
        }
        catch (Exception ex)
        {
            await Application.Current.Dispatcher.InvokeAsync(() =>
            {
                StatusMessage = $"규칙 파일 저장에 실패했습니다: {ex.Message}";
            });
            return false;
        }
        finally
        {
            await Application.Current.Dispatcher.InvokeAsync(() => IsBusy = false);
        }
    }

    private async Task<bool> RefreshAsync(CancellationToken cancellationToken)
    {
        try
        {
            IsBusy = true;
            StatusMessage = "설치 상태를 확인하는 중입니다.";

            IReadOnlyList<ModelStatusItem> items = await _dashboardService.LoadModelStatusesAsync(cancellationToken).ConfigureAwait(false);

            await Application.Current.Dispatcher.InvokeAsync(() =>
            {
                ModelStatuses = new ObservableCollection<ModelStatusItem>(items);
                StatusMessage = $"{items.Count}개 모델 상태를 확인했습니다.";
            });

            return true;
        }
        catch (Exception ex)
        {
            await Application.Current.Dispatcher.InvokeAsync(() =>
            {
                StatusMessage = $"상태 확인에 실패했습니다: {ex.Message}";
            });
            return false;
        }
        finally
        {
            await Application.Current.Dispatcher.InvokeAsync(() => IsBusy = false);
        }
    }

    private void UpdateStepState(int step)
    {
        WizardTitle = step switch
        {
            1 => "설치 상태 확인",
            2 => "설치 및 로그인 연결",
            3 => "공통 규칙 편집",
            4 => "완료",
            _ => "AI CLI 설치 마법사"
        };

        StepDescription = step switch
        {
            1 => "시스템에 설치된 AI CLI 도구의 현재 상태를 확인합니다.",
            2 => "필요한 CLI를 설치하고 계정 로그인 연결 상태를 맞춥니다.",
            3 => "RULE.md를 저장하면 같은 폴더의 CLAUDE.md와 AGENTS.md에 참조 안내를 맞춥니다.",
            4 => "모든 작업이 끝났습니다. 최종 상태를 다시 확인하세요.",
            _ => string.Empty
        };

        IsStep1Active = step == 1;
        IsStep2Active = step == 2;
        IsStep3Active = step == 3;
        IsStep4Active = step == 4;
        IsStep1Completed = step > 1;
        IsStep2Completed = step > 2;
        IsStep3Completed = step > 3;

        IsDataGridVisible = step != 3;
        IsRuleEditorVisible = step == 3;

        IsPreviousButtonVisible = step > 1;
        IsNextButtonVisible = step < 4;
        IsFinishButtonVisible = step == 4;
    }
}
