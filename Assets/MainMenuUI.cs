using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

public class MainMenuUI : MonoBehaviour {
    public Image CG2;
    public void StartGame()
    {
        StartCoroutine(StartLoad());
    }
    IEnumerator StartLoad()
    {
        CG2.DOFade(1, 1.2f);
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene(1);
    }
    public void ExitGame()
    {
        Application.Quit();
    }

}
