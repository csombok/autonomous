namespace Autonomous.Public
{
    public interface IPlayer
    {

        // void StartGameLoop(string playerId, IGameStateProvider gameStateProvider, IPlayerCommand command);
        void Initialize(string playerId);
        PlayerCommand Update(GameState gameState);
        void Finish();
    }
}
