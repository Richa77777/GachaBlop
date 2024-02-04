using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class IdenticalBallsTouch : MonoBehaviour
{
    [SerializeField] private ParticleSystem _popParticles;

    private Sprite _ballSprite;
    
    public ParticleSystem PopParticles { get => _popParticles; }

    private void Awake()
    {
        _ballSprite = GetComponent<SpriteRenderer>().sprite;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        SpriteRenderer collisionSpriteRenderer;

        if (collision.gameObject.TryGetComponent(out collisionSpriteRenderer))
        {
            if (collisionSpriteRenderer.sprite == _ballSprite)
            {
                Vector3 positionBetweenBalls = Vector3.zero;

                if (collision.relativeVelocity.x != 0)
                {
                    float thisX = transform.position.x;
                    float collisionX = collision.transform.position.x;
                    float[] values = { thisX, collisionX };

                    if (collision.relativeVelocity.x > 0)
                    {
                        positionBetweenBalls = new Vector3(values.Max() / 2 + values.Min() / 2, transform.position.y, transform.position.z);
                        BallsSpawner.Instance.SpawnCombinedBall(positionBetweenBalls, gameObject);
                    }
                }
                else if (collision.relativeVelocity.y != 0)
                {
                    float thisY = transform.position.y;
                    float collisionY = collision.transform.position.y;
                    float[] values = { thisY, collisionY };

                    if (collision.relativeVelocity.y > 0)
                    {
                        positionBetweenBalls = new Vector3(transform.position.x, values.Max() / 2 + values.Min() / 2, transform.position.z);
                        BallsSpawner.Instance.SpawnCombinedBall(positionBetweenBalls, gameObject);
                    }
                }
                collision.gameObject.SetActive(false);
                this.gameObject.SetActive(false);
            }
        }
    }
}