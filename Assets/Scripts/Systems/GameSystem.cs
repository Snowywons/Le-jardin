using UnityEngine;
using UnityEngine.UI;
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
    private SaveSystemComponent savesystem;

    [SerializeField] string pausePanelName;
    [SerializeField] string congratulationsPanelName;
    [SerializeField] string scorePointsTextName;

    private GameObject pausePanel;
    private GameObject congratulationsPanel;
    private Text scorePointsText;

    public bool isGameOver;


    private void Awake()
    {
        if (!Instance)
        {
            Instance = GetComponent<GameSystem>();
            Clock = clock;
           
        }
    }

    private void Start()
    {
        FindReferences();
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
        if (!pausePanel && !FindReferences()) return;

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

    private void GameIsOver()
    {
        if (!congratulationsPanel && !FindReferences()) return;

        congratulationsPanel.SetActive(true);
        isGameOver = true;
        Clock.SetPause(true);
        State = GameState.Pause;
    }

    public void AddScorePoints(int points)
    {
        if (!scorePointsText && !FindReferences()) return;

        savesystem.scorePoints += points;
        scorePointsText.text = savesystem.scorePoints.ToString();
    }

    private bool FindReferences()
    {
        var references = (CanvasContentComponent[]) FindObjectsOfType(typeof(CanvasContentComponent), true);
        pausePanel = references.Where(i => i.gameObject.name.Equals(pausePanelName)).FirstOrDefault().gameObject;
        congratulationsPanel = references.Where(i => i.gameObject.name.Equals(congratulationsPanelName)).FirstOrDefault().gameObject;
        scorePointsText = references.Where(i => i.gameObject.name.Equals(scorePointsTextName)).FirstOrDefault().gameObject.GetComponent<Text>();
        savesystem = FindObjectOfType<SaveSystemComponent>();

        return savesystem && pausePanel && congratulationsPanel && scorePointsText;
    }
}
