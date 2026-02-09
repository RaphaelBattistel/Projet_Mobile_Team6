using UnityEngine;
using UnityEditor;
using System.IO;

public class GameToolsWindow : EditorWindow
{
    private int _selectedTab = 0;
    private string[] _tabs = { "1. Créateur d'Items", "2. Gestion des Fusions" };

    private string _newItemName = "NouvelItem";
    private Sprite _newItemSprite;
    private Weight _newItemWeight = Weight.Light; 
    private bool _addPhysics = true; 

    private FusionDatabase _fusionDB;
    private ItemData _inputA, _inputB, _output;
    private Vector2 _scrollPos;

    [MenuItem("Steal Egg Run/Super Game Tools")]
    public static void ShowWindow()
    {
        GetWindow<GameToolsWindow>("Super Game Tools");
    }

    private void OnGUI()
    {
        GUILayout.Space(10);
        _selectedTab = GUILayout.Toolbar(_selectedTab, _tabs, GUILayout.Height(30));
        GUILayout.Space(15);

        switch (_selectedTab)
        {
            case 0:
                DrawItemCreator();
                break;
            case 1:
                DrawFusionManager();
                break;
        }
    }

    private void DrawItemCreator()
    {
        GUILayout.Label("CRÉATEUR D'OBJETS AUTOMATIQUE", EditorStyles.boldLabel);
        EditorGUILayout.HelpBox("Cet outil va créer le ScriptableObject ET le Prefab automatiquement.", MessageType.Info);
        GUILayout.Space(10);

        _newItemName = EditorGUILayout.TextField("Nom de l'objet", _newItemName);
        _newItemSprite = (Sprite)EditorGUILayout.ObjectField("Sprite (Image)", _newItemSprite, typeof(Sprite), false);
        _newItemWeight = (Weight)EditorGUILayout.EnumPopup("Poids", _newItemWeight);
        _addPhysics = EditorGUILayout.Toggle("Ajouter Physique (RB2D)", _addPhysics);

        GUILayout.Space(20);

        GUI.backgroundColor = Color.green;
        if (GUILayout.Button("CRÉER L'ITEM COMPLET", GUILayout.Height(40)))
        {
            CreateCompleteItem();
        }
        GUI.backgroundColor = Color.white;
    }

    private void CreateCompleteItem()
    {
        if (string.IsNullOrEmpty(_newItemName)) { Debug.LogError("Il faut un nom !"); return; }
        if (_newItemSprite == null) { Debug.LogError("Il faut un sprite !"); return; }

        string dataPath = "Assets/Data/Items";
        string prefabPath = "Assets/Prefabs/Items";
        CreateFolder(dataPath);
        CreateFolder(prefabPath);

        ItemData newData = CreateInstance<ItemData>();
        newData.Label = _newItemName;
        newData.Weight = _newItemWeight;
        newData.Sprite = _newItemSprite;

        string assetPath = $"{dataPath}/{_newItemName}_Data.asset";
        AssetDatabase.CreateAsset(newData, assetPath);

        GameObject tempObj = new GameObject(_newItemName);
        
        SpriteRenderer sr = tempObj.AddComponent<SpriteRenderer>();
        sr.sprite = _newItemSprite;

        tempObj.AddComponent<BoxCollider2D>(); 

        if (_addPhysics)
        {
            tempObj.AddComponent<Rigidbody2D>();
        }

        ItemHolder holder = tempObj.AddComponent<ItemHolder>();
        holder.Data = newData; 


        string prefabAssetPath = $"{prefabPath}/{_newItemName}.prefab";
        GameObject prefab = PrefabUtility.SaveAsPrefabAsset(tempObj, prefabAssetPath);
        
        newData.Prefab = prefab;
        EditorUtility.SetDirty(newData); 

        DestroyImmediate(tempObj); 
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log($"SUCCESS ! Item '{_newItemName}' créé avec succès dans {prefabPath}");
        
        _newItemName = "NouvelItem";
        _newItemSprite = null;
    }
    private void DrawFusionManager()
    {
        GUILayout.Label("TABLE DES FUSIONS", EditorStyles.boldLabel);

        _fusionDB = (FusionDatabase)EditorGUILayout.ObjectField("Base de Données", _fusionDB, typeof(FusionDatabase), false);

        if (_fusionDB == null)
        {
            EditorGUILayout.HelpBox("Assigne la FusionDatabase pour commencer.", MessageType.Warning);
            return;
        }

        GUILayout.Space(10);
        GUILayout.BeginVertical("box");
        GUILayout.Label("Nouvelle Recette", EditorStyles.miniBoldLabel);
        GUILayout.BeginHorizontal();
        
        _inputA = (ItemData)EditorGUILayout.ObjectField(_inputA, typeof(ItemData), false, GUILayout.Width(100));
        GUILayout.Label("+", GUILayout.Width(15));
        _inputB = (ItemData)EditorGUILayout.ObjectField(_inputB, typeof(ItemData), false, GUILayout.Width(100));
        GUILayout.Label("=", GUILayout.Width(15));
        _output = (ItemData)EditorGUILayout.ObjectField(_output, typeof(ItemData), false, GUILayout.Width(100));
        
        if (GUILayout.Button("Ajouter", GUILayout.Width(60)))
        {
            AddRecipe();
        }
        GUILayout.EndHorizontal();
        GUILayout.EndVertical();

        GUILayout.Space(15);
        GUILayout.Label($"Recettes existantes ({_fusionDB.Recipes.Count})", EditorStyles.boldLabel);

        _scrollPos = GUILayout.BeginScrollView(_scrollPos);
        for (int i = 0; i < _fusionDB.Recipes.Count; i++)
        {
            var r = _fusionDB.Recipes[i];
            GUILayout.BeginHorizontal("box");
            GUILayout.Label($"{GetItemName(r.ElementA)} + {GetItemName(r.ElementB)} -> {GetItemName(r.Resultat)}");
            
            GUI.backgroundColor = Color.red;
            if (GUILayout.Button("X", GUILayout.Width(25)))
            {
                _fusionDB.Recipes.RemoveAt(i);
                EditorUtility.SetDirty(_fusionDB);
            }
            GUI.backgroundColor = Color.white;
            GUILayout.EndHorizontal();
        }
        GUILayout.EndScrollView();
    }

    private void AddRecipe()
    {
        if (_inputA && _inputB && _output)
        {
            _fusionDB.Recipes.Add(new FusionDatabase.FusionRecipe { ElementA = _inputA, ElementB = _inputB, Resultat = _output });
            EditorUtility.SetDirty(_fusionDB);
            _inputA = null; _inputB = null; _output = null;
        }
    }


    
    private string GetItemName(ItemData d) => d != null ? d.Label : "???";

    private void CreateFolder(string path)
    {
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
    }
}