using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DumbyActionIndicator : MonoBehaviour {

    public Sprite MovingSprite;
    public Sprite SearchingSprite;
    public Sprite FightingSprite;

    SpriteRenderer spriteRenderer;

    Status _status;

    public Status Status {
        get { return _status; }
        set
        {
            _status = value;
            switch (_status)
            {
                case Status.Moving:
                    spriteRenderer.sprite = MovingSprite;
                    break;
                case Status.Searching:
                    spriteRenderer.sprite = SearchingSprite;
                    break;
                case Status.Fighting:
                    spriteRenderer.sprite = FightingSprite;
                    break;
                default:
                    break;
            }
        }
    }

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
}
    public enum Status
    {
        Moving,
        Searching,
        Fighting,
    }
