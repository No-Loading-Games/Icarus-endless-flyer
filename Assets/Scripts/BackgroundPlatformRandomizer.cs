using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class BackgroundPlatformRandomizer : MonoBehaviour
{
    private Random rand = new Random();

    [SerializeField]
    private List<Sprite> _bgPlatformSprites;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<SpriteRenderer>().sprite = _bgPlatformSprites[rand.Next(_bgPlatformSprites.Count)];
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
