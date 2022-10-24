using UnityEngine;
using TMPro;

public class Player : MonoBehaviour
{

    private int _score;
    public int Score
    {
        get => _score;
        set
        {
            _score = value;
            ScoreGUI.text = Score.ToString();
        }
    }

    public TextMeshProUGUI ScoreGUI;

    private void Start()
    {
        Score = 0;
        ScoreGUI.enabled = false;
    }

    private void Update()
    {
        if (GameManager.Instance.State != "Start")
            ScoreGUI.enabled = true;

        if (GameManager.Instance.State == "GameOver")
            ScoreGUI.enabled = false;
    }

}
