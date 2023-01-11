using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISaveEquipment
{
    void LoadData(ArmorInventoryData data);
    void SaveData(ref ArmorInventoryData data);
}
