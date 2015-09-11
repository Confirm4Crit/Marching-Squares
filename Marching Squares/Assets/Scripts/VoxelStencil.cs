using UnityEngine;

public class VoxelStencil
{
    private bool _fillType;

    public  void Initialize(bool fillType)
    {
        _fillType = fillType;
    }
    public bool Apply(int x, int y)
    {
        return _fillType;
    }
}