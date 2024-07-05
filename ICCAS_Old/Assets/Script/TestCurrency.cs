using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCurrency : MonoBehaviour
{
    public void OnClick()
    {
        DataBase.instance.SaveJsonToPlayfab();
    }

    public void CheckValue()
    {
        
    }
}
