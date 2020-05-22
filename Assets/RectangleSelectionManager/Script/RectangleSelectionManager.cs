using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mugitea.RectangleSelection
{
    [RequireComponent(typeof(LineRenderer))]
    public class RectangleSelectionManager : MonoBehaviour
    {

        private Vector3 mDragStartPos;
        private Vector3 mDragEndPos;

        [SerializeField] private float mSelectableDirection = 1f;

        private LineRenderer mLineRenderer;

        // Use this for initialization
        private void Awake()
        {
            mLineRenderer = GetComponent<LineRenderer>();

            mLineRenderer.positionCount = 5;
            mLineRenderer.startWidth = 0.05f;
            mLineRenderer.endWidth = 0.05f;

            mLineRenderer.enabled = false;
        }

        // Update is called once per frame
        private void Update()
        {
            DetectionDrag();
        }

        private void DetectionDrag()
        {
            if (Input.GetMouseButtonDown(0))
            {
                DragStart();
                mDragStartPos = Input.mousePosition;
                mDragStartPos = Camera.main.ScreenToWorldPoint(mDragStartPos);
                mDragStartPos.z = Camera.main.transform.position.z + 10f;
            }

            if (Input.GetMouseButton(0))
            {
                mDragEndPos = Input.mousePosition;
                mDragEndPos = Camera.main.ScreenToWorldPoint(mDragEndPos);
                mDragEndPos.z = Camera.main.transform.position.z + 10f;

                DrawRectangleSelectionArea();
            }

            if (Input.GetMouseButtonUp(0))
            {
                DragEnd();
                RectangleSelection();
            }

        }

        private GameObject[] RectangleSelection()
        {

            Collider[] colliders = Physics.OverlapBox((mDragStartPos + mDragEndPos) / 2f, new Vector3(Mathf.Abs(Mathf.Abs(mDragStartPos.x) - Mathf.Abs(mDragEndPos.x)), Mathf.Abs(Mathf.Abs(mDragStartPos.y) - Mathf.Abs(mDragEndPos.y)), Mathf.Abs(mSelectableDirection)) / 2f);
            GameObject[] gameObjects = new GameObject[colliders.Length];

            for(int index = 0;index < colliders.Length; index++) {

                gameObjects[index] = colliders[index].gameObject;
                print("範囲内のオブジェクト: " + gameObjects[index].name);

            }

            return gameObjects;
        }

        private void DrawRectangleSelectionArea()
        {
            mLineRenderer.SetPosition(0, mDragStartPos);
            mLineRenderer.SetPosition(1, new Vector3(mDragEndPos.x, mDragStartPos.y, mDragStartPos.z));
            mLineRenderer.SetPosition(2, new Vector3(mDragEndPos.x, mDragEndPos.y, mDragStartPos.z));
            mLineRenderer.SetPosition(3, new Vector3(mDragStartPos.x, mDragEndPos.y, mDragStartPos.z));
            mLineRenderer.SetPosition(4, mDragStartPos);
        }

        private void DragEnd()
        {
            mLineRenderer.enabled = false;
        }

        private void DragStart()
        {
            mLineRenderer.enabled = true;
        }
    }

}