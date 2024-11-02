using System.Collections.Generic;
using System.Linq;
using UnityEngine;

internal struct VertexData
{
    public Vector3 position;
    public Vector3 normal;
    public Vector2 uv;
}

public class GenerateTexture : MonoBehaviour
{
    [SerializeField]
    private ComputeShader computeShader;

    [SerializeField]
    private int textureSize;

    private Vector2 offset;
    private int handle;
    private ComputeBuffer vertexBuffer;

    private const int vertexCount = 512 * 512;

    private void Awake() {
        offset = new Vector2(0, 0);
        handle = computeShader.FindKernel("CSMain");
    }

    private void UpdateMesh() {
        var vertices = new VertexData[vertexCount];
        vertexBuffer.GetData(vertices);

        var indices = new List<int>();

        for (var row = 0; row <= 510; row++) {
            for (var col = 0; col <= 510; col++) {
                var idx = row * 511 + col;
                indices.Add(idx);
                indices.Add(idx + 513);
                indices.Add(idx + 1);
                indices.Add(idx);
                indices.Add(idx + 512);
                indices.Add(idx + 513);
            }
        }

        var mesh = new Mesh {
            vertices = vertices.Select(v => v.position).ToArray(),
            normals = vertices.Select(v => v.normal).ToArray(),
            uv = vertices.Select(v => v.uv).ToArray(),
            triangles = indices.ToArray(),
            name = "Terrain"
        };

        // gameObject.GetComponent<MeshFilter>().mesh = mesh;
    }

    // Start is called before the first frame update
    private void Start() {
        vertexBuffer = new ComputeBuffer(vertexCount, sizeof(float) * 8); // 3 for position, 3 for normal, 2 for uv

        computeShader.SetVector("Offset", offset);
        computeShader.SetFloat("Scale", 10);
        computeShader.SetBuffer(handle, "verticesBuffer", vertexBuffer);

        computeShader.Dispatch(handle, textureSize / 8, textureSize / 8, 1);
        UpdateMesh();
    }

    private void Update() {
        offset.x += Time.deltaTime * 100f;

        computeShader.SetVector("Offset", offset);
        computeShader.Dispatch(handle, textureSize / 8, textureSize / 8, 1);
    }

    private void OnDestroy() {
        vertexBuffer.Release();
    }
}