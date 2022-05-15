using UnityEngine;
using UnityEngine.UI;
#pragma warning disable 0649

public class PlayerUI : MonoBehaviour
{
    [SerializeField] Animation _playerUIAnimator;
    [SerializeField] AnimationClip _fadeOutAnimationClip;
    [SerializeField] AnimationClip _fadeInAnimationClip;
    public GameObject _scrollPanel;
    public Image spellCooldown;
    public Events.EventFadeComplete OnUIFadeComplete;

    private void Start()
    {
        GameManager.Instance.OnGameStateChanged.AddListener(HandleGameStateChanged);
    }

    void HandleGameStateChanged(GameManager.GameState currentstate, GameManager.GameState previousstate)
    {
        if (previousstate == GameManager.GameState.RUNNING && currentstate != GameManager.GameState.RUNNING) //Si le pr�c�dent state �tait WIN et le nouveau ne l'est pas
        {
            FadeOut(); //On sort de l'UI
        }
    }

    public void OnFadeOutComplete() //Fin de sortie
    {
        OnUIFadeComplete.Invoke(true); //On envoie l'event avec le choix entre false -> prochain niveau et true -> menu
        gameObject.SetActive(false); //On d�sactive le menu
    }

    public void OnFadeInComplete() //Fin d'entr�e
    {
        OnUIFadeComplete.Invoke(false); //On envoie l'event
        AudioManager.Instance.Play("Running", 1.0f, 1);
    }

    public void FadeIn() //D�but d'entr�e
    {
        Debug.Log("FadeInUI");
        gameObject.SetActive(true); //On active l'objet
        _playerUIAnimator.Stop(); //On active l'anim
        _playerUIAnimator.clip = _fadeInAnimationClip;
        _playerUIAnimator.Play();
    }

    public void FadeOut() //D�but de sortie
    {
        Debug.Log("FadeOutUI");
        _playerUIAnimator.Stop(); //On active l'anim
        _playerUIAnimator.clip = _fadeOutAnimationClip;
        _playerUIAnimator.Play();
    }
}