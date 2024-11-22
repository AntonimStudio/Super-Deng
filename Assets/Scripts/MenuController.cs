using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    public Image panel;
    public GameObject panelBlack;
    [SerializeField] private AnimationClip animTransit;
    public int indexStart;
    public int indexLevel;

    public void ClickStartPanelTransition()
    {
        StartCoroutine(WaitForAnimationToEnd(indexStart));
    }

    public void ClickLevelPanelTransition()
    {
        StartCoroutine(WaitForAnimationToEnd(indexLevel));
    }

    private IEnumerator WaitForAnimationToEnd(int index)
    {
        panel.enabled = true;
        Animator animator = panel.GetComponent<Animator>();
        animator.enabled = true;
        animator.Play(animTransit.name);

        yield return new WaitForSeconds(animTransit.length);
        SetBlackPanel();
        LoadSceneByIndex(index);
    }

    public void SetBlackPanel()
    {
        panelBlack.SetActive(true);
    }


    public void LoadSceneByName(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void LoadSceneByIndex(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }

    public void EnableImage(GameObject image)
    {
        image.SetActive(true);
    }

    public void DisableImage(GameObject image)
    {
        image.SetActive(false);
    }
}
