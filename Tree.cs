﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

/* this class resposible for generate diffrent positions to the map by using method "generatelevel"  in this method 
 * data, type and size should assign. also here the list of all rooms will save in a list 
 * by using generateTreeNode the parents and the children of each node, the link and doors will assign as well 
 * set fitness for each node will assin here 
 */


namespace MapPSO
{
    class Tree
    {
        public TreeNode Root { get; set; }
        public TreeNode[] nodes = new TreeNode[100];
        public List<TreeNode> ListOfNodeResult = new List<TreeNode>();
        public List<TreeNode> TraceNode = new List<TreeNode>();
        Dictionary<int, int> difficultyScore = new Dictionary<int, int> // Add a difucalty score to each type of cell 
        {
            {1,1},{2,4},{3,2},{4,1},{5,2},{6,3},
            {7,4},{8,4},{9,2},{10,2},{11,2},{12,2},{13,4 }
        };
        public double FitnessValue { get; set; }

        public Tree()
        {

            this.FitnessValue = 0;
            



        }

        public Tree(int V, int W, int H)
        {
            this.generatinglevel(V, W, H);
            this.CreatTreeNodes();
            FitnessValue = this.getFitness();
        }

        public void PrintAllTree(TreeNode current)
        {
            System.Diagnostics.Debug.WriteLine(" inside print  ");
            if (current.Children != null)
            {
                foreach (TreeNode child in current.Children)
                {

                    if ((child != null))
                    {
                        current = child;

                        System.Diagnostics.Debug.WriteLine(" node is = " + current.Data);
                        PrintAllTree(current);

                    }

                }


            }

        }


        public void CreatTreeNodes()
        {
            //System.Diagnostics.Debug.WriteLine(nodes.Count()+ "number of nodes");
            for (int k = 0; k < this.nodes.Count(); k++)
            {
                for (int l = 0; l < this.nodes.Count(); l++)
                {
                    if (k != l)
                    {
                        if ((this.nodes[k].Data.X == this.nodes[l].Data.X) & (this.nodes[k].Data.Y - this.nodes[l].Data.Y == -1))
                        {
                            // System.Diagnostics.Debug.WriteLine(" k= "+k+" l= "+l);
                            this.nodes[k].Children.Add(this.nodes[l]);
                            if (this.nodes[l].Parent1 == null)
                                this.nodes[l].Parent1 = this.nodes[k];
                            else if (nodes[l].Parent2 == null)
                                this.nodes[l].Parent2 = this.nodes[k];
                            // SetfitnessNode(nodes[k],nodes[l],1, nodes[k].Children.Count);
                            //System.Diagnostics.Debug.WriteLine(nodes[k].Data + " the child is " + nodes[l].Data + " the parent is " + nodes[l].Parent1.Data);

                        }
                        else if ((this.nodes[k].Data.Y == this.nodes[l].Data.Y) & (this.nodes[k].Data.X - this.nodes[l].Data.X == -1))
                        {
                            this.nodes[k].Children.Add(this.nodes[l]);
                            if (this.nodes[l].Parent1 == null)
                                this.nodes[l].Parent1 = this.nodes[k];
                            else if (nodes[l].Parent2 == null)
                                this.nodes[l].Parent2 = this.nodes[k];
                            // System.Diagnostics.Debug.WriteLine(" k1 = " + k + "   " + nodes[k].Data + " the child is " + nodes[l].Data);
                            // SetfitnessNode(nodes[k],nodes[l],2, nodes[k].Children.Count);
                        }

                    }
                }

            }
            // printNodes(nodes);



        }






       

        public void generatinglevel(int V, int W, int H)
        {

            // int eventNo = 0;

            // tree.Root = new TreeNode { Data = new Point(0, 0), size=0, Children = new List<TreeNode>(3), door = 1,link=1 };

            // nodes.Add(tree.Root);
            // Make random child nodes.

            TreeNode new_node;
            Random r = new Random();
            int ij = 0;
            for (int i = 1; i < W + 1; i++)
            {

                for (int j = 1; j < H + 1; j++)
                {


                    if ((i == 1) & (j == 1))
                    {
                        //room1.EventRoomSize1(e, step1, step2);
                        new_node = new TreeNode { Data = new Point(i, j), Size = 13, Link = 0, Children = new List<TreeNode>(3), Door = 0 };
                        this.Root = new_node;
                        this.nodes[0] = new_node; // need to put condition of depulacte 
                    }
                    else if ((i == W) & (j == H))
                    {
                        //room1.EventRoomSize1(e, step1, step2);
                        new_node = new TreeNode { Data = new Point(i, j), Size = 13, Link = 0, Children = new List<TreeNode>(3), Door = 0 };
                        this.nodes[99] = new_node; // need to put condition of depulacte 
                    }
                    else

                    {
                        int r1 = r.Next(1, V - 1);
                        // r1 = r.Next(1, V - 1);
                        new_node = new TreeNode { Data = new Point(i, j), Size = r1, Link = 0, Children = new List<TreeNode>(3), Door = 0 };

                        this.nodes[ij] = new_node;


                    }
                    ij++;
                }

            }


        }


        public double getFitness()
        {
            double CalculFitness = 0;
            double diffecltyScore = 1;
            double pinaltyScore1;
            double pinaltyScore2;
            if (this.nodes.Count() > 0)
            {

                TreeNode EndPoint = this.nodes[99];
                TreeNode StartPoint = this.nodes[0];
                //System.Diagnostics.Debug.WriteLine(" current is = " + current.Data);
                this.ListOfNodeResult.Clear();
                this.TraceNode.Clear();
                SearchLongLink(StartPoint);
                SearchLongLinkEnd(EndPoint);
                pinaltyScore1 = CalculateThePinaltyScoreStart();
                pinaltyScore2 = CalculateThePinaltyScoreEnd();
                if (this.ListOfNodeResult.Contains(EndPoint) || this.TraceNode.Contains(StartPoint))
                {

                    diffecltyScore = CalculateDiffecltyScour();
                }
             
                pinaltyScore1 = CalculateThePinaltyScoreStart();
                pinaltyScore2 = CalculateThePinaltyScoreEnd();
                double MapQuilty = 0.0;
                if (CountNodesWithTypeRepaetMoreThan10() == 1)
                    MapQuilty = 0.7;
                else if (CountNodesWithTypeRepaetMoreThan10() == 2)
                    MapQuilty = 0.5;
                else if (CountNodesWithTypeRepaetMoreThan10() == 3)
                    MapQuilty = 0.25;
                else if (CountNodesWithTypeRepaetMoreThan10() == 0)
                    MapQuilty = 1;


                CalculFitness = ((ListOfNodeResult.Count() * pinaltyScore1) + (TraceNode.Count() * pinaltyScore2)) * diffecltyScore * MapQuilty;

            }

            return CalculFitness;
        }

        public void SearchLongLink(TreeNode Current)
        {

            if (Current.Children != null)
            {
                foreach (TreeNode child in Current.Children)
                {


                    if ((child != null) && CheckTheDoor(Current, child))

                    {
                        Current = child;
                        if (!IsNodeAvailable(Current.Data, this.ListOfNodeResult))
                        { this.ListOfNodeResult.Add(Current); }

                        SearchLongLink(Current);

                    }

                }


            }
            // System.Diagnostics.Debug.WriteLine(" Maxlink = "+ MaxlinkUp);

        }

        void SearchLongLinkEnd(TreeNode current)
        {

            if (current.Parent1 != null)
            {

                if (CheckTheDoor(current.Parent1, current))
                {

                    current = current.Parent1;
                    if (!IsNodeAvailable(current.Data, this.TraceNode))
                    { this.TraceNode.Add(current); }
                    SearchLongLinkEnd(current);
                }

            }
            if (current.Parent2 != null)
            {

                if ((CheckTheDoor(current.Parent2, current)))
                {

                    current = current.Parent2;
                    if (!IsNodeAvailable(current.Data, this.TraceNode))
                    { this.TraceNode.Add(current); }
                    SearchLongLinkEnd(current);
                }


            }




        }

        public double CalculateDiffecltyScour()
        {
            double TheTotalOfSize = 0;
            foreach (var node in this.ListOfNodeResult)
            {
                if (difficultyScore.TryGetValue(node.Size, out int ScorValue))
                {
                    TheTotalOfSize += ScorValue;
                }
                else
                {
                    // Handle the case where the key is not available in the dictionary
                    // For example, you can use a default value or throw an exception.
                    // For now, let's use a default value of 0.
                    TheTotalOfSize += 0;
                }
            }
            return TheTotalOfSize;
        }

        public double CalculateThePinaltyScoreStart()
        {
            double ScoreValue = 1;
            TreeNode EndPoint = this.nodes[99];
            if (this.ListOfNodeResult.Count() > 0)
            {
                TreeNode lastItem1 = this.ListOfNodeResult[ListOfNodeResult.Count() - 1];

                if (this.ListOfNodeResult.Contains(EndPoint))
                    ScoreValue = 1;
                else if ((lastItem1.Data.X > 1 && lastItem1.Data.X < 6) && (lastItem1.Data.Y > 1 && lastItem1.Data.Y < 6))
                    ScoreValue = 0.25;
                else if ((lastItem1.Data.X > 1 && lastItem1.Data.X < 6) && (lastItem1.Data.Y > 4 && lastItem1.Data.Y < 11))
                    ScoreValue = 0.5;
                else if ((lastItem1.Data.X > 4 && lastItem1.Data.X < 11) && (lastItem1.Data.Y > 1 && lastItem1.Data.Y < 6))
                    ScoreValue = 0.5;
                else
                    ScoreValue = 0.7;
            }


            return ScoreValue;

        }
        public double CalculateThePinaltyScoreEnd()
        {
            double ScoreValue = 1;
            TreeNode StartPoint = this.nodes[0];
            int index = this.TraceNode.Count() - 1;
            if (this.TraceNode.Count() > 0)
            {
                TreeNode lastItem2 = this.TraceNode[index];
                if (this.TraceNode.Contains(StartPoint))
                    ScoreValue = 1;
                else if ((lastItem2.Data.X > 1 && lastItem2.Data.X < 6) && (lastItem2.Data.Y > 1 && lastItem2.Data.Y < 6))
                    ScoreValue = 0.7;
                else if ((lastItem2.Data.X > 1 && lastItem2.Data.X < 6) && (lastItem2.Data.Y > 4 && lastItem2.Data.Y < 11))
                    ScoreValue = 0.5;
                else if ((lastItem2.Data.X > 4 && lastItem2.Data.X < 11) && (lastItem2.Data.Y > 1 && lastItem2.Data.Y < 6))
                    ScoreValue = 0.5;
                else
                    ScoreValue = 0.25;
            }

            return ScoreValue;

        }

        Boolean CheckTheDoor(TreeNode ParentDoor, TreeNode ChildDoor)
        {
            int s1 = ParentDoor.Size;
            int s2 = ChildDoor.Size;
            int DoorCode = 0;
            Boolean TheDoor = false;
            if ((ParentDoor.Data.X == ChildDoor.Data.X) & (ParentDoor.Data.Y - ChildDoor.Data.Y == -1))
            {
                DoorCode = 1;
            }
            else if ((ParentDoor.Data.Y == ChildDoor.Data.Y) & (ParentDoor.Data.X - ChildDoor.Data.X == -1))
            {
                DoorCode = 2;
            }

            switch (DoorCode)
            {
                case 1:
                    {
                        if (new List<int> { 3, 5, 6, 7, 8, 9, 11, 13 }.Contains(s1) & new List<int> { 1, 3, 5, 6, 7, 8, 9, 11, 13 }.Contains(s2))
                        {
                            TheDoor = true;
                        }
                        break;

                    }
                case 2:
                    {
                        if (new List<int> { 4, 7, 8, 10, 12, 13 }.Contains(s1) & new List<int> { 2, 7, 8, 10, 12, 13 }.Contains(s2))
                        {
                            TheDoor = true;
                        }
                        break;
                    }
                default:
                    {
                        break;

                    }


            }
            return TheDoor;



        }

        public static bool IsNodeAvailable(Point nodeId, List<TreeNode> nodeList)
        {
            // Check if the node exists in the list
            foreach (TreeNode node in nodeList)
            {
                if (node.Data == nodeId)
                {
                    return true; // Node found
                }
            }

            return false; // Node not found
        }

        public Boolean reviewpositions()
        {
            if (this.nodes.Count() > 0)
            {
                TreeNode current = this.nodes[99];


                int MaxlinkUp = 1;

                SearchLongLink(this.nodes[0]);

                int MaxLinkDown = 1;
                SearchLongLinkEnd(current);
                if (MaxlinkUp > MaxLinkDown)
                    return true;
                else
                    return false;
            }
            else
                return false;

        }



        public void WriteMapInExcel(ExcelFile Xtest, int row, int col, String MessagInExcel)
        {
            System.Diagnostics.Debug.WriteLine(" father is ");
            int Row = row; int Col = col;

            System.Diagnostics.Debug.WriteLine(MessagInExcel);
            Xtest.writeXcelsheet(Row, Col, MessagInExcel);
            System.Diagnostics.Debug.WriteLine(MessagInExcel);
            Row++;
            for (int i = 0; i < this.nodes.Count(); i++)
            {
                Xtest.writeXcelsheet(Row, Col, this.nodes[i].Data.ToString() + ":" + this.nodes[i].Size);
                // System.Diagnostics.Debug.WriteLine(this.nodes[i].Data.ToString());
                if (this.nodes[i].Parent1 != null)
                {
                    Xtest.writeXcelsheet(Row, Col + 1, this.nodes[i].Parent1.Data.ToString() + ":" + this.nodes[i].Parent1.Size);
                }
                if (this.nodes[i].Children.Count() > 0)
                {

                    Xtest.writeXcelsheet(Row, Col + 2, this.nodes[i].Children[0].Data.ToString() + ":" + this.nodes[i].Children[0].Size);
                    if (this.nodes[i].Children.Count() > 1)
                    {
                        Xtest.writeXcelsheet(Row, Col + 3, this.nodes[i].Children[1].Data.ToString() + ":" + this.nodes[i].Children[1].Size);
                    }
                }


                Row++;

            }
            Xtest.excelClose();
            Xtest.ExcelSave();
        }

        public void MapSizeInExcel(ExcelFile Xtest, string TitleFile, int TheFitness)
        {



            for (int i = 0; i < this.nodes.Count(); i++)
            {
                Xtest.writeXcelsheet(this.nodes[i].Data.Y, this.nodes[i].Data.X, this.nodes[i].Size.ToString());


            }

            Xtest.writeXcelsheet(12, 1, TitleFile);
            Xtest.writeXcelsheet(12, 3, TheFitness.ToString());

            Xtest.excelClose();
            // Xtest.ExcelSave();
        }

        public List<int> CountNodesByType()
        {
            // Initialize a list to store the counts for each size (from 1 to 13)
            List<int> sizeCounts = Enumerable.Repeat(0, 13).ToList();

            // Loop through the nodes and count nodes for each size
            foreach (var node in this.nodes)
            {
                int size = node.Size;
                if (size >= 1 && size <= 13)
                {
                    sizeCounts[size - 1]++;
                }
            }

            return sizeCounts;
        }
        public int CountNodesWithTypeRepaetMoreThan10()
        {
            List<int> ListOfCount = this.CountNodesByType();
            int count = 0;

            foreach (var node in ListOfCount)
            {

                if (node > 12)
                {
                    count++;
                }
            }

            return count;
        }

        public void DrawGraph(PaintEventArgs e, int A, int step1, int step2, int ImgW, int ImgH)
        {
            Room room1 = new Room();
            TreeNode root1 = new TreeNode();

            for (int i = 0; i < 100; i++)
            {
                root1 = nodes[i];
                int r1 = root1.Size;


                switch (r1)
                {
                    case 1:

                        room1.NormalRoomSize1(e, root1.Data.X * A + step1, root1.Data.Y * A + step2);

                        break;

                    case 2:

                        room1.NormalRoomSize2(e, root1.Data.X * A + step1, root1.Data.Y * A + step2);

                        break;

                    case 3:
                        room1.NormalRoomSize3(e, root1.Data.X * A + step1, root1.Data.Y * A + step2);

                        break;

                    case 4:
                        room1.NormalRoomSize4(e, root1.Data.X * A + step1, root1.Data.Y * A + step2);

                        break;
                    case 5:
                        room1.NormalRoomSize5(e, root1.Data.X * A + step1, root1.Data.Y * A + step2);

                        break;
                    case 6:
                        room1.NormalRoomSize6(e, root1.Data.X * A + step1, root1.Data.Y * A + step2);

                        break;
                    case 7:
                        room1.NormalRoomSize7(e, root1.Data.X * A + step1, root1.Data.Y * A + step2);

                        break;

                    case 8:
                        room1.PolygonSize8(e, root1.Data.X * A + step1, root1.Data.Y * A + step2);

                        break;

                    case 9:

                        room1.PolygonSize9(e, root1.Data.X * A + step1, root1.Data.Y * A + step2);
                        break;

                    case 10:

                        room1.PolygonSize10(e, root1.Data.X * A + step1, root1.Data.Y * A + step2);
                        break;
                    case 11:

                        room1.HallWays11(e, root1.Data.X * A + step1, root1.Data.Y * A + step2);
                        break;

                    case 12:

                        room1.HallWays12(e, root1.Data.X * A + step1, root1.Data.Y * A + step2);
                        break;

                    case 13:

                        room1.EventRoomSize13(e, root1.Data.X * A + step1, root1.Data.Y * A + step2);

                        break;

                    default:
                        break;


                }
            }
            room1.ImagBorders(e, A + step1, A + step2, A * (ImgW), A * (ImgH));
        } // end of Dograph

        public void OnPaint(PaintEventArgs e)
        {
            //base.OnPaint(e);

            // Create a Graphics object from the PaintEventArgs
            Graphics g = e.Graphics;

            // Define the font and brush for the text
            Font font = new Font("Arial", 8);
            Brush brush = Brushes.Black;
            // string text = "Fitness value = "+ this.FitnessValue.ToString();
            string text1 = " ";
            string text2 = " ";
            List<int> typeCounts = CountNodesByType();
            for (int size = 1; size <= typeCounts.Count; size++)
            {
                text1 += typeCounts[size - 1].ToString() + "+";
            }
             text1 += "="+ CountNodesWithTypeRepaetMoreThan10().ToString();
             text1 = "Particle Swarm Optimaization #1";
            text2 = "Fitness Value = " + this.FitnessValue.ToString();
            // Calculate the position for the text
            int x = 150;
            int y = 400; int y2 = 415;

            // Draw the text on the form
            g.DrawString(text1, font, brush, x, y);
            g.DrawString(text2, font, brush, x, y2);
        }

    }
}
