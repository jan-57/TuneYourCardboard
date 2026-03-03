using UnityEngine;

public static class PauseManager
{
    public static bool GameIsPaused {  get; private set; }
    public static bool InputIsPaused {  get; private set; }    

    public static void SetGamePauseState(bool _newPauseState) { GameIsPaused = _newPauseState; }
    public static void SetinputPauseState(bool _newPauseState) { InputIsPaused = _newPauseState; }
}
