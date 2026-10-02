using System.Text.Json;

namespace PersonalTaskManager;

public partial class TaskDetailsPage : ContentPage
{
    private readonly TaskItem task;
    private readonly List<TaskItem> tasks;

    public TaskDetailsPage(TaskItem task, List<TaskItem> tasks)
    {
        InitializeComponent();

        this.task = task;
        this.tasks = tasks;

        TaskLabel.Text = task.Title;
        DescriptionLabel.Text = string.IsNullOrWhiteSpace(task.Description)
            ? "No description provided."
            : task.Description;

        CompletionCheckBox.IsChecked = task.IsCompleted;
        UpdateCompletionLabel();
    }

    private void OnCompletionChanged(object? sender, CheckedChangedEventArgs e)
    {
        task.IsCompleted = e.Value;

        UpdateCompletionLabel();

        SaveTasks();
    }

    private void UpdateCompletionLabel()
    {
        // Update the button text based on the task's completion status.
        CompletionLabel.Text = task.IsCompleted
            ? "Mark as Incomplete"
            : "Mark as Done";
    }

    private async void OnDeleteClicked(object? sender, EventArgs e)
    {
        bool confirm = await DisplayAlertAsync(
            "Delete Task",
            "Are you sure you want to delete this task?",
            "Delete",
            "Cancel");

        if (!confirm)
        {
            return;
        }

        tasks.Remove(task);

        SaveTasks();

        await Navigation.PopAsync();
    }

    private async void OnBackClicked(object? sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }

    // Saves changes to the task list in local device storage.
    private void SaveTasks()
    {
        string json = JsonSerializer.Serialize(tasks);

        Preferences.Default.Set("tasks", json);
    }
}