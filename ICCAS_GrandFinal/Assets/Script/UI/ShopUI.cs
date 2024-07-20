using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopUI : MonoBehaviour
{
    public int itemSelect;
    public GameObject purchasePanel;

    [Header("아이템 사용")]
    public GameObject[] items;

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

    #region 아이템 세팅 및 사용
    public void SetItemTexts()
    {
        for (int i = 0; i < items.Length; i++)
        {
            items[i].transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = DataBase.instance.playerData.itemAmount[i].ToString();
        }
    }

    public void UseHPPotion()
    {
        DataBase.instance.playerData.itemAmount[0] -= 1;
        SetItemTexts();

        if (GameManager.instance.pcNS[DataBase.instance.playerData.cSelect].GetComponent<PlayerController>().curHp <
                    GameManager.instance.pcNS[DataBase.instance.playerData.cSelect].GetComponent<PlayerController>().maxHp)
        {
            GameManager.instance.pcNS[DataBase.instance.playerData.cSelect].GetComponent<PlayerController>().curHp += 200;
        }
        else
        {
            GameManager.instance.pcNS[DataBase.instance.playerData.cSelect].GetComponent<PlayerController>().curHp += 200;
            GameManager.instance.pcNS[DataBase.instance.playerData.cSelect].GetComponent<PlayerController>().curHp =
                GameManager.instance.pcNS[DataBase.instance.playerData.cSelect].GetComponent<PlayerController>().maxHp;
        }
    }

    public void UseAtkPotion(int index)
    {
        int dmg = GameManager.instance.pcNS[DataBase.instance.playerData.cSelect].GetComponent<PlayerController>().dmg;

        switch (index)
        {
            case 0:
                DataBase.instance.playerData.itemAmount[1] -= 1;
                SetItemTexts();

                GameManager.instance.pcNS[DataBase.instance.playerData.cSelect].GetComponent<PlayerController>().dmg = (int)(dmg * 1.5f);
                break;

            case 1:
                
                break;
        }
        

        
    }

    #endregion
}
