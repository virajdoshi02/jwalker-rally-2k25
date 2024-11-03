using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamerPlacer : MonoBehaviour
{
    public GameObject gamer;

    Color[] colorPalette =
    {
        Color.blue,
        Color.white,
        Color.red,
        Color.yellow
    };

    private void Start()
    {
        PlaceGamers(true, (int)Random.Range(2.0f, 9.0f));
    }

    void PlaceGamers(bool ProcedurallyPlaceGamers, int NumGamers)
    {
        //Transform[] children = transform.GetComponentsInChildren<Transform>();
        GameObject[] children;

        if (ProcedurallyPlaceGamers)
        {
            Vector3 pos = transform.position;
            Vector2 XYBounds = new Vector2(transform.localScale.x, transform.localScale.z) / 2.0f;

            children = new GameObject[NumGamers];

            for (int i = 0; i < NumGamers; i++)
            {
                float randX = Random.Range(-1.0f, 1.0f) * XYBounds.x;
                float randZ = Random.Range(-1.0f, 1.0f) * XYBounds.y;

                Color randomColor = new Color();

                RaycastHit hit;
                Vector3 rayOrigin = transform.position;
                rayOrigin.x += randX;
                rayOrigin.z += randZ;

                if (Physics.Raycast(rayOrigin, transform.TransformDirection(-Vector3.up), out hit, Mathf.Infinity))
                {
                    Debug.DrawRay(rayOrigin, transform.TransformDirection(-Vector3.up) * hit.distance, Color.yellow);

                    float dy = -hit.distance;
                    rayOrigin.y += dy + (gamer.GetComponent<BoxCollider>().size.y / 2.0f);

                    children[i] = Instantiate(gamer);
                    children[i].transform.position = rayOrigin;
                    children[i].transform.rotation = Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation;

                    int colorPalettePick = Random.Range(0, 5);
                    randomColor = colorPalette[colorPalettePick];

                    children[i].transform.Find("man").GetComponent<MeshRenderer>().material.SetColor("_Color", randomColor);
                }
            }
        }
    }
}