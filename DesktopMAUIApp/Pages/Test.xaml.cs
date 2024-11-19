using Shared.Data.Test.Task;
using System.Collections.ObjectModel;
using System.Text.Json;

namespace DesktopMAUIApp.Pages;

public partial class Test : ContentPage
{
	public Test()
	{
		InitializeComponent();
        BindingContext = new TestPageViewModel();
    }

    private void OnSubmitClicked(object sender, EventArgs e)
    {
        // Здесь можно обработать ответы пользователя, если нужно
        DisplayAlert("Тест завершён", "Спасибо за ваши ответы!", "OK");
    }
    public class TestPageViewModel : BindableObject
    {
        private bool _isUnauthorized = true;
        private bool _isNotSended = true;
        private string _testName;
        private ObservableCollection<TaskViewModel> _tasks;

        public bool IsUnauthorized
        {
            get => _isUnauthorized;
            set { _isUnauthorized = value; OnPropertyChanged(); }
        }

        public bool IsNotSended
        {
            get => _isNotSended;
            set { _isNotSended = value; OnPropertyChanged(); }
        }

        public string TestName
        {
            get => _testName;
            set { _testName = value; OnPropertyChanged(); }
        }

        public ObservableCollection<TaskViewModel> Tasks
        {
            get => _tasks;
            set { _tasks = value; OnPropertyChanged(); }
        }

        public Command SaveAndSubmitCommand { get; }

        public TestPageViewModel()
        {
            SaveAndSubmitCommand = new Command(async () => await SaveAndSubmit());
            LoadTest();
        }

        private async void LoadTest()
        {
            // Replace with API service call
            await Task.Delay(1000);
            TestName = "Пример теста";
            Tasks = new ObservableCollection<TaskViewModel>
            {
                new TaskViewModel { InteractionType = "LongStringTask", Question = "Напишите ответ", VariableAnswers = new [] { new VariableAnswer { StringAnswer = "" } } },
                new TaskViewModel { InteractionType = "ShortStringTask", Question = "Краткий ответ", VariableAnswers = new [] { new VariableAnswer { StringAnswer = "" } } },
             new TaskViewModel { InteractionType = "OneVariantTask", Question = "Краткий ответ", VariableAnswers = new [] { new VariableAnswer { StringAnswer = "" } } },
             new TaskViewModel { InteractionType = "ManyVariantsTask", Question = "Краткий ответ", VariableAnswers = new [] { new VariableAnswer { StringAnswer = "" } } }
            };
        }

        private async Task SaveAndSubmit()
        {
            IsNotSended = false;
            var json = JsonSerializer.Serialize(this);
            // Simulate submit
            await Task.Delay(500);
        }
    }

    public class TaskViewModel
    {
        public string InteractionType { get; set; }
        public string Question { get; set; }
        public VariableAnswer[] VariableAnswers { get; set; }
    }
}