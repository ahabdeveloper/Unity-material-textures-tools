using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;

namespace AhabTools
{
    public class QuickCharacterMaterialSetter : EditorWindow
    {
        #region Variables
        private string newTexturesContainer = "Assets";
        private string newTexturesContainerAlt = "Assets";
        private bool auxVariablesFoldout = true;
        private bool useSecondPath = true;
        private string originalKeyword = "NX"; 
        private string newKeyword = "N1"; 
        private Vector2 scrollPosition;
        private bool renameTextures = true;
        private GUIStyle foldoutHeaderStyle;
        private GUIStyle foldoutContentStyle;
        #endregion

        [MenuItem("Tools/Ahab Tools/Quick Character Material Setter")]
        public static void ShowWindow()
        {
            GetWindow<QuickCharacterMaterialSetter>("Quick Character Material Setter v1.0.0.");
        }

        void OnGUI()
        {
            scrollPosition = GUILayout.BeginScrollView(scrollPosition);
            #region Hyperlink
            GUIStyle hyperlinkStyle = new GUIStyle();
            hyperlinkStyle.normal.textColor = Color.gray;
            hyperlinkStyle.fontStyle = FontStyle.Italic;
            Rect linkRect = EditorGUILayout.GetControlRect();
            if (GUI.Button(linkRect, "Click here to visit the GitHub repository of this tool to check the latest version.", hyperlinkStyle))
            {
                Application.OpenURL("https://github.com/ahabdeveloper/Unity-material-textures-tools");
            }
            #endregion

            GUILayout.Space(5);

            #region How to use infobox
            EditorGUILayout.HelpBox("How to Use:\n" +
                "\n" +
                "1. Set the path to the Main Parent Folder you wish to iterate through in search of new textures.\n" +
                "\n" +
                "2. If necessary, activate the 'Rename Textures' option and use it as follows:\n" +
                "\n" +
                " - Original Keyword: The string value you want to replace after identifying the original texture names.\n" +
                " - New Keyword: The new string value you want to add.\n" +
                "(For example, suppose we are creating mice of different colors. Our main material has a diffuse texture " +
                "named 'Mouse_Color_X', and we want to create a new red mouse with a diffuse map called 'Mouse_Color_Red'. " +
                "You would enter 'X' as the Original Keyword and 'Red' as the New Keyword.)\n" +
                "\n" +
                "3. Select all those materials which you want to reasign their textures in the Project Tab.\n" +
                "\n" +
                "4. If necessary activate a Secondary Path to lookf for textures.\n" +
                "\n" +
                "5. Execute the method by clicking the button!", MessageType.None);
            #endregion

            GUILayout.Space(5);
            GUILayout.Label("New Textures' Path Main Parent:");
            GUILayout.BeginHorizontal();
            newTexturesContainer = GUILayout.TextField(newTexturesContainer);
            GUIContent buttonContent = new GUIContent("R", "Register automatically the current selected folder in the Project Tab");  
            if (GUILayout.Button(buttonContent, GUILayout.Width(20), GUILayout.Height(20)))
            {
                SetNewTexturesContainerPathToSelectedFolder();
            }
            GUILayout.EndHorizontal();

            GUILayout.Space(5);

            GUIContent buttonContent1 = new GUIContent("Refill selected materials", "Don't forget to have the maeterials selected in the Project Tab!");
            if (GUILayout.Button(buttonContent1, GUILayout.Height(40)))
            {
                RefillAllMaterials();
            }

            GUILayout.Space(15);
            #region Foldout Styles
            if (foldoutHeaderStyle == null)
            {
                foldoutHeaderStyle = new GUIStyle("ShurikenModuleTitle")
                {
                    border = new RectOffset(2, 2, 2, 2),
                    fixedHeight = 22,
                    contentOffset = new Vector2(20f, -2f)
                };
                foldoutHeaderStyle.contentOffset = new Vector2(20f, -2f);
                foldoutHeaderStyle.fixedWidth = 0;
            }

            if (foldoutContentStyle == null)
            {
                foldoutContentStyle = new GUIStyle(GUI.skin.box)
                {
                    padding = new RectOffset(1, 1, 1, 1),
                    margin = new RectOffset(1, 1, 1, 1)
                };
            }
            #endregion
            #region Foldout drawing
            Rect backgroundRect9 = GUILayoutUtility.GetRect(1f, 22f, GUILayout.ExpandWidth(true));
            GUI.Box(backgroundRect9, GUIContent.none, foldoutHeaderStyle);
            auxVariablesFoldout = EditorGUI.Foldout(backgroundRect9, auxVariablesFoldout, "Auxiliar Options", true, foldoutHeaderStyle);
            #endregion
            if (auxVariablesFoldout)
            {
                EditorGUI.indentLevel++;
                EditorGUI.DrawRect(EditorGUILayout.BeginVertical(EditorStyles.helpBox), new Color(0.18f, 0.18f, 0.18f, 1f));
                GUILayout.Label("Rename Textures?");
                // Custom toggle buttons
                GUILayout.BeginHorizontal();
                GUIStyle activeStyle = new GUIStyle(GUI.skin.button);
                activeStyle.normal.background = GUI.skin.button.active.background;

                if (GUILayout.Button("On", renameTextures ? activeStyle : GUI.skin.button))
                {
                    renameTextures = true;
                }

                if (GUILayout.Button("Off", !renameTextures ? activeStyle : GUI.skin.button))
                {
                    renameTextures = false;
                }
                GUILayout.EndHorizontal();

                if (renameTextures)
                {
                    EditorGUILayout.HelpBox("Use the two following string variables to set those values to be renamed ", MessageType.Info);
                    GUILayout.Label("Original Keyword:");
                    originalKeyword = GUILayout.TextField(originalKeyword);
                    GUILayout.Label("New Keyword:");
                    newKeyword = GUILayout.TextField(newKeyword);
                }

                // Toggle for using a second path
                GUILayout.Label("New Textures' Secondary Path Main Parent:");
                // Custom toggle buttons
                GUILayout.BeginHorizontal();
                GUIStyle activeStyle1 = new GUIStyle(GUI.skin.button);
                activeStyle1.normal.background = GUI.skin.button.active.background;

                if (GUILayout.Button("On", useSecondPath ? activeStyle1 : GUI.skin.button))
                {
                    useSecondPath = true;
                }

                if (GUILayout.Button("Off", !useSecondPath ? activeStyle1 : GUI.skin.button))
                {
                    useSecondPath = false;
                }
                GUILayout.EndHorizontal();
                if (useSecondPath)
                {
                    GUILayout.Label("New Textures' Secondary Path Main Parent:");
                    GUILayout.BeginHorizontal();
                    newTexturesContainerAlt = GUILayout.TextField(newTexturesContainerAlt);
                    GUIContent buttonContent2 = new GUIContent("R", "Register automatically the current selected folder in the Project Tab");
                    if (GUILayout.Button(buttonContent2, GUILayout.Width(20), GUILayout.Height(20)))
                    {
                        SetNewTexturesContainerPathToSelectedFolderAlt();
                    }
                    GUILayout.EndHorizontal();
                }
                EditorGUILayout.EndVertical();
                EditorGUI.indentLevel--;
            }
            GUILayout.EndScrollView();
        }

        #region Auxiliar Methods
        private void RefillAllMaterials()
        {
            string[] extensions = new string[] { ".png", ".jpg", ".jpeg", ".tga", ".bmp", ".gif", ".psd", ".tif", ".tiff" }; // Supported texture formats

            foreach (Object obj in Selection.objects)
            {
                if (obj is Material)
                {
                    Material material = obj as Material;
                    Shader shader = material.shader;
                    Debug.Log($"Refilling textures for material: {material.name}");

                    int propertyCount = ShaderUtil.GetPropertyCount(shader);
                    for (int i = 0; i < propertyCount; i++)
                    {
                        if (ShaderUtil.GetPropertyType(shader, i) == ShaderUtil.ShaderPropertyType.TexEnv)
                        {
                            string propertyName = ShaderUtil.GetPropertyName(shader, i);
                            Texture texture = material.GetTexture(propertyName);
                            if (texture != null)
                            {
                                string textureName = texture.name;
                                string path = AssetDatabase.GetAssetPath(texture);
                                string extension = Path.GetExtension(path);

                                if (renameTextures && textureName.Contains(originalKeyword))
                                {
                                    string oldTextureName = textureName;
                                    textureName = textureName.Replace(originalKeyword, newKeyword);
                                    Debug.Log($"Renaming texture from {oldTextureName} to {textureName + extension}.");
                                }

                                string[] pathsToSearch = { newTexturesContainer };
                                if (useSecondPath)
                                {
                                    pathsToSearch = new string[] { newTexturesContainer, newTexturesContainerAlt };
                                }

                                Texture newTexture = FindTextureInPaths(pathsToSearch, textureName, extension);

                                if (newTexture != null)
                                {
                                    material.SetTexture(propertyName, newTexture);
                                    Debug.Log($"Updated {propertyName} with new texture {newTexture.name}");
                                }
                                else
                                {
                                    Debug.LogWarning($"Failed to load new texture for {propertyName} from potential paths for material {material.name}. Texture is {textureName}");
                                }
                            }
                        }
                    }
                }
            }
        }
        private Texture FindTextureInPaths(string[] paths, string textureName, string extension)
        {
            foreach (string path in paths)
            {
                string foundTexturePath = SearchDirectoryForTexture(path, textureName, extension);
                if (!string.IsNullOrEmpty(foundTexturePath))
                {
                    return AssetDatabase.LoadAssetAtPath<Texture>(foundTexturePath);
                }
            }
            return null;
        }
        private string SearchDirectoryForTexture(string basePath, string textureName, string extension)
        {
            Queue<string> directories = new Queue<string>();
            directories.Enqueue(basePath);
            Debug.Log($"Starting search in base path: {basePath} for texture: {textureName + extension}");
            while (directories.Count > 0)
            {
                string currentDirectory = directories.Dequeue();
                Debug.Log($"Searching in directory: {currentDirectory}");
                string[] files = Directory.GetFiles(currentDirectory, textureName + extension, SearchOption.TopDirectoryOnly);
                if (files.Length > 0)
                {
                    Debug.Log($"Found texture at: {files[0]}");
                    return files[0];
                }

                foreach (string directory in Directory.GetDirectories(currentDirectory))
                {
                    directories.Enqueue(directory);
                    Debug.Log($"Enqueuing sub-directory: {directory}");
                }
            }
            return null;
        }
        private void SetNewTexturesContainerPathToSelectedFolder()
        {
            if (Selection.activeObject && AssetDatabase.IsValidFolder(AssetDatabase.GetAssetPath(Selection.activeObject)))
            {
                newTexturesContainer = AssetDatabase.GetAssetPath(Selection.activeObject);
                Debug.Log($"New Textures Container path set to: {newTexturesContainer}");
            }
            else
            {
                Debug.LogWarning("Please select a folder in the Project tab.");
            }
        }
        private void SetNewTexturesContainerPathToSelectedFolderAlt()
        {
            if (Selection.activeObject && AssetDatabase.IsValidFolder(AssetDatabase.GetAssetPath(Selection.activeObject)))
            {
                newTexturesContainerAlt = AssetDatabase.GetAssetPath(Selection.activeObject);
                Debug.Log($"New Textures Container path set to: {newTexturesContainerAlt}");
            }
            else
            {
                Debug.LogWarning("Please select a folder in the Project tab.");
            }
        }
        #endregion
    }
}
