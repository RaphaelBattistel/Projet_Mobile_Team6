using UnityEngine;
using UnityEditor;
using System.Linq;

// ATTENTION LES GARS : Ce script DOIT rester dans un dossier nommé "Editor" !
public class TestSceneTool : EditorWindow
{
    private Vector2 _scrollPos;
    private FusionDatabase _fusionDB; // Pratique pour auto-configurer le manager

    // Ça rajoute un bouton tout en haut d'Unity pour lancer l'outil
    [MenuItem("Steal Egg Run/Créateur de Scène Test")]
    public static void ShowWindow()
    {
        GetWindow<TestSceneTool>("Test Scene Tool");
    }

    private void OnGUI()
    {
        GUILayout.Label("SETUP RAPIDE", EditorStyles.boldLabel);
        
        // La case pour glisser la database de fusions
        _fusionDB = (FusionDatabase)EditorGUILayout.ObjectField("Fusion DB", _fusionDB, typeof(FusionDatabase), false);

        // LE gros bouton qui fait tout le boulot d'installation
        if (GUILayout.Button("1. INITIALISER LA SCÈNE (Caméra + Grid + Managers)", GUILayout.Height(40)))
        {
            SetupScene();
        }

        GUILayout.Space(20);
        GUILayout.Label("SPAWNER D'OBJETS", EditorStyles.boldLabel);

        // Magie : on cherche automatiquement tous les objets "ItemData" dans le projet
        string[] guids = AssetDatabase.FindAssets("t:ItemData");
        
        if (guids.Length == 0)
        {
            EditorGUILayout.HelpBox("Aucun ItemData trouvé dans le projet !", MessageType.Warning);
            return;
        }

        // Si y'en a beaucoup, on met une barre de défilement
        _scrollPos = GUILayout.BeginScrollView(_scrollPos);

        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            ItemData item = AssetDatabase.LoadAssetAtPath<ItemData>(path);

            if (item == null) continue;

            GUILayout.BeginHorizontal("box");
            
            // On affiche la petite icône de l'objet si elle existe
            if (item.Sprite != null)
            {
                Texture2D preview = AssetPreview.GetAssetPreview(item.Sprite);
                if (preview != null) GUILayout.Label(preview, GUILayout.Width(30), GUILayout.Height(30));
            }
            
            GUILayout.Label(item.name, EditorStyles.boldLabel);

            // Le bouton pour faire pop l'objet au centre (0,0)
            if (GUILayout.Button("Spawn (0,0)"))
            {
                SpawnItem(item, Vector3.zero);
            }
            
            // Le bouton pour le faire pop là où on regarde dans la scène (super pratique)
            if (GUILayout.Button("Spawn (Souris)"))
            {
                SpawnAtSceneViewCenter(item);
            }

            GUILayout.EndHorizontal();
        }

        GUILayout.EndScrollView();
    }

    // La fonction qui installe toute la scène pour tester direct
    private void SetupScene()
    {
        // 1. On crée une caméra propre si y'en a pas
        if (Camera.main == null)
        {
            GameObject camObj = new GameObject("Main Camera");
            Camera cam = camObj.AddComponent<Camera>();
            cam.orthographic = true;
            cam.orthographicSize = 5;
            camObj.transform.position = new Vector3(0, 0, -10);
            camObj.tag = "MainCamera";
        }

        // 2. On installe le FusionManager
        if (FindFirstObjectByType<FusionManager>() == null)
        {
            GameObject managerObj = new GameObject("Managers");
            FusionManager fusion = managerObj.AddComponent<FusionManager>();
            
            // Si le dev a glissé la DB dans l'outil, on lui assigne automatiquement (ça évite un drag & drop chiant)
            if (_fusionDB != null)
            {
                SerializedObject so = new SerializedObject(fusion);
                so.FindProperty("_database").objectReferenceValue = _fusionDB;
                so.ApplyModifiedProperties();
            }
            
            Undo.RegisterCreatedObjectUndo(managerObj, "Create Managers");
        }

        // 3. On installe la grille de jeu
        if (FindFirstObjectByType<GridManager>() == null)
        {
            GameObject gridObj = new GameObject("Grid System");
            Grid grid = gridObj.AddComponent<Grid>();
            
            grid.cellSize = new Vector3(1, 1, 0);

            GridManager tester = gridObj.AddComponent<GridManager>();
            
            // On paramètre la grille automatiquement (Layer -1 veut dire qu'on check tout)
            SerializedObject so = new SerializedObject(tester);
            so.FindProperty("grid").objectReferenceValue = grid;
            so.FindProperty("draggableLayer").intValue = -1; 
            so.ApplyModifiedProperties();

            Undo.RegisterCreatedObjectUndo(gridObj, "Create Grid");
        }

        Debug.Log("Scène de test initialisée ! Let's go !");
    }

    // Fait apparaître un objet à la position voulue
    private void SpawnItem(ItemData item, Vector3 pos)
    {
        if (item.Prefab == null)
        {
            Debug.LogError($"L'item {item.name} n'a pas de Prefab assigné !");
            return;
        }

        // On instancie le prefab proprement dans l'éditeur
        GameObject obj = (GameObject)PrefabUtility.InstantiatePrefab(item.Prefab);
        obj.transform.position = pos;
        
        // On s'assure qu'il a bien son étiquette (ItemHolder)
        if (obj.GetComponent<ItemHolder>() == null)
        {
            ItemHolder holder = obj.AddComponent<ItemHolder>();
            holder.Data = item;
        }
        
        // Ça, c'est pour que le dev puisse faire "Ctrl+Z" s'il a cliqué sans faire exprès
        Undo.RegisterCreatedObjectUndo(obj, $"Spawn {item.name}");
        Selection.activeGameObject = obj; // Et on sélectionne l'objet fraîchement créé
    }

    // Pour spawner l'objet pile au milieu de ce que regarde le dev
    private void SpawnAtSceneViewCenter(ItemData item)
    {
        SceneView view = SceneView.lastActiveSceneView;
        if (view != null)
        {
            Vector3 center = view.camera.transform.position;
            center.z = 0; // On remet en 2D pour pas que l'objet parte dans l'espace
            SpawnItem(item, center);
        }
        else
        {
            SpawnItem(item, Vector3.zero);
        }
    }
}