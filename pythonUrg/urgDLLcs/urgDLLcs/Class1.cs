using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using SCIP_library;

namespace urgDLLcs
{
    public class urgClass
    {
        static SerialPort urg;
        List<long> distances;
        List<long> distances_raw;
        List<double> degrees;

        public urgClass(string port_name, int baudrate)
        {
            const int start_step = 0;
            const int end_step = 780;
            urg = new SerialPort(port_name, baudrate);
            urg.NewLine = "\n\n";

            try
            {
                urg.Open();
                urg.Write(SCIP_Writer.SCIP2());
                urg.ReadLine(); // ignore echo back
                urg.Write(SCIP_Writer.MD(start_step, end_step));
                urg.ReadLine(); // ignore echo back
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine("接続に失敗しました");
                urg.Close();
            }
            
           
        }

        ~urgClass()
        {
            urg.Write(SCIP_Writer.QT()); // stop measurement mode
            urg.ReadLine(); // ignore echo back
            urg.Close();
        }

        public void run()
        {
            double per = 360.0 / 1024;
            //double diff = 60 - per * 44;

            distances = new List<long>();
            distances_raw = new List<long>();
            degrees = new List<double>();

            long time_stamp = 0;

            string receive_data = urg.ReadLine();
            if (!SCIP_Reader.MD(receive_data, ref time_stamp, ref distances_raw))
            {
                Console.WriteLine(receive_data);
                return;
            }
            // show distance data
            for (int j = 0; j < distances_raw.Count; j++)
            {
                //https://sourceforge.net/p/urgnetwork/wiki/scip_capture_jp/
                double degree = (j - 384) * per;
                distances.Add(distances_raw[j]);
                degrees.Add(degree);
                //Console.WriteLine("time stamp: " + time_stamp.ToString() + " distance[" + j.ToString() + "] : "+ distances[j].ToString());
                //Console.WriteLine("degree[ " + j + "] :" + degree.ToString() + " distance[" + j.ToString() + "] : " + distances[j].ToString());
            }
            return;
        }

        public double pulldata_degree(int num)
        {
            if (num >= 0 && num < degrees.Count)
            {
                return degrees[num];
            }
            else
            {
                return -1;
            }
        }

        public long pulldata_distance(int num)
        {
            if (num >= 0 && num < distances.Count)
            {
                return distances[num];
            }
            else
            {
                return -1;
            }
        }
        
    }
}

  
