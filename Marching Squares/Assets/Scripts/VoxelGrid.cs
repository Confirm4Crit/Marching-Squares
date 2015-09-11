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
    public void Apply(int voxelX, int voxelY, VoxelStencil stencil)
    {
        _voxels[voxelY * Resoultion + voxelX] = stencil.Apply(voxelX, voxelY);
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
