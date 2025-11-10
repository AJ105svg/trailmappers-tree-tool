using System.Collections.Generic;
using UnityEngine;
using System.IO;

[System.Serializable]
public class ObjectData
{
    public Vector3 P;
    public Vector3 R;
    public Vector3 S;
    public string N;
    public InfoData I;
}

[System.Serializable]
public class InfoData
{
    public bool IsStatic;
    public bool CanCollide;
    public bool IsVisible;
    public string DisplayName;
    public string CustomTexture;
    public bool CustomModel;
    public float CustomWeight;
}

[System.Serializable]
public class CameraInfo
{
    public Vector3 P;
    public Vector3 R;
    public Vector3 S;
    public string N;
    public InfoData I;
}

[System.Serializable]
public class SpawnInfo
{
    public Vector3 P;
    public Vector3 R;
    public Vector3 S;
    public string N;
    public InfoData I;
}

[System.Serializable]
public class RootData
{
    public List<ObjectData> ObjectList = new();
    public CameraInfo CameraInfo;
    public SpawnInfo SpawnpointInfo;
    public string Name = "tree test";
    public bool PrettyPrint = false;
    public string Version = "1.3";
}

public class TreeGenerator : MonoBehaviour
{
    public int treeCount = 100000;
    public GameObject treePrefab;
    public float radius = 7500f;
    public LayerMask groundMask;
    public float rayHeight = 3500f;
    public float rayDistance = 5000f;

    public Texture2D heatmapTexture;
    [Range(0f, 1f)]
    public float Threshold = 0.1f;

    void Start()
    {
        GenerateMap();
    }

    void GenerateMap()
    {
        var baseInfo = new InfoData
        {
            IsStatic = true,
            CanCollide = true,
            IsVisible = true,
            DisplayName = "Savannah Tree Short Green",
            CustomTexture = "",
            CustomModel = false,
            CustomWeight = 0.0f
        };

        var data = new RootData();

        for (int i = 0; i < treeCount; i++)
        {
            float x = Random.Range(-radius, radius);
            float z = Random.Range(-radius, radius);

            if (heatmapTexture)
            {
                float u = Mathf.InverseLerp(-radius, radius, -x);
                float v = Mathf.InverseLerp(-radius, radius, -z);
                Color pixel = heatmapTexture.GetPixelBilinear(u, v);

                float density = pixel.grayscale;

                if (Random.value > density || density < Threshold)
                    continue;
            }

            Vector3 start = new Vector3(x, rayHeight, z);
            if (Physics.Raycast(start, Vector3.down, out RaycastHit hit, rayDistance, groundMask))
            {
                float y = hit.point.y;
                float rotY = Random.Range(0f, 180f);
                var obj = new ObjectData
                {
                    P = new Vector3(x, y, z),
                    R = new Vector3(0, rotY, 0),
                    S = new Vector3(1, 3, 1),
                    N = "PFB_INS_Savannah_Tree_Short",
                    I = baseInfo
                };

                data.ObjectList.Add(obj);

                if (treePrefab)
                    Instantiate(treePrefab, obj.P, Quaternion.Euler(obj.R));
            }
        }

        data.CameraInfo = new CameraInfo
        {
            P = new Vector3(0f, 0f, 0f),
            R = new Vector3(0f, 0f, 0f),
            S = Vector3.zero,
            N = "",
            I = new InfoData()
        };

        data.SpawnpointInfo = new SpawnInfo
        {
            P = new Vector3(0f, 2000f, 0f),
            R = Vector3.zero,
            S = Vector3.one,
            N = "",
            I = new InfoData()
        };

        string json = JsonUtility.ToJson(data, true);
        string path = Path.Combine(Application.dataPath, "map.json");
        File.WriteAllText(path, json);
        Debug.Log($"Generated {data.ObjectList.Count} trees. JSON saved to: {path}");
    }
}
