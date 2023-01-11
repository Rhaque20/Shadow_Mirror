using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SaveEquipment: MonoBehaviour
{
    [Header("File Storage Config")]
    [SerializeField]private string fileName,fileName2;

    private ArmorInventoryData id;
    public static SaveEquipment instance {get; private set;}
    private List<ISaveEquipment> saveSystemObjects;
    private List<ISaveLoadOut> saveLoadOutObject;
    private FileDataHandler fdh, fdh2;
    private PlayerPartyLoadOut ppl;

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
        fdh2 = new FileDataHandler(Application.persistentDataPath,fileName2, useEncryption);
        this.saveSystemObjects = FindAllSaveObjects();
        this.saveLoadOutObject = FindAllLoadOutObjects();
        LoadGame();
    }

    private List<ISaveEquipment> FindAllSaveObjects()
    {
        IEnumerable<ISaveEquipment> saveSystemObjects = FindObjectsOfType<MonoBehaviour>().
        OfType<ISaveEquipment>();

        return new List<ISaveEquipment>(saveSystemObjects);
    }

    private List<ISaveLoadOut> FindAllLoadOutObjects()
    {
        IEnumerable<ISaveLoadOut> saveLoadOutObject = FindObjectsOfType<MonoBehaviour>().
        OfType<ISaveLoadOut>();

        return new List<ISaveLoadOut>(saveLoadOutObject);
    }

    public void NewGame()
    {
        id = new ArmorInventoryData();
    }

    public void LoadGame()
    {
        id = fdh.LoadArmor();

        if (this.id == null)
        {
            Debug.Log("No Data for Equipment");
            NewGame();
        }

        foreach(ISaveEquipment ss in saveSystemObjects)
        {
            ss.LoadData(id);
        }

        ppl = fdh2.LoadLoadOut();

        if (this.ppl == null)
        {
            Debug.Log("Loadout is empty");
            ppl = new PlayerPartyLoadOut();
        }

        foreach(ISaveLoadOut sl in saveLoadOutObject)
        {
            sl.LoadData(ppl);
        }

    }

    public void SaveGame()
    {
        //Inventory i;
        foreach(ISaveEquipment ss in saveSystemObjects)
        {
            ss.SaveData(ref id);
            //i = ss.GetComponent<Inventory>();
            //if (ss.Contains(Inventory))
        }
        fdh.SaveArmors(id);

        //Debug.Log("We have "+id.listOfArmors[0].name);
        foreach(ISaveLoadOut sl in saveLoadOutObject)
        {
            sl.SaveData(ref ppl);
        }

        fdh2.SaveLoadOut(ppl);
        
    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }
}
