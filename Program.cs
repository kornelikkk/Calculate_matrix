using System;
using System.IO;

public class Calculator
{
    private const string LogFilePath = "CalculatorLog.txt";

    public static void Main(string[] args)
    {
        try
        {
            Console.WriteLine("Введите коэффициенты для уравнений в формате:");
            Console.WriteLine("a1 b1 c1 d1");
            Console.WriteLine("a2 b2 c2 d2");
            Console.WriteLine("a3 b3 c3 d3");

            double[,] coefficients = new double[3, 4];

            // Ввод коэффициентов для трех уравнений
            for (int i = 0; i < 3; i++)
            {
                string input = Console.ReadLine().Trim();
                if (string.IsNullOrWhiteSpace(input))
                {
                    throw new FormatException("Ввод не должен быть пустым.");
                }

                string[] parts = input.Split();
                if (parts.Length != 4)
                {
                    throw new FormatException("Неверный формат ввода данных. Ожидается 4 числа.");
                }

                for (int j = 0; j < 4; j++)
                {
                    if (!double.TryParse(parts[j], out coefficients[i, j]))
                    {
                        throw new FormatException($"Не удалось преобразовать '{parts[j]}' в число.");
                    }
                }
            }

            // Вывод введенных уравнений
            Console.WriteLine("Введенные уравнения:");
            for (int i = 0; i < 3; i++)
            {
                Console.WriteLine($"{coefficients[i, 0]}*x1 + {coefficients[i, 1]}*x2 + {coefficients[i, 2]}*x3 = {coefficients[i, 3]}");
            }

            // Логируем введенные данные
            LogInput(coefficients);

            // Решаем систему уравнений
            double[] result = Solve(coefficients);

            // Логируем результаты решения
            LogResult(result);

            // Выводим результат на экран
            Console.WriteLine("Решение системы уравнений:");
            for (int i = 0; i < result.Length; i++)
            {
                Console.WriteLine($"x{i + 1} = {result[i]}");
            }

            // Спрашиваем пользователя, хочет ли он просмотреть лог
            Console.WriteLine("Просмотреть протокол? (да/нет)");
            if (Console.ReadLine().ToLower() == "да")
            {
                ViewLog();
            }
        }
        catch (Exception ex)
        {
            // Логируем ошибки и выводим сообщение об ошибке
            LogError(ex.Message);
            Console.WriteLine("Ошибка: " + ex.Message);
        }
    }

    // Метод для логирования входных данных
    private static void LogInput(double[,] coefficients)
    {
        using (StreamWriter writer = new StreamWriter(LogFilePath, true))
        {
            writer.WriteLine("Входные данные:");
            for (int i = 0; i < 3; i++)
            {
                writer.WriteLine($"{coefficients[i, 0]} {coefficients[i, 1]} {coefficients[i, 2]} {coefficients[i, 3]}");
            }
        }
    }

    // Метод для логирования результатов
    private static void LogResult(double[] result)
    {
        using (StreamWriter writer = new StreamWriter(LogFilePath, true))
        {
            writer.WriteLine("Результаты:");
            for (int i = 0; i < result.Length; i++)
            {
                writer.WriteLine($"x{i + 1} = {result[i]}");
            }
        }
    }

    // Метод для логирования ошибок
    private static void LogError(string message)
    {
        using (StreamWriter writer = new StreamWriter(LogFilePath, true))
        {
            writer.WriteLine("Ошибка: " + message);
        }
    }

    // Метод для просмотра лога
    private static void ViewLog()
    {
        if (File.Exists(LogFilePath))
        {
            string logContent = File.ReadAllText(LogFilePath);
            Console.WriteLine("Протокол:");
            Console.WriteLine(logContent);
        }
        else
        {
            Console.WriteLine("Протокол не найден.");
        }
    }

    // Метод для решения системы уравнений методом Гаусса
    private static double[] Solve(double[,] coefficients)
    {
        double[,] matrix = {
            { coefficients[0, 0], coefficients[0, 1], coefficients[0, 2], coefficients[0, 3] },
            { coefficients[1, 0], coefficients[1, 1], coefficients[1, 2], coefficients[1, 3] },
            { coefficients[2, 0], coefficients[2, 1], coefficients[2, 2], coefficients[2, 3] }
        };

        // Прямой ход метода Гаусса
        for (int i = 0; i < 3; i++)
        {
            // Проверка на деление на ноль
            if (matrix[i, i] == 0) throw new Exception("Система не имеет уникального решения.");

            for (int j = i + 1; j < 3; j++)
            {
                double ratio = matrix[j, i] / matrix[i, i];
                for (int k = i; k < 4; k++)
                {
                    matrix[j, k] -= ratio * matrix[i, k];
                }
            }
        }

        // Обратный ход метода Гаусса
        double[] result = new double[3];
        for (int i = 2; i >= 0; i--)
        {
            result[i] = matrix[i, 3] / matrix[i, i];
            for (int j = i - 1; j >= 0; j--)
            {
                matrix[j, 3] -= matrix[j, i] * result[i];
            }
        }

        return result;
    }
}
