namespace System.Xml.Linq;

public static class XDocumentExtensions
{
    public static void SaveXmlFile(this XDocument doc, string filename)
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
