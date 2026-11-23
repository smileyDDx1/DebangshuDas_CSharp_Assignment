using System.Text.Json.Serialization;

public class TimeEntry
{
    public string? EmployeeName { get; set; }

    [JsonPropertyName("StarTimeUtc")]
    public DateTime StartTimeUtc { get; set; }

    public DateTime EndTimeUtc { get; set; }

    public DateTime? DeletedOn { get; set; }
}

public record EmployeeTotal(string Name, TimeSpan TotalTime)
{
    public string TotalText => $"{(int)TotalTime.TotalHours} h {TotalTime.Minutes:00} m";
}
