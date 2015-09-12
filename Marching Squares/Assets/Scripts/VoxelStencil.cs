public class VoxelStencil
{
    private bool _fillType;

    private int _centerX, _centerY, _radius;

    public  void Initialize(bool fillType, int radius)
    {
        _fillType = fillType;
        _radius = radius;
    }

    public void SetCenter(int x, int y)
    {
        _centerX = x;
        _centerY = y;
    }
    public bool Apply(int x, int y)
    {
        return _fillType;
    }

    public int XStart
    {
        get
        {
            return _centerX - _radius;
        }
    }

    public int XEnd
    {
        get
        {
            return _centerX + _radius;
        }
    }

    public int YStart
    {
        get
        {
            return _centerY - _radius;
        }
    }

    public int YEnd
    {
        get
        {
            return _centerY + _radius;
        }
    }
}