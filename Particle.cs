using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MapPSO
{
    class Particle
    {
       
        public List<Tree> Position { get; set; }
        public List<Tree> BestPosition { get; set; }
        public Tree Global { get; set; }
        public double Fitness { get; set; }
        public double BestFitness { get; set; }
        public List<Tree> Velocity { get; set; }
        public int IndexFitness { get; set; }
        
        public List<velocity> VelocityCompunt = new List<velocity>();


    }
       
}
