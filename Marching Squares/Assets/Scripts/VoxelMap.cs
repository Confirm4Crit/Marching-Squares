using UnityEngine;

public class VoxelMap : MonoBehaviour {

    public float Size = 2f;

    public int VoxelResolution = 8;
    public int ChunkResolution = 2;

    public VoxelGrid VoxelGridPrefab;

    private VoxelGrid[] _chunks;

    private float _chunkSize, _voxelSize, _halfSize;

    private void Awake()
    {
        _halfSize = Size * 0.5f;
        _chunkSize = Size / ChunkResolution;
        _voxelSize = _chunkSize / VoxelResolution;

        _chunks = new VoxelGrid[ChunkResolution * ChunkResolution];
        for (int i = 0, y = 0; y < ChunkResolution; y++)
        {
            for (int x = 0; x < ChunkResolution; x++, i++)
            {
                CreateChunk(i, x, y);
            }
        }
        BoxCollider box = gameObject.AddComponent<BoxCollider>();
        box.size = new Vector3(Size, Size);
    }

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            RaycastHit hitInfo;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo))
            {
                if (hitInfo.collider.gameObject == gameObject)
                {
                    EditVoxels(transform.InverseTransformPoint(hitInfo.point));
                }
            }
        }
    }

    private void EditVoxels(Vector3 point)
    {
        int voxelX = (int)((point.x + _halfSize) / _voxelSize);
        int voxelY = (int)((point.y + _halfSize) / _voxelSize);
        int chunkX = voxelX / VoxelResolution;
        int chunkY = voxelY / VoxelResolution;
        voxelX -= chunkX * VoxelResolution;
        voxelY -= chunkY * VoxelResolution;
        _chunks[chunkY * ChunkResolution + chunkX].SetVoxel(voxelX, voxelY, true);
        Debug.Log(voxelX + ", " + voxelY + " in chunk " + chunkX + ", " + chunkY);
    }

    private void CreateChunk(int i, int x, int y)
    {
        VoxelGrid chunk = Instantiate(VoxelGridPrefab);
        chunk.Initialize(VoxelResolution, _chunkSize);
        chunk.transform.parent = transform;
        chunk.transform.localPosition = new Vector3(x * _chunkSize - _halfSize, y * _chunkSize - _halfSize);
        _chunks[i] = chunk;
    }
}
