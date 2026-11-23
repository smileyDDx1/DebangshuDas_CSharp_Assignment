using System.Text.Json;

const string ApiUrl = "https://rc-vault-fap-live-1.azurewebsites.net/api/gettimeentries?code=vO17RnE8vuzXzPJo5eaLLjXjmRW07law99QTD90zat9FfOQJKKUcgQ==";

try
{
    using var http = new HttpClient();
    string json = await http.GetStringAsync(ApiUrl);
    var entries = JsonSerializer.Deserialize<List<TimeEntry>>(json) ?? new List<TimeEntry>();

    var employees = entries
        .Where(e => e.DeletedOn == null)
        .Where(e => !string.IsNullOrWhiteSpace(e.EmployeeName))
        .Where(e => e.EndTimeUtc > e.StartTimeUtc)
        .GroupBy(e => e.EmployeeName!.Trim())
        .Select(g => new EmployeeTotal(g.Key, TimeSpan.FromTicks(g.Sum(e => (e.EndTimeUtc - e.StartTimeUtc).Ticks))))
        .OrderByDescending(e => e.TotalTime)
        .ToList();

    Directory.CreateDirectory("output");
    HtmlReport.Create(employees, Path.Combine("output", "EmployeeTimeReport.html"));
    PieChart.Create(employees, Path.Combine("output", "EmployeeTimePieChart.png"));

    foreach (var employee in employees)
    {
        Console.WriteLine($"{employee.Name,-20} {employee.TotalText}");
    }

    Console.WriteLine();
    Console.WriteLine("HTML table and pie chart saved in the output folder.");
}
catch (Exception ex)
{
    Console.WriteLine("Error: " + ex.Message);
}

Console.WriteLine("Press Enter to exit.");
Console.ReadLine();
