using System.Text.Json;

namespace PersonalTaskManager;

public partial class MainPage : ContentPage
{
	private readonly List<TaskItem> tasks = new();

	public MainPage()
	{
		InitializeComponent();

		LoadTasks();

		TaskList.ItemsSource = tasks;
	}

	protected override void OnAppearing()
	{
		base.OnAppearing();

		TaskList.ItemsSource = null;
		TaskList.ItemsSource = tasks;
	}

	private async void OnAddTaskClicked(object? sender, EventArgs e)
	{
		string taskTitle = TaskEntry.Text?.Trim() ?? string.Empty;
		string description = DescriptionEntry.Text?.Trim() ?? string.Empty;

		if (string.IsNullOrWhiteSpace(taskTitle))
		{
			await DisplayAlertAsync(
				"Missing Task",
				"Please enter a task.",
				"OK");

			return;
		}

		TaskItem newTask = new()
		{
			Title = taskTitle,
			Description = description,
			IsCompleted = false
		};

		tasks.Add(newTask);

		SaveTasks();

		TaskList.ItemsSource = null;
		TaskList.ItemsSource = tasks;

		TaskEntry.Text = string.Empty;
		DescriptionEntry.Text = string.Empty;
	}

	private async void OnTaskTapped(object? sender, TappedEventArgs e)
	{
		if (sender is Border border &&
			border.BindingContext is TaskItem selectedTask)
		{
			await Navigation.PushAsync(
				new TaskDetailsPage(selectedTask, tasks));
		}
	}

	private void OnTaskCompletedChanged(object? sender, CheckedChangedEventArgs e)
	{
		SaveTasks();
	}

	private void SaveTasks()
	{
		string json = JsonSerializer.Serialize(tasks);

		Preferences.Default.Set("tasks", json);
	}

	private void LoadTasks()
	{
		string json = Preferences.Default.Get("tasks", string.Empty);

		if (string.IsNullOrWhiteSpace(json))
		{
			return;
		}

		try
		{
			List<TaskItem>? savedTasks =
				JsonSerializer.Deserialize<List<TaskItem>>(json);

			if (savedTasks != null)
			{
				tasks.AddRange(savedTasks);
			}
		}
		catch (JsonException)
		{
			Preferences.Default.Remove("tasks");
		}
	}
}