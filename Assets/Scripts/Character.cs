using UnityEngine;
using UnityEngine.UI;

public class Character : MonoBehaviour
{
    Rigidbody2D rb;

    
    float jumpingForce = 75f;
    float jumpSpeed = 1.25f;
    float fallingSpeed = 1.25f;

    [SerializeField]
    Animator anim;

    [SerializeField]
    Text textScore;

    [SerializeField]
    Text textMaxScore;

    [SerializeField]
    Text restart_textScore;

    [SerializeField]
    GameObject startPanel;

    [SerializeField]
    GameObject restartPanel;

    [SerializeField]
    GameObject cloud;

    [SerializeField]
    GameObject pipe;

    [SerializeField]
    GameObject pipe2;

    [SerializeField]
    AudioSource death;

    [SerializeField]
    AudioSource jumpSound;

    GameObject pipe_2;
    GameObject cloud2;
    //-5.90f kaybol 1.90f baþlama (x deðeri)
    //2f den -2.5 ye kadar aralýklý random
    int score = 0;
    int maxScore = 0;
    bool isGameStarted = false;
    int scoreValue = 15;
    int pipeCount = 0;
    float speed = 1f;
     
    void Start()
    {
        if (GameManager.isRestart)
        {
            startPanel.SetActive(false);
            isGameStarted = true;
        }
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;
        anim.speed = 0;
        pipe_2 = Instantiate(pipe2, new Vector3(5.32f, 1.2f, 0f), Quaternion.identity);
        cloud2 = Instantiate(cloud, new Vector3(6f, 2.40f, 20f), Quaternion.identity);

        PlayerPrefs.SetInt("max", maxScore);
        PlayerPrefs.Save();
    }
    private void FixedUpdate()
    {
        float y_value = Random.Range(1.25f, 3.50f);
        if (isGameStarted)
        {
            rb.gravityScale = 0.5f;
            anim.speed = 1; 
            if (Input.GetMouseButton(0)) //Input.GetTouch(0).tapCount > 0
            {  
                jumpSound.Play();
                transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 0);

                rb.velocity = new Vector2(rb.velocity.x, jumpingForce * jumpSpeed * Time.deltaTime);

                transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 25f);
            }  

            if (rb.velocity.y < 0)
            {
                transform.eulerAngles -= new Vector3(0, 0, 50f)*Time.deltaTime;
            }
            if (cloud.transform.position.x <= -4.3f)
            {
                cloud.transform.position = new Vector3(4f, y_value, cloud.transform.position.z);
            }
            if (cloud2.transform.position.x <= -4.3f)
            {
                cloud2.transform.position = new Vector3(4f, y_value, cloud.transform.position.z);
            }
        }
    }
    void Update()
    {

        float rast = Random.Range(0.50f, 3.20f);
        float rast2 = Random.Range(0.50f, 3.20f);

        if (pipe.transform.position.x <= -3.49f)
        {

            pipe.transform.position = new Vector3(pipe_2.transform.position.x + 4.32f, rast, pipe.transform.position.z);
            scoreAdd(scoreValue);
            pipeCount++;
            if (pipeCount % 5 == 0 && pipeCount >= 5)
            {
                speed += 0.1f;
            }
        } 

        if (pipe_2.transform.position.x <= -3.49f)
        {
            pipe_2.transform.position = new Vector3(pipe.transform.position.x + 4.32f, rast2, pipe_2.transform.position.z);
            scoreAdd(scoreValue);
            pipeCount++;
            if (pipeCount % 5 == 0 && pipeCount >= 5)
            {
                speed += 0.1f;
            }
        }
        
        if (isGameStarted)
        {
            cloud.transform.Translate(-0.75f * Time.deltaTime, 0, 0);
            cloud2.transform.Translate(-0.75f * Time.deltaTime, 0, 0);
            pipe.transform.Translate(-1.75f * Time.deltaTime *speed, 0, 0);
            pipe_2.transform.Translate(-1.75f * Time.deltaTime*speed, 0, 0);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Pipe") || collision.collider.CompareTag("Ground") )
        {
            if (score >= maxScore)
            {
                maxScore = score;
                checkScore();
                textMaxScore.text = "HIGHEST SCORE: " + PlayerPrefs.GetInt("Max").ToString();
            }

            death.Play();
            Destroy(this.gameObject);
            restartPanel.SetActive(true);
            isGameStarted = false;
        }
    } 
   
    void checkScore()
    {
        if (maxScore > PlayerPrefs.GetInt("Max"))
        {
            PlayerPrefs.SetInt("Max", maxScore);
        }
    }
    void scoreAdd(int value)
    { 
        score += value;
        textScore.text = "Score: " + score.ToString();
        restart_textScore.text = "Score: " + score.ToString();
    }
    public void startGame()
    {
        isGameStarted = true;
        startPanel.SetActive(false);
    }
}
