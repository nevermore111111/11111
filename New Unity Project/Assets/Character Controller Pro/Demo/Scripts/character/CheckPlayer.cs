using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPlayer : MonoBehaviour
{
    EnemyCharacter enemyCharacter;
    private void Awake()
    {
        enemyCharacter =transform.parent.GetComponentInChildren<EnemyCharacter>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if(enemyCharacter.selectEnemy == null)
        {
            MainCharacter mainCharacter;
            if(other.TryGetComponent<MainCharacter>(out mainCharacter))
            {
                enemyCharacter.selectEnemy = mainCharacter;
            }
        }
        
    }
}
