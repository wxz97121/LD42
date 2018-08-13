using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
public class Controller : MonoBehaviour
{
    public GameObject BoxNPCPrefab;
    public GameObject CatNPCPrefab;
    public StationInfo[] Station;
    public List<NPC> nowNPC;
    public GameObject LDoor, RDoor;
    public static float minX = -97, maxX = 82;
    public Text UIText;
    public TransformForce m_Character;
    [SerializeField] private float bpm;
    [System.Serializable]
    public struct StationInfo
    {
        //public bool isDown;
        public float TimeToThisStation;
        public float StationTime;
        [Range(0, 1)]
        public float RatioOfExit;
    }
    private void Awake()
    {
        m_Character = FindObjectOfType<TransformForce>();
        //nowNPC = new List<NPC>();
    }
    void Start()
    {
        Random.InitState(System.DateTime.Now.Second);
        StartCoroutine(StationTimer());
        StartCoroutine(Beat());
    }
    void ExitTrain(StationInfo st)
    {
        //随机找一堆东西下车。
        //将来可以根据关卡在这里做特殊逻辑
        int Start = (int)Mathf.Round(Random.Range(0, nowNPC.Count - 0.1f));
        for (int j = 0; j < nowNPC.Count * st.RatioOfExit; j++)
        {
            nowNPC[(j + Start) % nowNPC.Count].ExitFromTrain();
        }
    }
    IEnumerator SpawnNPC(int index)
    {
        //现在还没想好，但是可能得一关一关手写
        if (index == 0)
        {
            for (int i = 0; i < 15; i++)
            {
                int Dir = Random.value > 0.5 ? 1 : -1;
                nowNPC.Add(Instantiate(BoxNPCPrefab,
                    new Vector3(Random.Range(-70, 70), Random.Range(20, 30), 0), Quaternion.identity).GetComponent<NPC>());
                yield return new WaitForSeconds(Random.Range(0.25f, 0.5f));
            }
        }
        else if (index == 1)
        {
            for (int i = 0; i < 15; i++)
            {
                int Dir = Random.value > 0.5 ? 1 : -1;
                nowNPC.Add(Instantiate(BoxNPCPrefab,
                    new Vector3(Random.Range(-70, 70), Random.Range(20, 30), 0), Quaternion.identity).GetComponent<NPC>());
                yield return new WaitForSeconds(Random.Range(0.25f, 0.5f));
            }
        }
        else
        {
            for (int i = 0; i < 15; i++)
            {
                int Dir = Random.value > 0.5 ? 1 : -1;
                var GO = Random.value > 0.5 ? BoxNPCPrefab : CatNPCPrefab;
                nowNPC.Add(Instantiate(BoxNPCPrefab,
                    new Vector3(Random.Range(-70, 70), Random.Range(20, 30), 0), Quaternion.identity).GetComponent<NPC>());
                yield return new WaitForSeconds(Random.Range(0.25f, 0.5f));
            }
        }

    }
    void CleanNPC()
    {
        foreach (var m_NPC in nowNPC)
        {
            if (m_NPC.transform.position.x < minX || m_NPC.transform.position.x > maxX)

                Destroy(m_NPC.gameObject);

        }
        var tmpNPC = new List<NPC>();
        foreach (var m_NPC in nowNPC)
            if (!m_NPC || !m_NPC.alive) tmpNPC.Add(m_NPC);
        nowNPC = tmpNPC;
    }
    IEnumerator StationTimer()
    {
        for (int i = 0; i < Station.Length; i++)
        {
            var st = Station[i];
            yield return new WaitForSeconds(st.TimeToThisStation);
            //TODO：放地铁停止的动画和音效
            UIText.text = "地铁到站";
            LDoor.transform.DOMoveY(100, 0.5f);
            RDoor.transform.DOMoveY(100, 0.5f);
            ExitTrain(st);
            StartCoroutine(SpawnNPC(i));
            yield return new WaitForSeconds(st.StationTime);
            LDoor.transform.DOMoveY(-8.4f, 0.5f);
            RDoor.transform.DOMoveY(-8.4f, 0.5f);
            //死亡判定
            if (m_Character.transform.position.x < minX || m_Character.transform.position.y > maxX)
                StartCoroutine(Lose());
            //TODO：播放地铁开始运动的动画和音效
            UIText.text = "地铁正在前行";

        }
        StartCoroutine(Win());
    }
    public IEnumerator Lose()
    {
        UIText.text = "You Lose";
        Time.timeScale = 0.1f;
        StopAllCoroutines();
        yield break;
        //当主角被挤下去的时候调用这玩意
    }

    IEnumerator Win()
    {
        UIText.text = "You Win";
        //tmpString = "You Win";
        //tmpGUI = true;
        yield break;
        //条漫！
    }
    string tmpString = "地铁正在前行";
    bool tmpGUI = true;
    private void OnGUI()
    {
        //if (tmpGUI)
        //    GUI.Label(new Rect(50, 10, 1000, 800), tmpString);
    }

    IEnumerator Beat (){
        while (true){
            foreach(NPC npcinst in nowNPC){
                npcinst.BeatBehavior();
            }
            yield return new WaitForSeconds(60f / bpm);
        }
    }
}
