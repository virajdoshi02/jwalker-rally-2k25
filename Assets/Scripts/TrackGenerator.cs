using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackGenerator : MonoBehaviour
{
    [SerializeField]
    public List<Vector3> targets;
    public Terrain terrain;
    public Material trackMat;
    public Material floorMat;
    private Mesh trackMesh;
    private Queue<GameObject> lastPlanes;

    private void GenerateTrack()
    {
        float trackWidth = 10.0f;

        if (targets == null || targets.Count < 2)
        {
            return;
        }

        trackMesh = new Mesh();
        List<Vector3> vertices = new List<Vector3>();
        List<int> triangles = new List<int>();
        List<Vector2> uvs = new List<Vector2>(); // don't really need UVs tbh

        Vector3 origin = transform.position;

        for (int i = 0; i < targets.Count - 1; i++)
        {
            Vector3 currentPoint = targets[i] - origin;
            Vector3 nextPoint = targets[i + 1] - origin;

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

            // Create triangles for the segment
            int baseIndex = i * 4;
            triangles.Add(baseIndex);
            triangles.Add(baseIndex + 2);
            triangles.Add(baseIndex + 1);

            triangles.Add(baseIndex + 1);
            triangles.Add(baseIndex + 2);
            triangles.Add(baseIndex + 3);

            // Add UVs for basic texture mapping
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
        trackMesh.RecalculateBounds();

        for (int i = 0; i < targets.Count - 1; i++)
        {
            Debug.DrawLine(targets[i], targets[i + 1], Color.red, 90f); // Draws red lines for 10 seconds
        }

        // Attach the mesh to a MeshFilter and MeshRenderer

        MeshFilter meshFilter = GetComponent<MeshFilter>();
        if (meshFilter == null)
        {
            meshFilter = gameObject.AddComponent<MeshFilter>();
        }
        meshFilter.sharedMesh = trackMesh;

        // Assign a material (make sure you have a material assigned in the inspector or set it via code)
        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
        if (meshRenderer == null)
        {
            meshRenderer = gameObject.AddComponent<MeshRenderer>();
        }
        meshRenderer.material = trackMat;

        MeshCollider meshCollider = GetComponent<MeshCollider>();
        if (meshCollider == null)
        {
            meshCollider = gameObject.AddComponent<MeshCollider>();
        }
        meshCollider.sharedMesh = trackMesh;
    }

    void SpawnPlaneBelowTrack()
    {
        GameObject plane = GameObject.CreatePrimitive(PrimitiveType.Plane);
        plane.transform.position = targets[targets.Count - 1] + Vector3.down * 0.02f;
        plane.transform.localScale = new Vector3(5, 1, 5); // Adjust size as needed
        plane.GetComponent<Renderer>().material = floorMat;
        plane.GetComponent<Renderer>().material.color = Color.gray; // Change color if needed
        plane.tag = "Ground";

        lastPlanes.Enqueue(plane);
    }

    public void IncrementTrack()
    {
        //Debug.Log("up the track");
        Vector3 lastPos = transform.position;

        if (targets.Count > 0) lastPos = targets[targets.Count - 1];

        Vector3 newDir = -transform.right * 30.0f;
        Quaternion rotation = Quaternion.AngleAxis(Random.Range(-45.0f, 45.0f), transform.up);
        newDir = rotation * newDir;

        Vector3 nextPos = lastPos + newDir;

        targets.Add(nextPos);

        GenerateTrack();
        SpawnPlaneBelowTrack();
    }

    public void PopPlane()
    {
        GameObject lastPlane = lastPlanes.Dequeue();
        Debug.Log(lastPlane.transform.position);
        Destroy(lastPlane);
    }

    // Start is called before the first frame update
    private void Start() {
        Debug.Log("pee");
        lastPlanes = new Queue<GameObject>();

        for (var z = 0; z < 10; z++) {
            IncrementTrack();
        }

        return;
    }

    // Update is called once per frame
    private void Update() { }
}