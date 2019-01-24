using System;
using System.Collections.Generic;

namespace HongYuDLL
{
    public class XmlItem
    {
        private Dictionary<String, String> dicAttribute = new Dictionary<string, string>();
        //List<Attribute> lsAttribute = new List<Attribute>();
        List<XmlItem> lsChild = new List<XmlItem>();

        private string sKey = "";
        private string sValue = "";

        public string Key { get { return sKey; } set { sKey = value; } }
        public string Value { get { return sValue; } set { sValue = value; } }

        public Dictionary<String, String> Attributes  {  get { return dicAttribute; } }

        public List<XmlItem> Nodes { get { return lsChild; } }


        public XmlItem()
        {

        }

        private XmlItem getNode(string m_Key)
        {
            //return lsChild.Where(node => node.ItemKey == m_Key);
            foreach (XmlItem nod in lsChild)
            {
                if (nod.Key == m_Key)
                {
                    return nod;
                }
            };
            return null;
        }
        
        private void setNode(XmlItem m_Item)
        {
            XmlItem item = getNode(m_Item.Key);
            if (item != null)
            {
                item.sValue = m_Item.sValue;
            }

        }

        public void addChildNode(string m_Key, string m_Value)
        {
            XmlItem nod = new XmlItem();
            nod.sKey = m_Key;
            nod.sValue = m_Value;
            lsChild.Add(nod);
        }

        public XmlItem this[string m_Key]
        {
            get { return getNode(m_Key); }
            set { setNode(this); }
        }
    }

    public class XmlList
    {
        private List<XmlItem> lsXml = new List<XmlItem>();

        public Dictionary<String, String> Attributes { get; } = new Dictionary<string, string>();

        public List<XmlItem> GetNodes()
        {
            return lsXml;
        }

        public XmlItem GetNode(string m_Key)
        {
            foreach (XmlItem nod in lsXml)
            {
                if (nod.Key == m_Key)
                {
                    return nod;
                }
            };
            return null;
        }

        public void AddNode(string m_Key, string m_Value)
        {
            XmlItem nod = new XmlItem();
            nod.Key = m_Key;
            nod.Value = m_Value;
            lsXml.Add(nod);
        }

        public XmlItem this[string m_Key]
        {
            get { return GetNode(m_Key); }
           // set { setNode(this); }
        }

    }
}

#region Sample
//    //XmlList doc = new XmlList();
//    //doc.addNode("a", "B");

//    //XmlItem node = doc.getNode("a");
//    //node.addChildNode("c", "D");
//    //node["c"].addChildNode("e", "F");
//    //node["c"].Attributes["XX"] = "ABC";

//    Recipe rcp = new Recipe("Recipe.xml");
//    rcp.loadFile();

//    string t1 = rcp["RCP1"]["PLC_IN_L1011"].Value;
//    string t3 = rcp["RCP3"]["PLC_IN_L1031"].Value;

//    rcp["RCP1"]["PLC_IN_L1011"].Value = "555";
//    rcp["RCP3"]["PLC_IN_L1031"].Value = "777";

//    rcp.addNode("RCP4", "RCP4_1");
//    rcp["RCP4"].addChildNode("PLC_IN_L1041", "aaa");

//    rcp.saveFile();
#endregion