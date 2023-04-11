using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "New Effect Manager", menuName = "Custom/Effect Manager")]
public class EffectManagerData : ScriptableObject
{
    public List<Transform> effects;
    public List<string> effectNames;
    public List<GameObject> effectsPrefab;
}
