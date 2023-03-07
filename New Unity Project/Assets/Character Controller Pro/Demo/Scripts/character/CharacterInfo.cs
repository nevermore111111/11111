using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInfo : MonoBehaviour
{
    public List<GameObject> enemys;
    public GameObject selectEnemy;
    protected virtual void  Awake()
    {
       enemys = new List<GameObject>();
    }
}
