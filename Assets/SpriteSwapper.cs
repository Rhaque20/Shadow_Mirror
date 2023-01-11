using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using Unity.Collections;
//[ExecuteInEditMode]
public class SpriteSwapper : MonoBehaviour
{
    //public GameObject[] parts;
    public Sprite[] bodyparts;
    public UnityEngine.U2D.Animation.SpriteLibrary sl;
    public UnityEngine.U2D.Animation.SpriteResolver srr;
    public string category;
    // private SpriteLibraryAsset la => sl.SpriteLibraryAsset;
    // Start is called before the first frame update
    void Start()
    {
        
    }


    public void SpriteSwap(int index)
    {
        //Sprite s = parts[index].GetComponent<SpriteRenderer>().sprite;
        // Get the sprite at the index of bodyparts;
        Sprite s = bodyparts[index];
        // Get the label from the sprite resolver (as each resolver is assigned a category);
        string reflabel = srr.GetLabel();
        // Get the sprite from the category declared and the label obtained from the previous line
        // The category would be declared based on the body part you want to swap
        Sprite refarm = sl.GetSprite(category, reflabel);
        // Get all the bones and poses from sprite in the at the resolver
        // This means the sprite obtained from the resolve needed to have the bones and weights set up beforehand
        UnityEngine.U2D.SpriteBone[] bones = refarm.GetBones();
        NativeArray<Matrix4x4> poses = refarm.GetBindPoses();
        // give the index bodypart all the bones and poses.
        SpriteDataAccessExtensions.SetBones(s,bones);
        SpriteDataAccessExtensions.SetBindPoses(s,poses);
        // Declare a label for the new part
        const string clable = "forearm_narrow";
        // Insert the indexed bodypart to the sprite library (or override in this case)
        sl.AddOverride(s, category, clable);
        srr.SetCategoryAndLabel(category,clable);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
