using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopUI : MonoBehaviour
{
    public int itemSelect;
    public GameObject purchasePanel;

    public void OpenPurchasePanel(int index)
    {
        itemSelect = index;
        purchasePanel.SetActive(true);
    }

    public void PurchaseItem()
    {
        if (DataBase.instance.playerData.gold < DataBase.instance.itemData[itemSelect].cost)
        {
            Debug.Log("골드가 부족합니다.");
            return;
        }

        DataBase.instance.AddGold(-DataBase.instance.itemData[itemSelect].cost);
        DataBase.instance.playerData.itemAmount[itemSelect] += 1;

        purchasePanel.SetActive(false);
    }
}
