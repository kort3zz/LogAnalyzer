namespace LogAnalyzer;

internal class Program
{
    static void Main(string[] args)
    {
        if (!CommandLineOptions.ParseArguments(
                args,
                out CommandLineOptions? options,
                out string error))
        {
            Console.WriteLine("Ошибка: " + error);
            return;
        }

        if (!File.Exists(options!.Input))
        {
            Console.WriteLine("Файл не найден: " + options.Input);
            return;
        }

        LogParser parser = new LogParser();
        LogFilter filter = new LogFilter();
        LogAggregator aggregator = new LogAggregator();
        ResultWriter writer = new ResultWriter();

        int validLines = 0;
        int invalidLines = 0;
        int lineNumber = 0;

        using (StreamReader reader = new StreamReader(options.Input))
        {
            string? line;

            while ((line = reader.ReadLine()) != null)
            {
                lineNumber++;

                if (!parser.TryParse(line, out var entry, out string parseError))
                {
                    invalidLines++;

                    Console.WriteLine(
                        $"Строка {lineNumber}: {parseError}");

                    continue;
                }

                validLines++;

                if (filter.Matches(
                        entry!,
                        options.From,
                        options.To,
                        options.Level,
                        options.Source))
                {
                    aggregator.Add(entry!);
                }
            }
        }

        writer.WriteToConsole(
            aggregator,
            validLines,
            invalidLines,
            options.Top);

        string outputPath = options.Output ?? $"result.{options.Format}";

        writer.WriteToFile(
            aggregator,
            validLines,
            invalidLines,
            options.Top,
            options.Format,
            outputPath);
    }
}