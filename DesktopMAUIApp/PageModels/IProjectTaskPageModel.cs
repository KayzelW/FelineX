using CommunityToolkit.Mvvm.Input;
using DesktopMAUIApp.Models;

namespace DesktopMAUIApp.PageModels;

public interface IProjectTaskPageModel
{
    IAsyncRelayCommand<ProjectTask> NavigateToTaskCommand { get; }
    bool IsBusy { get; }
}