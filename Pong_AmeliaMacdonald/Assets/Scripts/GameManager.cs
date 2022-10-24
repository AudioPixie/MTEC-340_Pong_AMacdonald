using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public Player P1;
    public Player P2;

    private Player[] Players = new Player[2];
    public int winner;

    public float initBallSpeed;
    public float ballSpeedIncrement;
    public int pointsToVictory;

    private string _state;
    public string State
    {
        get => _state;
        set
        {
            _state = value;
        }
    }

    public KeyCode startKey;
    public KeyCode serveKey;
    public KeyCode pauseKey;
    public KeyCode leftKey;
    public KeyCode rightKey;

    public TextMeshProUGUI startGUI;
    public TextMeshProUGUI startSubtitle;
    public TextMeshProUGUI messagesGUI;
    public TextMeshProUGUI controlsGUI;
    public TextMeshProUGUI PlayerTags;
    public TextMeshProUGUI bestOfGUI;
    public TextMeshProUGUI winnerGUI;
    public TextMeshProUGUI victoryGUI;

    private AudioSource m_audioSource;

    public AudioClip navSound;
    public AudioClip startSound;
    public AudioClip victorySound;

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this);
        else
            Instance = this;

        Players[0] = P1;
        Players[1] = P2;

        m_audioSource = GetComponent<AudioSource>();

        pointsToVictory = 3;
    }

    private void Start()
    {
        State = "Start";

        startGUI.enabled = true;
        startSubtitle.enabled = true;

        messagesGUI.enabled = false;
        controlsGUI.enabled = false;
        PlayerTags.enabled = false;
        bestOfGUI.enabled = false;
        winnerGUI.enabled = false;
        victoryGUI.enabled = false;

        Instance.PlaySound(startSound);
    }

    public void Update()
    {
        bestOfGUI.text = "Best of:\n< " + pointsToVictory + " >\n(Arrow Keys)";


        if ((State == "Start" || State == "GameOver") && Input.GetKeyUp(startKey))
        {
            State = "Serve";

            Instance.PlaySound(navSound);

            startGUI.enabled = false;
            startSubtitle.enabled = false;
            winnerGUI.enabled = false;
            victoryGUI.enabled = false;

            messagesGUI.enabled = true;
            controlsGUI.enabled = true;
            PlayerTags.enabled = true;
            bestOfGUI.enabled = true;
        }

        else if (bestOfGUI.enabled == true && pointsToVictory < 10 && Input.GetKeyUp(rightKey))
        {
            Instance.PlaySound(navSound);
            pointsToVictory++;
        }

        else if (bestOfGUI.enabled == true && pointsToVictory > 1 && Input.GetKeyUp(leftKey))
        {
            Instance.PlaySound(navSound);
            pointsToVictory--;
        }

        else if (State == "Serve" && Input.GetKeyUp(serveKey))
        {
            State = "Play";

            Instance.PlaySound(navSound);

            messagesGUI.enabled = false;
            controlsGUI.enabled = false;
            bestOfGUI.enabled = false;
        }

        else if (Input.GetKeyUp(pauseKey) && State == "Play")
        {
            State = "Pause";
            Instance.PlaySound(navSound);
        }

        else if (Input.GetKeyUp(pauseKey) && State == "Pause")
        {
            State = "Play";
            Instance.PlaySound(navSound);
        }

    }

    public void UpdateScore(int player)
    {
        Players[player - 1].Score++;

        foreach (Player p in Players)
        {
            if (p.Score >= pointsToVictory)
            {

                if (Players[0].Score > Players[1].Score)
                    winner = 1;

                else if (Players[0].Score < Players[1].Score)
                    winner = 2;

                ResetGame();
                break;
            }
        }
    }

    private void ResetGame()
    {
        State = "GameOver";

        Instance.PlaySound(victorySound);

        messagesGUI.enabled = false;
        PlayerTags.enabled = false;

        victoryGUI.enabled = true;
        winnerGUI.enabled = true;
        winnerGUI.text = "Player " + winner + "\n\nPress space to play again";

        foreach (Player p in Players)
            p.Score = 0;
    }

    public void PlaySound(AudioClip clip, float volume=1.0f)
    {
        m_audioSource.volume = volume;
        m_audioSource.PlayOneShot(clip);
    }
}
