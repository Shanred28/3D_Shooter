using UnityEngine;

[System.Serializable]
public class FootStepProperties
{
    public float speed;
    public float delay;
}


public class FootStepSound : MonoBehaviour
{
    [SerializeField] private FootStepProperties[] _properties;
    [SerializeField] private CharacterController _characterController;

    private AudioSource _audioSource;
    private float _delay;
    private float _tick;
    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (IsPlay() == false) 
        {
            _tick = 0;
            return;
        } 

        _tick += Time.deltaTime;
        _delay = GetDelayBySpeed(GetSpeed());

        if (_tick >= _delay)
        {
            _audioSource.Play();
            _tick = 0;
        }
    }

    private float GetSpeed()
    {
        return _characterController.velocity.magnitude;
    }

    private float GetDelayBySpeed(float speed)
    {
        for (int i = 0; i < _properties.Length; i++)
        {
            if (speed <= _properties[i].speed)
            {
                return _properties[i].delay;
            }
        }
        return _properties[_properties.Length - 1].delay;
    }

    private bool IsPlay()
    { 
      if(GetSpeed() < 0.01f || _characterController.isGrounded == false) 
            return false;
        else
            return true;
    }
}
