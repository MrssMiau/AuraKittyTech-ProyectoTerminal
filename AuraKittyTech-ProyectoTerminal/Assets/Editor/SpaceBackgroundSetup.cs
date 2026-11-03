using UnityEngine;
using UnityEditor;

public class SpaceBackgroundSetup
{
    public static void Execute()
    {
        string texturePath = "Assets/Textures/SpaceBackground.png";
        string materialPath = "Assets/Materials/SpaceSkybox.mat";

        // 1. Load Texture
        Texture2D texture = AssetDatabase.LoadAssetAtPath<Texture2D>(texturePath);
        if (texture == null)
        {
            Debug.LogError("No se encontró la textura en " + texturePath);
            return;
        }

        // 2. Create Material
        if (!AssetDatabase.IsValidFolder("Assets/Materials"))
        {
            AssetDatabase.CreateFolder("Assets", "Materials");
        }

        Material skyboxMat = new Material(Shader.Find("Skybox/Panoramic"));
        skyboxMat.SetTexture("_MainTex", texture);
        
        AssetDatabase.CreateAsset(skyboxMat, materialPath);
        AssetDatabase.SaveAssets();

        // 3. Set Skybox
        RenderSettings.skybox = skyboxMat;
        RenderSettings.ambientMode = UnityEngine.Rendering.AmbientMode.Skybox;
        
        // Dynamic GI update might be needed
        DynamicGI.UpdateEnvironment();

        Debug.Log("Fondo de espacio configurado correctamente con la nebulosa generada.");
    }
}
