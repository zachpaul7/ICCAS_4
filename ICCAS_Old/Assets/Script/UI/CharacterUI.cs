using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CharacterUI : MonoBehaviour
{
    #region 캐릭터
    public int characterSelect = 0;

    // 캐릭터 선택 및 업그레이드 패널
    public GameObject[] characterObjs;
    public GameObject[] characterPanels;
    public GameObject[] lobbyCharacter;

    // 캐릭터 언락 & 업그레이드
    public GameObject characterUnlock;
    public GameObject[] nMaxLv0;
    public GameObject[] yMaxLv0;
    public GameObject[] nMaxLv1;
    public GameObject[] yMaxLv1;
    #endregion

    public void OpenCharacterSelect()
    {
        InitCharacter();
    }

    #region Charater 세팅
    public void InitCharacter()
    {
        SetCharacterListener();
        SetLockCharacter();

        for(int i = 0; i < DataBase.instance.characterData.level.Length; i++) 
        {
            DataBase.instance.AddCharacterLv(i, 0);
        }
    }

    private void SetCharacterListener()
    {
        for(int i = 0; i < characterObjs.Length; i++)
        {
            int temp = i;
            characterObjs[i].GetComponent<Button>().onClick.AddListener(() => OpenCharacterPanel(temp));
            characterObjs[i].transform.GetChild(1).GetComponent<Button>().onClick.AddListener(() => UnlockCharacater(temp));
        }
    }

    private void SetLockCharacter()
    {
        for(int i = 0; i < characterObjs.Length; i++)
        {
            Debug.Log(DataBase.instance.characterData.characterOpen[i]);
            if (DataBase.instance.characterData.characterOpen[i] == true)
            {
                characterObjs[i].transform.GetChild(1).gameObject.SetActive(false);
            }
            else
            {
                characterObjs[i].transform.GetChild(1).gameObject.SetActive(true);
            }
        }
    }
    #endregion

    #region Charater 선택
    
    public void OpenCharacterPanel(int index)
    {
        characterSelect = index;

        for(int i = 0; i < characterPanels.Length; i++)
        {
            if(i == index)
                characterPanels[i].SetActive(true);
            else
                characterPanels[i].SetActive(false);
        }
    }

    public void UnlockCharacater(int index)
    {
        characterSelect = index;

        if (DataBase.instance.characterData.characterOpen[index] != true)
        {
            characterUnlock.SetActive(true);
        }
    }

    public void UnlockCharacterConfirm()
    {
        if(DataBase.instance.playerData.gold < DataBase.instance.upgradeData[characterSelect].unlockGold)
        {
            Debug.Log("골드가 부족합니다.");
            return;
        }

        DataBase.instance.AddGold(-DataBase.instance.upgradeData[characterSelect].unlockGold);
        DataBase.instance.characterData.characterOpen[characterSelect] = true;
        
        characterObjs[characterSelect].transform.GetChild(1).gameObject.SetActive(false);
        characterUnlock.SetActive(false);
    }

    // 캐릭터 장착 & 업그레이드
    public void SelectCharacter(int index)
    {
        DataBase.instance.playerData.cSelect = index;

        for(int i = 0; i < characterObjs.Length; i++)
        {
            if(i == index)
            {
                lobbyCharacter[i].SetActive(true);
            }
            else
            {
                lobbyCharacter[i].SetActive(false);
            }
        }
    }

    public void UpgradeCharacter(int index)
    {
        if (DataBase.instance.characterData.level[index] >= DataBase.instance.upgradeData[index].maxLevel)
        {
            Debug.Log("최대 레벨입니다.");
            return;
        }
        else if (DataBase.instance.playerData.gold < DataBase.instance.upgradeData[characterSelect].upgradeGold)
        {
            Debug.Log("골드가 부족합니다.");
            return;
        }

        Debug.Log(-DataBase.instance.upgradeData[characterSelect].upgradeGold);

        DataBase.instance.AddGold(-DataBase.instance.upgradeData[characterSelect].upgradeGold);
        DataBase.instance.AddCharacterLv(index, 1);

        Debug.Log("정상적으로 업그레이드 되었습니다.");
    }


    #endregion

}
