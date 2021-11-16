using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.Linq;

public enum GameState
{
    Resume,
    Pause
}

public class GameSystem : MonoBehaviour
{
    public static GameSystem Instance { get; private set; }
    public AudioSource seedSound;

    public InventoryComponent PlayerInventory
    {
        get
        {
            if (!playerInventory)
                playerInventory = FindObjectsOfType<InventoryComponent>().FirstOrDefault(x => x.mode == InventoryMode.Player);
            return playerInventory;
        }
    }
    public InventoryComponent WarehouseInventory
    {
        get
        {
            if (!warehouseInventory)
                warehouseInventory = FindObjectsOfType<InventoryComponent>().FirstOrDefault(x => x.mode == InventoryMode.Warehouse);
            return warehouseInventory;
        }
    }
    public ClockComponent Clock { get; private set; }
    public GameState State { get; private set; }

    [SerializeField] ClockComponent clock;
    private InventoryComponent playerInventory;
    private InventoryComponent warehouseInventory;

    [SerializeField] GameObject pausePanel;
    [SerializeField] GameObject congratulationsPanel;

    public bool isGameOver;


    private void Awake()
    {
        if (!Instance)
        {
            Instance = GetComponent<GameSystem>();
            Clock = clock;
           
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            TogglePause();

        if (Clock.GetDay() > 31)
            GameIsOver();
    }

    private void TogglePause()
    {
        if (State.Equals(GameState.Resume))
        {
            Clock.SetPause(true);
            pausePanel.SetActive(true);
            State = GameState.Pause;
        }
        else
        {
            Clock.SetPause(false);
            pausePanel.SetActive(false);
            State = GameState.Resume;
        }
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnLevelFinishedLoading;
    }

    void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        //ResetInitValues();
    }

    private void GameIsOver()
    {
        congratulationsPanel.SetActive(true);
        isGameOver = true;
        Clock.SetPause(true);
        State = GameState.Pause;
    }
}
