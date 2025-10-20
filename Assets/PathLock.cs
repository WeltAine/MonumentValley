using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class PathLock : MonoBehaviour, ILock
{

    [Header("预期情况与观测目标")]
    //预期情况，与目标当前情况
    public List<Transform> expectCondstions;
    public List<Transform> observationTarget;

    [Header("链接")]
    /// <summary>
    /// 使用时元素数量应当为2的倍数
    /// </summary>
    public List<Walkable> linkPair;


    public bool IsUnlocked { get => isUnlocked; set => isUnlocked = value; }
    private bool isUnlocked = false;


    //通过比较位置与旋转的方式来检测是否匹配
    public bool Compare(int conditionIndex)
    {
        return 
            observationTarget[conditionIndex].position == expectCondstions[conditionIndex].position 
            && 
            observationTarget[conditionIndex].rotation.eulerAngles == expectCondstions[conditionIndex].rotation.eulerAngles;
    }

    public bool CompareAll()
    {
        for (int x = 0; x < observationTarget.Count; x++)
        {
            if (this.Compare(x))
            {
                continue;
            }

            this.Lock();
            return false;
        }

        this.UnLock();
        return true;
    }

    public void Lock()
    {
        for (int x = 0; x < linkPair.Count; x += 2)
            Walkable.Disrupt(linkPair[x], linkPair[x + 1]);
    }

    public void UnLock()
    {
        for(int x = 0; x < linkPair.Count; x += 2)
            Walkable.LinkAim(linkPair[x], linkPair[x+1]);
    }
}
