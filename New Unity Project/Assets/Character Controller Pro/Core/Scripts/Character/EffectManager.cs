using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class EffectManager : MonoBehaviour
{
    [Tooltip("�����asset����path���棬���ű��￴path")]
    public EffectManagerData effectManagerData;
    private string Path;
    void Start()
    {
        Path = "Assets/Character Controller Pro/Core/Scripts/Character/����/Effect Manager_Rusk.asset";
        {
            Path.Replace("Rusk",this.gameObject.name);
            effectManagerData = Resources.Load<EffectManagerData>("Effect Manager_Rusk");
        }
        // ���ʱ��������
        List<GameObject> effect = effectManagerData.effectsPrefab;
        List<string> effectList = effectManagerData.effectNames;
        List<Transform> effectListList2 = effectManagerData.effects;
    }
}
