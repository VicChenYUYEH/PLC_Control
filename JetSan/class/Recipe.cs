using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using HongYuDLL;

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
        public Dictionary<String, RecipeInfo> DicRecipeDetail { get; } = new Dictionary<string, RecipeInfo>();
        public Dictionary<String, RecipeInfo> DicSystemDetail { get; } = new Dictionary<string, RecipeInfo>();
        public string sFileName { get; set; }
        public string sCurrentRecipeId { get; set; }

        public Recipe(string m_Xml = "Recipe.xml")
        {
            sFileName = System.IO.Directory.GetCurrentDirectory() + "\\config\\" + m_Xml;

            initialRecipeDetail();
        }

        private void initialRecipeDetail()
        {
            if (!System.IO.File.Exists(sFileName)) return;
            
            XmlDocument XmlDoc = new XmlDocument();
            XmlDoc.Load(sFileName);

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
                            if (!DicSystemDetail.ContainsKey(para_id))
                            {
                                info.DeviceName = para_id;
                                info.Unit = node.Attributes["Unit"].Value;
                                info.SetPoint = 0;
                                info.Description = node.Attributes["Description"].Value;
                                info.Address = node.Attributes["DeviceName"].Value;
                                DicSystemDetail.Add(para_id, info);
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
                            if (!DicRecipeDetail.ContainsKey(para_id))
                            {
                                info.DeviceName = para_id;
                                info.Unit = node.Attributes["Unit"].Value;
                                info.SetPoint = 0;
                                info.Description = node.Attributes["Description"].Value;
                                info.Address = node.Attributes["DeviceName"].Value;
                                DicRecipeDetail.Add(para_id, info);
                            }
                        }
                    }
                }
            }
        }

        public bool LoadFile(string m_Recipe = "")
        {
            if (!System.IO.File.Exists(sFileName)) return false;

            this.GetNodes().Clear();

            XmlDocument XmlDoc = new XmlDocument();
            XmlDoc.Load(sFileName);

            XmlNodeList nodes = XmlDoc.SelectNodes("Recipe/Group");
            foreach (XmlNode chile_node in nodes)
            {
                string rcp_id = chile_node.Attributes["ID"].Value;
                string rcp_in_use = chile_node.Attributes["In_Use"].Value;

                this.AddNode(rcp_id, rcp_in_use);

                if (chile_node.HasChildNodes)
                {
                    foreach (XmlNode node in chile_node)
                    {
                        string para_id = node.Attributes["Name"].Value;
                        string para_value = node.Attributes["Value"].Value;

                        this[rcp_id].addChildNode(para_id, para_value);

                        if(rcp_id == "System")
                        {
                            DicSystemDetail.Remove(para_id);
                            RecipeInfo info = new RecipeInfo
                            {
                                DeviceName = para_id,
                                Unit = node.Attributes["Unit"].Value,
                                SetPoint = Convert.ToDouble(para_value),
                                Address = node.Attributes["DeviceName"].Value,
                                Description = node.Attributes["Description"].Value
                            };
                            DicSystemDetail.Add(para_id, info);
                        }
                        else if(m_Recipe == rcp_id)//寫入Dictionary
                        {
                            DicRecipeDetail.Remove(para_id);
                            RecipeInfo info = new RecipeInfo
                            {
                                DeviceName = para_id,
                                Unit = node.Attributes["Unit"].Value,
                                SetPoint = Convert.ToDouble(para_value),
                                Address = node.Attributes["DeviceName"].Value,
                                Description = node.Attributes["Description"].Value
                            };
                            DicRecipeDetail.Add(para_id, info);
                        }
                    }
                }
            }

            return true;
        }

        public bool SaveFile()
        {
            if (!System.IO.File.Exists(sFileName)) return false;

            XmlDocument XmlDoc = new XmlDocument();
            XmlDoc.Load(sFileName);

            XmlNodeList nodes = XmlDoc.SelectNodes("Recipe/Group");

            List<XmlItem> current = this.GetNodes();
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

            XmlDoc.Save(sFileName);
            return true;
        }

        public void AppendRecipe(string m_RcpId, string m_RcpInUse)
        {
            this.AddNode(m_RcpId, m_RcpInUse);

            //Create new recipe Body
            List<XmlItem> nodes = this.GetNodes();
            if (nodes.Count > 0)
            {
                foreach (XmlItem tmp_node in nodes[0].Nodes)
                {
                    this[m_RcpId].addChildNode(tmp_node.Key, "0");
                }
            }
        }

        public void EraseRecipe(string m_RecipeId)
        {
            XmlDocument XmlDoc = new XmlDocument();
            XmlDoc.Load(sFileName);

            foreach (XmlNode node in XmlDoc.SelectNodes("Recipe/Group"))
            {
                if (node.Attributes["ID"].Value == m_RecipeId)
                {
                    XmlNode parent = node.ParentNode;
                    parent.RemoveChild(node);
                    break;
                }
            }
            XmlDoc.Save(sFileName);
        }
        
    }
}
