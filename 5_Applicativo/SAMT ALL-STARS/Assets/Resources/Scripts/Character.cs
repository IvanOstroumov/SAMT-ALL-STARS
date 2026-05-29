using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.UI;

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

        public Character(CharacterData data, Ability ability, float duration, float cooldown, RuntimeAnimatorController controller, Sprite sprite, Sprite nameImage)
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

        public int CurrentHp { get ; set ; }

        public Ability Ability => ability;

        public float Duration => duration;

        public float Cooldown => cooldown;
        public RuntimeAnimatorController Controller { get; set; }
        public Sprite Sprite { get; set; }
        public Sprite  NameImage { get; set;}
    }
}