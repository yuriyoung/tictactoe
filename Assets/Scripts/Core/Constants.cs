using System.Collections.Generic;
using System.ComponentModel;

public class Constants
{
    public const int BoardSize = 3;
    public const float CellSize = 1.0f; // Size of each cell in the board
    public const int MaxPlayers = 2; // Maximum number of players in a game

    public enum GameMode
    {
        NoneMode,
        SingleMode,
        AIMode,
        OnlineMode
    }

    public enum MultiplayState
    {
        CreateRoom,
        JoinRoom,
        ExitRoom,
        StartGame,
        EndGame
    };

    public enum GameResult
    {
        None,
        Win,
        Lose,
        Draw
    }
}
