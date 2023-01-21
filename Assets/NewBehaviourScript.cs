
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using System.Text;

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

    public TextMeshProUGUI highScoreText, newHighScoreText;

    bool highScore = false;

    public AudioClip lidSound;
    public ParticleSystem particles;
    public Image focusScoreImage;
    public Image enterKeySymbol;

    void Start()
    {
        timer = time;
        if (PlayerPrefs.HasKey("HIGH_SCORE"))
        {
            var formattedTimerText = (PlayerPrefs.GetFloat("HIGH_SCORE").ToString("00.00")[0]).ToString()
                + (PlayerPrefs.GetFloat("HIGH_SCORE").ToString("00.00")[1]).ToString()
                + ":" + (PlayerPrefs.GetFloat("HIGH_SCORE").ToString("00.00")[3]).ToString()
                + (PlayerPrefs.GetFloat("HIGH_SCORE").ToString("00.00")[4]).ToString();

            highScoreText.gameObject.SetActive(true);
            highScoreText.text = PlayerPrefs.GetString("HIGH_SCORE_NAME") + " - " + formattedTimerText;
        }
    }


    void Update()
    {
        var formattedTimerText = (timer.ToString("00.00")[0]).ToString() + (timer.ToString("00.00")[1]).ToString() + ":" + (timer.ToString("00.00")[3]).ToString() + (timer.ToString("00.00")[4]).ToString();

        if (gameOver)
        {
            HandleHighScoreInput();
        }

        if(highScore)
        {
            focusScoreImage.enabled= true;  
            if (nameInput.gameObject.active)
            {
                FindObjectOfType<EventSystem>().SetSelectedGameObject(nameInput.gameObject);
                nameInput.MoveTextEnd(true);
                if(nameInput.text.Length > 3)
                {
                    nameInput.text = "";
                }
            }
            newHighScoreText.gameObject.SetActive(true);
            highScoreText.gameObject.SetActive(true);
            if(nameInput.text.Length == 0)
            {
                highScoreText.text = "___" + " - " + formattedTimerText;
            }
            else if(nameInput.text.Length == 1)
            {
                highScoreText.text = nameInput.text.ToUpper() + "__" + " - " + formattedTimerText;
            }
            else if(nameInput.text.Length == 2)
            {
                highScoreText.text = nameInput.text.ToUpper() + "_" + " - " + formattedTimerText;
            }
            else if(nameInput.text.Length == 3)
            {
                enterKeySymbol.enabled = true;

                highScoreText.text = nameInput.text.ToUpper() + " - " + formattedTimerText;
            }
        }

        timerText.text = formattedTimerText;

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
            lid.SetActive(true);
            gameOver = true;
            AudioSource.PlayClipAtPoint(lidSound, Camera.main.transform.position);
            particles.Play();
            Invoke(nameof(Finish), 0.5f);
            

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
        if(!highScore)
        {
            enterKeySymbol.enabled = true;  
        }
    }

    public void DeleteCandy()
    {
        candy--;

    }

}
