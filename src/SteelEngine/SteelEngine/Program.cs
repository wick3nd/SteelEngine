namespace SteelEngine
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var window = new WindowLoop(800, 600, "Steel Engine"))
            {
                window.Run();
            }
        }
    }
}
