using UnityEngine;
using UnityEditor;
using System.Linq;

public class TestSceneTool : EditorWindow
{
    private Vector2 _scrollPos;
    private FusionDatabase _fusionDB;

    [MenuItem("Steal Egg Run/Créateur de Scène Test")]
    public static void ShowWindow()
    {
        GetWindow<TestSceneTool>("Test Scene Tool");
    }

    private void OnGUI()
    {
        GUILayout.Label("SETUP RAPIDE", EditorStyles.boldLabel);
        
        _fusionDB = (FusionDatabase)EditorGUILayout.ObjectField("Fusion DB", _fusionDB, typeof(FusionDatabase), false);

        if (GUILayout.Button("1. INITIALISER LA SCÈNE (Caméra + Grid + Managers)", GUILayout.Height(40)))
        {
            SetupScene();
        }

        GUILayout.Space(20);
        GUILayout.Label("SPAWNER D'OBJETS", EditorStyles.boldLabel);

        string[] guids = AssetDatabase.FindAssets("t:ItemData");
        
        if (guids.Length == 0)
        {
            EditorGUILayout.HelpBox("Aucun ItemData trouvé dans le projet !", MessageType.Warning);
            return;
        }

        _scrollPos = GUILayout.BeginScrollView(_scrollPos);

        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            ItemData item = AssetDatabase.LoadAssetAtPath<ItemData>(path);

            if (item == null) continue;

            GUILayout.BeginHorizontal("box");
            
            if (item.Sprite != null)
            {
                Texture2D preview = AssetPreview.GetAssetPreview(item.Sprite);
                if (preview != null) GUILayout.Label(preview, GUILayout.Width(30), GUILayout.Height(30));
            }
            
            GUILayout.Label(item.name, EditorStyles.boldLabel);

            if (GUILayout.Button("Spawn (0,0)"))
            {
                SpawnItem(item, Vector3.zero);
            }
            
            if (GUILayout.Button("Spawn (Souris)"))
            {
                SpawnAtSceneViewCenter(item);
            }

            GUILayout.EndHorizontal();
        }

        GUILayout.EndScrollView();
    }

    private void SetupScene()
    {
        if (Camera.main == null)
        {
            GameObject camObj = new GameObject("Main Camera");
            Camera cam = camObj.AddComponent<Camera>();
            cam.orthographic = true;
            cam.orthographicSize = 5;
            camObj.transform.position = new Vector3(0, 0, -10);
            camObj.tag = "MainCamera";
        }

        if (FindFirstObjectByType<FusionManager>() == null)
        {
            GameObject managerObj = new GameObject("Managers");
            FusionManager fusion = managerObj.AddComponent<FusionManager>();
            
            if (_fusionDB != null)
            {
                SerializedObject so = new SerializedObject(fusion);
                so.FindProperty("_database").objectReferenceValue = _fusionDB;
                so.ApplyModifiedProperties();
            }
            
            Undo.RegisterCreatedObjectUndo(managerObj, "Create Managers");
        }

        if (FindFirstObjectByType<GridManager>() == null)
        {
            GameObject gridObj = new GameObject("Grid System");
            Grid grid = gridObj.AddComponent<Grid>();
            
            grid.cellSize = new Vector3(1, 1, 0);

            GridManager tester = gridObj.AddComponent<GridManager>();
            
            SerializedObject so = new SerializedObject(tester);
            so.FindProperty("grid").objectReferenceValue = grid;
            so.FindProperty("draggableLayer").intValue = -1; // Tout
            so.ApplyModifiedProperties();

            Undo.RegisterCreatedObjectUndo(gridObj, "Create Grid");
        }

        Debug.Log("Scène de test initialisée !");
    }

    private void SpawnItem(ItemData item, Vector3 pos)
    {
        if (item.Prefab == null)
        {
            Debug.LogError($"L'item {item.name} n'a pas de Prefab assigné !");
            return;
        }

        GameObject obj = (GameObject)PrefabUtility.InstantiatePrefab(item.Prefab);
        obj.transform.position = pos;
        
        if (obj.GetComponent<ItemHolder>() == null)
        {
            ItemHolder holder = obj.AddComponent<ItemHolder>();
            holder.Data = item;
        }
        
        Undo.RegisterCreatedObjectUndo(obj, $"Spawn {item.name}");
        Selection.activeGameObject = obj;
    }

    private void SpawnAtSceneViewCenter(ItemData item)
    {
        SceneView view = SceneView.lastActiveSceneView;
        if (view != null)
        {
            Vector3 center = view.camera.transform.position;
            center.z = 0;
            SpawnItem(item, center);
        }
        else
        {
            SpawnItem(item, Vector3.zero);
        }
    }
}