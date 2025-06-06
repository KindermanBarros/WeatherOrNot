using UnityEngine;
using FMODUnity;

namespace WeatherOrNot.App
{
    public class FMODEvents : MonoBehaviour
    {
        
        [field: Header("Ambience")]
        [field: SerializeField] public EventReference ambience { get; private set; }


        [field: Header("Music")]
        [field: SerializeField] public EventReference music { get; private set; }


        [field: Header("Player SFX")]
        [field: SerializeField] public EventReference playerFootsteps { get; private set; }
        [field: SerializeField] public EventReference playerJump { get; private set; }
        [field: SerializeField] public EventReference playerLand { get; private set; }
        [field: SerializeField] public EventReference playerDeath { get; private set; }


        [field: Header("Orbs SFX")]
        [field: SerializeField] public EventReference orbCollected { get; private set; }
        [field: SerializeField] public EventReference orbActivation { get; private set; }


        [field: Header("UI")]
        [field: SerializeField] public EventReference confirmButton { get; private set; }
        [field: SerializeField] public EventReference cancelButton { get; private set; }


        public static FMODEvents instance { get; private set; }

        private void Awake()
        {
            if (instance != null)
            {
                Debug.LogError("Found more than one FMODEvents in the scene");
            }
            instance = this;

            DontDestroyOnLoad(gameObject);
        }
    }
}
