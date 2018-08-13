using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class Controller : MonoBehaviour
{
    public GameObject BoxNPCPrefab;
    public GameObject CatNPCPrefab;
    public GameObject[] NPCPrefabs;
    public StationInfo[] Station;
    public List<NPC> nowNPC;
    public GameObject LDoor, RDoor;
    public static float minX, maxX;
    public Text UIText;
    public TransformForce m_Character;
    private TrainMove m_TrainMove;
    public Image EndCG;
    public Sprite End2, End3;
    [SerializeField] private float bpm;
    [System.Serializable]
    public struct StationInfo
    {
        //public bool isDown;
        public float TimeToThisStation;
        public float StationTime;
        [Range(0, 1)]
        public float RatioOfExit;
        public int PeopleGetOn;
    }
    private void Awake()
    {
        minX = LDoor.transform.position.x;
        maxX = RDoor.transform.position.x;
        m_Character = FindObjectOfType<TransformForce>();
        m_TrainMove = FindObjectOfType<TrainMove>();
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
    IEnumerator SpawnNPC(StationInfo st)
    {

        for (int i = 0; i < st.PeopleGetOn; i++)
        {
            int Dir = Random.value > 0.5 ? 1 : -1;
            var GO = Instantiate(CatNPCPrefab, new Vector3(Dir * 160, -60, 0), Quaternion.identity);
            GO.GetComponent<CatNPC>().Dir = -Dir;
            GO.GetComponent<CatNPC>().m_Character = m_Character;
            GO.transform.localScale = Random.Range(1f, 1.75f) * Vector3.one;
            //if (GO.GetComponent<JellySprite>())
            //{
            //    GO.GetComponent<JellySprite>().AddForce(new Vector2(-Dir, 1) * 10000000);
            //    print("???")
            //}
            nowNPC.Add(GO.GetComponent<NPC>());
            yield return new WaitForSeconds(Random.Range(0.25f, 0.75f));
        }
    }



    void CleanNPC()
    {
        foreach (var m_NPC in nowNPC)
        {
            if (m_NPC && m_NPC.transform.position.x < minX || m_NPC.transform.position.x > maxX || m_NPC.transform.position.y < -70)

                DestroyImmediate(m_NPC.gameObject);

        }
        var tmpNPC = new List<NPC>();
        foreach (var m_NPC in nowNPC)
            if (m_NPC) tmpNPC.Add(m_NPC);
        nowNPC = tmpNPC;
    }
    IEnumerator StationTimer()
    {
        for (int i = 0; i < Station.Length; i++)
        {
            var st = Station[i];
            yield return new WaitForSeconds(st.TimeToThisStation);
            //TODO：放地铁停止的动画和音效
            DOTween.To(x => m_TrainMove.MoveSpeed = x, 150, 0, 1);
            Camera.main.DOShakePosition(1, 1.5f, 5);
            UIText.text = "地铁到站";
            //如果是最后一站
            if (i + 1 == Station.Length)
            {
                UIText.text = "该下车啦！！快想办法下车！！";
                UIText.rectTransform.DOShakePosition(st.StationTime);
            }
            LDoor.transform.DOMoveY(100, 0.5f);
            RDoor.transform.DOMoveY(100, 0.5f);
            ExitTrain(st);
            StartCoroutine(SpawnNPC(st));
            yield return new WaitForSeconds(st.StationTime);
            LDoor.transform.DOMoveY(4, 0.5f);
            RDoor.transform.DOMoveY(4, 0.5f);
            //死亡判定
            if (m_Character.transform.position.x < minX || m_Character.transform.position.x > maxX || m_Character.transform.position.y < -70)
            {
                if (i + 1 != Station.Length) StartCoroutine(Lose("游戏失败，你中途被挤下了车"));
                else StartCoroutine(Win());
                yield break;
            }
            //TODO：播放地铁开始运动的动画和音效
            Camera.main.DOShakePosition(1, 1.5f, 5);
            DOTween.To(x => m_TrainMove.MoveSpeed = x, 0, 150, 1);
            //每到一站回一些血
            m_Character.HP += 0.2f * m_Character.InitalHP;
            m_Character.HP = Mathf.Clamp(m_Character.HP, 0, m_Character.InitalHP);
            UIText.text = "地铁正在前行";
            CleanNPC();

        }
        StartCoroutine(Lose("游戏失败，你坐过了站"));
    }
    public IEnumerator Lose(string s)
    {
        UIText.text = s;
        Time.timeScale = 0.1f;
        EndCG.color = Color.black;
        EndCG.DOFade(1, 0.1f);
        yield return new WaitForSeconds(0.3f);
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
        //当主角被挤下去的时候调用这玩意
    }

    IEnumerator Win()
    {
        GetComponent<AudioSource>().DOFade(0, 0.75f);
        UIText.DOFade(0, 0.75f);
        m_Character.enabled = false;
        EndCG.DOFade(1, 0.75f);
        yield return new WaitForSeconds(2);

        //接下来播放第二张图
        EndCG.DOColor(Color.black, 0.75f);
        yield return new WaitForSeconds(0.75f);
        EndCG.sprite = End2;
        EndCG.DOColor(Color.white, 0.75f);
        yield return new WaitForSeconds(2);
        //第三张
        EndCG.DOColor(Color.black, 0.75f);
        yield return new WaitForSeconds(0.75f);
        EndCG.sprite = End3;
        EndCG.DOColor(Color.white, 0.75f);
        yield return new WaitForSeconds(3);
        //The END;
        EndCG.DOColor(Color.black, 0.75f);
        UIText.text = "The End";
        UIText.DOFade(1, 0.75f);
        yield return new WaitForSeconds(5);
        Application.Quit();

    }
    string tmpString = "地铁正在前行";
    bool tmpGUI = true;
    private void OnGUI()
    {
        //if (tmpGUI)
        //    GUI.Label(new Rect(50, 10, 1000, 800), tmpString);
    }

    IEnumerator Beat()
    {
        while (true)
        {
            foreach (NPC npcinst in nowNPC)
            {
                npcinst.BeatBehavior();
            }
            yield return new WaitForSeconds(60f / bpm);
        }
    }
}
