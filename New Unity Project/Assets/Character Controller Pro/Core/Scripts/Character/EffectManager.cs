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
        Path = "Assets/Character Controller Pro/Core/Scripts/Character/杂项/Effect Manager_Rusk.asset";
        {
            Path.Replace("Rusk",this.gameObject.name);
            effectManagerData = AssetDatabase.LoadMainAssetAtPath(Path) as EffectManagerData;
        }
        // 访问保存的数据
        List<GameObject> effect = effectManagerData.effectsPrefab;
        List<string> effectList = effectManagerData.effectNames;
        List<Transform> effectListList2 = effectManagerData.effects;
    }
}
