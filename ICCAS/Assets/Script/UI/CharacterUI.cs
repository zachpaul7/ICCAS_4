using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterUI : MonoBehaviour
{
    public GameObject[] characterObjs;
    public GameObject[] characterPanels;


    #region Charater 세팅

    private void SetCharacterListener()
    {
        for(int i = 0; i < characterObjs.Length; i++)
        {
            int temp = i;
            characterObjs[i].GetComponent<Button>().onClick.AddListener(() => OpenCharacterPanel(temp));
        }
    }
    #endregion

    #region Charater 선택
    public void OpenCharacterPanel(int index)
    {
        for(int i = 0; i < characterPanels.Length; i++)
        {
            if(i == index)
                characterPanels[i].SetActive(true);
            else
                characterPanels[i].SetActive(false);
        }
    }

    // 캐릭터 장착
    public void SelectCharacter(int index)
    {
        DataBase.instance.playerData.cSelect = index;
    }
    #endregion

}
