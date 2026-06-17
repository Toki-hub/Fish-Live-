using UnityEngine;

[CreateAssetMenu(fileName = "FishData", menuName = "Fish/Fish Data")]
public class FishData : ScriptableObject
{
    [Header("Identitas")]
    public string fishName;
    public string latinName;

    [Header("Info")]
    public string habitat;
    public string description;
    public string conservationStatus;
    public float maxSizeCm;

    [Header("Visual")]
    public Sprite fishImage;   // foto ikan untuk panel info

    [Header("Display 3D")]
    public int modelIndex = 0;   // 0=snapper, 1=betta, 2=flyingfish
}