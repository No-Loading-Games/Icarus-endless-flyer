using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class NotificationBarUI : MonoBehaviour
{
    public Image itemImage;
    public TextMeshProUGUI description;

    private RectTransform _rectTransform;

    private GameManager _gameManager;
    void Start()
    {
        _rectTransform = GetComponent<RectTransform>();
        _gameManager = FindObjectOfType<GameManager>();
    }

    public void ShowNotification(Sprite sprite, string desc)
    {

        itemImage.sprite = sprite;
        description.text = desc;

        _rectTransform.DOMoveY(-4.781f, 1).OnComplete(() =>
        {
            _rectTransform.DOMoveY(_rectTransform.position.y, 2).OnComplete(() =>
            {
                _rectTransform.DOMoveY(-5.558f, 1);
                _gameManager.DeactNotificationBar();
            });
        });
    }

}
