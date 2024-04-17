# Unity Material Texture Tools

_A repository of different tools to use in Unity 3D to deal with materials and textures in multiple ways. Subject to updates in the future._

I created these tools to assist users in correcting potential issues with missing textures when importing Unity packages containing files for different Render Pipelines or preparing the Packges themselves for later distribution.

Nevertheless, this tool could have other potential uses, such as:

1. Serving as the foundation for an efficient tool to convert materials between different Render Pipelines.
2. Transforming materials in Unity projects that contain shaders and materials originally converted from Unreal projects. Often, vendors do not update to the most efficient shaders and instead use default ones for these conversions.

## **Material Texture Fixer (v1.0.0.):**

A simple tool with two main functions:

1. Registers and stores all the currently associated textures and their paths for any material selected in the Project tab. This information is stored in a JSON file created at the same location as the selected material.
2. If one or more materials are selected in the Project Tab and there are JSON files as described above in the same path, the tool will read their content and reassign the texture files to the materials according to the annotations in the JSON files.

## **Quick Character Material Setter (v1.0.0.):**

This Unity editor script,  is designed to streamline the process of updating materials with new textures within Unity. It provides functionality through a custom editor window, which you can access under "Tools/Ahab Tools/Quick Character Material Setter" in the Unity Editor menu. 

### **Features and Workflow**

**Custom Editor Window:**

* The script creates a custom editor window that provides a user-friendly interface for performing texture updates on materials. This window includes input fields, buttons, and informative text to guide you through the process.

**Texture Path Configuration:**

* You can specify the path where new textures are located. There's an option to automatically set this path to the currently selected folder in the Unity Project tab, enhancing your workflow efficiency.

**Texture Renaming:**

* The tool supports renaming textures based on keywords. You can specify an "Original Keyword" to look for in texture names and a "New Keyword" to replace it with. This feature is particularly useful for batch renaming textures to conform to new naming conventions or to specify texture types.

**Secondary Texture Path:**

* Optionally, you can specify a secondary path to look for textures, increasing the likelihood of finding the correct resources when they are not all stored in a single directory.

**Material Update:**

* Once paths and renaming rules are set, you can apply these to selected materials. The script searches through the specified directories, finds matching textures, and assigns them to the appropriate texture properties of the selected materials.

### **How to Use**

1\. Open the Editor Window:

* Navigate to "Tools/Ahab Tools/Quick Character Material Setter" in Unity’s top menu to open the editor window.

2\. Configure Paths and Keywords:

* Enter the main path for new textures.
* If needed, enable and configure the secondary path.
* Set the original and new keywords for renaming textures.

3\. Select Materials:

* In the Unity Project tab, select the materials you want to update.

4\. Execute Updates:

* Click the "Refill selected materials" button to start the process. The script will update the materials with the new textures based on the configured settings.



