using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Aurorunnner.Stage.Players
{
    public class PlayerCore : MonoBehaviour
    {
        [SerializeField]
        private PlayerParameters DefaultParameters = new PlayerParameters();
        private PlayerParameters currentParameters;
        public PlayerParameters CurrentParameters
        {
            get { return currentParameters; }
        }
        
        private PlayerState currentState = PlayerState.Walking;
        public PlayerState CurrentState
        {
            get { return currentState; }
            set { currentState = value; }
        }

        // Start is called before the first frame update
        void Start()
        {
            currentParameters = DefaultParameters;
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
