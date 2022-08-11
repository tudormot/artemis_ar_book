using UnityEngine;

namespace BookAR.Scripts.AssetControl.Common.ARLabels
{
    public class LineDrawer
    {
        private LineRenderer lineRenderer;
        private float lineSize;

        public LineDrawer( string uniqueTag, Material lineMaterial, float lineSize = 0.001f)
        {
            Debug.Log($"LineDrawer was create! uniquetag : {uniqueTag}");
            GameObject lineObj = new GameObject(uniqueTag);
            lineObj.transform.parent = GameObject.FindGameObjectWithTag("DynamicObjects").transform;
            if (lineObj == null) {
                Debug.Log("how the fuck is lineObj null?");
            }
            lineRenderer = lineObj.AddComponent<LineRenderer>();
            lineRenderer.material = lineMaterial;
            this.lineSize = lineSize;
            SetActive(false);

        }


        //Draws lines through the provided vertices
        public void DrawLineInGameView(Vector3 start, Vector3 end)
        {

            SetActive(true);

            //Set width
            lineRenderer.startWidth = lineSize;
            lineRenderer.endWidth = lineSize;

            //Set line count which is 2
            lineRenderer.positionCount = 2;

            //Set the postion of both two lines
            lineRenderer.SetPosition(0, start);
            lineRenderer.SetPosition(1, end);
        }

        public void SetActive(bool active) {
            // Debug.Log($"SetActive {active} . {lineRenderer.gameObject.name}");
            lineRenderer.gameObject.SetActive(active);
            
        }

        public void Destroy()
        {
            if (lineRenderer != null)
            {
                GameObject.Destroy(lineRenderer.gameObject);
            }
        }
    
    }
}