using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestCurrency : MonoBehaviour
{
    public GameObject save;

    private void Start()
    {
        SetListener();
    }

    public void SetListener()
    {
        save.GetComponent<Button>().onClick.AddListener(OnClick);
    }

    public void OnClick()
    {
        DataBase.instance.SaveJsonToPlayfab();
    }

    public void CheckValue()
    {
        
    }
}
