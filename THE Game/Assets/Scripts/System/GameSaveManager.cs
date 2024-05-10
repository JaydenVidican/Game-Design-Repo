using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using Unity.IO.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEngine;

public class GameSaveManager : MonoBehaviour
{
    public static GameSaveManager gameSave;
    public List<ScriptableObject> objects = new List<ScriptableObject>();
    public List<FloatValue> FloatValues = new List<FloatValue>();
    public List<BoolValue> BoolValues = new List<BoolValue>();
    public Inventory inventory;
    public String roomSave;
    

    void ResetScriptables()
    {
        for(int i = 0; i < objects.Count; i++)
        {
            if (File.Exists(Application.persistentDataPath + string.Format("/{0}.dat", i)))
            {
                File.Delete(Application.persistentDataPath + string.Format("/{0}.dat", i));
            }

        }
    }
    void ResetFloatValues()
    {
        for (int i = 0; i < FloatValues.Count; i++)
        {
            FloatValues[i].Reset();
        }
    }
    void ResetBoolValues()
    {
        for (int i = 0; i < BoolValues.Count; i++)
        {
            BoolValues[i].Reset();
        }
    }

    void Awake()
    {
        if (gameSave == null)
        {
            gameSave = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(this);
    }


    public void RoomStore(String newRoom)
    {
        roomSave = newRoom;
        SaveCurrentRoom();
    }

    public void SaveCurrentRoom()
    {
        FileStream file = File.Create(Application.persistentDataPath + "/currentRoom.dat");
        BinaryFormatter binary = new BinaryFormatter();
        binary.Serialize(file, roomSave);
        file.Close();
    }

    void OnEnable()
    {
        LoadScriptables();
    }
    void OnDisable()
    {
        //SaveScriptables();
    }
    
    public void SaveScriptables()
    {
        for(int i = 0; i < objects.Count; i++)
        {
            FileStream file = File.Create(Application.persistentDataPath 
            + string.Format("/{0}.dat", i));
            BinaryFormatter binary = new BinaryFormatter();
            var json = JsonUtility.ToJson(objects[i]);
            binary.Serialize(file, json);
            file.Close();

        }
        //SaveCurrentRoom();
        //Debug.Log(roomSave);
    }
    public void LoadScriptables()
    {

        /*
        if(File.Exists(Application.persistentDataPath + "/currentRoom.dat"))
        {
            FileStream file = File.Open(Application.persistentDataPath + "/currentRoom.dat", FileMode.Open);
            BinaryFormatter binary = new BinaryFormatter();
            roomSave = (string)binary.Deserialize(file);
            file.Close();
        }
        */
        for(int i = 0; i < objects.Count; i++)
        {
            if(File.Exists(Application.persistentDataPath + string.Format("/{0}.dat", i)))
            {
                FileStream file = File.Open(Application.persistentDataPath + string.Format("/{0}.dat", i), FileMode.Open);
                BinaryFormatter binary = new BinaryFormatter();
                JsonUtility.FromJsonOverwrite((string)binary.Deserialize(file), objects[i]);
                file.Close();
            }
        }
    }
    public void Reset()
    {
        ResetScriptables();
        ResetFloatValues();
        ResetBoolValues();
        inventory.Reset();
        //roomSave = null;
        //SaveCurrentRoom();
        SaveScriptables();
    }
}
