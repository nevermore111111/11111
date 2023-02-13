using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 
/// <summary>
public class UIManager : MonoBehaviour
{
    private ChangeClothes changeClothes;
    public GameObject actor;
    private void Awake()
    {
        changeClothes = actor.GetComponent<ChangeClothes>();
        Button buttonNext;
        buttonNext = GetButton("Button_ClothNext");
        buttonNext.onClick.AddListener(delegate { changeClothes.NextCloth(); });
        Button buttonFor;
        buttonNext = GetButton("Button_ClothFor");
        buttonNext.onClick.AddListener(delegate { changeClothes.ForCloth(); });
    }
    /// <summary>
    /// Ñ°ÕÒÏÂ¼¶µÄbutton
    /// </summary>
    /// <param name="button_Name"></param>
    /// <returns></returns>
    private Button GetButton(string button_Name)
    {
        return HelpTools.FindChildByName(this.transform, button_Name).GetComponent<Button>();
    }
}
