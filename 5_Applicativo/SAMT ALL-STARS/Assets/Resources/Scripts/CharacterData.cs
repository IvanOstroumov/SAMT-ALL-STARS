using System;
using UnityEngine;

namespace Resources.Scripts
{
    // I dati di un personaggio: quello che leggiamo dal JSON.

    [Serializable]
    public class CharacterData
    {
        [SerializeField] private string name;
        [SerializeField] public int maxHp;
        [SerializeField] private float speed;
        [SerializeField] private string description;
        [SerializeField] private int winCount;
        [SerializeField] private int gamesPlayed;
        [SerializeField] public float cooldown;
        [SerializeField] public float duration;

        public CharacterData(string name, int maxHp, float speed, string description,
                             int winCount, int gamesPlayed, float cooldown, float duration)
        {
            this.name = name;
            this.maxHp = maxHp;
            this.speed = speed;
            this.description = description;
            this.winCount = winCount;
            this.gamesPlayed = gamesPlayed;
            this.cooldown = cooldown;
            this.duration = duration;
        }

        public string Name => name;
        public int MaxHp => maxHp;
        public float Speed => speed;
        public string Description => description;
        public int WinCount
        {
            get => winCount;
            set => winCount = value;
        }

        public int GamesPlayed => gamesPlayed;
        public float Cooldown => cooldown;
        public float Duration => duration;
    }
}
