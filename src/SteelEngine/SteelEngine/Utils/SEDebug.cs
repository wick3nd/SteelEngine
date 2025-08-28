namespace SteelEngine.Utils
{
    public enum SEDebugState
    {
        Log,
        Info,
        Warning,
        Error
    };

    class SEDebug
    {
        private static readonly string _fileName = @$"Logs\log_{DateTime.Now.ToString().Replace(":", "").Replace(".", "-").Replace(" ", "_")}.txt";

        public static async void Log<T>(SEDebugState state, T text)
        {
            string log = $"> [{DateTime.Now}] | [{state}]    {text}";
            var previousColor = Console.ForegroundColor;

            switch (state)
            {
                case SEDebugState.Log: Console.ForegroundColor = ConsoleColor.White;
                    break;

                case SEDebugState.Info: Console.ForegroundColor = ConsoleColor.Cyan;
                    break;

                case SEDebugState.Warning: Console.ForegroundColor = ConsoleColor.DarkYellow;
                    break;

                case SEDebugState.Error: Console.ForegroundColor = ConsoleColor.DarkRed;
                    break;
            }

            Console.Write($"{log}\n");

            Console.ForegroundColor = previousColor;

            using StreamWriter output = new(_fileName, append: true);
            await output.WriteLineAsync(log);
        }
    }
}