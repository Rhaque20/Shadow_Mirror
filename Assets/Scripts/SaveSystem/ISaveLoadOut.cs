using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISaveLoadOut
{
    void LoadData(PlayerPartyLoadOut data);
    void SaveData(ref PlayerPartyLoadOut data);
}
