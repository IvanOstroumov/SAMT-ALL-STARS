using System.Collections.Generic;
using UnityEngine;

namespace Resources.Scripts
{
    // Tiene insieme le 4 mappe del gioco. Le sprite arrivano dal costruttore
    public class MapManager
    {
        private List<Map> maps = new List<Map>();

<<<<<<< Updated upstream

        public MapManager(Sprite sidonBack,Sprite sidonTiel, Sprite ivanBack,Sprite ivanTiel, Sprite quanBack,Sprite quanTiel, Sprite yasserBack,Sprite yasserTiel )
=======
        public MapManager(Sprite sidon, Sprite quan, Sprite yasser, Sprite ivan)
>>>>>>> Stashed changes
        {
            maps.Add(new Map("sidon", sidonBack,sidonTiel));
            maps.Add(new Map("ivan", ivanBack,ivanTiel));
            maps.Add(new Map("quan", quanBack,quanTiel));
            maps.Add(new Map("yasser", yasserBack,yasserTiel));
        }

        public Map getMapByName(string name)
        {
            foreach (var map in maps)
            {
                if (map.Name == name)
                    return map;
            }
            return null;
        }
    }
}
