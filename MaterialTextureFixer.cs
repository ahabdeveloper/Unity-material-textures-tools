using UnityEngine;
using UnityEditor;
using System.IO;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using Unity.Plastic.Newtonsoft.Json;

public class MaterialTextureFixer : EditorWindow
{
    [MenuItem("Tools/Ahab Tools/Material Texture Fixer")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(MaterialTextureFixer), false, "Material Texture Fixer");
    }

    void OnGUI()
    {
        GUILayout.Label("Step 1. Save textures' paths.", EditorStyles.boldLabel);
        EditorGUILayout.HelpBox("Registers and stores all the currently associated textures and their paths for any material selected in the Project tab. This information is stored in a JSON file created at the same location as the selected material.", MessageType.Info);
        if (GUILayout.Button("Store paths into JSON"))
        {
            SaveTexturesVariablesAndNamesToJson();
        }
        GUILayout.Label("Step 2. Refill textures.", EditorStyles.boldLabel);
        EditorGUILayout.HelpBox("If one or more materials are selected and there are JSON files as described above, the tool will read their content and reassign the texture files to the materials according to the annotations in the JSON files.", MessageType.Info);
        if (GUILayout.Button("Refill Materials"))
        {
            RefillMaterialsFromJson();
        }
    }
    #region Auxiliar methods
    private static void SaveTexturesVariablesAndNamesToJson()
    {
        foreach (Object selectedObject in Selection.objects)
        {
            if (selectedObject is Material)
            {
                Material selectedMaterial = selectedObject as Material;
                SaveMaterialTexturesToJson(selectedMaterial);
            }
        }

        if (Selection.objects.Length == 0 || Selection.objects.All(obj => obj.GetType() != typeof(Material)))
        {
            Debug.Log("Please select one or more materials in the project tab.");
        }
    }
    private static void SaveMaterialTexturesToJson(Material material)
    {
        string materialPath = AssetDatabase.GetAssetPath(material);
        string directoryPath = Path.GetDirectoryName(materialPath);
        Shader shader = material.shader;

        StringBuilder jsonBuilder = new StringBuilder();
        jsonBuilder.AppendLine("{");
        jsonBuilder.AppendLine("  \"Textures\": [");

        int propertyCount = ShaderUtil.GetPropertyCount(shader);
        bool firstEntry = true;
        for (int i = 0; i < propertyCount; i++)
        {
            if (ShaderUtil.GetPropertyType(shader, i) == ShaderUtil.ShaderPropertyType.TexEnv)
            {
                string propertyName = ShaderUtil.GetPropertyName(shader, i);
                Texture texture = material.GetTexture(propertyName);
                if (texture != null)
                {
                    if (!firstEntry)
                    {
                        jsonBuilder.AppendLine(",");
                    }
                    jsonBuilder.AppendLine($"    {{\"PropertyName\": \"{propertyName}\", \"TextureName\": \"{texture.name}\", \"FilePath\": \"{AssetDatabase.GetAssetPath(texture)}\"}}");
                    firstEntry = false;
                }
            }
        }

        jsonBuilder.AppendLine("  ]");
        jsonBuilder.AppendLine("}");

        string json = jsonBuilder.ToString();
        string sanitizedMaterialName = material.name.Replace('/', '_').Replace('\\', '_').Replace(':', '_');
        string fileName = $"{sanitizedMaterialName}_Textures.json";
        string fullPath = Path.Combine(directoryPath, fileName);

        File.WriteAllText(fullPath, json);
        Debug.Log($"Texture variables and names saved to JSON file at {fullPath}");
    }
    private static void RefillMaterialsFromJson()
    {
        foreach (Object selectedObject in Selection.objects)
        {
            if (selectedObject is Material)
            {
                Material selectedMaterial = selectedObject as Material;
                string materialPath = AssetDatabase.GetAssetPath(selectedMaterial);
                string directoryPath = Path.GetDirectoryName(materialPath);
                string sanitizedMaterialName = selectedMaterial.name.Replace('/', '_').Replace('\\', '_').Replace(':', '_');
                string fileName = $"{sanitizedMaterialName}_Textures.json";
                string fullPath = Path.Combine(directoryPath, fileName);

                if (File.Exists(fullPath))
                {
                    string json = File.ReadAllText(fullPath);
                    var textureInfoList = JsonConvert.DeserializeObject<Dictionary<string, List<Dictionary<string, string>>>>(json);
                    foreach (var item in textureInfoList["Textures"])
                    {
                        string propertyName = item["PropertyName"];
                        string filePath = item["FilePath"];
                        Texture texture = AssetDatabase.LoadAssetAtPath<Texture>(filePath);
                        if (texture != null)
                        {
                            selectedMaterial.SetTexture(propertyName, texture);
                        }
                    }
                    Debug.Log($"Textures refilled for material: {selectedMaterial.name}");
                }
                else
                {
                    Debug.LogError($"JSON file not found for material: {selectedMaterial.name}");
                }
            }
        }

        if (Selection.objects.Length == 0 || Selection.objects.All(obj => obj.GetType() != typeof(Material)))
        {
            Debug.Log("Please select one or more materials in the project tab.");
        }
    }
    #endregion
}
