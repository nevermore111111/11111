using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public abstract class CharacterInfo : MonoBehaviour
{
    //��Χȫ���ĵ���
    
    [Tooltip("���������λ�ĵ���tag")]
    public string enemyTag;
    [Tooltip("���������λ�ĵ����б�")]
    public List<CharacterInfo> enemies;
    [Tooltip("�����λ��ѡ��Ŀ��")]
    public CharacterInfo selectEnemy;
    [Tooltip("����������е���ײ���")]
    public SphereCollider characterSphere;
    protected virtual void  Awake()
    {
        enemies = new List<CharacterInfo>();
        characterSphere = GetComponent<SphereCollider>();
        characterSphere.isTrigger = true;
    }
    
}
