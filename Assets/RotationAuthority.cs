using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Unity.VisualScripting;
using System.Linq;


[System.Serializable]
public class RotationAuthority : MonoBehaviour, IAuthority
{
    public bool isManual = true;//是鼠标交互的机关，还是角色触发的机关

    [Header("机关锁")]
    [SerializeField] private List<MonoBehaviour> Locks;
    public List<ILock> locks
    {
        get
        {
            return Locks.Where(obj => obj is ILock)
                .Select(obj => obj as ILock)
                .ToList();
        }
    }


    [Header("控制对象")]
    //操纵的目标
    public Transform aim;

    [Header("参数设置")]
    public Vector3 aix = new Vector3(1, 0, 0);
    public float singleAngle = 90;
    public float duration = 0.5f;

    public bool isFinished = true;

    public bool IsFinished { get => isFinished; set => isFinished = value; }
    public bool IsUnlocked { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

    public void Action()
    {
        //aim.RotateAround(aim.transform.position, aix, singleAngle);
        if (isFinished)
        {
            isFinished = false;
            aim.DORotate(singleAngle * aix, duration, RotateMode.LocalAxisAdd)
                .OnComplete(() => { this.isFinished = true;
                    this.CompareAll();
                });
        }

    }


    /// <summary>
    /// 检测并自动设置机关的激活常态
    /// </summary>
    public void CompareAll()
    {
        for (int x = 0; x < this.Locks.Count; x++)
        {
            locks[x].CompareAll();
        }
    }

}
