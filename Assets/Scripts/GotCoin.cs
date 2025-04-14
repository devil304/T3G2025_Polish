using UnityEngine;

public class GotCoin : MonoBehaviour
{
    int _coinsCollected = 0;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Coin"))
        {
            Debug.Log("Got COIN !!!");
            _coinsCollected++;
            var particles = collision.gameObject.GetComponentInChildren<ParticleSystem>();
            particles.transform.parent = null;
            particles.Play();
            collision.gameObject.SetActive(false);
        }
    }
}
