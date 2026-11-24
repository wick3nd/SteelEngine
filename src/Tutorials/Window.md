# Chapter 1: Window creation

Lorem Ipsum
```C#
public class BaseApp : SEWindow
{
    public BaseApp() : base("mainWindow", new SEWindowSettings()
    {
        Title = "Test",
        WindowFlags = WindowFlags.Resizable,
        Context = ContextAPI.OpenGL
    }) { }
}
```

Lorem Ipsum
```C#
private static void Main()
{
    var window = new BaseApp();

    _ = new Program();  // Your program entry point

    window.Run();
}

```