using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackGenerator : MonoBehaviour
{
    [SerializeField]
    public List<Vector3> targets;
    public Terrain terrain;
    public Material trackMat;

    private void GenerateTrack()
    {
        float trackWidth = 10.0f;

        if (targets == null || targets.Count < 2)
        {
            return;
        }

        Mesh trackMesh = new Mesh();
        List<Vector3> vertices = new List<Vector3>();
        List<int> triangles = new List<int>();
        List<Vector2> uvs = new List<Vector2>(); // don't really need UVs tbh

        for (int i = 0; i < targets.Count - 1; i++)
        {
            Vector3 currentPoint = targets[i];
            Vector3 nextPoint = targets[i + 1];

            // Calculate the direction between points and perpendicular direction for width
            Vector3 forward = (nextPoint - currentPoint).normalized;
            Vector3 right = Vector3.Cross(Vector3.up, forward).normalized * trackWidth * 0.5f;

            // Create vertices for the quad
            Vector3 v1 = currentPoint - right;
            Vector3 v2 = currentPoint + right;
            Vector3 v3 = nextPoint - right;
            Vector3 v4 = nextPoint + right;

            // Add vertices to the list
            vertices.Add(v1); // Bottom left
            vertices.Add(v2); // Bottom right
            vertices.Add(v3); // Top left
            vertices.Add(v4); // Top right

            // Create triangles for the quad
            int baseIndex = i * 4;
            triangles.Add(baseIndex);
            triangles.Add(baseIndex + 2);
            triangles.Add(baseIndex + 1);

            triangles.Add(baseIndex + 1);
            triangles.Add(baseIndex + 2);
            triangles.Add(baseIndex + 3);

            // Add UVs (simple mapping)
            uvs.Add(new Vector2(0, 0));
            uvs.Add(new Vector2(1, 0));
            uvs.Add(new Vector2(0, 1));
            uvs.Add(new Vector2(1, 1));
        }

        // Assign vertices, triangles, and UVs to the mesh
        trackMesh.vertices = vertices.ToArray();
        trackMesh.triangles = triangles.ToArray();
        trackMesh.uv = uvs.ToArray();
        trackMesh.RecalculateNormals(); // Recalculate to ensure lighting works correctly

        Debug.Log(trackMesh.vertices.Length);

        // Attach the mesh to a MeshFilter and MeshRenderer
        MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();
        MeshRenderer meshRenderer = gameObject.AddComponent<MeshRenderer>();

        meshFilter.mesh = trackMesh;

        // Assign a material (make sure you have a material assigned in the inspector or set it via code)
        meshRenderer.material = trackMat;
    }
    
    public void IncrementTrack()
    {
        Vector3 lastPos = transform.position;

        if (targets.Count > 0) lastPos = targets[targets.Count - 1];
        Vector3 nextPos = lastPos - transform.right * 10.0f;

        targets.Add(nextPos);
        Instantiate(GameObject.CreatePrimitive(PrimitiveType.Sphere), nextPos, Quaternion.identity);
    }

    // Start is called before the first frame update
    private void Start() {
        Debug.Log("pee");

        for (var z = 0; z < 3; z++) {
            IncrementTrack();
        }

        GenerateTrack();

        return;
         // sorry all of this code below is unreachable lol
        var b = new bool[100, 100];

        // Loop through all points on the grid
        for (var x = 0; x < 100; x++)
        {
            for (var y = 0; y < 100; y++)
            {
                // Assume the point should have a hole (set to false)
                b[x, y] = false;

                // Check if the point is near any of the target points
                for (int i = 0; i < targets.Count; i++)
                {
                    Vector3 currentPos = terrain.GetPosition() + new Vector3(x, -terrain.GetPosition().y, y);
                    if (Vector3.Magnitude(currentPos - targets[i]) <= 20)
                    {
                        // Set to true if it's within 10 units of any target point
                        Debug.Log("Hole");
                        b[x, y] = true;
                        break; // No need to check further; we know this position shouldn't have a hole
                    }
                }
            }
        }

        // Set the holes in the terrain data
        terrain.terrainData.SetHoles(0, 0, b);
        terrain.terrainData.SyncHeightmap(); // Ensures Unity updates the visual representation
    }

    // Update is called once per frame
    private void Update() { }
}