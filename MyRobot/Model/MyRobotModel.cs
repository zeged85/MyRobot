using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MyRobot.Model
{
    class MyRobotModel : IRobotModel
    {

        public event PropertyChangedEventHandler PropertyChanged;

        ITelnetClient telnetClient;
        volatile Boolean stop;

        public void NotifyPropertyChanged(string PropName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(PropName));

        }

        private double sensor1;
        public double Sensor1
        {
            get {return sensor1; }
            set {sensor1 = value; NotifyPropertyChanged("Sensor1"); }
        }


        public MyRobotModel(ITelnetClient telnetClient)
        {
            this.telnetClient = telnetClient;
            stop = false;

        }

        public void start()
        {
            new Thread(delegate () {
                while (!stop)
                {
                    telnetClient.write("get sensor1 data");
                    Sensor1 = double.Parse(telnetClient.read());
                    //the same for the other sensros


                    Thread.Sleep(250);
                }
            }).Start();
        }

        public void connect(string ip, int port)
        {
            telnetClient.connect(ip, port);
        }

        public void disconnect()
        {
            stop = true;
            telnetClient.disconnect();
        }


        public byte[][] CamView
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

  
       

       

      

        public void move(double speed, int angle)
        {
            throw new NotImplementedException();
        }

        public void moveArm(int az, int e1, int e2, bool grip)
        {
            throw new NotImplementedException();
        }

        
    }
}
