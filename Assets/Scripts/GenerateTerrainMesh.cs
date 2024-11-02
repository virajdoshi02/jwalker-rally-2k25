using System.Linq;
using UnityEngine;

internal struct VertexData
{
    public Vector3 position;
    public Vector3 normal;
}

public class GenerateTerrainMesh : MonoBehaviour
{
    [SerializeField]
    private RenderTexture heightMap;

    [SerializeField]
    private ComputeShader terrainShader;

    private ComputeBuffer vertexBuffer;
    private ComputeBuffer indexBuffer;

    private const int vertexCount = 512 * 512;
    private int indexCount;
    private int handle;

    // Start is called before the first frame update
    private void Start() {
        indexCount = vertexCount * 3;
        handle = terrainShader.FindKernel("CSMain");

        vertexBuffer = new ComputeBuffer(vertexCount, sizeof(float) * 6); // 3 for position, 3 for normal
        indexBuffer = new ComputeBuffer(indexCount, sizeof(uint));

        terrainShader.SetBuffer(handle, "verticesBuffer", vertexBuffer);
        terrainShader.SetBuffer(handle, "indicesBuffer", indexBuffer);
        terrainShader.Dispatch(handle, 512 / 8, 512 / 8, 1);

        var vertices = new VertexData[vertexCount];
        vertexBuffer.GetData(vertices);

        var indices = new uint[indexCount];
        indexBuffer.GetData(indices);

        var mesh = new Mesh();
        mesh.vertices = vertices.Select(v => v.position).ToArray();
        mesh.normals = vertices.Select(v => v.normal).ToArray();
        mesh.triangles = indices.Select(i => (int)i).ToArray();

        gameObject.GetComponent<MeshFilter>().mesh = mesh;

        vertexBuffer.Release();
        indexBuffer.Release();
    }
}