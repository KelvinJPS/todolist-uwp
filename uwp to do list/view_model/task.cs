using System;

public class Task : INotifyPropertyChanged
{
    public string Name_task { get; set; }
    public DateTime Reminder { get; set; }
    public string name_list { get; set; }
    public DateTime date { get; set; }
    public string subtask { get; set; }
    public string priority { get; set; }

    public event PropertyChangedEventHandler PropertyChanged;
    private void NotifyPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}