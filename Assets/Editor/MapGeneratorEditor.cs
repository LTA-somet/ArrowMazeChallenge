
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

public class MapGeneratorEditor : EditorWindow
{
    public int width;
    public int height;
    public int numberObstacle;
    // private Transform tileParent;
    private GameObject tilePrefab;
    private GameObject tileObstacle;
    private GameObject startPrefab;
    private GameObject endPrefab;
    public bool straightRoad = false;

    [MenuItem("Window/Map Generator")]
    public static void ShowWindow()
    {
        GetWindow<MapGeneratorEditor>("Map Generator");
    }

    private void OnGUI()
    {
        GUILayout.Label("Map Settings", EditorStyles.boldLabel);

        width = EditorGUILayout.IntField("Width", width);
        height = EditorGUILayout.IntField("Height", height);
        numberObstacle = EditorGUILayout.IntField("Number Obstacle", numberObstacle);
        straightRoad = EditorGUILayout.Toggle("straightRoad", straightRoad);
        //tileParent = (Transform)EditorGUILayout.ObjectField("Tile Parent", tileParent, typeof(Transform), false);
        tilePrefab = (GameObject)EditorGUILayout.ObjectField("Tile Prefab", tilePrefab, typeof(GameObject), false);
        tileObstacle = (GameObject)EditorGUILayout.ObjectField("Tile Obstacle", tileObstacle, typeof(GameObject), false);
        startPrefab = (GameObject)EditorGUILayout.ObjectField("Start Prefab", startPrefab, typeof(GameObject), false);
        endPrefab = (GameObject)EditorGUILayout.ObjectField("End Prefab", endPrefab, typeof(GameObject), false);

        tilePrefab = Resources.Load<GameObject>("Prefabs/step");
        tileObstacle = Resources.Load<GameObject>("Prefabs/obstacle");
        startPrefab = Resources.Load<GameObject>("Prefabs/move");
        endPrefab = Resources.Load<GameObject>("Prefabs/destination");

        if (GUILayout.Button("Generate Map"))
        {
            GenerateMap();
        }
    }

    private void GenerateMap()
    {
        List<Vector2> pathMap = new List<Vector2>();

        if (tilePrefab == null || startPrefab == null || endPrefab == null)
        {
            Debug.LogError("Please assign all prefabs.");
            return;
        }

        // Xóa các đối tượng cũ trong scene
        var oldTiles = GameObject.FindGameObjectsWithTag("GeneratedTile");
        foreach (var tile in oldTiles)
        {
            DestroyImmediate(tile.gameObject);
        }
        var startPoint = (GameObject)PrefabUtility.InstantiatePrefab(startPrefab);
        var endPoint = (GameObject)PrefabUtility.InstantiatePrefab(endPrefab);

        Undo.RegisterCreatedObjectUndo(startPoint, "Create Start Point");
        Undo.RegisterCreatedObjectUndo(endPoint, "Create End Point");

        pathMap.Add((Vector2)startPoint.transform.position);

        if (straightRoad)
        {
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height - 1; y++)
                {
                    Vector2 position = new Vector2(x, y);
                    pathMap.Add(position);
                    var tile = (GameObject)PrefabUtility.InstantiatePrefab(tilePrefab);
                    if (position.Equals(startPoint))
                    {
                        StepScript step = tile.GetComponent<StepScript>();
                        Destroy(step);
                    }
                    tile.transform.position = position;
                    tile.tag = "GeneratedTile";
                    Undo.RegisterCreatedObjectUndo(tile, "Create Tile");
                }
            }
            startPoint.transform.position = Vector2.zero;
            startPoint.tag = "GeneratedTile";
            endPoint.transform.position = new Vector2(width - 1, height - 1);
            endPoint.tag = "GeneratedTile";
        }
        else
        {
            endPoint.transform.position = new Vector2(Random.Range(-3, 3), Random.Range(3, 5));
            endPoint.tag = "GeneratedTile";
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y <= endPoint.transform.position.y; y++)
                {

                    Vector2 position = new Vector2(x, y);
                    var tile = (GameObject)PrefabUtility.InstantiatePrefab(tilePrefab);
                    if (position.Equals(startPoint))
                    {
                        StepScript step = tile.GetComponent<StepScript>();
                        Destroy(step);
                    }
                    pathMap.Add(position);
                    //tile.transform.SetParent(tileParent.transform);
                    tile.transform.position = position;
                    tile.tag = "GeneratedTile";
                    Undo.RegisterCreatedObjectUndo(tile, "Create Tile");
                }
            }

            if (endPoint.transform.position.x > startPoint.transform.position.x)
            {
                for (int i = (int)startPoint.transform.position.x + 1; i < endPoint.transform.position.x; i++)
                {
                    Vector2 position = new Vector2(i, endPoint.transform.position.y);
                    if (position.Equals(endPoint.transform.position))
                        continue;
                    var tile = (GameObject)PrefabUtility.InstantiatePrefab(tilePrefab);
                    pathMap.Add(position);
                    //tile.transform.SetParent(tileParent.transform);
                    tile.transform.position = position;
                    tile.tag = "GeneratedTile";
                    Undo.RegisterCreatedObjectUndo(tile, "Create Tile");
                }
            }
            else if (endPoint.transform.position.x < startPoint.transform.position.x)
            {
                for (int i = (int)endPoint.transform.position.x + 1; i < startPoint.transform.position.x; i++)
                {
                    Vector2 position = new Vector2(i, endPoint.transform.position.y);
                    if (position.Equals(endPoint))
                    {
                        Debug.Log("continus");
                        continue;
                    }
                    else
                    {
                        pathMap.Add(position);
                        var tile = (GameObject)PrefabUtility.InstantiatePrefab(tilePrefab);
                        //tile.transform.SetParent(tileParent.transform);
                        tile.transform.position = position;
                        tile.tag = "GeneratedTile";
                        Undo.RegisterCreatedObjectUndo(tile, "Create Tile");
                    }
                    
                    
                }
            }
        }
        
        pathMap.Add((Vector2)endPoint.transform.position);

        GenerateObstacle(pathMap);

        // Đánh dấu scene là đã thay đổi để lưu lại các đối tượng mới
        EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
    }

    void GenerateObstacle(List<Vector2> path)
    {
        if (numberObstacle > 1)
        {
            // Thêm logic để tạo nhiều chướng ngại vật nếu cần
        }
        else if (numberObstacle == 1)
        {
            int index = Random.Range((-1 + path.Count) / 2 + 1, (-1 + path.Count) / 2 - 1);
            var tile = (GameObject)PrefabUtility.InstantiatePrefab(tileObstacle);
            tile.gameObject.GetComponent<SpriteRenderer>().sortingOrder = 1;
            Vector2 position = new Vector2(0, index);
            tile.transform.position = position;
            tile.tag = "GeneratedTile";
            Undo.RegisterCreatedObjectUndo(tile, "Create Obstacle");

            InitialAroundObstacle(position);
        }
    }

    List<Vector2> InitialAroundObstacle(Vector2 point)
    {
        List<Vector2> mapAroundObstacle = new List<Vector2>();
        for (int i = -1; i < 2; i++)
        {
            for (int j = -1; j < 2; j++)
            {
                if (i == 0 || j == 0)
                {
                    Vector2 position = new Vector2(point.x + i, point.y + j);
                    Collider2D collider = Physics2D.OverlapPoint(position);
                    if (position.Equals(point)) continue;
                    if (collider == null)
                    {
                        Debug.Log("Collider null " + position);
                        var tile = (GameObject)PrefabUtility.InstantiatePrefab(tilePrefab);
                        tile.transform.position = position;
                        tile.tag = "GeneratedTile";
                        mapAroundObstacle.Add(position);
                        Undo.RegisterCreatedObjectUndo(tile, "Create Tile Around Obstacle");
                    }
                    else
                    {
                        Debug.Log("Collider not null " + position);
                    }
                }
            }
        }
        return mapAroundObstacle;
    }
}