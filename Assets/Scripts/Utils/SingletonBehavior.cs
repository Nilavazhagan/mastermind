using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mastermind { 
    public class SingletonBehavior <T> : MonoBehaviour where T: SingletonBehavior<T>
    {
        public static T Instance { get; protected set; }

        protected virtual void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this);
                throw new System.Exception("An instance of this singleton already exists.");
            }
            else
            {
                Instance = (T)this;
            }
        }
    }
}