using UnityEngine;

public class VoxelMap : MonoBehaviour {
    public float Size = 2f;

    public int VoxelResolution = 8;
    public int ChunkResolution = 2;

    public VoxelGrid VoxelGridPrefab;

    private VoxelGrid[] _chunks;

    private float _chunkSize, _voxelSize, _halfSize;

    private static string[] _fillTypeNames = {"Filled", "Empty"};

    private int _fillTypeIndex, _radiusIndex;

    private static string[] _radiusNames = { "0", "1", "2", "3", "4", "5" };
    private void OnGUI()
    {
        GUILayout.BeginArea(new Rect(4f, 4f, 150f, 500f));
        GUILayout.Label("Fill Type");
        _fillTypeIndex = GUILayout.SelectionGrid(_fillTypeIndex, _fillTypeNames, 2);
        GUILayout.Label("Radius");
        _radiusIndex = GUILayout.SelectionGrid(_radiusIndex, _radiusNames, 6);
        GUILayout.EndArea();
    }
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
        int centerX = (int)((point.x + _halfSize) / _voxelSize);
        int centerY = (int)((point.y + _halfSize) / _voxelSize);
        int xStart = (centerX - _radiusIndex) / VoxelResolution;
        if (xStart < 0)
        {
            xStart = 0;
        }
        int xEnd = (centerX + _radiusIndex) / VoxelResolution;
        if (xEnd >= ChunkResolution)
        {
            xEnd = ChunkResolution - 1;
        }
        int yStart = (centerY - _radiusIndex) / VoxelResolution;
        if (yStart < 0)
        {
            yStart = 0;
        }
        int yEnd = (centerY + _radiusIndex) / VoxelResolution;
        if (yEnd >= ChunkResolution)
        {
            yEnd = ChunkResolution - 1;
        }
        int chunkX = centerX / VoxelResolution;
        int chunkY = centerY / VoxelResolution;
        centerX -= chunkX * VoxelResolution;
        centerY -= chunkY * VoxelResolution;
        VoxelStencil activeStencil = new VoxelStencil();
        activeStencil.Initialize(_fillTypeIndex == 0, _radiusIndex);

        activeStencil.SetCenter(centerX,centerY);
        _chunks[chunkY * ChunkResolution + chunkX].Apply(activeStencil);

        int voxelYOffset = yStart * VoxelResolution;
        for (int y = yStart; y <= yEnd; y++)
        {
            int i = y * ChunkResolution + xStart;
            int voxelXOffset = xStart * VoxelResolution;
            for (int x = xStart; x <= xEnd; x++, i++)
            {
                activeStencil.SetCenter(centerX - voxelXOffset, centerY - voxelYOffset);
                _chunks[i].Apply(activeStencil);
                voxelXOffset += VoxelResolution;
            }
            voxelYOffset += VoxelResolution;
        }
        Debug.Log(centerX + ", " + centerY + " in chunk " + chunkX + ", " + chunkY);
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
