using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static UnityEngine.GraphicsBuffer;


/// <summary>
/// Destructible object on scene.
/// </summary>
    public class Destructible : Entity, ISerializableEntity
    {
    [SerializeField] private UnityEvent _eventOnGetDamage;
    public UnityAction<Destructible> OnGetDamage; 

        #region Properties
        /// <summary>
        /// Object ignores damage.
        /// </summary>
        [SerializeField] protected bool m_Indestructible;
        public bool IsIndestructible => m_Indestructible;

        /// <summary>
        /// Starting quantity hitponts.
        /// </summary>
        [SerializeField] private int m_HitPoints;

        /// <summary>
        /// Current hitpoints.
        /// </summary>
        private int m_CurrentHitPoints;
        public int CurrentHitPoints => m_CurrentHitPoints;

        public int MaxHitPoints => m_HitPoints;

     [SerializeField] private int m_TeamId;
    public int TeamId => m_TeamId;

    [SerializeField] private UnityEvent m_EventOnDeath;
    public UnityEvent EventOnDeath => m_EventOnDeath;

    #endregion

    #region Unity Events
    protected virtual void Start()
        {
            m_CurrentHitPoints = m_HitPoints;
        }
        #endregion

        #region Public API

        public UnityEvent ChangeHp;
        /// <summary>
        /// Applying damage to an object.
        /// </summary>
        /// <param name="damage"> Damage apply object</param>
        public void ApplyDamage(int damage, Destructible other)
        {
            if (m_Indestructible) return;

            m_CurrentHitPoints -= damage;
            ChangeHp.Invoke();
            OnGetDamage?.Invoke(other);
            _eventOnGetDamage?.Invoke();


            if (m_CurrentHitPoints <= 0)
                OnDeath();
        }

    public void ApplyHill(int heal)
    {
        m_CurrentHitPoints += heal;
        if (m_CurrentHitPoints > m_HitPoints)
            m_CurrentHitPoints = m_HitPoints;
    }
    public void HealFull()
    {
        m_CurrentHitPoints = m_HitPoints;
        ChangeHp.Invoke();
    }

    public void SetHitPoint(int hitPoint)
    {
        m_CurrentHitPoints = Mathf.Clamp(hitPoint, 0, m_HitPoints);
    }
        #endregion

    #region Methods
    /// <summary>
    /// Overriding the object destruction event if the hitpoint is below zero.
    /// </summary>
        public bool  IsDestroy = false;
        protected virtual void OnDeath()
        {
            m_EventOnDeath?.Invoke();
            IsDestroy = true;
            Destroy(gameObject);
        }
    #endregion

    public static Destructible FindNearest(Vector3 position)
    { 
        float minDist = float.MaxValue;
        Destructible target = null;

        foreach (Destructible dest in m_AllDestructibles)
        {
            float curDist = Vector3.Distance(dest.transform.position, position);

            if (curDist < minDist)
            { 
                minDist = curDist;
                target = dest;
            }
        }
        return target;
    }

    public static Destructible FindNearestNonTiamMember(Destructible destructible)
    {
        float minDist = float.MaxValue;
        Destructible target = null;

        foreach (Destructible dest in m_AllDestructibles)
        {
            float curDist = Vector3.Distance(dest.transform.position, destructible.transform.position);

            if (curDist < minDist && destructible.TeamId != dest.TeamId)
            {
                minDist = curDist;
                target = dest;
            }
        }
        return target;
    }

    public static Destructible FindPlayer()
    {
        Destructible target = null;

        foreach (Destructible dest in m_AllDestructibles)
        {
            if(dest.m_TeamId == 0)
                target = dest;
        }
        return target;
    }

    public static List<Destructible> GetAllTeamMember(int teamId)
    { 
        List<Destructible> teamDestructible = new List<Destructible>();

        foreach (Destructible dest in m_AllDestructibles)
        { 
            if(dest.TeamId == teamId)
                teamDestructible.Add(dest);
        }
        return teamDestructible;
    }

    public static List<Destructible> GetAllNonTeamMember(int teamId)
    {
        List<Destructible> teamDestructible = new List<Destructible>();

        foreach (Destructible dest in m_AllDestructibles)
        {
            if (dest.TeamId != teamId)
                teamDestructible.Add(dest);
        }
        return teamDestructible;
    }

        private static HashSet<Destructible> m_AllDestructibles;
        public static IReadOnlyCollection<Destructible> AllDestructibles => m_AllDestructibles;

        protected virtual void OnEnable()
        {
            if (m_AllDestructibles == null)
                m_AllDestructibles = new HashSet<Destructible>();

            m_AllDestructibles.Add(this);
        }

        protected virtual void OnDestroy()
        {
            m_AllDestructibles.Remove(this);
        }



    public const int TeamIdNeutral = 0;


        #region Score
        [SerializeField] private int m_ScoreValue;
        public int ScoreValue => m_ScoreValue;



    #endregion


    // Serialize
    [System.Serializable]
    public class State
    {
        public Vector3 position;
        public int hitPoins;

        public State() { }
    }

    [SerializeField] private int m_EntityId;
    public long EntityId => m_EntityId;
    public virtual bool IsSerializable()
    {
        return m_CurrentHitPoints > 0;
    }

    public virtual string SerializeState()
    {
        State s = new State();

        s.position = transform.position;
        s.hitPoins = m_CurrentHitPoints;

        return JsonUtility.ToJson(s);
    }

    public virtual void DeserializeState(string state)
    {
        State s = JsonUtility.FromJson<State>(state);

        transform.position = s.position;
        m_HitPoints = s.hitPoins;
    }

    protected void SetIdTeam(int idTeeam)
    {
        m_TeamId = idTeeam;
    }
}

