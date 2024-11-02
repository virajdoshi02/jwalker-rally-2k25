using UnityEngine;

public class RCC : MonoBehaviour
{
    public static RCC I { get; private set; }

    private void Awake() {
        if (I == null) {
            I = this;
        }
        else {
            Destroy(gameObject);
        }
    }
}