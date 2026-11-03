using UnityEngine;
using UnityEditor;

public class SpriteChecker
{
    [MenuItem("Tools/Check Sprite")]
    public static void Execute()
    {
        string path = "Assets/Recursos/Aura KittyTech.png";
        Sprite sprite = AssetDatabase.LoadAssetAtPath<Sprite>(path);
        if (sprite == null)
        {
            Debug.LogError("Sprite NOT found at " + path);
            // Check if it's a texture
            Texture2D tex = AssetDatabase.LoadAssetAtPath<Texture2D>(path);
            if (tex != null)
            {
                Debug.Log("Texture2D found, but not Sprite. Texture type: " + (AssetImporter.GetAtPath(path) as TextureImporter).textureType);
            }
        }
        else
        {
            Debug.Log("Sprite found! Size: " + sprite.rect.width + "x" + sprite.rect.height);
        }
    }
}
