using TMPro;
using UnityEngine;

public class EconomyManager : Singleton<EconomyManager>
{
    private TMP_Text goldText;
    private int currentGold;
    
    private const string COIN_AMOUNT_TEXT = "Gold Amount Text";
    
    public void UpdateCurrentGold(int amount = 1)
    {
        if (goldText == null)
        {
            goldText = GameObject.Find(COIN_AMOUNT_TEXT).GetComponent<TMP_Text>();
        }
        
        currentGold += amount;
        goldText.text = currentGold.ToString("D3");
    }
}
