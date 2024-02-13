using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MapPSO
{
    public partial class Form1 : Form
    {
        Room room = new Room();
        static int step1 = 50;
        static int step2 = 50;
        static int ImgW = 10; // the size of the map
        static int ImgH = 10; // the size of the map
        static int NumberOfShapTypes = 14;
        static int MapArea = ImgW * ImgH;
        static int PubSize = 600;
        static int Maxloop = 200;
        Particle particle = new Particle();
        int row = 1;
        int col = 3;


        //ExcelFile excelTest = new ExcelFile(@"C:\Users\ncd2763\Documents\Data\PSO\test.xlsx", 1);

        Tree Gbest = new Tree();
                
        static System.Random r = new System.Random();
        public int r1; public int r2;

        //int[] reveiwPositionGlobal = new int[MapArea - 1];
       // int[] reveiwPositionLocal = new int[MapArea - 1];

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
          // excelTest.excelclear();
            DoMapPSO();
         // excelTest.ExcelSave();
        // excelTest.excelClose();
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {

            
            int A = room.width + 2;
            Gbest.DrawGraph(e, A, step1, step2, ImgW, ImgH);
            Gbest.OnPaint(e);
            
        }

        void DoMapPSO()
        {
            InitializeParticle(particle);
            
            int Iteration = 0;
            do
            {
                // System.Diagnostics.Debug.WriteLine( "   " + Iteration, " print the position");
                UpdateInertiaWeight(particle);
                double CurrentFitness = particle.Fitness;

                UpdatePositions(particle);
                particle.Position = particle.BestPosition;
                particle.Global = particle.BestPosition.OrderByDescending(tree => tree.getFitness()).First();
                particle.Fitness = particle.Global.getFitness();
                particle.BestFitness = particle.Fitness;
                particle.IndexFitness = particle.BestPosition.IndexOf(Gbest);
                particle.Velocity= GenerateRandomPopulation();
               
                Gbest = particle.Global; // best map in the position 
                double gb = particle.BestFitness; // the fitness of the bst position
                particle.IndexFitness = particle.BestPosition.IndexOf(Gbest);
                System.Diagnostics.Debug.WriteLine(Iteration + "(" + particle.IndexFitness + ")" + "," + gb + " =" +
                    Gbest.CalculateThePinaltyScoreStart().ToString() +" = " + Gbest.CalculateThePinaltyScoreEnd().ToString());
                row++;
               //excelTest.writeXcelsheet(row, col, Iteration.ToString());
              // excelTest.writeXcelsheet(row, col + 1, gb.ToString());
                Iteration++;
            } while (Iteration < Maxloop);


        }


        void UpdateInertiaWeight(Particle particle)
        {

            double best;

            particle.VelocityCompunt.Clear();
            double[] dest = new double[PubSize];
            particle.Global = particle.BestPosition.OrderByDescending(tree => tree.getFitness()).First();
            velocity tempVelocity = new velocity();
            for (int j = 0; j < PubSize; j++)
            {
                best = particle.BestPosition[j].getFitness();
                dest[j] = particle.Global.getFitness() - best;
                //System.Diagnostics.Debug.WriteLine("the dest is " + dest[j]);
            }

            double s = dest.Max();

            for (int j = 0; j < PubSize; j++)
            {
                tempVelocity.Inertia = dest[j] / s;

               // System.Diagnostics.Debug.WriteLine("Inertia is " + tempVelocity.Inertia + "Dest is "+ dest[j] + "Max is " + s);

                if (tempVelocity.Inertia < 0.3)
                {
                    tempVelocity.C1 = 11; // local based of privous experment this should 11 location 
                    tempVelocity.C2 = 16; // global bsed of privous was 3 location and here 16 or 27
                }
                else
                {
                    tempVelocity.C1 = 11;
                    tempVelocity.C2 = 22;// should 22 to equal 4 in prevoius application 
                }
                //System.Diagnostics.Debug.WriteLine("the iteration is " + j);
                particle.VelocityCompunt.Add(new velocity());
                particle.VelocityCompunt[j] = tempVelocity; // Assign the modified value back to the list
            }
        }

        void UpdatePositions(Particle particle)
        {

            //System.Diagnostics.Debug.WriteLine("the current is " + Cparticle.Global.getFitness());
            // particle.Global = particle.BestPosition.OrderByDescending(tree => tree.getFitness()).First();
            int A = 1; // counter to check every node in the map
            int Atrac = MapArea - 2;
            int Gtrack = particle.Global.ListOfNodeResult.Count();
            int GtracTrack = particle.Global.TraceNode.Count();
            for (int j = 0; j < PubSize; j++)
            {
                if (j != particle.IndexFitness)
                {
                    // System.Diagnostics.Debug.WriteLine("the current is " + Cparticle.BestPosition[j].getFitness());
                    int Xc1 = particle.VelocityCompunt[j].C1; // local 
                    int Xc2 = particle.VelocityCompunt[j].C2; // global
                    int L = 0;   // counter to take the information from local
                    int G = 0;  // counter to take the information from global 

                   
                    particle.Velocity[j].getFitness();
                    double goodNewTrack = particle.Velocity[j].CalculateThePinaltyScoreStart();
                    particle.BestPosition[j].getFitness();
                    double CurrentTrack = particle.BestPosition[j].CalculateThePinaltyScoreStart();
                                          
                        
                    A = 1;
                    Atrac = MapArea - 1;
                    G = 0;
                    while (G < Xc2)
                        {
                           Point targetId = new Point();
                           targetId = particle.BestPosition[j].nodes[A].Data;
                           TreeNode foundNode = particle.Global.ListOfNodeResult.Find(node => node.Data == targetId);
                           if (foundNode != null)
                             {
                                    particle.BestPosition[j].nodes[A].Size = foundNode.Size;
                              }
                           targetId = particle.BestPosition[j].nodes[Atrac].Data;
                           foundNode = particle.Global.TraceNode.Find(node => node.Data == targetId);
                           if (foundNode != null)
                             {
                                 particle.BestPosition[j].nodes[Atrac].Size = foundNode.Size;
                              }
                           G++;
                           A++;
                           Atrac--;
                        }
                    if (goodNewTrack > CurrentTrack)
                     {
                          L = 0;
                          while (L < Xc1)
                           {
                                Point targetId = new Point();
                                targetId = particle.BestPosition[j].nodes[A].Data;
                                TreeNode foundNode = particle.Velocity[j].ListOfNodeResult.Find(node => node.Data == targetId);
                                if (foundNode != null)
                                 {
                                     particle.BestPosition[j].nodes[A].Size = foundNode.Size;
                                  }
                                targetId = particle.BestPosition[j].nodes[Atrac].Data;
                                foundNode = particle.Global.TraceNode.Find(node => node.Data == targetId);
                                 if (foundNode != null)
                                 {
                                   particle.BestPosition[j].nodes[Atrac].Size = foundNode.Size;
                                 }

                                 L++;
                                 A++;
                                 Atrac--;
                            }


                      }
                        else
                         {
                            L = 0;
                            while (L < Xc1)
                             {
                                int newSize = r.Next(1,14);
                                particle.BestPosition[j].nodes[A].Size = newSize;
                                L++;
                                 A++;
                              }

                         }
                   
                } // end if (j != particle.IndexFitness)
              
                particle.BestPosition[j].FitnessValue = particle.BestPosition[j].getFitness();
                int flag = 0;
                double newLocalFitness = 0;
                double oldLocalFitness = particle.BestPosition[j].FitnessValue;
                while ((oldLocalFitness > newLocalFitness) && (flag < 7))
                {
                    Tree NewMap = new Tree(NumberOfShapTypes, ImgW, ImgH);
                    newLocalFitness = NewMap.FitnessValue;
                    flag++;
                    if ((oldLocalFitness < newLocalFitness))
                    {
                        particle.BestPosition[j] = NewMap;
                    }
                    // System.Diagnostics.Debug.WriteLine("the new is .. " + newLocalFitness + "and the old is "+ oldLocalFitness);

                }

                particle.BestPosition[j].FitnessValue = particle.BestPosition[j].getFitness();
                // particle.Velocity[j]= particle.Position[j];

            } // end for 

            //particle.Global = TreeBestFitness(particle.BestPosition);
            
            //System.Diagnostics.Debug.WriteLine("the new position is " + Cparticle.Global.getFitness());

        }  // end update position 


        void UpdatePositions2(Particle particle)
        {
            int Xc1;
            int Xc2;
            velocity Intena2 = new velocity();
            Random random = new Random();
            double randomNumber = random.NextDouble();
            if (randomNumber < 0.2)
            {
                Xc1 = 66;
                Xc2 = 100;
            }
            else
            {
                Xc1 = 66;
                Xc2 = 132;
            }
           
            
            Tree Global = particle.BestPosition[PubSize-1];
            double Fitness = Global.getFitness();
            int j = 0;
            int X1 = 0; int X2 = 0;
            while ( X2< Xc2)
            {
                //System.Diagnostics.Debug.WriteLine("the old position is " + particle.BestPosition[j].getFitness());
                for (int i=1; i<  particle.BestPosition[j].nodes.Count()-1; i++)
                {
                    if (Global.ListOfNodeResult.Count() > Global.TraceNode.Count())
                    {
                        Point targetId = new Point();
                        targetId = particle.BestPosition[j].nodes[i].Data;
                        TreeNode foundNode = Global.ListOfNodeResult.Find(node => node.Data == targetId);
                        if (foundNode != null)
                        {
                            particle.BestPosition[j].nodes[i].Size = foundNode.Size;
                        }
                    }
                    else
                    {
                        Point targetId = new Point();
                        targetId = particle.BestPosition[j].nodes[i].Data;
                        TreeNode foundNode = Global.TraceNode.Find(node => node.Data == targetId);
                        if (foundNode != null)
                        {
                            particle.BestPosition[j].nodes[i].Size = foundNode.Size;
                        }
                    }
                }
               // System.Diagnostics.Debug.WriteLine("the new position is " + particle.BestPosition[j].getFitness());
                X2++;
                j++;
               
            } // end of the loop of taking the global information
            j = j + Xc1-1;
            while (j < PubSize)
            {
                int flag = 0;
                double newLocalFitness = 0;
                double oldLocalFitness = particle.BestPosition[j].getFitness();
                while ((oldLocalFitness > newLocalFitness) && (flag < 3))
                {
                    Tree NewMap = new Tree(NumberOfShapTypes, ImgW, ImgH);
                    newLocalFitness = NewMap.getFitness();
                    flag++;
                    if ((oldLocalFitness < newLocalFitness))
                    {
                        particle.BestPosition[j] = NewMap;
                    }
                    // System.Diagnostics.Debug.WriteLine("the new is .. " + newLocalFitness + "and the old is "+ oldLocalFitness);

                }

                X1++;
                j++;
            } // end of search locally 


        }


        private static List<Tree> GenerateRandomPopulation()
        {
            // Generate a random population of trees for initialization
            List<Tree> population = new List<Tree>();

            for (int i = 0; i < PubSize; i++)
            {
                Tree NT = new Tree(NumberOfShapTypes, ImgW, ImgH);
                population.Add(NT);
            }

            return population;
        }

       

        void PubWriteLine(List<Tree> printList, string msg)
        {
            foreach( Tree node in printList)
            {
                System.Diagnostics.Debug.WriteLine("the fitness is  " + node.getFitness() + msg);
            }
        }

        void InitializeParticle(Particle particle)
        {
           
           particle.Position = GenerateRandomPopulation(); 
           particle.BestPosition = particle.Position;
           particle.Global = particle.BestPosition.OrderByDescending(tree => tree.getFitness()).First();
          
           particle.Global.FitnessValue = particle.Global.getFitness();
           particle.Fitness = particle.Global.FitnessValue;
           particle.BestFitness = particle.Fitness;
           particle.IndexFitness = particle.BestPosition.IndexOf(particle.Global);
           particle.Velocity = GenerateRandomPopulation();
           
        }

    }
}
