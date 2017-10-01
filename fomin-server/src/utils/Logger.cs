using System;
using System.Diagnostics;

namespace fomin_server.utils
{
    public static class Logger
    {
        public static bool PrintTime = true;

        public static void D(string message, params object[] args)
        {
            Log(message, args, LogLevel.Debug);
        }

		public static void I(string message, params object[] args)
		{
			Log(message, args);
		}

		public static void W(string message, params object[] args)
		{
			Log(message, args, LogLevel.Warning);
		}

		public static void E(string message, params object[] args)
		{
			Log(message, args, LogLevel.Error);
		}

        public static void Log(string message,
                                object[] args = null,
                                LogLevel level = LogLevel.Info)
        {
            if (args != null && args.NotEmpty())
                message = string.Format(message, args);
            var temp = Console.ForegroundColor;

            switch (level)
            {
                case LogLevel.Debug:
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    break;

                case LogLevel.Info:
                    Console.ForegroundColor = ConsoleColor.Gray;
                    break;

                case LogLevel.Warning:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    break;

                case LogLevel.Error:
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
            }

            string callInfo = "";

            StackTrace stackTrace = new StackTrace(0, true);
            var stackFrames = stackTrace.GetFrames();
            if (stackFrames != null)
            {
                StackFrame frame = stackFrames[2];
                var methodBase = frame.GetMethod();
                Type clazz = methodBase.DeclaringType;

                if (clazz != null)
                {
                    callInfo = $"/{clazz.Name}({frame.GetFileLineNumber()})";
                }
                
            }
            
            Console.WriteLine("{0}{1}{2}: {3}", GetTimestamp(DateTime.Now), 
                level.ToString().ToUpper(), callInfo, message);

            Console.ForegroundColor = temp;
        }

        public static string GetTimestamp(DateTime value)
        {
            if (!PrintTime) return "";
            return value.ToString("MM-dd HH:mm:ss.ffff") + ": ";
        }
    }

    public enum LogLevel
    {
        Debug,
        Info,
        Warning,
        Error
    }
}
