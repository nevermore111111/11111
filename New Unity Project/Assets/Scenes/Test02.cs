using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
//[System.Serializable] ��һ�� C# ���ԣ������Ա����ڱ��һ���࣬����������ʵ�����Ա����л������л��ǽ�����ת���ɿɴ洢��ɴ���ĸ�ʽ�Ĺ��̡�ͨ�����������л���������Ӧ�ó��������ɵر���ͻָ������״̬�����߽������һ��Ӧ�ó����䵽��һ��Ӧ�ó���
//�������Ա����л������û�̳�monobehavior�Ļ�������ʹ��������������������ֶλ�������չʾ����������̳���monobehavior����ô���Զ������������
//��һ���౻���Ϊ [System.Serializable] �������еĹ����ֶκ����Զ�����Ĭ�����л��������Ҫ��ĳЩ�ֶ��ų������л�֮�⣬����ʹ�� [NonSerialized] �����������Щ�ֶΡ����⣬������ʵ�� ISerializable �ӿ����Զ������л��ͷ����л����̡�

//��Ҫע����ǣ�������ʽ�ؽ��ã��������� Unity �е� MonoBehaviour ���඼�ᱻ�Զ����Ϊ [System.Serializable]��������Ϊ Unity �����л�������Ԥ�Ƽ�ʱ��Ҫʹ�����л�������ͻָ������״̬��
public class Test02 
{
    public int Speed;
    public int MaxSpeed;
    public int MinSpeed;
    public float Ac;
}
