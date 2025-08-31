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

        //按下鼠标左键（0），右键（1），中间键（2）//那滑动滚轮呢？
        if (Input.GetMouseButtonDown(0))
        {
            //基于鼠标设置投射视线，检测是碰到了那个地块（目的地块）
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


                //(我在说什么啊)这里和我常用的写法不一样，我虽然也会解耦，但是我会将ExplorePath()放到FindPath()里，函数套函数，这又增加了关联性，这种完全拆开的写法或许确实更好
                FindPath();
                //ExplorePath();
                if (this.finalPath.Any())//确保找到路径
                {
                    //this.BuildPath();
                    this.walking = true;

                    Walk();
                    //Sequence s = 
                }

            }
            else if (hit.transform.gameObject.layer == LayerMask.NameToLayer("RaycastButton"))//Button
            {
                //hit.transform.gameObject.GetComponent<Button>().aim.Rotate(Vector3.up, -90);
                //视线投射不会触发trigger？，但能检测到trigger
                //已确认，没触发ontrigger和oncollider，绑上刚体也不行//检测的设计本省就不是为了触发那些事件，只是为了查明可能会碰到什么，而不是触发意料之外的行为
                if (hit.transform.GetComponent<Button>().isFinished)
                {
                    hit.transform.GetComponent<Button>().isFinished = false;
                    //hit.transform.GetComponent<Button>().aim.DOLocalRotate(new Vector3(0, -90.0f, 0), 0.5f, RotateMode.LocalAxisAdd)
                    //Vector3 rotato = new Vector3(0.0f, hit.transform.rotation.eulerAngles.y, 0.0f);
                    //hit.transform.GetComponent<Button>().aim.DORotateQuaternion( Quaternion.Euler( rotato + new Vector3(0, -90.0f, 0)), 0.5f)
                    //旧方法可能是因为万向锁的缘故，当旋转y时可能瞬间翻转
                    hit.transform.GetComponent<Button>().aim.DORotate( new Vector3(-90, 0, 0), 0.5f, RotateMode.LocalAxisAdd)
                        .OnComplete(() => { hit.transform.GetComponent<Button>().isFinished = true;
                            Debug.Log(hit.transform.rotation);
                        });
                }
            }
        }
    }


    private void FindPath()
    {
        List<Transform> nextPath = new List<Transform>();//准备探索队列（只是初步的）
        List<Transform> pastPath = new List<Transform>();//准备标记队列（不需要再探索的，也就是已经探索过的）
        //整体寻路思路是这样的，准备两个队列，一个记录之后可以走的，一个记录已经走过的，探索队列，没取出一个就相当于踩上该点，则该店进入标记队列，它带来的可能形追加到探索队列。就这样不断试探路线。
        //为了能够回溯路线，还需要记录前驱，也就是Walkable中的proPath

        
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
        nextPath.Remove(current);//记得移除，因为要移入pastPath

        pastPath.Add(current);

        if(current == clickCube)
        {
            return;//放心，clickCube的前驱在clickCube放入nextPath中时就已经确定好了
        }


        foreach(WalkPath _possiblePath in current.GetComponent<Walkable>().possiblePath)
        {
            if ((!pastPath.Contains(_possiblePath.target)) && _possiblePath.active)//没有走过，且是可踏过的
            {
                nextPath.Add(_possiblePath.target);
                _possiblePath.target.GetComponent <Walkable>().proPath = current;//记得设置前驱
            }
        }


        if (nextPath.Any())//nextPath可能没有新的路了
        {
            ExplorePath(nextPath, pastPath);
        }
        else
        {
            
        }
    }
    //学了不少List函数，First(),Add(),Remove(),Any(),Countains(),Claer() 


    private void BuildPath()
    {
        for(Transform aim = clickCube; aim != currentCube; aim = aim.GetComponent<Walkable>().proPath)
        {
            finalPath.Add(aim);//形成路径倒置
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


        //这里这里
        s.AppendCallback(() => this.Claer());//看不懂，为什么返回一个函数指针，序列的最后一个操作设置为清理List
    }


    //获取玩家脚下地块
    private void GetCurrentCube()
    {
        Ray inspectRay = new Ray(this.transform.position, Vector3.down);

        RaycastHit hit;
        //有两种Raycast，一种是Physics提供的，一种是Collider提供的，前者可以选择mask来检测特定层的碰撞（会忽略自身的碰撞体, 射线检测无法探测到射线起点位于碰撞体内的碰撞器），后者仅检测射线是否与该Collider相交，总得来说两者及他们的重载都是通过，起点，方向，长度（前者有默认长度无限大），可以选用RayCastHit（来捕获碰撞信息）这些要素构成的
        //this.GetComponent<MeshCollider>().Raycast(inspectRay,out hit, 2.0f);
        Physics.Raycast(inspectRay, out hit, 2.0f);

        if(hit.transform.GetComponent<Walkable>() != null)
        {
            currentCube = hit.transform;//获取当前所站块
        }

    }



}
