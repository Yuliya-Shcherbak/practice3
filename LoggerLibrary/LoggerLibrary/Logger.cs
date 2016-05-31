using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

public interface IPrintLog
{
    void Print(string message);
}

public class PrintToFile : IPrintLog
{
    public void Print(string message)
    {
        using (StreamWriter sw = File.AppendText("log.txt"))
        {
            sw.Write(DateTime.Now + " :");
            sw.WriteLine(message);
        }
    }
}

public class PrintToConsole : IPrintLog
{
    public void Print(string message)
    {
        Console.WriteLine("{0} : {1}", DateTime.Now, message);
    }
}

public class Logger
{
    public IPrintLog LogContext { get; set; }

    public Logger(IPrintLog _logcontext)
    {
        LogContext = _logcontext;
    }

    public void NowPrint(IPrintLog print, string message)
    {
        LogContext.Print(message);
    }

    public void Info(string firstname, string lastname, string action)
    {
        string message = " User " + firstname + " " + lastname + " was " + action;
        NowPrint(LogContext, message);
    }
    public void Debug(string action)
    {
        string message = " Users list was requsted for " + action;
        NowPrint(LogContext, message);
    }
    public void Warning(string firstname, string lastname)
    {
        string message = " failed to remove User " + firstname + " " + lastname;
        NowPrint(LogContext, message);
    }
    public void Eror(string firstname, string lastname)
    {
        string message = " failed to add new User " + firstname + " " + lastname;
        NowPrint(LogContext, message);
    }
}