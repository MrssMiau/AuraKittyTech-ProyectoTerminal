using System;
using UnityEngine;

[Serializable]
public class ModelData
{
    public string modelName;
    public string creatorName;
    public string folderPath;
    public Sprite previewImage; // In a real scenario, this might be loaded from a file or generated
}
