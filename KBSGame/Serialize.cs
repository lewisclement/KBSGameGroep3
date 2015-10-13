using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace KBSGame
{
    class Serialize
    {
        public Serialize()
        {
        }
        public void SerializeObject<T>(T serializableObject, String FileName)
        {
            if (serializableObject == null) { Console.WriteLine("empty"); }

            //try
            //{
                XmlDocument xmlDocument = new XmlDocument();
                XmlSerializer serializer = new XmlSerializer(serializableObject.GetType());
                using (MemoryStream stream = new MemoryStream())
                {
                
                    serializer.Serialize(stream, serializableObject);
                    stream.Position = 0;
                    xmlDocument.Load(stream);
                    xmlDocument.Save(FileName);
                    stream.Close();
                }
            }
            //catch (Exception ex)
            //{
            //    Console.WriteLine(ex.Message);
            //}
        }
    }
//}
