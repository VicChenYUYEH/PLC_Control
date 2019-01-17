using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace HyTemplate
{
    public struct RecipeInfo
    {
        public string DeviceName { get; set; }
        public double SetPoint { get; set; }
        public string Unit { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }
    }

    public class Recipe : XmlList
    {
        Dictionary<String, RecipeInfo> dicRecipe = new Dictionary<string, RecipeInfo>();
        Dictionary<String, RecipeInfo> dicSystem = new Dictionary<string, RecipeInfo>();
        public Dictionary<String, RecipeInfo> RecipeDetail { get { return dicRecipe; } }
        public Dictionary<String, RecipeInfo> SystemDetail { get { return dicSystem; } }
        public string FileName { get; set; }
        public string CurrentRecipeId { get; set; }

        public Recipe(string m_Xml = "Recipe.xml")
        {
            FileName = System.IO.Directory.GetCurrentDirectory() + "\\config\\" + m_Xml;

            InitialRecipeDetail();
        }

        private void InitialRecipeDetail()
        {
            if (!System.IO.File.Exists(FileName)) return;
            
            XmlDocument XmlDoc = new XmlDocument();
            XmlDoc.Load(FileName);

            XmlNodeList nodes = XmlDoc.SelectNodes("Recipe/Group");
            foreach (XmlNode chile_node in nodes)
            {
                string ID = chile_node.Attributes["ID"].Value;
                if (ID == "System")
                {
                    if (chile_node.HasChildNodes)
                    {
                        foreach (XmlNode node in chile_node)
                        {
                            String para_id = node.Attributes["Name"].Value;
                            RecipeInfo info = new RecipeInfo();
                            if (!dicSystem.ContainsKey(para_id))
                            {
                                info.DeviceName = para_id;
                                info.Unit = node.Attributes["Unit"].Value;
                                info.SetPoint = 0;
                                info.Description = node.Attributes["Description"].Value;
                                info.Address = node.Attributes["DeviceName"].Value;
                                dicSystem.Add(para_id, info);
                            }
                        }
                    }
                }
                else
                {
                    if (chile_node.HasChildNodes)
                    {
                        foreach (XmlNode node in chile_node)
                        {
                            String para_id = node.Attributes["Name"].Value;
                            RecipeInfo info = new RecipeInfo();
                            if (!dicRecipe.ContainsKey(para_id))
                            {
                                info.DeviceName = para_id;
                                info.Unit = node.Attributes["Unit"].Value;
                                info.SetPoint = 0;
                                info.Description = node.Attributes["Description"].Value;
                                info.Address = node.Attributes["DeviceName"].Value;
                                dicRecipe.Add(para_id, info);
                            }
                        }
                    }
                }
            }
        }

        public bool loadFile(string recipe = "")
        {
            if (!System.IO.File.Exists(FileName)) return false;

            this.getNodes().Clear();

            XmlDocument XmlDoc = new XmlDocument();
            XmlDoc.Load(FileName);

            XmlNodeList nodes = XmlDoc.SelectNodes("Recipe/Group");
            foreach (XmlNode chile_node in nodes)
            {
                String rcp_id = chile_node.Attributes["ID"].Value;
                String rcp_in_use = chile_node.Attributes["In_Use"].Value;

                this.addNode(rcp_id, rcp_in_use);

                if (chile_node.HasChildNodes)
                {
                    foreach (XmlNode node in chile_node)
                    {
                        String para_id = node.Attributes["Name"].Value;
                        String para_value = node.Attributes["Value"].Value;

                        this[rcp_id].addChildNode(para_id, para_value);

                        if(rcp_id == "System")
                        {
                            dicSystem.Remove(para_id);
                            RecipeInfo info = new RecipeInfo();
                            info.DeviceName = para_id;
                            info.Unit = node.Attributes["Unit"].Value;
                            info.SetPoint = Convert.ToDouble(para_value);
                            info.Address = node.Attributes["DeviceName"].Value;
                            info.Description = node.Attributes["Description"].Value;
                            dicSystem.Add(para_id, info);
                        }
                        else if(recipe == rcp_id)//寫入Dictionary
                        {
                            dicRecipe.Remove(para_id);
                            RecipeInfo info = new RecipeInfo();
                            info.DeviceName = para_id;
                            info.Unit = node.Attributes["Unit"].Value;
                            info.SetPoint = Convert.ToDouble(para_value);
                            info.Address = node.Attributes["DeviceName"].Value;
                            info.Description = node.Attributes["Description"].Value;
                            dicRecipe.Add(para_id, info);
                        }
                    }
                }
            }

            return true;
        }

        public bool saveFile()
        {
            if (!System.IO.File.Exists(FileName)) return false;

            XmlDocument XmlDoc = new XmlDocument();
            XmlDoc.Load(FileName);

            XmlNodeList nodes = XmlDoc.SelectNodes("Recipe/Group");

            List<XmlItem> current = this.getNodes();
            foreach (XmlItem item in current)
            {
                bool find = false;
                foreach (XmlNode chile_node in nodes)
                {
                    String rcp_id = chile_node.Attributes["ID"].Value;
                    if (item.Key != rcp_id) continue;

                    find = true;
                    chile_node.Attributes["In_Use"].Value = this[rcp_id].Value;
                    
                    if (chile_node.HasChildNodes)
                    {
                        foreach (XmlNode node in chile_node)
                        {
                            String para_id = node.Attributes["Name"].Value;
                            node.Attributes["Value"].Value = this[rcp_id][para_id].Value;
                        }
                    }
                }

                if (!find)
                {
                    XmlNode group = nodes.Item(1).CloneNode(true); //建立第2個一模一樣的RCP
                    group.Attributes["ID"].Value = item.Key;
                    group.Attributes["In_Use"].Value = item.Value;
                    XmlNodeList rcp_node = XmlDoc.SelectNodes("Recipe");
                    rcp_node.Item(0).AppendChild(group);
                }
            }

            XmlDoc.Save(FileName);
            return true;
        }

        public void appendRecipe(string m_RcpId, string m_RcpInUse)
        {
            this.addNode(m_RcpId, m_RcpInUse);

            //Create new recipe Body
            List<XmlItem> nodes = this.getNodes();
            if (nodes.Count > 0)
            {
                foreach (XmlItem tmp_node in nodes[0].Nodes)
                {
                    this[m_RcpId].addChildNode(tmp_node.Key, "0");
                }
            }
        }

        public void eraseRecipe(string m_RecipeId)
        {
            XmlDocument XmlDoc = new XmlDocument();
            XmlDoc.Load(FileName);

            foreach (XmlNode node in XmlDoc.SelectNodes("Recipe/Group"))
            {
                if (node.Attributes["ID"].Value == m_RecipeId)
                {
                    XmlNode parent = node.ParentNode;
                    parent.RemoveChild(node);
                    break;
                }
            }
            XmlDoc.Save(FileName);
        }
    }
}
