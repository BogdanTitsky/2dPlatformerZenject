using System;
using CodeBase.Logic;
using UnityEngine;

namespace CodeBase.Environment
{
    public class Spikes : MonoBehaviour
    {
        [SerializeField] private float damage = 10;
        private IHealth _health;

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (_health == null)
                _health = other.gameObject.GetComponent<IHealth>();

            _health?.TakeDamage(damage);
        }
    }
}