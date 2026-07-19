using System.Collections;
using UnityEngine;

namespace Puzzle3D
{
    public class ItemAnimator : MonoBehaviour
    {
        [SerializeField] private Vector3 _startPositionOffset;
        [SerializeField] private float speed = 40f;

        public IEnumerator MoveToEndPosition()
        {
            Vector3 endPosition = transform.position;

            transform.localPosition += _startPositionOffset;

            while (Vector3.Distance(transform.position, endPosition) > 0.001f)
            {
                transform.position = Vector3.MoveTowards(transform.position, endPosition, speed * Time.deltaTime);

                yield return null;
            }

            transform.position = endPosition;
        }
    }
}