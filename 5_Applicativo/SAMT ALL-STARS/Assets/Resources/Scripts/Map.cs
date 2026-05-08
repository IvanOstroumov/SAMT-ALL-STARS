using UnityEngine;
using UnityEngine.UI;

namespace Resources.Scripts
{
    public class Map
    {
        private string name;
        private Sprite image;
        
        public Map(string name, Sprite image)
        {
            this.name = name;
            this.image = image;
        }

        public string Name => name;

        public Sprite Image => image;
    }
}