using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Runtime.InteropServices;

public class SatelliteContainer
{
    private string _filePath;
    private List<SatelliteElement> satList = new List<SatelliteElement>();
    
    
    public SatelliteContainer(string filePath)
    {
        this._filePath = filePath;

    }

    public void processFile()
    {
        int counter = 0;  
        string line;  
        
        // Read the file and display it line by line.  
        System.IO.StreamReader file = new System.IO.StreamReader(this._filePath);  
        while((line = file.ReadLine()) != null)  
        {  
            SatelliteElement sat = new SatelliteElement();
            System.Console.WriteLine(line);  
            counter++;  
            if(line.Substring(0,1)=="0")
            {
                sat.ProcessFirstLine(line);
                
            }
            if(line.Substring(0,1)=="2")
            {
                //sat = new SatelliteElement();

                /*Sólo añadimos los que tienen excentricidad < 1 porque es la
                 * que puede calcular nuestro código orbital*/
                sat.ProcessSecondLine(line);
                if(sat.Eccentricity < 1.0f)
                {
                    this.satList.Add(sat);
                }



            }
           
        }  
        
        file.Close();  
    }

    public string GetSatList()
    {
        string strSatList = "";
        foreach(SatelliteElement sat in this.satList)
        {
            strSatList += sat.SatName + " \n ";


            
        }

        return strSatList;
    }

    public float[][] GettingParameters()
    {
        float[][] matriz = new float[this.satList.Count][];
        for(int i = 0; i < this.satList.Count; i++)
        {
            SatelliteElement sat = satList[i];
            matriz[i] = new float[] {
                sat.SatelliteCatalogNumber,
                sat.Inclination,
                sat.RightAscension,
                sat.Eccentricity,
                sat.ArgumentOfPerigee,
                sat.MeanAnomaly,
                sat.MeanMotion,
                sat.RevolutionNumber,
                sat.CheckSum };
        }
        
        
        return matriz;
    }

}