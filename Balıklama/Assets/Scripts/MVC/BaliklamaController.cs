using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;
using PrimeTween;
using UnityEngine.Events;


public class BaliklamaController : MonoBehaviour
{
    public UnityAction<List<Bubble> ,int, int, int> LevelStarted;
    public UnityAction<List<Bubble>> PlayTheLevel;
    [SerializeField] private FishController fishController;
    [SerializeField] private BaliklamaView baliklamaView;

    private BaliklamaModel _gModel;
    private GameData _gameData;
    private int _gameStartCountDown;
    private List<Bubble> _bubbles;
    
    
    [SerializeField] public int level;
    
    private void Start()
    {
        _bubbles = new List<Bubble>();
        fishController.HitBubble += () => { Debug.Log("Hit bubble"); };
        fishController.UserReady += () => { baliklamaView.PlayTheLevel(); };
        level = 1;
        string json = Path.Combine(Application.streamingAssetsPath, "GameParams.json");
        _gModel = new BaliklamaModel();

        if (!File.Exists(json))
        {
            Debug.LogError("JSON file not found at path: " + json);
            return;
        }
        string jsonText = File.ReadAllText(json);
        _gameData = JsonConvert.DeserializeObject<GameData>(jsonText);
        GetLevelData(_gameData, level);
    }

    private void GetLevelData(GameData gameData, int level)
    {
        _gModel.Initialise(gameData, level);
        LevelStart();
    }

    private void LevelStart()
    {
        Debug.Log("Level started");
        LevelStarted?.Invoke(_bubbles ,_gModel.bubbleCount, _gModel.spawnCount,_gModel.speed);
        StartCountDown();
    }

    private void StartCountDown()
    {
        _gameStartCountDown = 3;
        Tween handle = Tween.Custom(
            startValue: _gameStartCountDown,
            endValue: -1,
            duration: _gameStartCountDown,
            onValueChange: value => { baliklamaView.ChangeCountDownText(Mathf.RoundToInt(value)); },
            Ease.Linear
        );
        handle.OnComplete(() =>
        {
            PlayTheLevel?.Invoke(_bubbles);
        });
    }
}

[System.Serializable]
public class GameData
{
    public List<int> bubbleCount;
    public List<int> spawnCount;
    public List<int> speed;
}
