using OpenTK.Graphics.Glx;
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
        private static readonly string _fileName = @$"Logs\log_{DateTime.Now:yyyyMMddhhmmss}.txt";
        private static readonly StreamWriter stream = new(_fileName, true);

        private static readonly Dictionary<SEDebugState, ConsoleColor> _colorMap = new()
        {
            { SEDebugState.Log, ConsoleColor.White },
            { SEDebugState.Info, ConsoleColor.Cyan },
            { SEDebugState.Warning, ConsoleColor.DarkYellow },
            { SEDebugState.Error, ConsoleColor.DarkRed }
        };

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static async Task LogAsync<T>(SEDebugState state, T text, bool throwException = false)
        {
            string log = $" [{DateTime.Now:hh:mm:ss}] | [{state}]    {text}{Environment.NewLine}";
            var previousColor = Console.ForegroundColor;

            if (!throwException)
            {
                Console.ForegroundColor = _colorMap.GetValueOrDefault(state, previousColor);
                await stream.WriteAsync(log);

#if DEBUG
                Console.Out.Write($"{log}");
#endif
                Console.ForegroundColor = previousColor;

                return;
            }

            await stream.WriteAsync(log);
            throw new Exception(log);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static async void Log<T>(SEDebugState state, T text, bool throwException = false)
        {
            string log = $" [{DateTime.Now:hh:mm:ss}] | [{state}]    {text}{Environment.NewLine}";
            var previousColor = Console.ForegroundColor;

            if (!throwException)
            {
                Console.ForegroundColor = _colorMap.GetValueOrDefault(state, previousColor);
                await stream.WriteAsync(log);

#if DEBUG
                Console.Out.Write($"{log}");
#endif
                Console.ForegroundColor = previousColor;

                return;
            }

            await stream.WriteAsync(log);
            throw new Exception(log);
        }
    }
}