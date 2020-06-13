using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Transform Player;
    public bool IsGameOn;
    public float CurrentScore = 0;
    public int Difficulty = 1;


    public static GameManager Instance;

    private void Awake()
    {
        if (Instance)
            Destroy(gameObject);
        else
            Instance = this;
    }

    private void Start()
    {
        ActionSystem.OnLevelLoaded?.Invoke();
    }


    private void OnEnable()
    {
        ActionSystem.OnLevelLoaded += ResetGlobalValues;
        ActionSystem.OnGameStarted += SetGameOn;
        ActionSystem.OnGameEnded+= SetGameOff;
    }

    private void OnDisable()
    {
        ActionSystem.OnLevelLoaded -= ResetGlobalValues;
        ActionSystem.OnGameStarted -= SetGameOn;
        ActionSystem.OnGameEnded -= SetGameOff;
    }

    void ResetGlobalValues()
    {
        CurrentScore = 0;
    }

    void SetGameOn()
    {
        IsGameOn = true;
    }
    void SetGameOff()
    {
        IsGameOn = false;
    }
}
