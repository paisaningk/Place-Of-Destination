using System;
using Assets.Script.Base;
using UnityEngine;
using Random = UnityEngine.Random;
using Sound;

namespace Assets.Script.Pickup
{
    public class Gold : MonoBehaviour
    {
        public int goldAmount;

        private void OnTriggerEnter2D(Collider2D other)
        {
            
            if (other.CompareTag("Player"))
            {
                SoundManager.Instance.Play(SoundManager.Sound.Coin);
                AddGold(other,goldAmount);
            }
        }

        private void AddGold(Collider2D other ,int goldPlus)
        {
            var player = other.GetComponent<PlayerCharacter>();
            player.Gold += goldPlus;
            Debug.Log($"Player Gold {player.Gold}");
                
            Destroy(gameObject);
        }
    }
}
