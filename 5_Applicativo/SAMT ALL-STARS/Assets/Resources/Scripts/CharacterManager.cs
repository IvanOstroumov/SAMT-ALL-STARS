using System;
using System.Collections.Generic;
using UnityEngine;

namespace Resources.Scripts
{
    
    public class CharacterManager
    {
        List<Character> characters;

        public CharacterManager()
        {
            try
            {
                characters = new List<Character>();
                getCharsFromJSON();
            }
            catch (Exception e)
            {
                Debug.Log(e);
                throw;
            }
            

        }

        public Character getCharByName(string name)
        {
            name = name.ToLower();
            foreach (var character in characters)
            {
                if (character.Data.Name == name)
                    return character;
            }
            return null;
        }

        public void getCharsFromJSON()
        {
            TextAsset jsonFile = UnityEngine.Resources.Load<TextAsset>("Scripts/characters");

            if (jsonFile == null)
            {
                Debug.LogError("characters.json non trovato in Assets/Resources/Scripts/");
                characters = null;
            }

            CharacterDataList jsonList = JsonUtility.FromJson<CharacterDataList>(jsonFile.text);
            
            foreach (CharacterData data in jsonList.characters)
            {
                Character character = new Character(data, null, data.duration, data.cooldown,null,null,null);
                characters.Add(character);
            }
        }

        // Wrapper per JsonUtility — contiene l'array di CharacterData
        [Serializable]
        public class CharacterDataList
        {
            public CharacterData[] characters;
        }

    }
}