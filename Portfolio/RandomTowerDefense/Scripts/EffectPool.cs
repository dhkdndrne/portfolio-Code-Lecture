using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectPool : MonoBehaviour
{
    public static EffectPool instance;

    //이펙트 프리팹 개수
    public GameObject[] prefabs = new GameObject[21];
    //프리팹 각각 넣어줄 큐
    Queue<GameObject> skill1 = new Queue<GameObject>();
    Queue<GameObject> skill2 = new Queue<GameObject>();
    Queue<GameObject> skill3 = new Queue<GameObject>();
    Queue<GameObject> skill4 = new Queue<GameObject>();
    Queue<GameObject> skill5 = new Queue<GameObject>();
    Queue<GameObject> skill6 = new Queue<GameObject>();
    Queue<GameObject> skill7 = new Queue<GameObject>();
    Queue<GameObject> skill8 = new Queue<GameObject>();
    Queue<GameObject> skill9 = new Queue<GameObject>();
    Queue<GameObject> skill10 = new Queue<GameObject>();
    Queue<GameObject> skill11 = new Queue<GameObject>();
    Queue<GameObject> skill12 = new Queue<GameObject>();
    Queue<GameObject> skill13 = new Queue<GameObject>();
    Queue<GameObject> skill14 = new Queue<GameObject>();
    Queue<GameObject> skill15 = new Queue<GameObject>();
    Queue<GameObject> skill16 = new Queue<GameObject>();
    Queue<GameObject> skill17 = new Queue<GameObject>();
    Queue<GameObject> skill18 = new Queue<GameObject>();
    Queue<GameObject> skill19 = new Queue<GameObject>();
    Queue<GameObject> skill20 = new Queue<GameObject>();
    Queue<GameObject> skill21 = new Queue<GameObject>();

    //큐 관리할 딕셔너리
    public Dictionary<string, Queue<GameObject>> m_dictionary = new Dictionary<string, Queue<GameObject>>();
    int count=0;
    void Start()
    {
        instance = this;
        CreateQueue();
        SetDic();
    }
    public void SetDic()
    {
        m_dictionary.Add("skill1", skill1);
        m_dictionary.Add("skill2", skill2);
        m_dictionary.Add("skill3", skill3);
        m_dictionary.Add("skill4", skill4);
        m_dictionary.Add("skill5", skill5);
        m_dictionary.Add("skill6", skill6);
        m_dictionary.Add("skill7", skill7);
        m_dictionary.Add("skill8", skill8);
        m_dictionary.Add("skill9", skill9);
        m_dictionary.Add("skill10", skill10);
        m_dictionary.Add("skill11", skill11);
        m_dictionary.Add("skill12", skill12);
        m_dictionary.Add("skill13", skill13);
        m_dictionary.Add("skill14", skill14);
        m_dictionary.Add("skill15", skill15);
        m_dictionary.Add("skill16", skill16);
        m_dictionary.Add("skill17", skill17);
        m_dictionary.Add("skill18", skill18);
        m_dictionary.Add("skill19", skill19);
        m_dictionary.Add("skill20", skill20);
        m_dictionary.Add("skill21", skill21);
    }
    public void CreateQueue()
    {
        GameObject obj;
 
        for (int i = 0; i < prefabs.Length; i++)
        {
            for (int j = 0; j < 100; j++)
            {
                
                obj = Instantiate(prefabs[i], new Vector3(-999, -999, -999), Quaternion.identity, transform.GetChild(0));
                AudioCtrl.instance.InputAudioSource(count, obj);
                switch (i)
                {
                    case 0:
                        skill1.Enqueue(obj);
                    
                        break;
                    case 1:
                        skill2.Enqueue(obj);
                       
                        break;
                    case 2:
                        skill3.Enqueue(obj);
                       
                        break;
                    case 3:
                        skill4.Enqueue(obj);
                       
                        break;
                    case 4:
                        skill5.Enqueue(obj);
                       
                        break;
                    case 5:
                        skill6.Enqueue(obj);
                        
                        break;
                    case 6:
                        skill7.Enqueue(obj);
                       
                        break;
                    case 7:
                        skill8.Enqueue(obj);
         
                        break;
                    case 8:
                        skill9.Enqueue(obj);
                     
                        break;
                    case 9:
                        skill10.Enqueue(obj);

                        break;
                    case 10:
                        skill11.Enqueue(obj);
         
                        break;
                    case 11:
                        skill12.Enqueue(obj);
                      
                        break;
                    case 12:
                        skill13.Enqueue(obj);

                        break;
                    case 13:
                        skill14.Enqueue(obj);
                        
                        break;
                    case 14:
                        skill15.Enqueue(obj);
               
                        break;
                    case 15:
                        skill16.Enqueue(obj);
                   
                        break;
                    case 16:
                        skill17.Enqueue(obj);
                  
                        break;
                    case 17:
                        skill18.Enqueue(obj);
                       
                        break;
                    case 18:
                        skill19.Enqueue(obj);
                     
                        break;
                    case 19:
                        skill20.Enqueue(obj);
               
                        break;
                    case 20:
                        skill21.Enqueue(obj);
 
                        break;

                }

                obj.SetActive(false);
                count++;
            }
        }
    }
    //큐에 넣어주는함수
    public void InsertQueue(string key, GameObject _obj)
    {
        m_dictionary[key].Enqueue(_obj);
        _obj.SetActive(false);
    }
    //큐에서 객체를 빌리는 함수
    public GameObject GetQueue(string key)
    {
        GameObject t_object = m_dictionary[key].Dequeue();
        t_object.SetActive(true);
        return t_object;
    }
    public IEnumerator DeleteEffect(string _key,float _effTimer,GameObject _obj)
    {
        yield return new WaitForSeconds(_effTimer);
        InsertQueue(_key, _obj);
      
    }
}
