using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventTest : MonoBehaviour
{
    //很多程序都有一个需求，就是当一个特定的事件发生时，程序的其他部分可以得到该事件已经发生的通知。
    //发布者订阅者模式可以满足这种需求。其他类可以注册，以便在这些事件发生时受到发布者的通知。这些订阅者类通过向发布者提供一个方法来注册，并且获取通知。当事情发生时，发布者通过触发事件，然后执行订阅者提交的所有事件。

    //发布者：发布某个事件的类或者结构，其他类可以在该事件发生时得到通知
    //订阅者：注册，并且在事件触发时得到通知的类或者结构
    //事件处理程序：由注册者注册到事件的方法，在发布者触发事件时执行。

    //事件处理程序的方法可以定义在事件所在的类或者结构中，也可以定义在不同的类或者结构中

    //触发事件：调用invoke或者触发fire事件的术语，事件被触发时，所有注册到它的方法都会被依次调用。



    //事件声明在一个类中，声明：
    //创建事件比较简单，只需要委托类型和名称
    public delegate void Action01();
    Action Action;
    public event Action01 Action1;
    //创建委托事件不需要new，并且需要增家关键字event和对应的委托

    public event Action01 Action2, Action3;//可以一次声明多个委托
    //可以使用static让事件变为静态的
    //事件是一个成员，所以不能在一段可执行代码中声明一个事件
    //2添加事件，订阅者向事件添加事件处理程序。
    //3触发事件


}
