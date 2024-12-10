using System;
using CodeBase.Hero;
using CodeBase.Logic;
using UnityEngine;

namespace CodeBase.Environment
{
    public class Spikes : MonoBehaviour
    {
        [SerializeField] private float damage = 20;
        private IHealth _health;
        private HeroMove _heroMove;

        private void OnCollisionEnter2D(Collision2D other)
        {
            _health ??= other.gameObject.GetComponent<IHealth>();
            _heroMove ??= other.gameObject.GetComponent<HeroMove>();

            _heroMove.KnockUp();
            _health?.TakeDamage(damage);
        }
    }
}