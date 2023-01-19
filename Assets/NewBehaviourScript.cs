
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class NewBehaviourScript : MonoBehaviour
{
    public int maxCandy;
    int candy;

    public GameObject candyPrefab;

    public float time, penalty;
    float timer, penaltyTimer;

    public TextMeshProUGUI timerText, penaltyText, candyCount;

    public GameObject lid;

    bool gameOver = false;

    bool gameStarted = false;

    public TMP_InputField nameInput;

    public TextMeshProUGUI highScoreText;

    bool highScore = false;

    void Start()
    {
        timer = time;
        if (PlayerPrefs.HasKey("HIGH_SCORE"))
        {
            highScoreText.gameObject.SetActive(true);
            highScoreText.text = PlayerPrefs.GetString("HIGH_SCORE_NAME") + " - " + PlayerPrefs.GetFloat("HIGH_SCORE").ToString("00.00");
        }
    }


    void Update()
    {
        if(gameOver)
        {
            HandleHighScoreInput();
        }

        if(highScore)
        {
            highScoreText.gameObject.SetActive(true);
            if(nameInput.text.Length == 0)
            {
                highScoreText.text = "___" + " - " + timer.ToString("00.00");
            }
            else if(nameInput.text.Length == 1)
            {
                highScoreText.text = nameInput.text.ToUpper() + "__" + " - " + timer.ToString("00.00");
            }
            else if(nameInput.text.Length == 2)
            {
                highScoreText.text = nameInput.text.ToUpper() + "_" + " - " + timer.ToString("00.00");
            }
            else if(nameInput.text.Length == 3)
            {
                highScoreText.text = nameInput.text.ToUpper() + " - " + timer.ToString("00.00");

            }

        }

        timerText.text = timer.ToString("00.00");

        penaltyText.text = penaltyTimer.ToString("00.00");

        penaltyTimer -= Time.deltaTime;

        if (penaltyTimer < 0)
        {
            penaltyTimer = 0;
            penaltyText.enabled = false;
        }
        else
        {
            penaltyText.enabled = true;
        }
        if (timer < 0)
        {
            timer = 0;
        }

        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
        {
            gameStarted = true; 
        }

        if (gameStarted)
        {
            if (!gameOver)
            {
                timer -= Time.deltaTime;
                candyCount.text = candy.ToString("000") + "/" + maxCandy.ToString("000");
            }

            if (!gameOver)
            {
                if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
                {
                    SpawnNewCandy();
                }
                if (Input.GetKeyDown(KeyCode.Space) && penaltyTimer <= 0)
                {
                    AttemptToCloseLid();
                }
            }
        }
    }

    private void HandleHighScoreInput()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (nameInput.gameObject.active)
            {
                if (nameInput.text.Length == 3)
                {
                    PlayerPrefs.SetFloat("HIGH_SCORE", timer);
                    PlayerPrefs.SetString("HIGH_SCORE_NAME", nameInput.text.ToUpper());
                    SceneManager.LoadScene(0);
                }
            }
            else
            {
                SceneManager.LoadScene(0);
            }
        }
    }

    private void AttemptToCloseLid()
    {
        if (candy == maxCandy)
        {
            Finish();

        }
        else
        {
            penaltyTimer += penalty;
            Debug.Log("Not yet!");
        }
    }

    private void SpawnNewCandy()
    {
        candy++;
        var newCandy = Instantiate(candyPrefab, new Vector2(Random.Range(-1, 1), transform.position.y), Quaternion.identity);
        newCandy.GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(-5f, 5f), 0), ForceMode2D.Impulse);
        newCandy.GetComponent<Rigidbody2D>().AddTorque(Random.Range(-30f, 30f));

        if (candy > maxCandy)
        {
            penaltyTimer += penalty;
            Invoke(nameof(DeleteCandy), penaltyTimer);
            Destroy(newCandy, penaltyTimer);
        }
    }

    private void Finish()
    {
        lid.SetActive(true);
        gameOver = true;
        candyCount.text = "COMPLETE!";
        candyCount.GetComponent<Animator>().enabled = true;


        if (PlayerPrefs.HasKey("HIGH_SCORE"))
        {
            if (timer > PlayerPrefs.GetFloat("HIGH_SCORE"))
            {
                highScore = true;
                nameInput.gameObject.SetActive(true);
                EventSystem.current.SetSelectedGameObject(nameInput.gameObject);
            }
        }
        else
        {
            highScore = true;
            nameInput.gameObject.SetActive(true);
            EventSystem.current.SetSelectedGameObject(nameInput.gameObject);
        }
    }

    public void DeleteCandy()
    {
        candy--;

    }

}
