using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterUI : MonoBehaviour
{
    public int characterSelect = 0;

    #region 캐릭터 선택 및 업그레이드 패널
    [Header("캐릭터 선택 및 업그레이드 패널")]
    public GameObject[] characterObjs;
    public GameObject[] characterPanels;
    public GameObject[] characterNS;
    public GameObject[] characterS;

    #endregion

    #region 캐릭터 언락 & 업그레이드
    [Header("캐릭터 언락 & 업그레이드")]
    public GameObject characterUnlock;
    public GameObject[] nMaxLv0;
    public GameObject[] yMaxLv0;
    public GameObject[] nMaxLv1;
    public GameObject[] yMaxLv1;
    #endregion

    #region 캐릭터 스킨 선택 & 언락
    [Header("캐릭터 스킨 선택 & 언락")]
    public GameObject cSkinPanel;
    public GameObject cSkinConfirm;
    public GameObject[] cSkinSelect;
    public GameObject[] cSkinLock;

    public int characterSkinSelect;
    private Button[][] cSkinBtn;
    private Image[][] cSkinBg;
    private Image[][] cSkinImg;
   
    #endregion

    #region 캐릭터 업그레이드
    [Header("캐릭터 업그레이드 세팅")]
    public TextMeshProUGUI[] characterHp;
    public TextMeshProUGUI[] characterAtk;
    #endregion

    private void Start()
    {
        SetCharacterSkin();
    }

    public void OpenCharacterSelect()
    {
        InitCharacter();
    }

    #region Charater 세팅
    public void InitCharacter()
    {
        SetLockCharacter();

        for (int i = 0; i < DataBase.instance.characterData.level.Length; i++) 
        {
            DataBase.instance.AddCharacterLv(i, 0);
        }

        for (int i = 0; i < characterObjs.Length; i++)
        {
            if (i == DataBase.instance.playerData.cSelect)
            {
                if (DataBase.instance.playerData.cSkinEquip[DataBase.instance.playerData.cSelect] == 1)
                {
                    characterS[i].SetActive(true);
                }
                else
                {
                    characterNS[i].SetActive(true);
                }
            }
            else
            {
                characterS[i].SetActive(false);
                characterNS[i].SetActive(false);
            }
        }
    }

    private void SetLockCharacter()
    {
        for(int i = 0; i < characterObjs.Length; i++)
        {
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

        SetUpgradeText(index);
        
        for (int i = 0; i < characterPanels.Length; i++)
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

        for (int i = 0; i < characterObjs.Length; i++)
        {
            if (i == index)
            {
                if (DataBase.instance.playerData.cSkinEquip[index] == 1)
                {
                    characterS[i].SetActive(true);
                    characterNS[i].SetActive(false);
                }
                else
                {
                    characterS[i].SetActive(false);
                    characterNS[i].SetActive(true);
                }
            }
            else
            {
                characterS[i].SetActive(false);
                characterNS[i].SetActive(false);
            }
        }
    }
    #endregion

    #region 캐릭터 업그레이드

    public void SetUpgradeText(int index)
    {
        characterHp[index].text = DataBase.instance.characterInfos[index].maxHp[DataBase.instance.characterData.level[index]].ToString();
        characterAtk[index].text = DataBase.instance.characterInfos[index].damage[DataBase.instance.characterData.level[index]].ToString();
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

        DataBase.instance.AddGold(-DataBase.instance.upgradeData[characterSelect].upgradeGold);
        DataBase.instance.AddCharacterLv(index, 1);
        SetUpgradeText(index);

        Debug.Log("정상적으로 업그레이드 되었습니다.");
    }


    #endregion

    #region 캐릭터 스킨 선택 및 해금
    public void SetCharacterSkin()
    {
        cSkinBtn = new Button[8][];
        cSkinBg = new Image[8][];
        cSkinImg = new Image[8][];
        
        for (int i = 0; i < 8; i++)
        {
            cSkinBtn[i] = new Button[2];
            cSkinBg[i] = new Image[2];
            cSkinImg[i] = new Image[2];
            
            for(int j = 0; j < 2; j++)
            {
                cSkinBtn[i][j] = cSkinSelect[i].transform.GetChild(j).GetComponent<Button>();
                cSkinBg[i][j] = cSkinSelect[i].transform.GetChild(j).GetComponent<Image>();
                cSkinImg[i][j] = cSkinSelect[i].transform.GetChild(j).GetChild(0).GetComponent<Image>();
            }
        }
    }

    public void SetLockCharacterSkin()
    {
        for (int i = 0; i < cSkinLock.Length; i++)
        {
            if (i == characterSelect && DataBase.instance.playerData.characterSkin[characterSelect])
            {
                cSkinLock[i].SetActive(false);
            }
            else
            {
                cSkinLock[i].SetActive(true);
            }
        }
    }

    // 스킨 버튼에 할당
    public void OpenCharacterSkinPanel()
    {
        cSkinPanel.SetActive(true);

        for (int i = 0; i < cSkinSelect.Length; i++)
        {
            if (i == characterSelect)
            {
                cSkinSelect[i].SetActive(true);
            }
            else
            {
                cSkinSelect[i].SetActive(false);
            }
        }

        SetLockCharacterSkin();

        if (DataBase.instance.playerData.cSkinEquip[characterSelect] == 1)
            CharacterSkinSelect(1);
        else
            CharacterSkinSelect(0);
    }

    // 캐릭터 스킨 선택 (각 스킨에 버튼에 할당)
    public void CharacterSkinSelect(int index)
    {
        characterSkinSelect = index;

        for(int i = 0; i < cSkinBg[characterSelect].Length; i++)
        {
            if(i == index)
            {
                Color c1 = cSkinBg[characterSelect][i].color;
                Color c2 = cSkinImg[characterSelect][i].color;

                c1.a = 1f;
                c2.a = 1f;

                cSkinBg[characterSelect][i].color = c1;
                cSkinImg[characterSelect][i].color = c2;
            }
            else
            {
                Color c1 = cSkinBg[characterSelect][i].color;
                Color c2 = cSkinImg[characterSelect][i].color;

                c1.a = 0.7f;
                c2.a = 0.7f;

                cSkinBg[characterSelect][i].color = c1;
                cSkinImg[characterSelect][i].color = c2;
            }
        }
    }

    public void BuyCharacterSkin()
    {
        cSkinConfirm.SetActive(true); 
    }

    public void BuyCharacterSkinConfirm()
    {
        if (DataBase.instance.playerData.characterSkin[characterSelect])
        {
            Debug.Log("이미 해금된 스킨입니다.");
            return;
        }
        if (DataBase.instance.playerData.gold < 8000)
        {
            Debug.Log("골드가 부족합니다.");
            return;
        }

        DataBase.instance.AddGold(-8000);
        DataBase.instance.playerData.characterSkin[characterSelect] = true;

        cSkinLock[characterSelect].SetActive(false);
        cSkinConfirm.SetActive(false);
    }

    public void CharacterSkinSelectConfirm(GameObject gameObj)
    {
        DataBase.instance.playerData.cSkinEquip[characterSelect] = characterSkinSelect;
        SelectCharacter(characterSelect);

        gameObj.SetActive(false);
    }

    #endregion

}
