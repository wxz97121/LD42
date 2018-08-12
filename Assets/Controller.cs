using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    public GameObject tmpSpawn;
    public StationInfo[] Station;
    public List<NPC> nowNPC;
    public static float minX = -80, maxX = 80;
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
        //nowNPC = new List<NPC>();
    }
    void Start()
    {
        Random.InitState(System.DateTime.Now.Second);
        StartCoroutine(StationTimer());
    }
    void ExitTrain(StationInfo st)
    {
        //随机找一堆东西下车。
        //将来可以根据关卡在这里做特殊逻辑
        int Start = (int)Mathf.Round(Random.Range(0, nowNPC.Count - 0.1f));
        //print(st.RatioOfExit);
        //print(nowNPC.Count);
        for (int j = 0; j < nowNPC.Count * st.RatioOfExit; j++)
        {
            print(j);
            nowNPC[(j + Start) % nowNPC.Count].ExitFromTrain();
        }
    }
    void SpawnNPC(StationInfo st)
    {
        for (int i = 0; i < 5; i++) Instantiate(tmpSpawn, new Vector3(Random.Range(-70, -45), Random.Range(-30, 30), 0), Quaternion.identity);
        //现在还没想好，但是可能得一关一关手写
    }
    void CleanNPC()
    {
        //TODO：这有BUG……还是应该加个坐标范围，不在那个范围里的NPC直接Clean掉
        var tmpNPC = new List<NPC>();
        foreach (var m_NPC in nowNPC)
            if (!m_NPC.alive) tmpNPC.Add(m_NPC);
        nowNPC = tmpNPC;
    }
    IEnumerator StationTimer()
    {
        for (int i = 0; i < Station.Length; i++)
        {
            var st = Station[i];
            yield return new WaitForSeconds(st.TimeToThisStation);
            //TODO：放地铁停止的动画和音效
            tmpString = "地铁到站";
            ExitTrain(st);
            SpawnNPC(st);
            yield return new WaitForSeconds(st.StationTime);
            //TODO：播放地铁开始运动的动画和音效
            tmpString = "地铁正在前行";

        }
        StartCoroutine(Win());
    }
    public IEnumerator Lose()
    {
        tmpString = "You Lose";
        yield break;
        //当主角被挤下去的时候调用这玩意
    }
    IEnumerator Win()
    {
        tmpString = "You Win";
        tmpGUI = true;
        yield break;
        //条漫！
    }
    string tmpString = "地铁正在前行";
    bool tmpGUI = true;
    private void OnGUI()
    {
        if (tmpGUI)
            GUI.Label(new Rect(50, 10, 500, 500), tmpString);
    }
}
