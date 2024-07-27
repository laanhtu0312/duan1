using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using UnityEngine.UI;

public class ResourceManager : MonoBehaviour
{
    public int gold = 100;
    public Text goldText;

    void Start()
    {
        UpdateGoldText();
    }

    public void AddGold(int amount)
    {
        gold += amount;
        UpdateGoldText();
    }

    public bool SpendGold(int amount)
    {
        if (gold >= amount)
        {
            gold -= amount;
            UpdateGoldText();
            return true;
        }
        return false;
    }

    void UpdateGoldText()
    {
        goldText.text = "Gold: " + gold.ToString();
    }
}
