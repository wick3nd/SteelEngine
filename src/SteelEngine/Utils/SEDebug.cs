using System.Runtime.CompilerServices;

namespace SteelEngine.Utils
{
    public enum SEDebugState
    {
        Debug,
        Log,
        Info,
        Warning,
        Error
    };
    
    class SEDebug
    {   
        private static readonly string _fileName = @$"Logs\log_{DateTime.Now.ToString().Replace(":", "").Replace(".", "-").Replace(" ", "_")}.txt";

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static async Task LogAsync<T>(SEDebugState state, T text, bool throwException = false)
        {
            using StreamWriter output = new(_fileName, true);
            string log = $"[{DateTime.Now}] | [{state}]    {text}";
            var previousColor = Console.ForegroundColor;

            if (!throwException)
            {
                Console.ForegroundColor = state switch
                {
                    SEDebugState.Log => ConsoleColor.White,
                    SEDebugState.Info => ConsoleColor.Cyan,
                    SEDebugState.Warning => ConsoleColor.DarkYellow,
                    SEDebugState.Error => ConsoleColor.DarkRed,
                    _ => previousColor
                };
                await output.WriteLineAsync(log);

                Console.Out.Write($"{log}\n");
                Console.ForegroundColor = previousColor;

                return;
            }

            await output.WriteLineAsync(log);
            throw new Exception(log);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static async void Log<T>(SEDebugState state, T text, bool throwException = false)
        {
            string log = $" [{DateTime.Now}] | [{state}]    {text}";
            var previousColor = Console.ForegroundColor;
            using StreamWriter output = new(_fileName, true);

            if (!throwException)
            {
                Console.ForegroundColor = state switch
                {
                    SEDebugState.Log => ConsoleColor.White,
                    SEDebugState.Info => ConsoleColor.Cyan,
                    SEDebugState.Warning => ConsoleColor.DarkYellow,
                    SEDebugState.Error => ConsoleColor.DarkRed,
                    _ => previousColor
                };
                await output.WriteLineAsync(log);

                Console.Out.Write($"{log}{Environment.NewLine}");
                Console.ForegroundColor = previousColor;

                return;
            }

            await output.WriteLineAsync(log);
            throw new Exception(log);
        }
    }
}