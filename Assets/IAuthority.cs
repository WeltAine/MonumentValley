using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using DG.Tweening;
using UnityEditor;
using System;
using JetBrains.Annotations;

/// <summary>
/// 锁接口，指定了机关锁的开启，关闭，检测是否匹配等
/// </summary>
public interface ILock
{

    bool IsUnlocked { get; set; }

    void UnLock();

    void Lock();

    bool Compare(int conditionIndex);

    bool CompareAll();


}

public interface IAuthority
{
    bool IsFinished { get; set; }
    void Action();//机关的活动方法

    void CompareAll();//解锁匹配检测
}


