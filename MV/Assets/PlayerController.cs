using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DG.Tweening;

public class PlayerController : MonoBehaviour
{
    //��¼��ǰ���ڷ���
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
        transform.parent = currentCube;//�̶�λ�ã����������Ҵ�����תƽ̨�ϣ���ƽ̨��תʱ����ҽ�һ�������ת�����Ǳ������ڿ���

        //������������0�����Ҽ���1�����м����2��//�ǻ��������أ�
        if (Input.GetMouseButtonDown(0))
        {
            //�����������Ͷ�����ߣ�������������Ǹ��ؿ飨Ŀ�ĵؿ飩
            //ScreenPointToRay�ǽ���Ļ�ϵĵ㣨������Ϊ��λ����Ļ���½�Ϊ0��0����������һ��Vector3���ú���������zֵ
            Ray clickInspect =  Camera.main.ScreenPointToRay(Input.mousePosition);//mousePositon��Vector3����Ϊ�����ǻ�����Ļ��(����ָ����mousePosition����������������Ϊ����)��Ҳ��������Ϊ��λ...��������zֵĬ����0
            //��ScreenPointToRay���Ƶ���ViewPointToRay������˵����һ�£������Ǻ��ߵĲ���ʹ�õ��ǹ�һ�����꣬��Ļ���½�����(0,0)������Ļ���Ͻǲ���ȡ����������������(1,1)
            RaycastHit hit;
            Physics.Raycast(clickInspect, out hit, Mathf.Infinity, ~0, QueryTriggerInteraction.Collide);

            if (hit.transform.gameObject.GetComponent<Walkable>() != null)
            {
                finalPath.Clear();//���֮ǰ��·��

                //this.GetCurrentCube();
                clickCube = hit.transform;


                //(����˵ʲô��)������ҳ��õ�д����һ��������ȻҲ���������һὫExplorePath()�ŵ�FindPath()������׺��������������˹����ԣ�������ȫ�𿪵�д������ȷʵ����
                FindPath();
                //ExplorePath();
                if (this.finalPath.Any())//ȷ���ҵ�·��
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
                //����Ͷ�䲻�ᴥ��trigger�������ܼ�⵽trigger
                //��ȷ�ϣ�û����ontrigger��oncollider�����ϸ���Ҳ����//������Ʊ�ʡ�Ͳ���Ϊ�˴�����Щ�¼���ֻ��Ϊ�˲������ܻ�����ʲô�������Ǵ�������֮�����Ϊ
                if (hit.transform.GetComponent<Button>().isFinished)
                {
                    hit.transform.GetComponent<Button>().isFinished = false;
                    //hit.transform.GetComponent<Button>().aim.DOLocalRotate(new Vector3(0, -90.0f, 0), 0.5f, RotateMode.LocalAxisAdd)
                    //Vector3 rotato = new Vector3(0.0f, hit.transform.rotation.eulerAngles.y, 0.0f);
                    //hit.transform.GetComponent<Button>().aim.DORotateQuaternion( Quaternion.Euler( rotato + new Vector3(0, -90.0f, 0)), 0.5f)
                    //�ɷ�����������Ϊ��������Ե�ʣ�����תyʱ����˲�䷭ת
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
        List<Transform> nextPath = new List<Transform>();//׼��̽�����У�ֻ�ǳ����ģ�
        List<Transform> pastPath = new List<Transform>();//׼����Ƕ��У�����Ҫ��̽���ģ�Ҳ�����Ѿ�̽�����ģ�
        //����Ѱ·˼·�������ģ�׼���������У�һ����¼֮������ߵģ�һ����¼�Ѿ��߹��ģ�̽�����У�ûȡ��һ�����൱�ڲ��ϸõ㣬��õ�����Ƕ��У��������Ŀ�����׷�ӵ�̽�����С�������������̽·�ߡ�
        //Ϊ���ܹ�����·�ߣ�����Ҫ��¼ǰ����Ҳ����Walkable�е�proPath

        
        //��ʼ��nextPath��pastPath
        pastPath.Add(currentCube);
        foreach(WalkPath next in currentCube.GetComponent<Walkable>().possiblePath)
        {
            if(!(next.active == false))
            {
                nextPath.Add(next.target);
                next.target.GetComponent<Walkable>().proPath = currentCube;
                //��һ�μ�¼ǰ��
            }
            
        }

        ExplorePath(nextPath, pastPath);
        BuildPath();

    }

    private void ExplorePath(List<Transform> nextPath, List<Transform> pastPath)
    {
        //��ȡ�µ�̽����
        Transform current = nextPath.First();
        nextPath.Remove(current);//�ǵ��Ƴ�����ΪҪ����pastPath

        pastPath.Add(current);

        if(current == clickCube)
        {
            return;//���ģ�clickCube��ǰ����clickCube����nextPath��ʱ���Ѿ�ȷ������
        }


        foreach(WalkPath _possiblePath in current.GetComponent<Walkable>().possiblePath)
        {
            if ((!pastPath.Contains(_possiblePath.target)) && _possiblePath.active)//û���߹������ǿ�̤����
            {
                nextPath.Add(_possiblePath.target);
                _possiblePath.target.GetComponent <Walkable>().proPath = current;//�ǵ�����ǰ��
            }
        }


        if (nextPath.Any())//nextPath����û���µ�·��
        {
            ExplorePath(nextPath, pastPath);
        }
        else
        {
            
        }
    }
    //ѧ�˲���List������First(),Add(),Remove(),Any(),Countains(),Claer() 


    private void BuildPath()
    {
        for(Transform aim = clickCube; aim != currentCube; aim = aim.GetComponent<Walkable>().proPath)
        {
            finalPath.Add(aim);//�γ�·������
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
                s.Join(this.transform.DOLookAt(finalPath[i].position, 1.5f, AxisConstraint.Y, Vector3.up));//����
            }
        }


        //��������
        s.AppendCallback(() => this.Claer());//��������Ϊʲô����һ������ָ�룬���е����һ����������Ϊ����List
    }


    //��ȡ��ҽ��µؿ�
    private void GetCurrentCube()
    {
        Ray inspectRay = new Ray(this.transform.position, Vector3.down);

        RaycastHit hit;
        //������Raycast��һ����Physics�ṩ�ģ�һ����Collider�ṩ�ģ�ǰ�߿���ѡ��mask������ض������ײ��������������ײ��, ���߼���޷�̽�⵽�������λ����ײ���ڵ���ײ���������߽���������Ƿ����Collider�ཻ���ܵ���˵���߼����ǵ����ض���ͨ������㣬���򣬳��ȣ�ǰ����Ĭ�ϳ������޴󣩣�����ѡ��RayCastHit����������ײ��Ϣ����ЩҪ�ع��ɵ�
        //this.GetComponent<MeshCollider>().Raycast(inspectRay,out hit, 2.0f);
        Physics.Raycast(inspectRay, out hit, 2.0f);

        if(hit.transform.GetComponent<Walkable>() != null)
        {
            currentCube = hit.transform;//��ȡ��ǰ��վ��
        }

    }



}
