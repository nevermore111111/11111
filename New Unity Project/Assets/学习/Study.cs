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
            del?.Invoke(1);//这里使用?是表示检查
                           //我们在委托名称del后面添加了一个问号。这告诉编译器，在尝试调用Invoke方法之前，先检查委托是否为空引用。如果委托是空引用，则调用操作将短路，不会调用Invoke方法，并且代码将继续执行，而不会引发异常。如果委托不是空引用，则调用Invoke方法并将1作为参数传递。

        }
        void Fun02(int a)
        {

        }
        //调用委托的两种方法，一种是invoke，一种是直接像方法一样去调用
        delegate T1 Del<T,out T1>(int a,T n);

        private static void MySort<T,T1,T2>(T[] array,Func<T,T,bool> handler)
        {
            //取出元素和后面的元素比较
            for(int i = 0; i < array.Length-1; i++)
            {
                for(var j = i+1; j < array.Length; j++)
                {
                    if (handler(array[i], array[j]))//升序
                    {
                        T tem = array[i];
                        array[i] = array[j];
                        array[j] = tem;
                    }
                }
            }
        }
        /*
        总结：
        抽象类：一个概念的抽象（普通成员、抽象成员）
        接口：一组行为的抽象
        委托：一类行为的抽象
        */

        //可以被foreach的条件： 可以被数的 ，实现这个接口就可以IEnumerable
        

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