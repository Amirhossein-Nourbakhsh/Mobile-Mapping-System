using Mapbox.Json;
using Mapbox.Unity.Map;
using Mapbox.Unity.Utilities;
using Mapbox.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;


public class BikeController : MonoBehaviour
{
    

    private float m_horizontalInput;
    private float m_verticalInput;
    private float m_steeringAngle;

    public WheelCollider frontWheelR;
    public WheelCollider rearWheelR;
    public WheelCollider frontWheelL;
    public WheelCollider rearWheelL;

    public Transform frontTR;
    public Transform rearTR;
    public Transform frontTL;
    public Transform rearTL;
    public Transform refrencePoint;
    public Transform GCP1;
    public Transform GCP2;
    public Transform GCP3;
    public Transform GCP4;

    public float maxSteerAngle = 30;
    public float motorForce = 50;

    public Transform bikeBody;

    public AbstractMap Map;

    [SerializeField] private Text XY;
    [SerializeField] private Text LatLong;
    [SerializeField] private Text verticalDisplacement;
    [SerializeField] private Text ValueIRI;

    public float _originalRotX;
    public float _originalRotZ;


    Window_Graph window_graph;

    float i_counter_x_axis = 0;

    public Button segment1;

    List<GPSData> arrayGPS = new List<GPSData>();
    List<double> arrayVerticalDBetweenTwoPoint = new List<double>();
    int counterStoreData = 0;

    private void Start()
    {
         _originalRotX = bikeBody.rotation.x;
        _originalRotZ = bikeBody.rotation.z;
        window_graph = GameObject.FindGameObjectWithTag("myWindowGraph").GetComponent<Window_Graph>();

    }

    public void GetInput()
    {
        m_horizontalInput = Input.GetAxis("Horizontal");
        m_verticalInput = Input.GetAxis("Vertical");
      //  Debug.Log("H: " + m_horizontalInput);
       // Debug.Log("V: " + m_verticalInput);

    }

    public void Steer()
    {
        m_steeringAngle = maxSteerAngle * m_horizontalInput;
        frontWheelR.steerAngle = m_steeringAngle;
        frontWheelL.steerAngle = m_steeringAngle;

    }

    private void Accelerate()
    {
       
        frontWheelR.motorTorque = m_verticalInput * motorForce;
        frontWheelL.motorTorque = m_verticalInput * motorForce;

    }

    private void UpdateWheelPoses()
    {
       
        verticalDisplacement.text = "(" + refrencePoint.position.y + ")";

        UpdateWheelPose(frontWheelR, frontTR);
        UpdateWheelPose(rearWheelR, rearTR);
        UpdateWheelPose(frontWheelL, frontTL);
        UpdateWheelPose(rearWheelL, rearTL);

    

    }



    private void UpdateWheelPose(WheelCollider _collider, Transform _transform)
    {
        //  Quaternion bodyRotation = transform.rotation * Quaternion.Euler(0f, 0f, -frontWheelR.steerAngle * m_verticalInput);
        //  bikeBody.rotation = Quaternion.Lerp(bikeBody.rotation, bodyRotation, 0.02f);

       // var latLng = frontTR.GetGeoPosition(new Vector2d(43.768419, -79.504172), 1);
        var latLng2 = frontTR.GetGeoPosition(new Vector2d(0, 0), 1);
        //Debug.Log("XY=" + latLng2);

        
        // York University Location
       
        //Debug.Log("testCenterPoint=" + testCenterPoint);


        var llpos = new Vector2d(latLng2[0], latLng2[1]);
        var pos = Conversions.GeoToWorldPosition(llpos, Map.CenterLatitudeLongitude, Map.WorldRelativeScale);
        // Debug.Log("latlng=" + pos);
        XY.text = "(" + pos + ")";


	///////////////////////// Lat Lng //////////////////////////////
	var a =  0.0820877620;
	var b=  -0.239524485;
	var c= 43.7647518;
	var d= -0.148612373;
	var e=  0.435345927;
	var f=-79.5018448;
	var g= 0.00186931199;
	var h= -0.00547156062;

	var XGround = ((latLng2[0]*a) + (latLng2[1]*b) + c) / (latLng2[0]*g + (latLng2[1])*h + 1);
	var YGround = ((latLng2[0]*d) + (latLng2[1]*e) + f) / (latLng2[0]*g + latLng2[1]*h + 1);




         
	LatLong.text= "("+ XGround + " , " + YGround + ")";
        ///////////////////////////////////////////////////////////////

        // create new object as a new point
        /*  var objToSpawn =  GameObject.CreatePrimitive(PrimitiveType.Cube); ;
          objToSpawn.AddComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
          objToSpawn.AddComponent<MeshFilter>();
          objToSpawn.AddComponent<BoxCollider>();
          objToSpawn.AddComponent<MeshRenderer>();
          objToSpawn.transform.position = new Vector3(-13, 6, -390);*/



        // var gg = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        // gg.transform.position = new Vector3((float)pos.x, 0, (float)pos.y);


        // StoreData(""+XGround , ""+YGround , ""+refrencePoint.position.y + ", time=" + System.DateTime.Now);
        //   StoreData("" + latLng2[0], "" + latLng2[1], "" + refrencePoint.position.y + ", time=" + System.DateTime.Now);
       
        StoreSegmentData(""+XGround , ""+YGround ,"collectedData", ""+refrencePoint.position.y,  "time=" + System.DateTime.Now, "details");
        counterStoreData++;


        window_graph.CreateCircle(new Vector2(i_counter_x_axis, refrencePoint.position.y*10));
        if (i_counter_x_axis > 280) {
            i_counter_x_axis = 0;
        }
        i_counter_x_axis = (float)(i_counter_x_axis + 0.1);




        Vector3 _pose = _transform.position;
        Quaternion _quat = _transform.rotation;

        _collider.GetWorldPose(out _pose, out _quat);

        _transform.position = _pose;
        _transform.rotation = _quat;

    }


    



    private void Awake()
    {
        Assert.IsNotNull(XY);
        Assert.IsNotNull(verticalDisplacement);
        segment1.onClick.AddListener(onClickSegment);
    
    }

    void onClickSegment()
    {


        Debug.Log("Clicked");
	var latLng2 = frontTR.GetGeoPosition(new Vector2d(0, 0), 1);
 	var llpos = new Vector2d(latLng2[0], latLng2[1]);
        var pos = Conversions.GeoToWorldPosition(llpos, Map.CenterLatitudeLongitude, Map.WorldRelativeScale);
	var a =  0.0820877620;
	var b=  -0.239524485;
	var c= 43.7647518;
	var d= -0.148612373;
	var e=  0.435345927;
	var f=-79.5018448;
	var g= 0.00186931199;
	var h= -0.00547156062;

       

	var XGround = ((latLng2[0]*a) + (latLng2[1]*b) + c) / (latLng2[0]*g + (latLng2[1])*h + 1);
	var YGround = ((latLng2[0]*d) + (latLng2[1]*e) + f) / (latLng2[0]*g + latLng2[1]*h + 1);
	
        if (segment1.GetComponentInChildren<Text>().text == "End segment")
        {
            segment1.GetComponentInChildren<Text>().text = "Start segment";
	    
	    StoreSegmentData(""+XGround , ""+YGround, "endPoint", "0","0", "btnSegment");
        }
        else {
            segment1.GetComponentInChildren<Text>().text = "End segment";
	    StoreSegmentData(""+XGround , ""+YGround, "startPoint", "0","0", "btnSegment");
        }
       
    }


    private void FixedUpdate()
    {
        GetInput();
        Steer();
        Accelerate();
        UpdateWheelPoses();
    }


    public void ResetBike()
    {
        // Move the rigidbody up by 3 metres
        bikeBody.Translate(0, 3, 0);
        // Reset the rotation to what it was when the car was initialized
        bikeBody.rotation = (Quaternion.Euler(new Vector3(_originalRotX, bikeBody.rotation.y, _originalRotZ)));

        var xyGCP1 = GCP1.GetGeoPosition(new Vector2d(0, 0), 1);
        var xyGCP2 = GCP2.GetGeoPosition(new Vector2d(0, 0), 1);
        var xyGCP3 = GCP3.GetGeoPosition(new Vector2d(0, 0), 1);
        var xyGCP4 = GCP4.GetGeoPosition(new Vector2d(0, 0), 1);

	var xyGCPG1= new Vector2d(xyGCP1 [0], xyGCP1 [1]);
	var xyGCPG2= new Vector2d(xyGCP2 [0], xyGCP2 [1]);
	var xyGCPG3= new Vector2d(xyGCP3 [0], xyGCP3 [1]);
	var xyGCPG4= new Vector2d(xyGCP4 [0], xyGCP4 [1]);

        var pos1 = Conversions.GeoToWorldPosition(xyGCP1 , Map.CenterLatitudeLongitude, Map.WorldRelativeScale);
	var pos2 = Conversions.GeoToWorldPosition(xyGCP2 , Map.CenterLatitudeLongitude, Map.WorldRelativeScale);
	var pos3 = Conversions.GeoToWorldPosition(xyGCP3 , Map.CenterLatitudeLongitude, Map.WorldRelativeScale);
	var pos4 = Conversions.GeoToWorldPosition(xyGCP4 , Map.CenterLatitudeLongitude, Map.WorldRelativeScale);

        Debug.Log("GCP1=" + pos1);
        Debug.Log("GCP2=" + pos2);
        Debug.Log("GCP3=" + pos3);
        Debug.Log("GCP4=" + pos4);
	// LatLng1 = 43.76460965191316, -79.50573052982492
	// LatLng2 = 43.77357233736434, -79.49954387627602
	// LatLng3 = 43.77424204945979, -79.51031157923717
	// LatLng4 = 43.768285519198436, -79.50430626001368

	// GCP1=-3.22926,-11.85182
	// GCP2=34.13335,6.68059
	// GCP3=35.21071,-30.09657
	// GCP4=11.80795,-7.59701




    }

    public void StoreData(string _lat, string _lng, string  _verticalD)
    {
        
        var Line = _lat+","+_lng+","+_verticalD;
        File.AppendAllText(@"C:\Users\user\Desktop\Amir\MMS\Final Project\Unity\test5\Assets\Scripts\test.txt", Line + Environment.NewLine);

       
       
    }

    public void StoreSegmentData(string _lat, string _lng, string  _type, string _vertivalID ,string _time,  string _callFrom)
    {

        

    if (_callFrom == "btnSegment"){
	// type is startPoint or endPoint
        var Line =_type +"=" + _lat+","+_lng;
        File.AppendAllText(@"C:\Users\user\Desktop\Amir\MMS\Final Project\Unity\test5\Assets\Scripts\segments.txt", Line + Environment.NewLine);
        
            // call from btn and the btn text is End segment which means user just clicked on btn to start collecting data
           if (segment1.GetComponentInChildren<Text>().text == "End segment")
            {
              //  effect = new List<GPSData>();
            } else
            {
                // means the user clicked to stop collecting data
                var output = JsonConvert.SerializeObject(arrayGPS);
               // Debug.Log(output);
                string path = Application.dataPath + @"\Scripts\GPS.json";
                File.AppendAllText(path, output);

                // here we need to calculate IRI 
                double lat1 = double.Parse (arrayGPS[0].lat) * Math.PI/180;
                double lon1 = double.Parse(arrayGPS[0].lon) * Math.PI / 180;
                double lat2 = double.Parse(arrayGPS[arrayGPS.Count-1].lat) * Math.PI / 180;
                double lon2 = double.Parse(arrayGPS[arrayGPS.Count-1].lon) * Math.PI / 180;
                double deltaPhi = lat2 - lat1;
                double deltaLambda = lon2 - lon1;
                double a = (Math.Sin(deltaPhi / 2)* Math.Sin(deltaPhi / 2)) + Math.Cos(lat1) * Math.Cos(lat2) * 
                    (Math.Sin(deltaLambda / 2) * Math.Sin(deltaLambda / 2));
                double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a)) * 1000000;
                double d = 637.1 * c;

                // here I have to calculate the vrtical displacement
                double verticalSum = arrayVerticalDBetweenTwoPoint.Sum();
                var IRI = verticalSum / d;
                ValueIRI.text = ""+ (IRI);


            }
             

}
	else if (_callFrom == "details" & segment1.GetComponentInChildren<Text>().text == "End segment"){

            if (counterStoreData % 10 == 0)
            {
                var Line = _lat + "," + _lng + "," + _vertivalID;
                File.AppendAllText(@"C:\Users\user\Desktop\Amir\MMS\Final Project\Unity\test5\Assets\Scripts\segments.txt", Line + Environment.NewLine);

                GPSData myObject = new GPSData();
                myObject.lat = _lat;
                myObject.lon = _lng;
                myObject.vertical = _vertivalID;
                arrayGPS.Add(myObject);
                if(arrayGPS.Count > 1)
                {
                    
                   arrayVerticalDBetweenTwoPoint.Add(Math.Abs(double.Parse(arrayGPS[arrayGPS.Count - 1].vertical) - double.Parse(arrayGPS[arrayGPS.Count - 2].vertical)));
                }
            }
         
     
            


        }
        
	
       
       
    }


 

    [System.Serializable]
    public class GPSData
    {
        public string lat;
        public string lon;
        public string vertical;
    }



}
