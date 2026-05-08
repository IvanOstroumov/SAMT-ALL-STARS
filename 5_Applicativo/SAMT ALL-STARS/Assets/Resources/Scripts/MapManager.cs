using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Resources.Scripts
{
    public class MapManager
    {
        private List<Map> maps = new List<Map>();


        public MapManager(Sprite sidon, Sprite quan, Sprite yasser, Sprite ivan)
        {
            maps.Add(new Map("sidon", sidon));
            maps.Add(new Map("ivan", ivan));
            maps.Add(new Map("quan", quan));
            maps.Add(new Map("yasser", yasser));
        }

        public Map getMapByName(string name)
        {
            foreach (var map in maps)
            {
                if (map.Name == name)
                {
                    return map;
                }
            }
            return null;


        }

    }
}