using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyRobot.Model
{
    interface IRobotModel : INotifyPropertyChanged
    {
        //connection to the robot;
        void connect(string ip, int port);
        void disconnect();
        void start();

        //sensors properties
        double Sensor1 { set; get; }
        byte[][] CamView { set; get; }

        //activate actuators
        void move(double speed, int angle);
        void moveArm(int az, int e1, int e2, bool grip);
    }
}
