using System.Xml;
using System.Xml.Linq;

namespace AlchiwebApp.Console.AddToBitPlatform.Extensions;

public static class XDocumentExtensions
{
    public static void SaveCsproj(this XDocument doc, string filename)
    {
        if (doc != null)
        {
            XmlWriterSettings xws = new XmlWriterSettings
            {
                OmitXmlDeclaration = true,
                Indent = true,
            };
            using (XmlWriter xw = XmlWriter.Create(filename, xws))
                doc.Save(xw);
        }
    }

}
