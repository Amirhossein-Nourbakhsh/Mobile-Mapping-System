using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class SimulateGPS : MonoBehaviour
{

    public Button simulateGPS;
    public Transform bikeBody;
    public float speed = 2.0f;



    private void Awake()
    {

        simulateGPS.onClick.AddListener(ReadString);

    }
     void ReadString()
    {
        var filename = @"\Scripts\test.txt";

       
            var sourse = new StreamReader(Application.dataPath + "/" + filename);
            var fileContents = sourse.ReadToEnd();
            sourse.Close();
            var lines = fileContents.Split("\n"[0]);

        for (int i = 0; i < lines.Length; i++)
        {

          var pointAPosition = new Vector3(bikeBody.position.x, bikeBody.position.y, 0);

            string[] splitArray = lines[i].Split(char.Parse(","));
            float x = float.Parse(splitArray[0]);
            float y = float.Parse(splitArray[1]);
            var pointBPosition = new Vector3(x, y, 0);

            Debug.Log(lines[i]);
            bikeBody.position = Vector3.MoveTowards(bikeBody.position, pointBPosition, speed);
        }
       
        


    }
    // Start is called before the first frame update
    void Start()
    {
      
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
