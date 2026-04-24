using System.Collections.Generic;

namespace Resources.Scripts
{
    public class MapManager
    {
        private List<Map> maps = new List<Map>();

        public MapManager()
        {
            
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