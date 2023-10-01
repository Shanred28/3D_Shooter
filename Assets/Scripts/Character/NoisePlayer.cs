using System.Collections.Generic;
using UnityEngine;

public class NoisePlayer : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private SpaceSoldier soldier;

    private List<Destructible> enemyTeam;

    private void Start()
    {
        enemyTeam = Destructible.GetAllNonTeamMember(soldier.TeamId);
    }

    private void Update()
    {
        if (audioSource.isPlaying == true)
        {
            ApllyNoise();
        }
    }

    private void ApllyNoise()
    {
        foreach (Destructible dest in enemyTeam)
        {
            AiAlienSoldier ai = dest.transform.root.GetComponent<AiAlienSoldier>();

            if (ai != null && ai.enabled == true)
            {
                ai.ApplyHearling(transform.position);
            }
        }
        
    }
}
