using UnityEngine;
using FMODUnity;

namespace WeatherOrNot.App
{
    public class FMODEvents : MonoBehaviour
    {
        
        [field: Header("Ambiences")]
        [field: SerializeField] public EventReference clearAmbience { get; private set; }
        [field: SerializeField] public EventReference rainAmbience { get; private set; }
        [field: SerializeField] public EventReference snowAmbience { get; private set; }
        [field: SerializeField] public EventReference thunderstormAmbience { get; private set; }
        [field: SerializeField] public EventReference windyAmbience { get; private set; }


        [field: Header("Music")]
        [field: SerializeField] public EventReference music { get; private set; }


        [field: Header("Player SFX")]
        [field: SerializeField] public EventReference playerFootsteps { get; private set; }


        [field: Header("Orbs SFX")]
        [field: SerializeField] public EventReference orbCollected { get; private set; }


        public static FMODEvents instance { get; private set; }

        private void Awake()
        {
            if (instance != null)
            {
                Debug.LogError("Found more than one FMODEvents in the scene");
            }
            instance = this;
        }
    }
}
