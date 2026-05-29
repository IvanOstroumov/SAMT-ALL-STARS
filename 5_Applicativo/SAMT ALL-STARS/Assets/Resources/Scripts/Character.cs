using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.UI;

namespace Resources.Scripts
{
    // L'istanza "viva" di un personaggio durante la partita.
    // Wrappa i CharacterData (dati immutabili dal JSON) e aggiunge lo stato
    public class Character
    {
        private CharacterData data;
        private int currentHp;
        private Ability ability;
        private float duration;
        private float cooldown;
        private RuntimeAnimatorController controller;
        private Sprite sprite;

<<<<<<< Updated upstream
        public Character(CharacterData data, Ability ability, float duration, float cooldown, RuntimeAnimatorController controller, Sprite sprite, Sprite nameImage)
=======
        public Character(CharacterData data, int currentHp, Ability ability,
                         float duration, float cooldown,
                         RuntimeAnimatorController controller, Sprite sprite)
>>>>>>> Stashed changes
        {
            this.data = data;
            this.currentHp = data.maxHp;
            this.ability = ability;
            this.duration = duration;
            this.cooldown = cooldown;
            this.controller = controller;
            this.sprite = sprite;
            this.NameImage = nameImage;
        }

        public CharacterData Data => data;

        // CurrentHp ha set pubblico perche' il PlayerController lo aggiorna a ogni TakeDamage. 
        public int CurrentHp { get; set; }

        public Ability Ability => ability;
        public float Duration => duration;
        public float Cooldown => cooldown;
        
        public RuntimeAnimatorController Controller { get; set; }
        public Sprite Sprite { get; set; }
<<<<<<< Updated upstream
        public Sprite  NameImage { get; set;}
=======
>>>>>>> Stashed changes
    }
}
