namespace SteelEngine
{
    public static class Time
    {
        public static double DeltaTimeD { get; internal set; }
        public static float DeltaTime => (float)DeltaTimeD;
    }
}
