using MyRobot.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyRobot.ViewModel
{
    class RobotViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private IRobotModel model;
        public RobotViewModel(IRobotModel model)
        {
            this.model = model;
            model.PropertyChanged +=
                delegate (Object sender, PropertyChangedEventArgs e)
                {
                    NotifyPropertyChanged("VM_" + e.PropertyName);
                };
        }

        public void NotifyPropertyChanged(string PropName)
        {

        }

    //properties

        public double VM_Sensor1
        {
            get { return model.Sensor1; }
        }


        private int angle;
        private double robotSpeed;

        public double VM_RobotSpeed
        {
            get { return robotSpeed; }
            set
            {
                robotSpeed = value;
                model.move(robotSpeed, angle);
            }
        } //same for vm-robotangle

        private int az, elv1, elv2;
        private bool grip;
        public int VM_Azimuth
        {
            get { return az; }
            set
            {
                az = value;
            //    model.moveArm(az, elv1, elv2, grip);
            }
        }

        public void MoveRobot(double speed, int angle)
        {
            model.move(speed, angle);
        }
    }
}
