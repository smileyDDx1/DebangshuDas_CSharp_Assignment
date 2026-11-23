using SkiaSharp;

public static class PieChart
{
    private static readonly SKColor[] Colors =
    {
        SKColor.Parse("#1F77B4"), SKColor.Parse("#D62728"), SKColor.Parse("#2CA02C"), SKColor.Parse("#E66100"),
        SKColor.Parse("#9467BD"), SKColor.Parse("#8C564B"), SKColor.Parse("#C2185B"), SKColor.Parse("#5F6B73"),
        SKColor.Parse("#00838F"), SKColor.Parse("#827717"),
    };

    public static void Create(List<EmployeeTotal> employees, string path)
    {
        int width = 1100;
        int height = 700;
        float centerX = 340;
        float centerY = 380;
        float radius = 270;

        double totalHours = employees.Sum(e => e.TotalTime.TotalHours);

        using var surface = SKSurface.Create(new SKImageInfo(width, height));
        var canvas = surface.Canvas;
        canvas.Clear(SKColors.White);

        using var titleFont = new SKFont(SKTypeface.FromFamilyName("Arial", SKFontStyle.Bold), 28);
        using var font = new SKFont(SKTypeface.FromFamilyName("Arial"), 18);
        using var labelFont = new SKFont(SKTypeface.FromFamilyName("Arial", SKFontStyle.Bold), 18);
        using var textPaint = new SKPaint { Color = SKColors.Black, IsAntialias = true };
        using var whitePaint = new SKPaint { Color = SKColors.White, IsAntialias = true };
        using var slicePaint = new SKPaint { IsAntialias = true };

        canvas.DrawText("Percentage of total time worked by employee", 40, 55, SKTextAlign.Left, titleFont, textPaint);

        var pieRect = new SKRect(centerX - radius, centerY - radius, centerX + radius, centerY + radius);
        float startAngle = -90;

        for (int i = 0; i < employees.Count; i++)
        {
            var employee = employees[i];
            double percent = employee.TotalTime.TotalHours / totalHours * 100;
            float sweepAngle = (float)(percent * 360 / 100);
            slicePaint.Color = Colors[i % Colors.Length];

            using var slice = new SKPath();
            slice.MoveTo(centerX, centerY);
            slice.ArcTo(pieRect, startAngle, sweepAngle, false);
            slice.Close();
            canvas.DrawPath(slice, slicePaint);

            double middleAngle = (startAngle + sweepAngle / 2) * Math.PI / 180;
            float labelX = centerX + (float)(Math.Cos(middleAngle) * radius * 0.65);
            float labelY = centerY + (float)(Math.Sin(middleAngle) * radius * 0.65);
            canvas.DrawText($"{percent:0.0}%", labelX, labelY + 6, SKTextAlign.Center, labelFont, whitePaint);

            float legendY = 140 + i * 45;
            canvas.DrawRect(680, legendY - 16, 22, 22, slicePaint);
            canvas.DrawText($"{employee.Name} - {employee.TotalText} ({percent:0.0}%)", 715, legendY, SKTextAlign.Left, font, textPaint);

            startAngle += sweepAngle;
        }

        using var image = surface.Snapshot();
        using var data = image.Encode(SKEncodedImageFormat.Png, 100);
        File.WriteAllBytes(path, data.ToArray());
    }
}
