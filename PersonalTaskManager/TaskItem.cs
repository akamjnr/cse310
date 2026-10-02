namespace PersonalTaskManager;

// Represents a task and the information saved for each task.
public class TaskItem
{
    public string Title { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public bool IsCompleted { get; set; }
}