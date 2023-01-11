using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D.Animation;

public class EquipmentSprites : MonoBehaviour
{
    public Sprite[] rarityFrames = new Sprite[6];
    public Sprite[] rarityBar = new Sprite[6];
    public SpriteLibraryAsset armorIcons, weaponIcons;
    public Sprite[] blankGear = new Sprite[7];
    //public List<SetBonus> bonusDesc = new List<SetBonus>();
    public SetBonus[] bonusDesc = new SetBonus[1];

    public Sprite GetArmorIcon(string category, string label)
    {
        return armorIcons.GetSprite(category,label);
    }
    
}