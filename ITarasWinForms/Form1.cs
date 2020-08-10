using Karambolo.PO;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public List<Node> nodesList = new List<Node>();
        public Form1()
        {
            InitializeComponent();

            var parser = new POParser();
            POParseResult result;
            using (var reader = new StreamReader(@"C:\LOGTEST\example.po"))
            result = parser.Parse(reader);
            if (result.Success)
            {
                POCatalog catalog = result.Catalog;
                
                int index = 0;               
                foreach (IPOEntry item in catalog)
                {
                    var context = item.Key.ContextId;
                    string[] nodes = context.Split('.');

                    Node nodeListitem = new Node()
                    {
                        Levels = nodes.Length,
                        Nodes = nodes,   
                        Names = context,
                        Id = item.Key.Id                        
                    };

                    int treeLevelCount = 1;                     
                    foreach (string node in nodes)
                    {
                        List<Node> fullNodNameTemp = nodesList.Where(x => x.Names == context).ToList();
                        if (fullNodNameTemp.Count == 0)
                        {
                            List<Node> nodeNameTemp = nodesList.Where(x => x.Nodes.Contains(node)).ToList();

                            if (nodeNameTemp.Count == 0)
                            {
                                switch (treeLevelCount)
                                {
                                    case 1:
                                        treeView1.Nodes.Add(node, node);                                        
                                        break;
                                    case 2:
                                        treeView1.Nodes[index].Nodes.Add(node, node);                                        
                                        break;
                                    case 3:
                                        treeView1.Nodes[index].Nodes[0].Nodes.Add(node, node);
                                        index++;                                                                            
                                        break;
                                    default:                                        
                                        break;
                                }                                
                            }                            
                        }                       
                        treeLevelCount++;                        
                    }
                    nodesList.Add(nodeListitem);
                }                
            }
            else
            {
                IDiagnostics diagnostics = result.Diagnostics;                
            }
        }


        private void treeView1_DoubleClick(object sender, EventArgs e)
        {            
            List<Node> selectedNodes = nodesList.Where(x => x.Nodes.Contains(treeView1.SelectedNode.Name)).ToList();
            listView1.Clear();
            foreach (Node item in selectedNodes)
            {
                listView1.Items.Add(item.Id);
            }       
        }
    }

    public class Node
    {
        public string Id { get; set; }
        public int Levels { get; set; }
        public string Names { get; set; }
        public string[] Nodes { get; set; }     
        
    }
}
