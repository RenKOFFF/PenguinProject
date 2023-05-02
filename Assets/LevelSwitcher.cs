using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSwitcher : MonoBehaviour
{
    public void RestartLevel()
    {
        LoadLevel(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadLevel(int levelIndex)
    {
        SceneManager.LoadScene(levelIndex);
    }

    public void NextLevelOrLast()
    {
        var nextLevel = SceneManager.GetActiveScene().buildIndex + 1;
        if (nextLevel <= SceneManager.sceneCountInBuildSettings - 1)
            SceneManager.LoadScene(nextLevel);
        else
        {
            SceneManager.LoadScene(nextLevel - 1);
        }
    }
}