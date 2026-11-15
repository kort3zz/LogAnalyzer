using System.Text;
using System.Text.Json;

namespace LogAnalyzer;

public class ResultWriter
{
    public void WriteToConsole(
        LogAggregator aggregator,
        int validLines,
        int invalidLines,
        int top)
    {
        Console.WriteLine("===== РЕЗУЛЬТАТЫ АНАЛИЗА =====");

        Console.WriteLine($"Всего корректных строк: {validLines}");
        Console.WriteLine($"Всего ошибочных строк: {invalidLines}");
        Console.WriteLine($"Событий после фильтрации: {aggregator.TotalEvents}");

        Console.WriteLine();
        Console.WriteLine("===== КОЛИЧЕСТВО ПО УРОВНЯМ =====");

        foreach (var item in aggregator.Levels)
        {
            Console.WriteLine($"{item.Key}: {item.Value}");
        }

        Console.WriteLine();
        Console.WriteLine("===== КОЛИЧЕСТВО ПО ИСТОЧНИКАМ =====");

        foreach (var item in aggregator.Sources)
        {
            Console.WriteLine($"{item.Key}: {item.Value}");
        }

        Console.WriteLine();
        Console.WriteLine("===== ТОП КОДОВ ОШИБОК =====");

        foreach (var item in aggregator.GetTopErrorCodes(top))
        {
            Console.WriteLine($"{item.Key}: {item.Value}");
        }
    }

    public void WriteToFile(
        LogAggregator aggregator,
        int validLines,
        int invalidLines,
        int top,
        string format,
        string outputPath)
    {
        if (format == "json")
        {
            WriteJson(
                aggregator,
                validLines,
                invalidLines,
                top,
                outputPath);
        }
        else if (format == "csv")
        {
            WriteCsv(
                aggregator,
                validLines,
                invalidLines,
                top,
                outputPath);
        }
    }

    private void WriteJson(
        LogAggregator aggregator,
        int validLines,
        int invalidLines,
        int top,
        string outputPath)
    {
        var result = new
        {
            ValidLines = validLines,
            InvalidLines = invalidLines,
            TotalEvents = aggregator.TotalEvents,
            Levels = aggregator.Levels,
            Sources = aggregator.Sources,
            TopErrorCodes = aggregator.GetTopErrorCodes(top)
        };

        string json = JsonSerializer.Serialize(
            result,
            new JsonSerializerOptions
            {
                WriteIndented = true
            });

        File.WriteAllText(outputPath, json);

        Console.WriteLine();
        Console.WriteLine("JSON-отчёт сохранён: " + outputPath);
    }

    private void WriteCsv(
        LogAggregator aggregator,
        int validLines,
        int invalidLines,
        int top,
        string outputPath)
    {
        StringBuilder csv = new StringBuilder();

        csv.AppendLine("Категория;Название;Количество");

        csv.AppendLine($"Статистика;Корректные строки;{validLines}");
        csv.AppendLine($"Статистика;Ошибочные строки;{invalidLines}");
        csv.AppendLine($"Статистика;События после фильтрации;{aggregator.TotalEvents}");

        foreach (var item in aggregator.Levels)
        {
            csv.AppendLine($"Уровень;{item.Key};{item.Value}");
        }

        foreach (var item in aggregator.Sources)
        {
            csv.AppendLine($"Источник;{item.Key};{item.Value}");
        }

        foreach (var item in aggregator.GetTopErrorCodes(top))
        {
            csv.AppendLine($"Код ошибки;{item.Key};{item.Value}");
        }

        File.WriteAllText(outputPath, csv.ToString(), Encoding.UTF8);

        Console.WriteLine();
        Console.WriteLine("CSV-отчёт сохранён: " + outputPath);
    }
}