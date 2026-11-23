using OpenTK.Graphics.OpenGL;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

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
    
    public static class SEDebug
    {
        internal static GLDebugProc DebugMessageDelegate = OnDebugMessage;
        private static readonly string _fileName = @$"Logs\log_{DateTime.Now:yyyyMMddhhmmss}.txt";
        private static readonly StreamWriter _stream = new(_fileName, true);
        private static string _debugStringCache = "";

        private static readonly Dictionary<DebugType, SEDebugState> _stateMap = new()
        {
            { DebugType.DebugTypeError, SEDebugState.Error },
            { DebugType.DebugTypeDeprecatedBehavior, SEDebugState.Warning },
            { DebugType.DebugTypeOther, SEDebugState.Debug },
            { DebugType.DebugTypePerformance, SEDebugState.Warning },
            { DebugType.DebugTypeUndefinedBehavior, SEDebugState.Error },
            { DebugType.DontCare, SEDebugState.Info }
        };

        internal static void Init()
        {
            if (!Directory.Exists("Logs")) Directory.CreateDirectory("Logs");
        }

        public static async void Log<T>(SEDebugState state, T text, bool throwException = false)
        {
            string log = $" [{DateTime.Now:hh:mm:ss}] | [{Pad(state)}]    {text}{Environment.NewLine}";
            _debugStringCache += log;

            if (!throwException) return;

            Flush();
            throw new Exception();
        }

        /// <summary>
        /// Should not be needed but is here because of a dumb logging implementation
        /// </summary>
        public static async void Pad()
        {
            _debugStringCache += Environment.NewLine;
        }

        private static void OnDebugMessage(DebugSource source, DebugType type, uint id, DebugSeverity severity, int length, IntPtr pMessage, IntPtr pUserParam)      // The pointer you gave to OpenGL, explained later.
        {
            string message = Marshal.PtrToStringAnsi(pMessage, length);

            string log = $" [{DateTime.Now:hh:mm:ss}] | [{Pad(_stateMap.GetValueOrDefault(type, SEDebugState.Debug))}]    {message}{Environment.NewLine}";
            _debugStringCache += log;

            if (type == DebugType.DebugTypeError)
            {
                Flush();
                throw new Exception();
            }
        }

        internal static void Flush()
        {
            Console.Out.Write($"{_debugStringCache}");


            _stream.Write(_debugStringCache);
            _stream.Flush();

            _debugStringCache = "";
        }

        static string Pad(SEDebugState state)
        {
            string text = state.ToString();
            return text.Length > 9 ? text[.. 9] : text.PadRight(9);
        }
    }
}