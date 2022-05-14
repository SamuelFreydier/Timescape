using UnityEngine.Events;

public class Events
{
    [System.Serializable] public class EventGameState : UnityEvent<GameManager.GameState, GameManager.GameState> { } //Changement de State
    [System.Serializable] public class EventFadeComplete : UnityEvent<bool> { } //Lorsqu'une animation fade a �t� compl�t�e pour le menu
    [System.Serializable] public class EventLoseWinFadeComplete : UnityEvent<bool, bool> { } //Animation fade compl�t�e pour le panel win ou lose
    [System.Serializable] public class EventLoadLevel : UnityEvent<string> { } //Lorsque un niveau se charge
};