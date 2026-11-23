using System.Net;
using System.Text;

public static class HtmlReport
{
    public static void Create(List<EmployeeTotal> employees, string path)
    {
        var html = new StringBuilder();
        html.AppendLine("<!DOCTYPE html>");
        html.AppendLine("<html>");
        html.AppendLine("<head>");
        html.AppendLine("<meta charset=\"utf-8\">");
        html.AppendLine("<title>Employee time worked</title>");
        html.AppendLine("<style>");
        html.AppendLine("body { font-family: Arial, sans-serif; margin: 40px; }");
        html.AppendLine("table { border-collapse: collapse; }");
        html.AppendLine("th, td { border: 1px solid #ccc; padding: 8px 16px; text-align: left; }");
        html.AppendLine("th { background-color: #2f4f6f; color: white; }");
        html.AppendLine(".less-than-100 { background-color: #f8d7da; }");
        html.AppendLine("</style>");
        html.AppendLine("</head>");
        html.AppendLine("<body>");
        html.AppendLine("<h2>Employee time worked</h2>");
        html.AppendLine("<p>Highlighted rows: less than 100 hours worked</p>");
        html.AppendLine("<table>");
        html.AppendLine("<tr><th>Name</th><th>Total time worked</th></tr>");

        foreach (var employee in employees)
        {
            string rowClass = employee.TotalTime.TotalHours < 100 ? " class=\"less-than-100\"" : "";
            html.AppendLine($"<tr{rowClass}><td>{WebUtility.HtmlEncode(employee.Name)}</td><td>{employee.TotalText}</td></tr>");
        }

        html.AppendLine("</table>");
        html.AppendLine("</body>");
        html.AppendLine("</html>");

        File.WriteAllText(path, html.ToString());
    }
}
