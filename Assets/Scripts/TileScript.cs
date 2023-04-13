using UnityEngine;
using UnityEngine.Serialization;

public class TileScript : MonoBehaviour
{
    public Vector3 TargetPosition { get; set; }
    public Vector3 CorrectPosition { get; private set; }

    [SerializeField] private float lerpSpeed;

    public int tileNumber;
    public bool inRightPlace;
    
    private SpriteRenderer _sprite;
    
    private const float Precision = 0.0001f;

    private void Awake()
    {
        TargetPosition = transform.position;
        CorrectPosition = TargetPosition;
        _sprite = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        //Change position
        if (transform.position != TargetPosition)
        {
            transform.position = Vector3.Lerp(transform.position, TargetPosition, lerpSpeed);
        }

        //Change color if correct position
        if (Vector3.Distance(TargetPosition, CorrectPosition) < Precision)
        {
            _sprite.color = Color.green;
            inRightPlace = true;
        }
        else
        {
            _sprite.color = Color.white;
            inRightPlace = false;
        }
    }
}
