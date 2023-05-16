using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    [SerializeField]
    private GameObject _laserPrefab;

    [SerializeField]
    private GameObject _tripeShotPrefab;

    [SerializeField]
    private GameObject _leftEngine;

    [SerializeField]
    private GameObject _rightEngine;

    [SerializeField]
    private GameObject _shieldVisualizer;

    [SerializeField]
    private GameObject _laserAudioObject;
    private AudioSource _laserAudioSource;

    private SpawnManager _spawnManager;
    private UIManager _uiManager;

    [SerializeField]
    private float _speed = 3.5f;
    [SerializeField]
    private float _speedMultiplier = 2f;

    [SerializeField]
    private float _fireRate = 0.15f;
    private float canFire = -1.0f;

    [SerializeField]
    private int _lives = 3;

    [SerializeField]
    private bool _isShieldActive = false;


    [SerializeField]
    private bool _isTripleShotActive = false;

    private int _score = 0;



    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(0, 0, 0);
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();

        _leftEngine.SetActive(false);
        _rightEngine.SetActive(false);

        _laserAudioSource = _laserAudioObject.GetComponent<AudioSource>();

        if (_laserAudioSource == null)
        {
            Debug.LogError("Laser Audio is NULL");
        }

        if (_spawnManager == null)
        {
            Debug.LogError("Spawn Manager is NULL");
        }

        if (_uiManager == null)
        {
            Debug.LogError("UI Manager is NULL");
        }

    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();

        if (Input.GetKeyDown(KeyCode.Space) && Time.time > canFire) 
        {
            FireLaser();
        }

    }

    void CalculateMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);


        transform.Translate(direction * _speed * Time.deltaTime);


        if (transform.position.x >= 11.3f)
        {
            transform.position = new Vector3(-11.3f, transform.position.y, 0);
        }
        else if (transform.position.x <= -11.3f)
        {
            transform.position = new Vector3(11.3f, transform.position.y, 0);
        }

        if (transform.position.y >= 7.6)
        {
            transform.position = new Vector3(transform.position.x, -5.6f, 0);
        }
        else if (transform.position.y <= -5.6f)
        {
            transform.position = new Vector3(transform.position.x, 7.6f, 0);
        }
    }

    void FireLaser()
    {
        canFire = Time.time + _fireRate;

        if (_isTripleShotActive)
        {
            Instantiate(_tripeShotPrefab,transform.position, Quaternion.identity);
        }
        else
        {
            Instantiate(_laserPrefab, transform.position + new Vector3(0, 0.8f, 0), Quaternion.identity);
        }

        _laserAudioSource.Play();

    }

    public void Damage()
    {
        if (_isShieldActive)
        {
            _isShieldActive = false;
            _shieldVisualizer.SetActive(false);
            return;
        }


        _lives -= 1;

        if (_lives == 2)
        {
            _leftEngine.SetActive(true);
        } else if (_lives == 1)
        {
            _rightEngine.SetActive(true);
        }


        if (_lives < 1)
        {
            _spawnManager.OnPlayerDeath();
            Destroy(this.gameObject);
        }
        _uiManager.UpdateLives(_lives);
    }

    public void TripleShotActivate()
    {
        _isTripleShotActive = true;
        StartCoroutine(TripleShotPowerDownRoutine());
    }

    IEnumerator TripleShotPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _isTripleShotActive = false;
    }

    public void SpeedBoostActivate()
    {
        _speed *= _speedMultiplier;
        StartCoroutine(SpeedBoostPowerDownRoutine());
    }

    IEnumerator SpeedBoostPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _speed /= _speedMultiplier;
    }

    public void ShieldsActivate()
    {
        _isShieldActive = true;
        _shieldVisualizer.SetActive(true);
    }

    public int getScore()
    {
        return _score;
    }

    public void addToScore(int number)
    {
        _score += number;
        _uiManager.UpdateScore(_score);
    }

}
