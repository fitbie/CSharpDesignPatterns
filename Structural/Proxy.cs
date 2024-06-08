// One of proxy examples in .NET - Lazy<> type.
using System.Numerics;

public class ImageProxy : Image
{
    private string imageAdress;
    private readonly Image? image;

    public ImageProxy(string fileName) => imageAdress = fileName;

    public override void SetSize(Vector2 size) => this.size = size;

    public override void Draw(Vector2 position, Vector2 size)
    {
        // Load image from storage by I/O operation.
        image!.Draw(position, size);
    }

}

public class Image
{
    protected Vector2 size;
    protected Vector2 position;
    public virtual void SetSize(Vector2 size) {}
    public virtual void SetPosition(Vector2 position) {}
    public virtual void Draw(Vector2 position, Vector2 size) {}
}