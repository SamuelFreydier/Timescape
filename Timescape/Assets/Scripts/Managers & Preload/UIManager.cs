using System.Collections;
using UnityEngine;
#pragma warning disable 0649
public class UIManager : Singleton<UIManager> //Manager pour l'UI en g�n�ral
{
    [SerializeField] private MainMenu _mainMenu;
    [SerializeField] private Camera _dummyCamera;
    [SerializeField] private PauseMenu _pauseMenu;
    [SerializeField] private WinMenu _winMenu;
    [SerializeField] private LoseMenu _loseMenu;
    [SerializeField] private PlayerUI _uiMenu;
    public Events.EventFadeComplete OnMainMenuFadeComplete;
    public Events.EventFadeComplete OnUIFadeComplete;
    public Events.EventLoseWinFadeComplete OnLoseMenuFadeComplete;
    public Events.EventLoseWinFadeComplete OnWinMenuFadeComplete;
    private bool _waitAfterClick = false;

    public PlayerUI UIPlayer
    {
        get { return _uiMenu; }
    }
    public bool AfterClick
    {
        get { return _waitAfterClick; }
    }

    private void HandleGameStateChanged(GameManager.GameState currentstate, GameManager.GameState previousstate)
    {
        _pauseMenu.gameObject.SetActive(currentstate == GameManager.GameState.PAUSED); //Si le state PAUSE est actif, on active le menu de pause, sinon on le d�sactive
        _winMenu.gameObject.SetActive(currentstate == GameManager.GameState.WIN); //Et pour le menu de victoire
        _uiMenu.gameObject.SetActive(currentstate == GameManager.GameState.RUNNING && previousstate != GameManager.GameState.PREGAME);
        _loseMenu.gameObject.SetActive(currentstate == GameManager.GameState.LOSE);

        if (_winMenu.gameObject.activeInHierarchy)
        {
            _winMenu.FadeIn(); //Pareil pour le menu de victoire
        }
        if(_loseMenu.gameObject.activeInHierarchy)
        {
            _loseMenu.FadeIn();
        }
        if (_uiMenu.gameObject.activeInHierarchy && previousstate != GameManager.GameState.PREGAME)
        {
            _uiMenu.FadeIn();
        }
        //Si l'ancien state �tait lose ou win et que le nouveau est running
        if ((previousstate == GameManager.GameState.WIN || previousstate == GameManager.GameState.PAUSED || previousstate == GameManager.GameState.LOSE) && currentstate == GameManager.GameState.RUNNING)
        {
            SetDummyCameraActive(false); //on d�sactive la cam�ra UI
        }
        if (currentstate == GameManager.GameState.PREGAME)
        {
            _mainMenu.gameObject.SetActive(true);
            _mainMenu.FadeIn();
        }

    }

    private void HandleEventFadeCompleted(bool fadeInfadeOut)
    {
        if (GameManager.Instance.CurrentGameState == GameManager.GameState.RUNNING)
        {
            _uiMenu.gameObject.SetActive(true);
            _uiMenu.FadeIn();
        }
        OnMainMenuFadeComplete.Invoke(fadeInfadeOut); //On envoie l'event globalement
    }

    private void HandleLoseEventFadeCompleted(bool fadeInfadeOut, bool choice)
    {
        OnLoseMenuFadeComplete.Invoke(fadeInfadeOut, choice); //Global

        if (fadeInfadeOut) //Si FadeOut
        {
            if (choice == true) //Si le choix est true
            {
                GameManager.Instance.RestartGame(); //On revient au menu
            }
            if (choice == false) //Sinon
            {
                Debug.Log(GameManager.Instance.LevelName);
                GameManager.Instance.LoadLevel(GameManager.Instance.LevelName); //On recharge le niveau
            }
        }
    }

    private void HandleWinEventFadeCompleted(bool fadeInfadeOut, bool choice)
    {
        OnWinMenuFadeComplete.Invoke(fadeInfadeOut, choice); //Global
        if (!fadeInfadeOut) //FadeIn
        {
            Debug.Log("fadeInUnload");
            GameManager.Instance.UnloadLevel(GameManager.Instance.LevelName); //D�chargement de niveau
        }
        if (fadeInfadeOut) //FadeOut
        {
            if (choice == true) //Si true
            {
                GameManager.Instance.RestartGame(); //On va au Menu
            }
            if (choice == false) //Sinon
            {
                GameManager.Instance.LoadLevel("Level1"); //On va au niveau suivant, ici c'est forc�ment le niveau 2
            }
        }
    }

    private void HandleLoadLevel(string levelname)
    {
        StartCoroutine(WaitAfterClick());
        if (GameManager.Instance.CurrentGameState != GameManager.GameState.PREGAME)
        {
            return;
        }
        GameManager.Instance.StartGame(levelname);
    }

    private void HandleUIEventFadeComplete(bool fadeinfadeout)
    {
        OnUIFadeComplete.Invoke(fadeinfadeout);
    }
    private void Start()
    {
        GameManager.Instance.OnGameStateChanged.AddListener(HandleGameStateChanged);
        _mainMenu.OnMainMenuFadeComplete.AddListener(HandleEventFadeCompleted);
        _mainMenu.OnMainMenuLevelLoad.AddListener(HandleLoadLevel);
        _winMenu.OnWinMenuFadeComplete.AddListener(HandleWinEventFadeCompleted);
        _loseMenu.OnLoseMenuFadeComplete.AddListener(HandleLoseEventFadeCompleted);
        _uiMenu.OnUIFadeComplete.AddListener(HandleUIEventFadeComplete);
    }

    public void SetDummyCameraActive(bool active)
    {
        _dummyCamera.gameObject.SetActive(active);
    }

    public void OpenScroll(string title, string text)
    {
        GameObject scroll = _uiMenu._scrollPanel;
        scroll.SetActive(true);
        scroll.GetComponent<Parchemin>().UpdateScroll(title, text);
        Time.timeScale = 0f;
    }
    
    IEnumerator WaitAfterClick()
    {
        _waitAfterClick = true;
        yield return new WaitForSeconds(0.5f);
        _waitAfterClick = false;
    }
}