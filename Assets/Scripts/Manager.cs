using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoSingleton<Manager>
{
    public MusicConfig MusicConfig;

    public List<ObjectBase> Object;

    public List<ObjectBase> TriggerObject;

    public override void Awake()
    {
        base.Awake();
        Object = new List<ObjectBase>();
        TriggerObject = new List<ObjectBase>();
    }
    //将作祟物体加入Object管理
    void AddObject(ObjectBase go)
    {
        this.Object.Add(go);
    }
}
