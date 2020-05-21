using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Serialization;


namespace PARLIB
{
    public class KeyDesc
    {
        public string key;
        public Desc desc;
        public KeyDesc() { }
        public KeyDesc(string _key, Desc _desc)
        {
            key = _key;
            desc = _desc;
        }
    }
    public class Desc
    {
        public string description;
        public bool actual;
        public Access accsess;
        public Desc()
        {
            description = null;
            actual = false;
            accsess = new Access();
        }
    }

    [SerializableAttribute]
    public class Descs : Dictionary<string, Desc>
    {
        public Descs() { }
        public Descs(string _fname)
        {
            try
            {
                using (FileStream s = new FileStream(_fname, FileMode.Open))
                    Deserialize(s);
            }
            catch (Exception e)
            {
                object o = e;
            }
        }
        public void Save(string _fname)
        {
            using (FileStream s = new FileStream(_fname, FileMode.Create))
                Serialize(s);
        }
        public new Desc this[string _key]
        {
            get
            {
                Desc desc;
                if (!TryGetValue(_key, out desc))
                    return (null);
                return (desc);
            }
            set
            {
                Remove(_key);
                Add(_key, value);
            }
        }

        void Serialize(Stream _stream)
        {
            List<KeyDesc> L = new List<KeyDesc>();
            XmlSerializer xml = new XmlSerializer(typeof(List<KeyDesc>));
            foreach (string key in Keys)
                L.Add(new KeyDesc(key, this[key]));
            xml.Serialize(_stream, L);
        }
        void Deserialize(Stream _stream)
        {
            XmlSerializer xml = new XmlSerializer(typeof(List<KeyDesc>));
            List<KeyDesc> L = (List<KeyDesc>)xml.Deserialize(_stream);
            Clear();
            foreach (KeyDesc kd in L)
                Add(kd.key, kd.desc);
        }
    }
}
