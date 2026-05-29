using System;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
namespace Resources.Scripts
{
    
    public class CharacterManager
    {
        List<Character> characters;
        private string SavePath =>
            Path.Combine(Application.persistentDataPath, "characters.json");


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
            // Se il file non esiste ancora, copia quello di default
            if (!File.Exists(SavePath))
            {
                CreateDefaultJSON();
            }

            string json = File.ReadAllText(SavePath);

            CharacterDataList jsonList =
                JsonUtility.FromJson<CharacterDataList>(json);
            
            foreach (CharacterData data in jsonList.characters)
            {
                Character character = new Character(data, null, data.duration, data.cooldown,null,null,null);
                characters.Add(character);
            }
        }
        
        public void saveCharsToJSON()
        {
            CharacterDataList dataList = new CharacterDataList();

            List<CharacterData> datas = new();

            foreach (Character character in characters)
            {
                datas.Add(character.Data);
            }

            dataList.characters = datas.ToArray();

            string json = JsonUtility.ToJson(dataList, true);

            File.WriteAllText(SavePath, json);

            Debug.Log("JSON salvato in: " + SavePath);
        }
        
        private void CreateDefaultJSON()
        {
            TextAsset defaultJson =
                UnityEngine.Resources.Load<TextAsset>("Scripts/characters");

            if (defaultJson == null)
            {
                Debug.LogError(
                    "characters.json non trovato in Resources/Scripts/"
                );
                return;
            }

            File.WriteAllText(SavePath, defaultJson.text);

            Debug.Log("Creato JSON iniziale in: " + SavePath);
        }

        // Wrapper per JsonUtility — contiene l'array di CharacterData
        [Serializable]
        public class CharacterDataList
        {
            public CharacterData[] characters;
        }

    }
}