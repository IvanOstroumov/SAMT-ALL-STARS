using UnityEditor.Animations;
using UnityEngine;

namespace Resources.Scripts
{
    public class Character
    {
        private CharacterData data;
        private int currentHp;
        private Ability ability;
        private float duration;
        private float cooldown;
        private RuntimeAnimatorController controller;
        private Sprite sprite;

        public Character(CharacterData data, int currentHp, Ability ability, float duration, float cooldown, RuntimeAnimatorController controller, Sprite sprite)
        {
            this.data = data;
            this.currentHp = currentHp;
            this.ability = ability;
            this.duration = duration;
            this.cooldown = cooldown;
            this.controller = controller;
            this.sprite = sprite;
        }

        public CharacterData Data => data;

        public int CurrentHp => currentHp;

        public Ability Ability => ability;

        public float Duration => duration;

        public float Cooldown => cooldown;
        public RuntimeAnimatorController Controller { get; set; }
        public Sprite Sprite { get; set; }


    }
}