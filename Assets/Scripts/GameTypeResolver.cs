using System;
using OOO.Utils;
using UnityEngine;
using UnityEngine.XR;


namespace OOO
{
    public class GameTypeResolver : MonoBehaviour
    {
        [SerializeField, Help("Add default value so that the mods are usable in the editor play.")]
        public GameType type = default;
        [SerializeField, Help("Add default IP address to look for.")]
        public string ip = default;
        
        public static GameTypeResolver Instance = null;

        private void Awake()
        {
            if (Instance == null) {
                type = ParseGameType();
                ip = ParseIP();

                XRSettings.enabled = (type == GameType.VR);

                Instance = this;
                
                EventHub.Instance.FireEvent(new GameTypeLoadedEvent());
                
                DontDestroyOnLoad(gameObject);
            }
            else if (Instance != this) {
                DestroyImmediate(gameObject);
            }
        }

        private GameType ParseGameType()
        {
            var parseResult = Enum.TryParse(GetArg(TYPE_ARG), out GameType t);
            return parseResult ? t : type;
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

    public enum GameType
    {
        UNDEFINED,

        MOBILE,
        VR
    }
}