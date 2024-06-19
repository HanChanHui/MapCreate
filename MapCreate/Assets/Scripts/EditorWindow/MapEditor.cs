using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapEditor : MonoBehaviour
{
    public float worldTime = 0f;
    public float deltaTime = 0f;
    public float fps = 0;

    [Range(0, 100)] public float tileSizeX;
    [Range(0, 100)] public float tileSizeY;





    private void Update() 
    {
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
        worldTime += Time.deltaTime;
        fps = 1.0f / deltaTime;
    }
}
