using UnityEngine;

namespace CodeBase.Environment
{
    public class Sign : MonoBehaviour
    {
        [SerializeField] private Collider2D collider;
        [SerializeField] private GameObject textMessage;

        private void Start() => textMessage.SetActive(false);

        private void OnTriggerEnter2D(Collider2D other) => textMessage.SetActive(true);

        private void OnTriggerExit2D(Collider2D other) => textMessage.SetActive(false);
    }
}
