public class UIDrawer
{
    public enum OS { MacOS, WinOS, LinuxOS }
    private OSImplementator osImplementator;

    public UIDrawer(OS os) => SetOSImplementator(os);

    public void SetOSImplementator(OS os) => osImplementator = os switch
        {
            OS.WinOS => new WindowsImp(),
            OS.LinuxOS => new LinuxImp(),
            OS.MacOS => new MacImp(),
            _ => throw new ArgumentException("Passed OS doesnt match with any pre-defined OS!")
        };


    public void DrawWindow() => osImplementator.DrawWindow();

}

public abstract class OSImplementator
{
    public abstract void DrawWindow();
}

public class WindowsImp : OSImplementator
{
    public override void DrawWindow() => Console.WriteLine("Windows window");
}

public class MacImp : OSImplementator
{
    public override void DrawWindow() => Console.WriteLine("Mac window");
}

public class LinuxImp : OSImplementator
{
    public override void DrawWindow() => Console.WriteLine("Linux window");
}
