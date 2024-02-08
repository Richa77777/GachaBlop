using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class IdenticalBallsTouch : MonoBehaviour
{
    [SerializeField] private ParticleSystem _popParticles;
    [SerializeField] private string _ballsLayer = "Balls";

    private SpriteRenderer _ballSpriteRenderer;
    private bool _isBlocked = false;

    public ParticleSystem PopParticles { get => _popParticles; }
    public SpriteRenderer BallSpriteRenderer { get => _ballSpriteRenderer; }
    public bool IsBlocked { get => _isBlocked; set => _isBlocked = value; }


    private void Awake()
    {
        _ballSpriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (gameObject.layer != LayerMask.NameToLayer(_ballsLayer))
        {
            gameObject.layer = LayerMask.NameToLayer(_ballsLayer);
        }

        IdenticalBallsTouch identicalBallsTouch;

        if (collision.gameObject.TryGetComponent(out identicalBallsTouch))
        {
            if (identicalBallsTouch.BallSpriteRenderer.sprite == _ballSpriteRenderer.sprite)
            {
                if (_isBlocked == false && identicalBallsTouch.IsBlocked == false)
                {
                    Vector3 positionBetweenBalls = Vector3.zero;
                    Vector2 contactNormal = collision.contacts[0].normal;
                    float angle = Mathf.Atan2(contactNormal.y, contactNormal.x) * Mathf.Rad2Deg;

                    if (angle > -45 && angle <= 45)
                    {
                        _isBlocked = true;
                        identicalBallsTouch.IsBlocked = true;

                        float thisX = transform.position.x;
                        float collisionX = collision.transform.position.x;
                        float[] values = { thisX, collisionX };

                        positionBetweenBalls = new Vector3(values.Max() / 2 + values.Min() / 2, transform.position.y, transform.position.z);
                        BallsSpawner.Instance.SpawnCombinedBall(positionBetweenBalls, gameObject);

                        Destroy(collision.gameObject);
                        Destroy(this.gameObject);
                    }
                    else if (angle > -135 && angle <= -45)
                    {
                        _isBlocked = true;
                        identicalBallsTouch.IsBlocked = true;

                        float thisY = transform.position.y;
                        float collisionY = collision.transform.position.y;
                        float[] values = { thisY, collisionY };

                        positionBetweenBalls = new Vector3(transform.position.x, values.Max() / 2 + values.Min() / 2, transform.position.z);
                        BallsSpawner.Instance.SpawnCombinedBall(positionBetweenBalls, gameObject);

                        Destroy(collision.gameObject);
                        Destroy(this.gameObject);
                    }
                }
            }
        }
    }
}