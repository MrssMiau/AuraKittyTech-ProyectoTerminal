using UnityEngine;
using UnityEditor;

public class TextureFixer
{
    [MenuItem("Tools/Force Logo Sprite")]
    public static void Execute()
    {
        string path = "Assets/Recursos/Aura KittyTech.png";
        TextureImporter importer = AssetImporter.GetAtPath(path) as TextureImporter;
        if (importer == null)
        {
            Debug.LogError("Importer not found for " + path);
            return;
        }

        importer.textureType = TextureImporterType.Sprite;
        importer.spriteImportMode = SpriteImportMode.Single;
        importer.mipmapEnabled = false;
        importer.alphaIsTransparency = true;
        
        AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate);
        Debug.Log("Forced re-import of " + path + " as Sprite.");
    }
}
