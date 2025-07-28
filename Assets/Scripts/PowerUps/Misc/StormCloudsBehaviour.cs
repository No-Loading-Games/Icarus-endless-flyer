using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StormCloudsBehaviour : MonoBehaviour
{
    public float timer = 0;
    public float speed;
    public float direction;
    public float xPos = 0;

    public float startingPoint;
    public float loopStartingPoint; //The point on the sprite where it starts to repeat the sprite movement
    public float loopReferencePoint;  // The point on the sprite where it goes back to when repeating movement

    public float spriteSize;

    public bool notEnding = true;

    private SpriteRenderer _sprite;
    private Transform _transform;

    // Start is called before the first frame update
    void OnEnable()
    {
        _sprite = GetComponent<SpriteRenderer>();
        _transform = GetComponent<Transform>();
        _transform.position = new Vector2(0, _transform.position.y);
        startingPoint = _transform.position.x;

        spriteSize = GetComponent<SpriteRenderer>().bounds.size.x;
        
        xPos = spriteSize / 2;
        
        _transform.localScale = new Vector2(direction*2f, 2f);

        loopStartingPoint = (0.63f * spriteSize); //63%
        loopReferencePoint = ((0.596f * spriteSize)- (spriteSize /2)) * (-direction); //59.6%
        StartCoroutine(StartMovement());
    }

    public IEnumerator StartMovement()
    {
        float prevXpos = _transform.position.x;
        while (notEnding)
        {
            _transform.Translate(Vector2.right * direction * speed);
            xPos += Mathf.Abs(_transform.position.x - prevXpos);
            prevXpos = _transform.position.x;
            if (xPos > loopStartingPoint)
            {
                _transform.position = new Vector2(loopReferencePoint, _transform.position.y);
                xPos = 0.404f * spriteSize;
            }

            yield return null;
        }

    }

    public IEnumerator DespawnMovement(float duration)
    {
        Color startValue = _sprite.color;
        Color endValue = startValue;
        endValue.a = 0;

        float time = 0;

        while(time < duration)
        {
            _transform.Translate(Vector2.right * direction * speed);
            _sprite.color = Color.Lerp(startValue, endValue, time/duration);
            time += Time.deltaTime;
            yield return null;
        }

        _sprite.color = endValue;
        gameObject.SetActive(false);

        StopAllCoroutines();
    }


}
