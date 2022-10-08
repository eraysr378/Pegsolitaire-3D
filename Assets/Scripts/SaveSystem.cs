
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    public static void SaveBoard(GameManager board)
    {
        string path = null;
        // save the board into the correct file
        BinaryFormatter formatter = new BinaryFormatter();
        if(SceneManager.GetActiveScene().name == "FirstBoard")
        {
            path = Application.persistentDataPath + "/FirstBoard.bin";
        }
        if (SceneManager.GetActiveScene().name == "SecondBoard")
        {
            path = Application.persistentDataPath + "/SecondBoard.bin";
        }
        if (SceneManager.GetActiveScene().name == "ThirdBoard")
        {
            path = Application.persistentDataPath + "/ThirdBoard.bin";
        }
        if (SceneManager.GetActiveScene().name == "FourthBoard")
        {
            path = Application.persistentDataPath + "/FourthBoard.bin";
        }
        if (SceneManager.GetActiveScene().name == "FifthBoard")
        {
            path = Application.persistentDataPath + "/FifthBoard.bin";
        }
        if (SceneManager.GetActiveScene().name == "SixthBoard")
        {
            path = Application.persistentDataPath + "/SixthBoard.bin";
        }
        FileStream stream = new FileStream(path, FileMode.Create);
        BoardData data = new BoardData(board);
        formatter.Serialize(stream, data);
        stream.Close();
    }
    //loads the last save
    public static BoardData LoadBoard()
    {
        string path = null;
        // these if statements are used to find correct save file
        if (SceneManager.GetActiveScene().name == "FirstBoard")
        {
            path = Application.persistentDataPath + "/FirstBoard.bin";
        }
        if (SceneManager.GetActiveScene().name == "SecondBoard")
        {
            path = Application.persistentDataPath + "/SecondBoard.bin";
        }
        if (SceneManager.GetActiveScene().name == "ThirdBoard")
        {
            path = Application.persistentDataPath + "/ThirdBoard.bin";
        }
        if (SceneManager.GetActiveScene().name == "FourthBoard")
        {
            path = Application.persistentDataPath + "/FourthBoard.bin";
        }
        if (SceneManager.GetActiveScene().name == "FifthBoard")
        {
            path = Application.persistentDataPath + "/FifthBoard.bin";
        }
        if (SceneManager.GetActiveScene().name == "SixthBoard")
        {
            path = Application.persistentDataPath + "/SixthBoard.bin";
        }
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path,FileMode.Open);

            BoardData data =  formatter.Deserialize(stream) as BoardData;
            stream.Close();
            return data;
        }
        else
        {
            Debug.LogError("Save file doesn't exist in "+path);
            return null;
        }
    }
}
