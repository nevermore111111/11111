using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
//[System.Serializable] 是一个 C# 属性，它可以被用于标记一个类，表明这个类的实例可以被序列化。序列化是将对象转换成可存储或可传输的格式的过程。通过将对象序列化，可以在应用程序中轻松地保存和恢复对象的状态，或者将对象从一个应用程序传输到另一个应用程序。
//这个类可以被序列化，如果没继承monobehavior的话，可以使用这个属性来把这个类的字段或者属性展示出来。如果继承了monobehavior，那么会自动附带这个属性
//当一个类被标记为 [System.Serializable] 后，其所有的公共字段和属性都将被默认序列化。如果需要将某些字段排除在序列化之外，可以使用 [NonSerialized] 属性来标记这些字段。此外，还可以实现 ISerializable 接口来自定义序列化和反序列化过程。

//需要注意的是，除非显式地禁用，否则所有 Unity 中的 MonoBehaviour 子类都会被自动标记为 [System.Serializable]。这是因为 Unity 在序列化场景和预制件时需要使用序列化来保存和恢复对象的状态。
public class Test02 
{
    public int Speed;
    public int MaxSpeed;
    public int MinSpeed;
    public float Ac;
}
