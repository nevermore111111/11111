using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class EffectManager : MonoBehaviour
{
    public EffectManagerData effectManagerData;
    public string Path;
    void Start()
    {
        Path = "Assets/Character Controller Pro/Core/Scripts/Character/����/Effect Manager_Rusk.asset";
        {
            Path.Replace("Rusk",this.gameObject.name);
            effectManagerData = AssetDatabase.LoadMainAssetAtPath(Path) as EffectManagerData;
        }
        // ���ʱ��������
        List<GameObject> effect = effectManagerData.effectsPrefab;
        List<string> effectList = effectManagerData.effectNames;
        List<Transform> effectListList2 = effectManagerData.effects;
    }
}
