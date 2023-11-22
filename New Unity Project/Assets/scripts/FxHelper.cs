using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FxHelper", menuName = "FxHelper")]
public class FxHelper : ScriptableObject
{
    public ParticleSystem[] AllFx;
    public ParticleSystem[] weaponHittedFX;
}
