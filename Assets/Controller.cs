using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    public StationInfo[] Station;
    public List<NPC> nowNPC;
    [System.Serializable]
    public struct StationInfo
    {
        public bool isDown;
        public float TimeToThisStation;
        public float StationTime;
        [Range(0, 1)]
        public float RatioOfExit;
    }
    private void Awake()
    {
        nowNPC = new List<NPC>();
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
        for (int j = 0; j < nowNPC.Count * st.RatioOfExit; j++)
        {
            nowNPC[ (j+Start) % nowNPC.Count].ExitFromTrain();
        }
    }
    void SpawnNPC(StationInfo st)
    {
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
            ExitTrain(st);
            SpawnNPC(st);
            yield return new WaitForSeconds(st.StationTime);
            //TODO：播放地铁开始运动的动画和音效
            
        }
        StartCoroutine(Win());
    }
    IEnumerator Lose()
    {
        yield break;
        //当主角被挤下去的时候调用这玩意
    }
    IEnumerator Win()
    {
        yield break;
        //条漫！
    }

}
