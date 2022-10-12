using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelExit : MonoBehaviour
{
    [SerializeField] float endLevelTime = 2f;
    [SerializeField] ParticleSystem winFX;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(!(collision.tag == "Player")) { return; }
        StartCoroutine(LoadNextLevel());
    }

    IEnumerator LoadNextLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;

        winFX.Play();

        yield return new WaitForSecondsRealtime(endLevelTime);

        if(nextSceneIndex == SceneManager.sceneCountInBuildSettings)
        {
            nextSceneIndex = 0;
        }
        FindObjectOfType<ScenePersist>().ResetScenePersist();
        SceneManager.LoadScene(nextSceneIndex);
    }
}
