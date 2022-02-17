// See https://aka.ms/new-console-template for more information
using System;
using System.Diagnostics;

var upstreamActivity = new Activity("Upstream");

upstreamActivity.Start();
ConsoleColor.Green.WriteLine(upstreamActivity.OperationName);
ConsoleColor.Cyan.Write("traceparent: ");
PrintActivitySegments(activity: upstreamActivity.Id);
Console.WriteLine("upstream traceparent: {0}", upstreamActivity.ParentId);
CallChildActivity(3);
upstreamActivity.Stop();

void CallChildActivity(int nested)
{
    if (nested-- == 0)
        return;
    var downstreamActivity = new Activity($"Context-Id: {nested}");
    downstreamActivity.Start();
    ConsoleColor.DarkGray.WriteLine($"{downstreamActivity.OperationName} {downstreamActivity.SpanId.ToString()} {downstreamActivity.TraceId.ToString()}" );

    ConsoleColor.Cyan.Write("traceparent (Id):       ");
    PrintActivitySegments(activity: downstreamActivity.Id);

    ConsoleColor.Cyan.Write("traceparent (ParentId): ");
    PrintActivitySegments(activity: downstreamActivity.ParentId);
    
    CallChildActivity(nested);

    downstreamActivity.Stop();
}

void PrintActivitySegments(string? activity)
{
    if (activity == null) return;

    var activitySegment = activity.Split("-");
    ConsoleColor.DarkCyan.Write(activitySegment[0]);
    ConsoleColor.DarkGray.Write("-");
    ConsoleColor.DarkGreen.Write(activitySegment[1]);
    ConsoleColor.DarkGray.Write("-");
    ConsoleColor.DarkBlue.Write(activitySegment[2]);
    ConsoleColor.DarkGray.Write("-");
    ConsoleColor.DarkMagenta.WriteLine(activitySegment[3]);
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