using UnityEngine;

[SelectionBase]
public class VoxelGrid : MonoBehaviour
{

    public int Resoultion;

    public GameObject VoxelPrefab;
    // ReSharper disable once NotAccessedField.Local
    private bool[] _voxels;
    private float _voxelSize;

    private Material[] _voxelMaterials;
    public void Initialize(int resolution, float size)
    {
        Resoultion = resolution;
        _voxelSize = size / resolution;
        _voxels = new bool[resolution * resolution];
        _voxelMaterials = new Material[_voxels.Length];
        for (int i = 0, y = 0; y < resolution; y++)
        {
            for (int x = 0; x < resolution; x++, i++)
            {
                CreateVoxel(i, x, y);
            }
        }
        SetVoxelColor();
    }
    public void Apply(VoxelStencil stencil)
    {
        int xStart = stencil.XStart;
        if (xStart < 0)
        {
            xStart = 0;
        }
        int xEnd = stencil.XEnd;
        if (xEnd >= Resoultion)
        {
            xEnd = Resoultion - 1;
        }
        int yStart = stencil.YStart;
        if (yStart < 0)
        {
            yStart = 0;
        }
        int yEnd = stencil.YEnd;
        if (yEnd >= Resoultion)
        {
            yEnd = Resoultion - 1;
        }
        for (int y = yStart; y <= yEnd; y++)
        {
            int i = y*Resoultion + xStart;
            for (int x = xStart; x <= xEnd; x++, i++)
            {
                _voxels[i] = stencil.Apply(x, y);
            }
        }
        SetVoxelColor();
    }
    private void SetVoxelColor()
    {
        for (int i = 0; i < _voxels.Length; i++)
        {
            _voxelMaterials[i].color = _voxels[i] ? Color.black : Color.white;
        }
    }

    private void CreateVoxel(int i, int i1, int i2)
    {
        GameObject o = Instantiate(VoxelPrefab);
        o.transform.parent = transform;
        o.transform.localPosition = new Vector3((i1 + 0.5f) * _voxelSize, (i2 + 0.5f) * _voxelSize);
        o.transform.localScale = Vector3.one * _voxelSize *.9f;
        _voxelMaterials[i] = o.GetComponent<MeshRenderer>().material;
    }

   
}
