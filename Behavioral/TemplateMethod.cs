namespace Patterns.GOF;

/// <summary>
/// Template method is method inside some base class (usually <see langword="abstract"/> class), which defines structure ("skeleton") of algorithm, 
/// but its specific steps are left to the implementation in inherited classes, usually this achieved by putting those "steps" into separate abstract (or virtual) methods.
/// <br/> Such approach also called "Bracketing in order to generalize", i.e., instead of defining a single method in the base class, e.g. a method for processing file data, 
/// which will then be overridden in derived classes, besides - most likely the common steps will be repeated (e.g. open a file, load bytes into a buffer, process according
/// to the file type, return the result) - we put these steps into separate, usually abstract methods, leaving our "main" method as a public entry point that consists of
/// abstract operations, which are then implemented in derived.
/// <br/> This is a very useful and often used pattern that allows you to replace all or individual parts of complex logic. 
/// Often, the base class defined as non-abstract, and the methods are marked virtual, allowing you to override only a single step of the algorithm.
/// </summary>
public class TemplateMethodPattern
{
    public static void Start()
    {
        // Of course this is super silly example and we could apply other learned patterns here, but i want to keep it simple to show Template method work.

        string[] files = ["Kitties.jpeg", "ValuableData.svg"];

        FileContentDisplay display;

        foreach (var file in files)
        { 
            display = Path.GetExtension(file) switch
            {
                ".jpeg" => new JpegFileContentDisplay(),
                ".svg" => new SvgFileContentDisplay(),
                _ => throw new ArgumentException(Path.GetExtension(file))
            };

            // We're calling only one Template method here, which is consist of different abstract step.
            display.TemplateMethod(file);
        }
    }
}


public abstract class FileContentDisplay
{
    // Instead of making it virtual or abstract and repeat huge, but structurally same logic in inheritors, we extract its' steps into separate abstract methods.
    public void TemplateMethod(string filePath)
    {
        // Of course such implementation is silly in terms of IO programming, it is just an example.
        byte[] buffer = new byte[256];
        int bytesRead = ReadData(ref buffer); // Abstract step 1

        if (HandleData(new Span<byte>(buffer, 0, bytesRead))) // Abstract step 2
        {
            DisplayData(); // Abstract step 3
        }
    }


    protected abstract int ReadData(ref byte[] buffer);
    protected abstract bool HandleData(Span<byte> buffer);
    protected abstract void DisplayData();
}


// All implementation are mocks just to show the logic
public class JpegFileContentDisplay : FileContentDisplay
{
    protected override int ReadData(ref byte[] buffer)
    {
        Console.WriteLine("Read N bytes from file system into buffer..");
        return 10;
    }

    protected override bool HandleData(Span<byte> buffer)
    {
        Console.WriteLine("Decoding JPEG bytes to color blocks and store them..");
        return true;
    }

    protected override void DisplayData()
    {
        Console.WriteLine("Pretty JPEG image, or not\n");
    }
}

public class SvgFileContentDisplay : FileContentDisplay
{
    protected override int ReadData(ref byte[] buffer)
    {
        Console.WriteLine("Read N bytes from TCP socket into buffer..");
        return 51;
    }

    protected override bool HandleData(Span<byte> buffer)
    {
        Console.WriteLine("Decoding SVG bytes into table entries and store them..");
        return true;
    }

    protected override void DisplayData()
    {
        Console.WriteLine("|Some|Table|With|Valuable|Data|\n");
    }
}