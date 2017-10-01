using System;

namespace fomin_server.utils
{
    public static class Logger
    {
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

            Console.WriteLine("[{0}] {1}", level, message);

            Console.ForegroundColor = temp;
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
