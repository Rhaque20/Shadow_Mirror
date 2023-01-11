using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SaveSystem: MonoBehaviour
{
    [Header("File Storage Config")]
    [SerializeField]private string fileName;

    private GameData gd;
    public static SaveSystem instance {get; private set;}
    private List<ISaveSystem> saveSystemObjects;
    private FileDataHandler fdh;

    [SerializeField]private bool useEncryption;


    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Should not occur");
        }
        instance = this;
    }

    private void Start()
    {
        fdh = new FileDataHandler(Application.persistentDataPath,fileName, useEncryption);
        this.saveSystemObjects = FindAllSaveObjects();
        LoadGame();
    }

    private List<ISaveSystem> FindAllSaveObjects()
    {
        IEnumerable<ISaveSystem> saveSystemObjects = FindObjectsOfType<MonoBehaviour>().
        OfType<ISaveSystem>();

        return new List<ISaveSystem>(saveSystemObjects);
    }

    public void NewGame()
    {
        gd = new GameData();
    }

    public void LoadGame()
    {
        gd = fdh.Load();

        if (this.gd == null)
        {
            Debug.Log("No Data");
            NewGame();
        }

        foreach(ISaveSystem ss in saveSystemObjects)
        {
            ss.LoadData(gd);
        }
    }

    public void SaveGame()
    {
        Inventory i;
        foreach(ISaveSystem ss in saveSystemObjects)
        {
            ss.SaveData(ref gd);
            //i = ss.GetComponent<Inventory>();
            //if (ss.Contains(Inventory))
        }

        //fdh.Save(gd);
    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }
}
