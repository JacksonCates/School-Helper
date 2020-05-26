using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/* Description:
 * Is the blueprints for our grade object. Contains data is the
 * the name, pointsEarned, pointsTotal, and percGrade. Calculates
 * percGrade;
 */

namespace SchoolHelper
{
    class Grade
    {
        // Vars
        private string name;
        private int pointsEarned;
        private int pointsTotal;
        private double percGrade;


        // Default Constructor
        public Grade()
        {
            name = "No Name";
            pointsEarned = -1;
            PointsTotal = -1;
        }


        // Property functions
        public string Name
        {
            get => name;
            set => name = value;
        }
        public int PointsEarned
        {
            get => pointsEarned;
            set => pointsEarned = value;
        }
        public int PointsTotal
        {
            get => pointsTotal;
            set => pointsTotal = value;
        }
        public double GetPercGrade()
        {
            CalcPercGrade();
            return percGrade;
        }

        private void CalcPercGrade()
        {
            percGrade = Convert.ToDouble(pointsEarned) / pointsTotal;
            percGrade *= 100;
        }
    }
}
