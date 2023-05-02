using System.Collections;
using UnityEngine;

public class UIPanel : MonoBehaviour
{
    [SerializeField] private GameObject _winPanel;
    [SerializeField] private GameObject _losePanel;
    [SerializeField] private GameObject _bgPanel;
    
    [Min(0.2f), SerializeField] private float _showPanelDelay = 1f;

    private void OnEnable()
    {
        GameManager.Instance.GameLose += OnGameLose;
        GameManager.Instance.GameWon += OnGameWon;
    }
    
    private void OnDisable()
    {
        GameManager.Instance.GameLose -= OnGameLose;
        GameManager.Instance.GameWon -= OnGameWon;
    }

    private void OnGameWon()
    {
        StartCoroutine(ShowPanel(_winPanel, _showPanelDelay));
    }

    private void OnGameLose()
    {
        StartCoroutine(ShowPanel(_losePanel, _showPanelDelay));
    }

    private IEnumerator ShowPanel(GameObject panel, float delay)
    {
        yield return new WaitForSeconds(delay);

        _bgPanel.SetActive(true);
        panel.SetActive(true);
    }
}
