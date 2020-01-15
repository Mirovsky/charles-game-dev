namespace OOO.Utils
{
    public class GameEvent
    {
    }

    /** Fired when the GameTypeResolver has parsed the arguments. */
    public class GameTypeLoadedEvent : GameEvent
    {
    }

    /** Fired when player collects the key. */
    public class KeyCollectedEvent : GameEvent
    {
        public KeyType keyType;
    }

    /** Fired when player (each of them) enters the exit area. */
    public class ExitOccupancyChangeEvent : GameEvent
    {
        public enum Type
        {
            ENTER,
            LEAVE
        }

        public Type type;
        public PlayerType playerType;
    }

    /** Fired when 2D player collects all keys and exits should open. */
    public class ExitOpenEvent : GameEvent
    {
    }

    public class LevelCompleteEvent : GameEvent
    {
    }

    public class GameOverEvent : GameEvent
    {
        public enum GameOverReason
        {
            TIME_UP,
            FALL,
            UNKNOWN
        }

        public GameOverReason gameOverReasong = GameOverReason.UNKNOWN;
    }

    public class GamePauseEvent : GameEvent
    {
        public bool state;
    }
}