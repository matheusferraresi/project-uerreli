using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AreaExit : MonoBehaviour
{
    [SerializeField] private string sceneToLoad;
    [SerializeField] private string sceneTransitionName;

    private float waitToLoadTime = 1f;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<PlayerController>())
        {
            StartCoroutine(LoadSceneRoutine());
            UIFade.Instance.FadeToBlack();
        }
    }

    private IEnumerator LoadSceneRoutine()
    {
        yield return new WaitForSeconds(waitToLoadTime);
        
        SceneManager.LoadScene(sceneToLoad);
        SceneManagement.Instance.SetTransitionName(sceneTransitionName);
    }
}