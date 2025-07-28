 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heart : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3f;

    private BoxCollider2D _collider;

    private float _direction = 1f;
    // Start is called before the first frame update
    void Start()
    {
        _collider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("Screen size: " + Screen.width + ", " + Screen.height);
        if (transform.position.x > 0 && (transform.position.x >= Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height)).x - 0.25f))
        {
            _direction = -1f;
        }
        else if (transform.position.x < 0 && (transform.position.x <= Camera.main.ScreenToWorldPoint(new Vector2(0, 0)).x + 0.25f))
        {
            _direction = 1f;
        }

        transform.Translate(_speed * Time.deltaTime * new Vector2(_direction, -0.3f));
    }

/*    private void OnCollisionEnter(Collision collision)
    {

        PlayerController player = collision.gameObject.GetComponent<PlayerController>();

        if(player == null)
        {
            return;
        }

        HeartManager heartManager = FindObjectOfType<HeartManager>();

        heartManager.SetHearts(heartManager.GetHearts() + 1);
        Destroy(this.gameObject);
    }*/
}
