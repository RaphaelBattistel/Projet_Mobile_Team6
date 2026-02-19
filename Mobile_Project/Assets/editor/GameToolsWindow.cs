using UnityEngine;
using UnityEditor;
using System.IO;

// ATTENTION LES GARS : Comme pour l'autre, ce script DOIT rester dans le dossier "Editor" !
public class GameToolsWindow : EditorWindow
{
    private int _selectedTab = 0;
    private string[] _tabs = { "1. Créateur d'Items", "2. Gestion des Fusions" };

    // Les trucs pour fabriquer un item (Onglet 1)
    private string _newItemName = "NouvelItem";
    private Sprite _newItemSprite;
    private Weight _newItemWeight = Weight.Light; 
    private bool _addPhysics = true; 

    // Les trucs pour les fusions (Onglet 2)
    private FusionDatabase _fusionDB;
    private ItemData _inputA, _inputB, _output;
    private Vector2 _scrollPos;

    // Le raccourci magique tout en haut de Unity
    [MenuItem("Steal Egg Run/Super Game Tools")]
    public static void ShowWindow()
    {
        GetWindow<GameToolsWindow>("Super Game Tools");
    }

    private void OnGUI()
    {
        GUILayout.Space(10);
        // La barre pour switcher entre les deux outils
        _selectedTab = GUILayout.Toolbar(_selectedTab, _tabs, GUILayout.Height(30));
        GUILayout.Space(15);

        // On affiche ce qu'il faut en fonction de l'onglet
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

    // ==========================================
    // ONGLET 1 : L'USINE À OBJETS
    // ==========================================
    private void DrawItemCreator()
    {
        GUILayout.Label("CRÉATEUR D'OBJETS AUTOMATIQUE", EditorStyles.boldLabel);
        EditorGUILayout.HelpBox("Cet outil va créer le ScriptableObject ET le Prefab automatiquement. Pratique, hein ?", MessageType.Info);
        GUILayout.Space(10);

        // Les champs à remplir par le Game Designer
        _newItemName = EditorGUILayout.TextField("Nom de l'objet", _newItemName);
        _newItemSprite = (Sprite)EditorGUILayout.ObjectField("Sprite (Image)", _newItemSprite, typeof(Sprite), false);
        _newItemWeight = (Weight)EditorGUILayout.EnumPopup("Poids", _newItemWeight);
        _addPhysics = EditorGUILayout.Toggle("Ajouter Physique (RB2D)", _addPhysics);

        GUILayout.Space(20);

        // LE bouton vert de la validation
        GUI.backgroundColor = Color.green;
        if (GUILayout.Button("CRÉER L'ITEM COMPLET", GUILayout.Height(40)))
        {
            CreateCompleteItem();
        }
        GUI.backgroundColor = Color.white;
    }

    // C'est ici que la magie de la création opère
    private void CreateCompleteItem()
    {
        // Petites sécus pour éviter de faire n'importe quoi
        if (string.IsNullOrEmpty(_newItemName)) { Debug.LogError("Il faut un nom !"); return; }
        if (_newItemSprite == null) { Debug.LogError("Il faut un sprite !"); return; }

        // On définit où on va ranger les trucs et on crée les dossiers si besoin
        string dataPath = "Assets/Data/Items";
        string prefabPath = "Assets/Prefabs/Items";
        CreateFolder(dataPath);
        CreateFolder(prefabPath);

        // 1. On fabrique les données (ItemData)
        ItemData newData = CreateInstance<ItemData>();
        newData.Label = _newItemName;
        newData.Weight = _newItemWeight;
        newData.Sprite = _newItemSprite;

        string assetPath = $"{dataPath}/{_newItemName}_Data.asset";
        AssetDatabase.CreateAsset(newData, assetPath); // Et on le sauvegarde dans les dossiers

        // 2. On fabrique l'objet de base (le Prefab temp)
        GameObject tempObj = new GameObject(_newItemName);
        
        SpriteRenderer sr = tempObj.AddComponent<SpriteRenderer>();
        sr.sprite = _newItemSprite;

        tempObj.AddComponent<BoxCollider2D>(); // Un collider de base pour les collisions

        if (_addPhysics)
        {
            tempObj.AddComponent<Rigidbody2D>(); // La gravité si on a coché la case
        }

        // On colle l'étiquette et on la lie aux données fraîchement créées
        ItemHolder holder = tempObj.AddComponent<ItemHolder>();
        holder.Data = newData; 

        // 3. On sauvegarde le tout en beau Prefab
        string prefabAssetPath = $"{prefabPath}/{_newItemName}.prefab";
        GameObject prefab = PrefabUtility.SaveAsPrefabAsset(tempObj, prefabAssetPath);
        
        // Et on relie le Prefab aux données pour que la boucle soit bouclée !
        newData.Prefab = prefab;
        EditorUtility.SetDirty(newData); 

        // On fait le ménage dans la scène et on rafraîchit Unity
        DestroyImmediate(tempObj); 
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log($"SUCCESS ! Item '{_newItemName}' créé avec succès dans {prefabPath}");
        
        // On remet les cases à zéro pour le prochain
        _newItemName = "NouvelItem";
        _newItemSprite = null;
    }

    // ==========================================
    // ONGLET 2 : LE TABLEAU DES FUSIONS
    // ==========================================
    private void DrawFusionManager()
    {
        GUILayout.Label("TABLE DES FUSIONS", EditorStyles.boldLabel);

        // La case pour glisser le livre de recettes
        _fusionDB = (FusionDatabase)EditorGUILayout.ObjectField("Base de Données", _fusionDB, typeof(FusionDatabase), false);

        if (_fusionDB == null)
        {
            EditorGUILayout.HelpBox("Assigne la FusionDatabase pour commencer.", MessageType.Warning);
            return;
        }

        GUILayout.Space(10);
        GUILayout.BeginVertical("box");
        GUILayout.Label("Nouvelle Recette", EditorStyles.miniBoldLabel);
        
        // L'UI en ligne pour A + B = C
        GUILayout.BeginHorizontal();
        _inputA = (ItemData)EditorGUILayout.ObjectField(_inputA, typeof(ItemData), false, GUILayout.Width(100));
        GUILayout.Label("+", GUILayout.Width(15));
        _inputB = (ItemData)EditorGUILayout.ObjectField(_inputB, typeof(ItemData), false, GUILayout.Width(100));
        GUILayout.Label("=", GUILayout.Width(15));
        _output = (ItemData)EditorGUILayout.ObjectField(_output, typeof(ItemData), false, GUILayout.Width(100));
        
        // Le bouton pour ajouter la recette direct
        if (GUILayout.Button("Ajouter", GUILayout.Width(60)))
        {
            AddRecipe();
        }
        GUILayout.EndHorizontal();
        GUILayout.EndVertical();

        GUILayout.Space(15);
        GUILayout.Label($"Recettes existantes ({_fusionDB.Recipes.Count})", EditorStyles.boldLabel);

        // On liste toutes les recettes avec une scrollbar si ça devient trop long
        _scrollPos = GUILayout.BeginScrollView(_scrollPos);
        for (int i = 0; i < _fusionDB.Recipes.Count; i++)
        {
            var r = _fusionDB.Recipes[i];
            GUILayout.BeginHorizontal("box");
            GUILayout.Label($"{GetItemName(r.ElementA)} + {GetItemName(r.ElementB)} -> {GetItemName(r.Resultat)}");
            
            // Le petit bouton rouge pour supprimer une recette foirée
            GUI.backgroundColor = Color.red;
            if (GUILayout.Button("X", GUILayout.Width(25)))
            {
                _fusionDB.Recipes.RemoveAt(i);
                EditorUtility.SetDirty(_fusionDB); // On dit à Unity qu'on a modifié un truc
            }
            GUI.backgroundColor = Color.white;
            GUILayout.EndHorizontal();
        }
        GUILayout.EndScrollView();
    }

    // L'action d'ajouter la recette dans les datas
    private void AddRecipe()
    {
        // On check si le GD a bien rempli les 3 cases
        if (_inputA && _inputB && _output)
        {
            _fusionDB.Recipes.Add(new FusionDatabase.FusionRecipe { ElementA = _inputA, ElementB = _inputB, Resultat = _output });
            EditorUtility.SetDirty(_fusionDB); // Sauvegarde
            // On vide les cases pour être prêt pour la suite
            _inputA = null; _inputB = null; _output = null;
        }
    }

    // Petite fonction de confort pour afficher les noms sans se taper des erreurs "null"
    private string GetItemName(ItemData d) => d != null ? d.Label : "???";

    // Fonction utilitaire pour créer des dossiers si le GD a supprimé les anciens par erreur
    private void CreateFolder(string path)
    {
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
    }
}