using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{

    [SerializeField]
    private float _speed = 3.0f;

    [SerializeField]
    private int _powerupID = 0;

    [SerializeField]
    private AudioClip _clip;


    enum Powerups
    {
        TripleShot,
        Speed,
        Shields,
    }


    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);
        
        if (transform.position.x <= -5.6f)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag.Equals("Player"))
        {
            Player player = other.transform.GetComponent<Player>();

            if (player != null)
            {
                switch (_powerupID)
                {
                    case (int)Powerups.TripleShot:
                        player.TripleShotActivate();
                        break;
                    case (int)Powerups.Speed:
                        player.SpeedBoostActivate();
                        break;
                    case (int)Powerups.Shields:
                        player.ShieldsActivate();
                        break;
                }
            }

            AudioSource.PlayClipAtPoint(_clip, transform.position);

            Destroy(this.gameObject);
            
        }
    }

}
