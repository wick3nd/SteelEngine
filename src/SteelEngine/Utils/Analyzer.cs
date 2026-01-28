namespace SteelEngine.Utils
{
#pragma warning disable CS0649 // temporarily
    public static class Analyzer
    {
        internal static ulong totalEBOSize;
        internal static ulong totalFBOSize;
        internal static ulong totalRBOSize;
        internal static ulong totalSSBOSize;
        internal static ulong totalUBOSize;
        internal static ulong totalVAOSize;
        internal static ulong totalVBOSize;
        internal static ulong totalTextureSize;
        internal static ulong totalMeshSize;
        internal static ulong totalShaderSize;

        public static ulong GetVRamUsage() =>
            totalEBOSize +
            totalFBOSize +
            totalRBOSize +
            totalSSBOSize +
            totalUBOSize +
            totalVAOSize +
            totalVBOSize +
            totalTextureSize +
            totalMeshSize +
            totalShaderSize;

        public static int GetFrameRate()
        {
            return 0;
        }

        public static int Get1PercentLow()
        {
            return 0;
        }

        public static int GetMinFrameRate()
        {
            return 0;
        }

        public static int GetMaxFrameRate()
        {
            return 0;
        }

        public static float GetFrameTime()
        {
            return 0;
        }

        public static string GetBinds()
        {
            return "";
        }
    }
}