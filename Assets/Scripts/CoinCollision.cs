using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class CoinCollision : MonoBehaviour
{
    [SerializeField]
    private float _maxPitch;

    [SerializeField]
    private GameObject _pickUpNotifVFX;
    [SerializeField]
    private AnimationClip _pickUpAnim;

    private GameManager _gameManager;

    private Random _rand = new Random();


    // Start is called before the first frame update
    void Start()
    {
        _gameManager = FindObjectOfType<GameManager>(); 
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        Coin coin = collision.gameObject.GetComponent<Coin>();

        if (coin == null)
            return;

        AudioManager.Instance.PlaySFX("Coin Pick Up", 0f, 0.1f, _maxPitch);
        
        _gameManager.AddGold();

        StartCoroutine(ActivatePickUpNotif(coin.transform.position));

        
        Destroy(coin.gameObject);
    }

    public IEnumerator ActivatePickUpNotif(Vector2 coinPos)
    {
        int animLayer = _rand.Next(0, 3);

        //GameObject notif = Instantiate(_pickUpNotifVFX, coinPos, Quaternion.identity);
        GameObject notif = Instantiate(_pickUpNotifVFX, this.transform.position , Quaternion.identity);
        notif.transform.localScale = new Vector2 (2,2);
        //notif.transform.localScale = new Vector2 (1,1);

        notif.GetComponent<Animator>().SetLayerWeight(animLayer,1);

        notif.GetComponent<Animator>().CrossFade("gold notif", 0.1f, animLayer);

        yield return new WaitForSeconds(_pickUpAnim.length);

        Destroy(notif);
    }
}
