using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Rusk
{
    /// <summary>
    /// 
    /// <summary>
    public class Study:MonoBehaviour
    {
        ArrayList ArrayList;
        
        private void Start()
        {
            int aaa = 0;
            
            string bbb = "bbb";
            
            ArrayList.Add(aaa);
            ArrayList.Add(bbb);
        }
        delegate void Delga(int a);
        void Fun01()
        {
            Delga del;
            Study study = new Study();
            int random = UnityEngine. Random.Range(1, 11);
            del = random <= 5 ? study.Fun02 :study.Fun02;
            del(1);
            del?.Invoke(1);//����ʹ��?�Ǳ�ʾ���
                           //������ί������del���������һ���ʺš�����߱��������ڳ��Ե���Invoke����֮ǰ���ȼ��ί���Ƿ�Ϊ�����á����ί���ǿ����ã�����ò�������·���������Invoke���������Ҵ��뽫����ִ�У������������쳣�����ί�в��ǿ����ã������Invoke��������1��Ϊ�������ݡ�

        }
        void Fun02(int a)
        {

        }
        //����ί�е����ַ�����һ����invoke��һ����ֱ���񷽷�һ��ȥ����
        delegate T1 Del<T,out T1>(int a,T n);

        private static void MySort<T,T1,T2>(T[] array,Func<T,T,bool> handler)
        {
            //ȡ��Ԫ�غͺ����Ԫ�رȽ�
            for(int i = 0; i < array.Length-1; i++)
            {
                for(var j = i+1; j < array.Length; j++)
                {
                    if (handler(array[i], array[j]))//����
                    {
                        T tem = array[i];
                        array[i] = array[j];
                        array[j] = tem;
                    }
                }
            }
        }
        /*
        �ܽ᣺
        �����ࣺһ������ĳ�����ͨ��Ա�������Ա��
        �ӿڣ�һ����Ϊ�ĳ���
        ί�У�һ����Ϊ�ĳ���
        */

        //���Ա�foreach�������� ���Ա����� ��ʵ������ӿھͿ���IEnumerable
        

    }
    public class TestClass : IEnumerable
    {
        public IEnumerator GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
    public class TestClassIe 
    {
       
    }

}