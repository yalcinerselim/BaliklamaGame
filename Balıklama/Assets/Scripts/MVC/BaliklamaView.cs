using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PrimeTween;

public class BaliklamaView : MonoBehaviour
{
    [SerializeField] private GameObject fish;
    [SerializeField] private List<GameObject> bubblePrefabs;
    [SerializeField] private List<GameObject> sandSpawnPoints;
    [SerializeField] private List<GameObject> waterSpawnPoints;
    [SerializeField] private BaliklamaController controller;
    [SerializeField] private GameObject sceneParent;
    [SerializeField] private Text countDownText;
    [SerializeField] private Bubble bubble;
    [SerializeField] private Hand hand;
    [SerializeField] private Text timer;
    
    
    private List<Bubble> _bubbles = new List<Bubble>();
    private int _speed;
    private Tween _tween;

    private void Awake()
    {
        controller.LevelStarted += SetValues;
        controller.PlayTheLevel += SetTheScene;
    }

    private void Start()
    {
        fish.GetComponent<Collider>().enabled = false;
    }

    private void SetTheScene(List<Bubble> Bubbles)
    {
        _bubbles = Bubbles;
        foreach (var bubble in Bubbles)
        {
            bubble.PlayCreateAnimation();
            bubble.OutOfScreen += DestroyBubble;
        }
        fish.GetComponent<Collider>().enabled = true;
        hand.gameObject.SetActive(true);
        hand.PlayHandAnimation();
    }
    
    public void PlayTheLevel()
    {
        hand.gameObject.SetActive(false);
        foreach (var bubble in _bubbles)
        {
            bubble.MoveUp(_speed);
        }
        StartTimer();
    }

    private void StartTimer()
    {
        Tween handle = Tween.Custom(
            startValue: 90f,
            endValue: 0,
            duration: 90f,
            onValueChange: value =>
            {
                timer.text = Mathf.CeilToInt(value).ToString();
            },
            ease: Ease.Linear
        );
        handle.OnComplete(() =>
        {
            countDownText.text = "Game Over";
        });
    }

    private void DestroyBubble(Bubble bubble)
    {
        bubble.OutOfScreen -= DestroyBubble;
        _bubbles.Remove(bubble);
        bubble.PlayDestroyAnimation();
        _tween = Tween.Delay(1f).OnComplete(bubble.DestroyBubble);
    }

    private void SetValues(List<Bubble> bubbles, int bubbleCount, int spawnCount, int speed)
    {
        for (var i = 0; i < bubbleCount; i++)
        {
            GameObject bubbleInstance = Instantiate(bubblePrefabs[0], sceneParent.transform);
            if (spawnCount > 0)
            {
                bubbleInstance.transform.position = sandSpawnPoints[i].transform.position;
                spawnCount -= 1;
            }
            else
            {
                bubbleInstance.transform.position = waterSpawnPoints[i].transform.position;
            }
            Bubble bubble = bubbleInstance.GetComponent<Bubble>();
            bubbles.Add(bubble);
        }
        _speed =  speed;
    }

    public void ChangeCountDownText(int value)
    {
        if (value == 0)
        {
            countDownText.text = "GO!";
        }
        else if (value == -1)
        {
            countDownText.text = "";
        }
        else
        {
            countDownText.text = value.ToString();
        }
    }
}
