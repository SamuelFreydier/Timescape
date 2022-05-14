using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    public GameObject[] SystemPrefabs; //Liste d'objets syst�me (d'autres managers) � instancier d�s le d�but
    public Events.EventGameState OnGameStateChanged; //Event qui se lance quand le State change
    private List<AsyncOperation> _loadOperations; //Liste d'op�rations de chargement
    private List<GameObject> _instanciedSystemPrefabs; //Liste des systemPrefabs qui ont �t� instanci�s
    private string _currentLevelName = string.Empty; //Nom du niveau actuel
    private GameState _currentGameState = GameState.PREGAME; //State actuel

    public enum GameState //Ensemble des State
    {
        PREGAME, //Menu essentiellement
        RUNNING, //En train de jouer, en plein niveau
        PAUSED, //Jeu en pause (pendant un RUNNING)
        WIN, //Le joueur a gagn�
        LOSE
    }

    public string LevelName //Renvoie le nom du niveau actuel
    {
        get { return _currentLevelName; }
    }

    public GameState CurrentGameState //Renvoie le State actuel
    {
        get { return _currentGameState; }
    }

    public void RestartGame() //Retour au menu
    {
        if (_currentGameState != GameState.PREGAME)
        {
            UpdateState(GameState.PREGAME);
        }
    }
    public void TogglePause() //Passage du jeu en pause ou retour au jeu depuis une pause
    {
        if (_currentGameState == GameState.RUNNING)
        {
            UpdateState(GameState.PAUSED);
        }
        else if (_currentGameState == GameState.PAUSED)
        {
            UpdateState(GameState.RUNNING);
        }
    }

    public void TriggerWin() //D�clenchement de la victoire du joueur
    {
        if (_currentGameState != GameState.WIN)
        {
            UpdateState(GameState.WIN);
        }
    }

    public void TriggerLose()
    {
        if(_currentGameState != GameState.LOSE)
        {
            UpdateState(GameState.LOSE);
        }
    }

    private void UpdateState(GameState state) //Mise � jour du State actuel
    {
        GameState previousGameState = _currentGameState;
        switch (state)
        {
            case GameState.PREGAME:
                {
                    _currentGameState = GameState.PREGAME;
                    Time.timeScale = 1f;
                    Debug.Log("Pregame");
                }
                break;
            case GameState.RUNNING:
                {
                    _currentGameState = GameState.RUNNING;
                    Time.timeScale = 1f;
                    Debug.Log("Running");
                }
                break;
            case GameState.PAUSED:
                {
                    _currentGameState = GameState.PAUSED;
                    Time.timeScale = 0;
                    Debug.Log("Pause");
                }
                break;
            case GameState.WIN:
                {
                    _currentGameState = GameState.WIN;
                    Time.timeScale = 1f;
                    Debug.Log("Win");
                }
                break;
            case GameState.LOSE:
                {
                    _currentGameState = GameState.LOSE;
                    Time.timeScale = 1f;
                    Debug.Log("Lose");
                }
                break;
            default:
                {
                    _currentGameState = GameState.PREGAME;
                    Time.timeScale = 1f;
                    Debug.Log("Pregame");
                }
                break;
        }
        OnGameStateChanged.Invoke(_currentGameState, previousGameState);
    }

    public void LoadLevel(string levelName) //Chargement d'un niveau
    {
        AsyncOperation ao = SceneManager.LoadSceneAsync(levelName, LoadSceneMode.Additive);
        if (ao == null)
        {
            Debug.LogError("[GameManager] Unable to load level " + levelName);
            return;
        }
        ao.completed += OnLoadOperationComplete;
        _loadOperations.Add(ao);
        _currentLevelName = levelName;
    }

    public void UnloadLevel(string levelName) //D�chargement d'un niveau
    {
        AsyncOperation ao = SceneManager.UnloadSceneAsync(levelName);
        if (ao == null)
        {
            Debug.LogError("[GameManager] Unable to unload level " + levelName);
            return;
        }
        ao.completed += OnUnloadOperationComplete;
    }

    private void OnLoadOperationComplete(AsyncOperation ao) //Fin de chargement de niveau
    {
        if (_loadOperations.Contains(ao))
        {
            _loadOperations.Remove(ao);
        }
        if (_loadOperations.Count == 0)
        {
            UpdateState(GameState.RUNNING);
        }
        Debug.Log("Load Complete");
    }

    private void OnUnloadOperationComplete(AsyncOperation ao) //Fin de d�chargement de niveau
    {
        Debug.Log("Unload Complete");
    }

    private void Start()
    {
        _loadOperations = new List<AsyncOperation>();
        _instanciedSystemPrefabs = new List<GameObject>();
        InstantiateSystemPrefabs();
    }

    void InstantiateSystemPrefabs() //On instancie tous les �l�ments n�cessaires au syst�me de jeu (Les managers comme UIManager)
    {
        GameObject prefabInstance;
        for (int i = 0; i < SystemPrefabs.Length; i++)
        {
            prefabInstance = Instantiate(SystemPrefabs[i]);
            _instanciedSystemPrefabs.Add(prefabInstance);
        }
    }

    protected override void OnDestroy()
    {
        for (int i = 0; i < _instanciedSystemPrefabs.Count; i++)
        {
            Destroy(_instanciedSystemPrefabs[i]);
        }
        _instanciedSystemPrefabs.Clear();
        base.OnDestroy();
    }

    public void StartGame(string levelname) //Lance un niveau
    {
        LoadLevel(levelname);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) //Si on appuie sur echap...
        {

            if (_currentGameState != GameState.RUNNING && _currentGameState != GameState.PAUSED) //... et que le State actuel est Pause ou Running
            {
                return;
            }
            TogglePause(); //On met le jeu en pause ou on le reprend depuis une pause
        }

    }
}