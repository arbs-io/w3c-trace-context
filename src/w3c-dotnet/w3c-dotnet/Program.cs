using System;
using System.Diagnostics;

var upstreamActivity = new Activity("Context-Id: 0");

upstreamActivity.Start();
ConsoleColor.Green.WriteLine($"Starting w3c context trace");

ConsoleColor.DarkCyan.Write($"ParentId:\t");
var cursorPosition = Console.GetCursorPosition();
Console.SetCursorPosition(42, cursorPosition.Top);
PrintActivitySegments(activity: upstreamActivity);

var rnd = new Random().Next(3, 20);
for (int i = 0; i < rnd; i++)
{
    TracedFunction(new Random().Next(3, 10), 0);
}
upstreamActivity.Stop();

void TracedFunction(int nested, int level)
{
    if (nested == 0)
        return;
    var activity = new Activity($"Context-Id: {nested}");
    activity.Start();

    ConsoleColor.Cyan.Write($"{new String(' ', level)}");
    PrintActivitySegments(activity);

    var rnd = new Random().Next(0, nested);
    TracedFunction(rnd, level + 1);

    activity.Stop();
}

void PrintActivitySegments(Activity activity)
{
    if (activity == null) return;
        
    if (activity.Parent != null)
    {
        ConsoleColor.Gray.Write($" {activity.SpanId}");
        ConsoleColor.DarkGray.Write($" << {activity.Parent.SpanId}");
    }
    
    var activitySegment = activity.Id.Split("-");
    var cursorPosition = Console.GetCursorPosition();
    Console.SetCursorPosition(42, cursorPosition.Top);
    ConsoleColor.DarkCyan.Write(activitySegment[0]);
    ConsoleColor.DarkGray.Write("-");
    ConsoleColor.Yellow.Write(activitySegment[1]);
    ConsoleColor.DarkGray.Write("-");
    ConsoleColor.Green.Write(activitySegment[2]);
    ConsoleColor.DarkGray.Write("-");
    ConsoleColor.Cyan.WriteLine(activitySegment[3]);
}

#region Extension
public static class ConsoleExtensions
{
    public static void WriteLine(this ConsoleColor foregroundColor, string value)
    {
        var originalForegroundColor = Console.ForegroundColor;

        Console.ForegroundColor = foregroundColor;
        Console.WriteLine(value);

        Console.ForegroundColor = originalForegroundColor;

    }
    public static void Write(this ConsoleColor foregroundColor, string value)
    {
        var originalForegroundColor = Console.ForegroundColor;

        Console.ForegroundColor = foregroundColor;
        Console.Write(value);

        Console.ForegroundColor = originalForegroundColor;
    }
    public static void ClearLastLine()
    {
        Console.SetCursorPosition(0, Console.CursorTop - 1);
        Console.Write(new string(' ', Console.BufferWidth));
        Console.SetCursorPosition(0, Console.CursorTop - 1);
    }
}
#endregion //Extension