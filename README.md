A simple tool with two main functions:

1. Registers and stores all the currently associated textures and their paths for any material selected in the Project tab. This information is stored in a JSON file created at the same location as the selected material.
2. If one or more materials are selected in the Project Tab and there are JSON files as described above in the same path, the tool will read their content and reassign the texture files to the materials according to the annotations in the JSON files.
I created this tool to assist users in correcting potential issues with missing textures when importing Unity packages containing files for different Render Pipelines.

Nevertheless, this tool could have other potential uses, such as:

Serving as the foundation for an efficient tool to convert materials between different Render Pipelines.
Transforming materials in Unity projects that contain shaders and materials originally converted from Unreal projects. Often, vendors do not update to the most efficient shaders and instead use default ones for these conversions.





