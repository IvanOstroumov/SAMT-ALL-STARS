using UnityEngine;

namespace Resources.Scripts
{
    public class Player
    {
        private int id;
        private Color color;
        private InputType inputType;
        private Character character;

        public Player(int id, Color color, InputType inputType, Character character)
        {
            this.id = id;
            this.color = color;
            this.inputType = inputType;
            this.character = character;
        }

        public int ID => id;

        public Color Color => color;

        public InputType InputType => inputType;

        public Character Character => character;
    }
}