using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AniCon : MonoBehaviour
{
    public void DisableDamageText(int index)
    {
        UIManager.instance.exUI.DisableDamageText(index);
    }
}
