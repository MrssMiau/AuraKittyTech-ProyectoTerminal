using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ModelUIManager : MonoBehaviour
{
    [Header("UI References")]
    public GameObject listItemPrefab;
    public Transform listContent;
    public TextMeshProUGUI tabTitleText;
    
    [Header("Buttons")]
    public Button btnEscaneados;
    public Button btnRealizados;
    public Button btnLoad;
    public Button btnSave;

    [Header("Configuration")]
    public string currentUserName = "Juanito";
    private string modelsPath = "Assets/Models";

    private void Awake()
    {
        // Asignar listeners por código para asegurar funcionamiento en Play Mode
        if (btnEscaneados != null) btnEscaneados.onClick.AddListener(ShowEscaneados);
        if (btnRealizados != null) btnRealizados.onClick.AddListener(ShowRealizados);
        if (btnLoad != null) btnLoad.onClick.AddListener(LoadSelectedModel);
        if (btnSave != null) btnSave.onClick.AddListener(SaveCurrentModel);
    }

    private void Start()
    {
        ShowEscaneados();
    }

    public void ShowEscaneados()
    {
        Debug.Log("Mostrando Escaneados");
        if (tabTitleText != null) tabTitleText.text = "Modelos Escaneados";
        string path = Path.Combine(modelsPath, "Escaneados");
        RefreshList(path, "Sistema", false);
    }

    public void ShowRealizados()
    {
        Debug.Log("Mostrando Realizados");
        if (tabTitleText != null) tabTitleText.text = "Realizados Anteriormente";
        string path = Path.Combine(modelsPath, "Guardados");
        RefreshList(path, currentUserName, true);
    }

    private void RefreshList(string folderPath, string creator, bool recursive)
    {
        // Limpiar lista actual
        foreach (Transform child in listContent)
        {
            Destroy(child.gameObject);
        }

        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }

        SearchOption option = recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;
        string[] files = Directory.GetFiles(folderPath, "*.obj", option);

        foreach (string filePath in files)
        {
            string fileName = Path.GetFileNameWithoutExtension(filePath);
            string folderName = Path.GetFileName(Path.GetDirectoryName(filePath));
            CreateModelItem(fileName, creator, folderName);
        }
    }

    private void CreateModelItem(string modelName, string creator, string folder)
    {
        GameObject item = Instantiate(listItemPrefab, listContent);
        item.SetActive(true);
        
        TextMeshProUGUI textComp = item.GetComponentInChildren<TextMeshProUGUI>();
        if (textComp != null)
        {
            string cleanName = modelName.Replace("_", " ");
            textComp.text = $"{cleanName} por {folder}";
        }
    }

    public void SaveCurrentModel()
    {
        Debug.Log($"Guardando modelo actual para: {currentUserName}");
    }
    
    public void LoadSelectedModel()
    {
        Debug.Log("Cargando modelo seleccionado...");
    }
}
