using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[System.Serializable]
public class RatingData
{
    public int[] Normal;
    public int[] Epic;
    public int[] Legend;
}

[System.Serializable]
public class UpgradeDataWrapper
{
    public RatingData cost;
    public RatingData rate;
}

public class UpgradeData : MonoBehaviour
{
    public Dictionary<ITEM_RATING, int[]> upgradeGoldCost = new();
    public Dictionary<ITEM_RATING, int[]> successRates = new();

    void Awake()
    {      
        string filePath = @"C:\Users\USER\OneDrive\πŸ≈¡ »≠∏È\UpgradeData.json"; // πŸ≈¡»≠∏È ∞Ê∑Œ
        string jsonText = File.ReadAllText(filePath);
        var data = JsonUtility.FromJson<UpgradeDataWrapper>(jsonText);

        upgradeGoldCost[ITEM_RATING.Normal] = data.cost.Normal;
        upgradeGoldCost[ITEM_RATING.Epic] = data.cost.Epic;
        upgradeGoldCost[ITEM_RATING.Legend] = data.cost.Legend;

        successRates[ITEM_RATING.Normal] = data.rate.Normal;
        successRates[ITEM_RATING.Epic] = data.rate.Epic;
        successRates[ITEM_RATING.Legend] = data.rate.Legend;
    }
}
