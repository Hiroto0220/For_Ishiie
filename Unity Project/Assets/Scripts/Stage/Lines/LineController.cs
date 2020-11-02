using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Aurorunnner.Utilities;

namespace Aurorunnner.Stage.Lines
{
    public class LineController : MonoBehaviour
    {
        [SerializeField]
        private float livingSecond = 5f;

        private float livingTimeCount = 0;

        LineManager lineManager;

        private void Start()
        {
            lineManager = Locator<LineManager>.Instance;
        }

        private void OnEnable()
        {
            livingTimeCount = 0;
        }

        // Update is called once per frame
        void Update()
        {
            livingTimeCount += Time.deltaTime;
            if (livingTimeCount >= livingSecond && lineManager.lineGameObjects[0] != this.gameObject)
            {
                lineManager.DestroyLineGameObject(this.gameObject);
            }
        }
    }
}