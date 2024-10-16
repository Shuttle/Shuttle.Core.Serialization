using System;
using System.Text;
using System.Xml;

namespace Shuttle.Core.Serialization;

public class XmlSerializerOptions
{
    public bool Indent { get; set; }
    public bool OmitXmlDeclaration { get; set; } = true;
    public Encoding Encoding { get; set; } = Encoding.UTF8;
    public NewLineHandling NewLineHandling { get; set; } = NewLineHandling.None;
    public string NewLineChars { get; set; } = Environment.NewLine;
    public bool NewLineOnAttributes { get; set; }
    public bool CheckCharacters { get; set; } = true;
    public ConformanceLevel ConformanceLevel { get; set; } = ConformanceLevel.Document;
    public bool CloseOutput { get; set; }
    public string IndentChars { get; set; } = "  ";
    public NamespaceHandling NamespaceHandling { get; set; } = NamespaceHandling.Default;
    public bool DoNotEscapeUriAttributes { get; set; }
    public bool WriteEndDocumentOnClose { get; set; } = true;
    public int MaxArrayLength { get; set; } = 16384;
    public int MaxStringContentLength { get; set; } = 8192;
    public int MaxNameTableCharCount { get; set; } = 16384;
    public int MaxBytesPerRead { get; set; } = 4096;
    public int MaxDepth { get; set; } = 32;
}