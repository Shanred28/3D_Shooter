using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


    /// <summary>
    /// Destructible object on scene.
    /// </summary>
    public class Destructible : Entity
    {
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
        public void ApplyDamage(int damage)
        {
            if (m_Indestructible) return;

            m_CurrentHitPoints -= damage;
            ChangeHp.Invoke();
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

        [SerializeField] private int m_TeamId;
        public int TeamId => m_TeamId;

        [SerializeField] private UnityEvent m_EventOnDeath;
        public UnityEvent EventOnDeath => m_EventOnDeath;

        #region Score
        [SerializeField] private int m_ScoreValue;
        public int ScoreValue => m_ScoreValue;

        #endregion
    
}

