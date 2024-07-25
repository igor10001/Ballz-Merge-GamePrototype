using UnityEngine;


    public class BallDestruct : MonoBehaviour
    {
        public delegate void OnTriggerEnterHandler(Collider2D collider);

        // Declare the event
        public event OnTriggerEnterHandler OnTriggerEnterEvent;

        private void OnTriggerEnter2D(Collider2D other)
        {
            OnTriggerEnterEvent?.Invoke(other);
        }
    }
