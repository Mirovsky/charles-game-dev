using System;
using OOO.Utils;
using UnityEngine;
using UnityEngine.XR;


namespace OOO
{
    public class GameTypeResolver : MonoBehaviour
    {
        [Help("Add default value so that the mods are usable in the editor play.")]
        public PlayerType playerType = default;
        [Help("Add default IP address to look for.")]
        public string ip = default;
        [Help("Forces type of game - Host or Client (leave default for automatic resolve)")]
        public GameType gameType = default;
        
        public static GameTypeResolver Instance = null;

        private void Awake()
        {
            if (Instance == null) {
                Instance = this;

                playerType = ParsePlayerType();
                ip = ParseIP();

                XRSettings.enabled = (playerType == PlayerType.VR);
                
                DontDestroyOnLoad(gameObject);
            } else if (Instance != this) {
                DestroyImmediate(gameObject);
            }
        }

        private PlayerType ParsePlayerType()
        {
            var parseResult = Enum.TryParse(GetArg(TYPE_ARG), out PlayerType t);
            return parseResult ? t : playerType;
        }

        private string ParseIP()
        {
            var parseResult = GetArg(IP_ARG);
            return parseResult != null ? parseResult : ip;
        }

        private string GetArg(params string[] names) {
            var args = Environment.GetCommandLineArgs();
            for (int i = 0; i < args.Length; i++) {
                foreach (var name in names) {
                    if (args[i] == name && args.Length > i + 1) {
                        return args[i + 1];
                    }
                }
            }

            return null;
        }

        private const string TYPE_ARG = "-type";
        private const string IP_ARG = "-ip";
    }

    public enum PlayerType
    {
        UNDEFINED,

        MOBILE,
        VR
    }

    public enum GameType
    {
        DEFAULT,

        CLIENT,
        HOST
    }
}