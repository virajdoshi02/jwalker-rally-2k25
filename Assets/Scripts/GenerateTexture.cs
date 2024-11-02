using UnityEngine;

public class GenerateTexture : MonoBehaviour
{
    [SerializeField]
    private ComputeShader computeShader;

    [SerializeField]
    private RenderTexture heightMap;

    [SerializeField]
    private int textureSize;

    private Vector2 offset;
    private int handle;

    private void Awake() {
        offset = new Vector2(0, 0);
        handle = computeShader.FindKernel("CSMain");
    }

    // Start is called before the first frame update
    private void Start() {
        computeShader.SetVector("Offset", offset);
        computeShader.SetFloat("Scale", 10);
        computeShader.SetTexture(handle, "Result", heightMap);

        computeShader.Dispatch(handle, textureSize / 8, textureSize / 8, 1);

        gameObject.GetComponent<MeshRenderer>().material.mainTexture = heightMap;
    }

    private void Update() {
        offset.x += Time.deltaTime * 100f;
        computeShader.SetVector("Offset", offset);
        computeShader.Dispatch(handle, textureSize / 8, textureSize / 8, 1);
    }
}