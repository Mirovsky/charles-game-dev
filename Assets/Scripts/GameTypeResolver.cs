using System;
using OOO.Utils;
using UnityEngine;

namespace OOO
{
    public class GameTypeResolver : MonoBehaviour
    {
        [Tooltip("Add default value so that the mods are usable in the editor play.")] [SerializeField]
        public GameType type = default;
        
        public static GameTypeResolver Instance = null;

        private void Awake() {
            if (Instance == null) {
                type = ParseGameType(); 
                Instance = this;
                
                EventHub.Instance.FireEvent(new GameTypeLoadedEvent());
                
                DontDestroyOnLoad(gameObject);
            }
            else if (Instance != this) {
                DestroyImmediate(gameObject);
            }
        }

        private GameType ParseGameType() {
            var parseResult = Enum.TryParse(GetArg(TYPE_ARG), out GameType t);
            return parseResult ? t : type;
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
    }

    public enum GameType
    {
        UNDEFINED,

        MOBILE,
        VR
    }
}