using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Aurorunnner.Utilities;

namespace Aurorunnner.Stage.Lines
{
    public class LineManager : MonoBehaviour
    {
        // Lineのポジションを保存するList
        public List<GameObject> lineGameObjects = new List<GameObject>();

        // PlayerGameObject
        [SerializeField]
        private GameObject player = null;

        // ポジションを更新する距離
        [SerializeField]
        private float nextLineDistance = 0.5f;

        private List<GameObject> lineGameObjectsPool = new List<GameObject>();
        [SerializeField]
        private GameObject linePrefab = null;
        [SerializeField]
        private Transform lineParent = null;

        private void Awake()
        {
            Locator<LineManager>.Bind(this);
        }

        // Start is called before the first frame update
        void Start()
        {
            GameObject line = GetLineGameObject();
            line.transform.position = player.transform.position;
            lineGameObjects.Add(line);
        }

        // Update is called once per frame
        void Update()
        {
            if ((lineGameObjects[0].transform.position - player.transform.position).magnitude >= nextLineDistance)
            {
                UpdateLines();
            }
        }

        private void UpdateLines()
        {
            GameObject line = GetLineGameObject();
            line.transform.position = player.transform.position;
            lineGameObjects.Insert(0, line);

            for(int i = 2; i < lineGameObjects.Count - 1; i++)
            {
                bool checkCross = CheckLineCross(lineGameObjects[0].transform.position, lineGameObjects[1].transform.position, lineGameObjects[i].transform.position, lineGameObjects[i + 1].transform.position);
                if(checkCross)
                {
                    lineGameObjects.Clear();
                    ClearLineGameObjects();
                    StartCoroutine(Cross());
                    GameObject firstLine = GetLineGameObject();
                    firstLine.transform.position = player.transform.position;
                    lineGameObjects.Add(firstLine);
                    break;
                }
            }
        }

        /// <summary>
        /// 線分ABと線分CDの交差判定（直線の方程式より）
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="c"></param>
        /// <param name="d"></param>
        /// <returns></returns>
        private bool CheckLineCross(Vector3 a, Vector3 b, Vector3 c, Vector3 d)
        {
            float s, t;

            s = (a.x - b.x) * (c.y - a.y) - (a.y - b.y) * (c.x - a.x);
            t = (a.x - b.x) * (d.y - a.y) - (a.y - b.y) * (d.x - a.x);
            if (s * t > -0.0001f)
                return false;

            s = (c.x - d.x) * (a.y - c.y) - (c.y - d.y) * (a.x - c.x);
            t = (c.x - d.x) * (b.y - c.y) - (c.y - d.y) * (b.x - c.x);
            if (s * t > -0.0001f)
                return false;
            return true;
        }

        public GameObject GetLineGameObject()
        {
            foreach (GameObject line in lineGameObjectsPool)
            {
                if (line.activeSelf == false)
                {
                    line.SetActive(true);
                    return line;
                }
            }

            var newLine = Instantiate(linePrefab, lineParent);
            lineGameObjectsPool.Add(newLine);
            return newLine;
        }

        public void DestroyLineGameObject(GameObject line)
        {
            lineGameObjects.Remove(line);
            line.SetActive(false);
        }

        public void ClearLineGameObjects()
        {
            foreach (GameObject line in lineGameObjectsPool)
            {
                if (line.activeSelf == true)
                {
                    line.SetActive(false);
                }
            }

        }

        [SerializeField]
        private GameObject CrossUI = null;
        IEnumerator Cross()
        {
            CrossUI.SetActive(true);
            yield return new WaitForSeconds(0.5f);
            CrossUI.SetActive(false);

        }

    }
}
