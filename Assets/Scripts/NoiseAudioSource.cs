using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class NoiseAudioSource : MonoBehaviour
{
    [SerializeField] private float _maxDistanceHeart; 

    private AudioSource _audioSource;
    public AudioClip audioClip { get { return _audioSource.clip; } set { _audioSource.clip = value; } }
    public void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public void Play()
    {
        _audioSource.Play();
        foreach (var dest in Destructible.AllDestructibles)
        {
            if (dest is ISoundListener == true)
            {
                float dist = Vector3.Distance(transform.position, dest.transform.position);
                if (dist < _maxDistanceHeart)
                {
                    (dest as ISoundListener).Heard(dist);
                }               
            }
        }
    }
}
