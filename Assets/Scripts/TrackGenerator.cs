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

    private void GenerateTrack() {
        var trackWidth = 20.0f;

        if (targets == null || targets.Count < 2) {
            return;
        }

        trackMesh = new Mesh();
        var vertices = new List<Vector3>();
        var triangles = new List<int>();
        var uvs = new List<Vector2>();

        var origin = transform.position;
        var currentUVY = 0.0f;

        for (var i = 0; i < targets.Count - 1; i++) {
            var currentPoint = targets[i] - origin;
            var nextPoint = targets[i + 1] - origin;

            // Calculate the direction between points and perpendicular direction for width
            var forward = (nextPoint - currentPoint).normalized;
            var right = Vector3.Cross(Vector3.up, forward).normalized * (trackWidth * 0.5f);

            // Create vertices for the quad centered on the points
            var v1 = currentPoint - right;
            var v2 = currentPoint + right;
            var v3 = nextPoint - right;
            var v4 = nextPoint + right;

            // Add vertices to the list, avoiding duplicates at segment joints
            if (i == 0) {
                // First segment, add all four vertices
                vertices.Add(v1);
                vertices.Add(v2);
            }

            vertices.Add(v3);
            vertices.Add(v4);

            // Create triangles for the segment
            var baseIndex = i * 2;
            triangles.Add(baseIndex);
            triangles.Add(baseIndex + 2);
            triangles.Add(baseIndex + 1);

            triangles.Add(baseIndex + 1);
            triangles.Add(baseIndex + 2);
            triangles.Add(baseIndex + 3);

            var segmentLength = Vector3.Distance(currentPoint, nextPoint);
            var uvIncrement = segmentLength / trackWidth; // Adjust UV scaling as needed

            if (i == 0) {
                // First segment, initial UV mapping
                uvs.Add(new Vector2(0, currentUVY));
                uvs.Add(new Vector2(1, currentUVY));
            }

            currentUVY += uvIncrement;
            uvs.Add(new Vector2(0, currentUVY));
            uvs.Add(new Vector2(1, currentUVY));
        }

        // Assign vertices, triangles, and UVs to the mesh
        trackMesh.vertices = vertices.ToArray();
        trackMesh.triangles = triangles.ToArray();
        trackMesh.uv = uvs.ToArray();
        trackMesh.RecalculateNormals(); // Recalculate to ensure lighting works correctly
        trackMesh.RecalculateBounds();

        // Attach the mesh to a MeshFilter and MeshRenderer
        var meshFilter = GetComponent<MeshFilter>();
        if (meshFilter == null) {
            meshFilter = gameObject.AddComponent<MeshFilter>();
        }

        meshFilter.sharedMesh = trackMesh;

        var meshRenderer = GetComponent<MeshRenderer>();
        if (meshRenderer == null) {
            meshRenderer = gameObject.AddComponent<MeshRenderer>();
        }

        meshRenderer.material = trackMat;

        // Add a MeshCollider if needed
        var meshCollider = GetComponent<MeshCollider>();
        if (meshCollider == null) {
            meshCollider = gameObject.AddComponent<MeshCollider>();
        }

        meshCollider.sharedMesh = trackMesh;
    }


    private void SpawnPlaneBelowTrack() {
        var plane = GameObject.CreatePrimitive(PrimitiveType.Plane);
        plane.transform.position = targets[targets.Count - 1] + Vector3.down * 0.02f;
        plane.transform.localScale = new Vector3(5, 1, 5); // Adjust size as needed
        plane.GetComponent<Renderer>().material = floorMat;
        plane.GetComponent<Renderer>().material.color = Color.gray; // Change color if needed
        plane.tag = "Ground";

        lastPlanes.Enqueue(plane);
    }

    public void IncrementTrack() {
        //Debug.Log("up the track");
        var lastPos = transform.position;

        if (targets.Count > 0) lastPos = targets[targets.Count - 1];

        var newDir = -transform.right * 30.0f;
        newDir.y += Random.Range(-1.0f, 4.0f);
        var rotation = Quaternion.AngleAxis(Random.Range(-45.0f, 45.0f), transform.up);
        newDir = rotation * newDir;

        var nextPos = lastPos + newDir;

        targets.Add(nextPos);

        GenerateTrack();
        //SpawnPlaneBelowTrack();
    }

    public void PopPlane() {
        //GameObject lastPlane = lastPlanes.Dequeue();
        //Debug.Log(lastPlane.transform.position);
        //Destroy(lastPlane);
    }

    // Start is called before the first frame update
    private void Start() {
        // Debug.Log("pee");
        lastPlanes = new Queue<GameObject>();

        for (var z = 0; z < 10; z++) {
            IncrementTrack();
        }

        return;
    }

    // Update is called once per frame
    private void Update() { }
}