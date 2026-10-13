using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField][Min(0)] int startingHP = 20;

    private int currentHP;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        currentHP = startingHP;
    }

    void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }

    public int CurrentHP => currentHP;

    public void DecrementHP(int dec)
    {
        currentHP -= dec;
        if (currentHP <= 0)
        {
            restartGame();
        }
    }

    public static void restartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void OnGUI()
    {
        GUI.Label(new Rect(16, 16, 240, 32), $"Base HP: {currentHP}");
    }
}
