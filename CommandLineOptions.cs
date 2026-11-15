namespace LogAnalyzer;

public class CommandLineOptions
{
    public string Input { get; set; } = string.Empty;
    public DateTime? From { get; set; }
    public DateTime? To { get; set; }
    public string? Level { get; set; }
    public string? Source { get; set; }
    public int Top { get; set; } = 5;
    public string Format { get; set; } = "json";
    public string? Output { get; set; }

    public static bool ParseArguments(
        string[] args,
        out CommandLineOptions? options,
        out string error)
    {
        options = new CommandLineOptions();
        error = string.Empty;

        for (int i = 0; i < args.Length; i++)
        {
            string argument = args[i];

            if (i + 1 >= args.Length)
            {
                error = $"Не указано значение для параметра {argument}";
                options = null;
                return false;
            }

            string value = args[i + 1];
            i++;

            switch (argument)
            {
                case "--input":
                    options.Input = value;
                    break;

                case "--from":
                    if (!DateTime.TryParse(
                            value,
                            null,
                            System.Globalization.DateTimeStyles.RoundtripKind,
                            out DateTime from))
                    {
                        error = "Неверная дата для параметра --from";
                        options = null;
                        return false;
                    }

                    options.From = from;
                    break;

                case "--to":
                    if (!DateTime.TryParse(
                            value,
                            null,
                            System.Globalization.DateTimeStyles.RoundtripKind,
                            out DateTime to))
                    {
                        error = "Неверная дата для параметра --to";
                        options = null;
                        return false;
                    }

                    options.To = to;
                    break;

                case "--level":
                    string[] validLevels =
                    {
                        "TRACE", "DEBUG", "INFO", "WARN", "ERROR"
                    };

                    if (!validLevels.Contains(value.ToUpper()))
                    {
                        error = "Недопустимый уровень: " + value;
                        options = null;
                        return false;
                    }

                    options.Level = value.ToUpper();
                    break;

                case "--source":
                    options.Source = value;
                    break;

                case "--top":
                    if (!int.TryParse(value, out int top) || top < 1)
                    {
                        error = "Параметр --top должен быть целым числом больше нуля";
                        options = null;
                        return false;
                    }

                    options.Top = top;
                    break;

                case "--format":
                    if (value != "json" && value != "csv")
                    {
                        error = "Формат должен быть json или csv";
                        options = null;
                        return false;
                    }

                    options.Format = value;
                    break;

                case "--output":
                    options.Output = value;
                    break;

                default:
                    error = "Неизвестный параметр: " + argument;
                    options = null;
                    return false;
            }
        }

        if (string.IsNullOrWhiteSpace(options.Input))
        {
            error = "Необходимо указать параметр --input";
            options = null;
            return false;
        }

        if (options.From.HasValue &&
            options.To.HasValue &&
            options.From.Value >= options.To.Value)
        {
            error = "Дата начала должна быть раньше даты окончания";
            options = null;
            return false;
        }

        return true;
    }
}