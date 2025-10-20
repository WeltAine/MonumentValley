using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DG.Tweening;

public class PlayerController : MonoBehaviour
{
    //记录当前所在方块
    public Transform currentCube;
    public Transform clickCube;

    public List<Transform> finalPath = new List<Transform>();


    public bool walking = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        this.GetCurrentCube();
        transform.parent = currentCube;//固定位置，这样如果玩家处于旋转平台上，且平台旋转时，玩家将一起跟着旋转，而是被遗留在空中

        //按下鼠标左键（0），右键（1），中间键（2）
        if (Input.GetMouseButtonDown(0))
        {
            //基于鼠标设置投射视线，检测是碰到了哪个地块（目的地块）
            //ScreenPointToRay是将屏幕上的点（以像素为单位，屏幕左下脚为0，0），参数是一个Vector3，该函数会无视z值
            Ray clickInspect =  Camera.main.ScreenPointToRay(Input.mousePosition);//mousePositon是Vector3，因为坐标是基于屏幕的(这里指的是mousePosition的坐标是以像素作为表达的)（也是以像素为单位...），所以z值默认是0
            //与ScreenPointToRay相似的是ViewPointToRay，可以说几乎一致，区别是后者的参数使用的是归一化坐标，屏幕左下角仍是(0,0)，但屏幕右上角不再取决于像素数，而是(1,1)
            RaycastHit hit;
            Physics.Raycast(clickInspect, out hit, Mathf.Infinity, ~0, QueryTriggerInteraction.Collide);

            if (hit.transform.gameObject.GetComponent<Walkable>() != null)
            {
                finalPath.Clear();//清除之前的路径

                //this.GetCurrentCube();
                clickCube = hit.transform;


                FindPath();
            
                if (this.finalPath.Any())//确保找到路径
                {
                    this.walking = true;

                    Walk();
                }

            }
            else if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Manual"))//Button
            {
                if (hit.transform.GetComponent<Button>().Authority.IsFinished)
                {
                    {
                        //hit.transform.GetComponent<Button>().isFinished = false;
                        ////hit.transform.GetComponent<Button>().aim.DOLocalRotate(new Vector3(0, -90.0f, 0), 0.5f, RotateMode.LocalAxisAdd)
                        ////Vector3 rotato = new Vector3(0.0f, hit.transform.rotation.eulerAngles.y, 0.0f);
                        ////hit.transform.GetComponent<Button>().aim.DORotateQuaternion( Quaternion.Euler( rotato + new Vector3(0, -90.0f, 0)), 0.5f)
                        ////旧方法可能是因为万向锁的缘故，当旋转y时可能瞬间翻转
                        //hit.transform.GetComponent<Button>().aim.DORotate(new Vector3(-90, 0, 0), 0.5f, RotateMode.LocalAxisAdd)
                        //    .OnComplete(() =>
                        //    {
                        //        hit.transform.GetComponent<Button>().isFinished = true;
                        //        Debug.Log(hit.transform.rotation);
                        //    });
                    }

                    hit.transform.GetComponent<Button>().OnClick();
                }
            }
        }
    }


    //广度优先算法
    private void FindPath()
    {
        List<Transform> nextPath = new List<Transform>();
        List<Transform> pastPath = new List<Transform>();

        
        //初始化nextPath和pastPath
        pastPath.Add(currentCube);
        foreach(WalkPath next in currentCube.GetComponent<Walkable>().possiblePath)
        {
            if(!(next.active == false))
            {
                nextPath.Add(next.target);
                next.target.GetComponent<Walkable>().proPath = currentCube;
                //第一次记录前驱
            }
            
        }

        ExplorePath(nextPath, pastPath);
        BuildPath();

    }

    private void ExplorePath(List<Transform> nextPath, List<Transform> pastPath)
    {
        //获取新的探索点
        Transform current = nextPath.First();
        nextPath.Remove(current);

        pastPath.Add(current);

        if(current == clickCube)
        {
            return;
        }


        foreach(WalkPath _possiblePath in current.GetComponent<Walkable>().possiblePath)
        {
            if ((!pastPath.Contains(_possiblePath.target)) && _possiblePath.active)//没有走过，且是可踏过的
            {
                nextPath.Add(_possiblePath.target);
                _possiblePath.target.GetComponent <Walkable>().proPath = current;//设置前驱
            }
        }


        if (nextPath.Any())
        {
            ExplorePath(nextPath, pastPath);
        }
        else
        {
            
        }
    }


    private void BuildPath()
    {

        if (!clickCube.GetComponent<Walkable>().proPath)
        {
            Debug.Log("无路径");
            return;
        }

        for(Transform aim = clickCube; aim != currentCube; aim = aim.GetComponent<Walkable>().proPath)
        {
            finalPath.Add(aim);//形成倒序路径
        }
    }

    private void Claer()
    {
        foreach (Transform aim in finalPath)
        {
            aim.GetComponent<Walkable>().proPath = null;
        }

        finalPath.Clear();

        this.walking = false;
    }

    private void Walk()
    {
        Sequence s = DOTween.Sequence();

        for(int i = finalPath.Count() - 1; i >= 0; i--)
        {
            s.Append(this.transform.DOMove(finalPath[i].GetComponent<Walkable>().GetGizmosPositon() + 0.5f * Vector3.up, 0.2f).SetEase(Ease.Linear));

            if (finalPath[i].GetComponent<Walkable>().isRote)
            {
                s.Join(this.transform.DOLookAt(finalPath[i].position, 1.5f, AxisConstraint.Y, Vector3.up));//并行
            }
        }

        //清理收尾
        s.AppendCallback(() => this.Claer());
    }


    //获取玩家脚下地块
    private void GetCurrentCube()
    {
        Ray inspectRay = new Ray(this.transform.position, Vector3.down);

        RaycastHit hit;
        Physics.Raycast(inspectRay, out hit, 2.0f, ~0, QueryTriggerInteraction.Ignore);

        if(hit.transform != null && hit.transform.GetComponent<Walkable>() != null)
        {
            currentCube = hit.transform;//获取当前所站块
        }

    }



}
