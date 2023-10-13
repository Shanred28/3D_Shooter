using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class SceneSerializer : MonoBehaviour
{
    [System.Serializable]
    public class SceneObjectState
    {
        public int sceneId;
        public long entityId;
        public string state;
    }

    [SerializeField] private PrefabsDataBase _prefabsData;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F5))
        {
            SaveToFile("Test.dat");
        }

        if (Input.GetKeyDown(KeyCode.F9))
        {
            LoadFromFile("Test.dat");
        }

    }
    public void SaveScene()
    {
        SaveToFile("Test.dat");
    }

    public void LoadScene()
    {
        LoadFromFile("Test.dat");
    }

    private void SaveToFile(string filePath)
    { 
        List<SceneObjectState> savedObjects = new List<SceneObjectState>();

        // Receiving all SaveObjects in scene
        foreach (var entity in FindObjectsOfType<Entity>())
        { 
           ISerializableEntity serializableEntity = entity as ISerializableEntity;

            if(serializableEntity == null) continue;

            if(serializableEntity.IsSerializable() == false) continue;

            SceneObjectState s = new SceneObjectState();

            s.entityId = serializableEntity.EntityId;
            s.state = serializableEntity.SerializeState();

            savedObjects.Add(s);
        }

        if (savedObjects.Count == 0) return;

        //Save in file
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/" + filePath);

        bf.Serialize(file, savedObjects);
        file.Close();

        Debug.Log("Scene save. Path file: " + Application.persistentDataPath + "/" + filePath);
    }

    private void LoadFromFile(string filePath)
    {
        Player.Instance.Destroy();

        foreach (var entity in FindObjectsOfType<Entity>())
        {
            Destroy(entity.gameObject);
        }

        //Filling in the list of information about all uploaded objects.
        List<SceneObjectState> loadedObjects = new List<SceneObjectState>();

        if (File.Exists(Application.persistentDataPath + "/" + filePath))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/" + filePath, FileMode.Open);

            loadedObjects = (List<SceneObjectState>) bf.Deserialize(file);

            file.Close();
        }

        //Spawn player

        foreach (var v in loadedObjects)
        {
            if (_prefabsData.IsPlayerId(v.entityId) == true)
            {
                GameObject p = _prefabsData.CreatePlayer();

                p.GetComponent<ISerializableEntity>().DeserializeState(v.state);
                loadedObjects.Remove(v);
                break;
            }           
        }

        //Spawn all object
        foreach (var v in loadedObjects)
        {
            GameObject g = _prefabsData.CreateEntityFromId(v.entityId);

            g.GetComponent<ISerializableEntity>().DeserializeState(v.state);
        }

        Debug.Log("Scene loaded. Path file: " + Application.persistentDataPath + "/" + filePath);
    }
}
