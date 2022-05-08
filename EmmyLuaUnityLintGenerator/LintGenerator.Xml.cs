using System;
using System.Collections.Generic;
using System.Xml;

public static partial class LintGenerator
{
    class ParamDescriptor
    {
        public string name;
        public string summary;
        public string[] summarylines;
    }
    class Descriptor
    {
        public string name;
        public string summary = "";
        public string @return = "";
        public List<ParamDescriptor> @params = new List<ParamDescriptor>();
        public string[] summarylines;
        public string[] @returnlines;

        public void NoFunctionName()
        {
            var left = name.IndexOf('(');
            if (left < 0)
                return;
            name = name.Substring(0, left);
        }

        public void SplitLines()
        {
            summarylines = summary.Split('\n');
            @returnlines = @return.Split('\n');
            foreach (var p in @params)
            { 
                p.summarylines = p.summary.Split('\n');
            }
        }
    }

    static void PrepareXML(string xmlFile)
    {
        try
        {
            var doc = new XmlDocument();
            doc.Load(xmlFile);
            ParseXml(doc);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
        }

        foreach (var d in descriptors)
        {
            if (!dictDescriptor.TryGetValue(d.name, out var dlist))
            {
                dlist = new List<Descriptor>();
                dictDescriptor.Add(d.name, dlist);
            }
            d.SplitLines();
            dlist.Add(d);
        }
    }

    static void ParseXml(XmlNode node)
    {
        var nodename = node.Name.ToLower();
        switch (nodename)
        {
            case "member":
                {
                    var name = GetAttribName(node);
                    if (!string.IsNullOrEmpty(name))
                    {
                        lastDescriptor = new Descriptor
                        {
                            name = name.Substring(2, name.Length - 2),
                        };
                        lastDescriptor.NoFunctionName();
                        descriptors.Add(lastDescriptor);
                    }
                    else
                    {
                        lastDescriptor = null;
                    }
                    lastSummary = false;
                    lastReturn = false;
                }
                break;
            case "summary":
                lastSummary = true;
                lastReturn = false;
                break;
            case "returns":
                lastSummary = false;
                lastReturn = true;
                break;
            case "para":
                if (lastDescriptor != null)
                {
                    var childnode = node.FirstChild as XmlNode;
                    if (childnode != null)
                    {
                        if (lastSummary)
                        {
                            lastDescriptor.summary += childnode.Value;
                        }
                        else if (lastReturn)
                        {
                            lastDescriptor.@return += childnode.Value;
                        }
                    }
                }
                break;
            case "param":
                if (lastDescriptor != null)
                {
                    var childnode = node.FirstChild as XmlNode;
                    lastSummary = false;
                    lastReturn = false;
                    lastDescriptor.@params.Add(new ParamDescriptor
                    {
                        name = GetAttribName(node),
                        summary = childnode!= null ?childnode.Value : string.Empty,
                    });
                }
                break;
            default:
                //Console.WriteLine($"Warning! {nodename} is not supported!");
                break;
        }

        foreach (var child in node.ChildNodes)
        {
            if (child is XmlNode childnode)
                ParseXml(childnode);
        }
    }

    static string GetAttribName(XmlNode node)
    {
        var iter = node.Attributes.GetEnumerator();
        while (iter.MoveNext())
        {
            var a = iter.Current;
            var xa = a as XmlAttribute;
            if (string.CompareOrdinal(xa.Name.ToLower(), "name") == 0)
            {
                return xa.Value;
            }
        }
        return null;
    }

    static List<Descriptor> GetDescriptors(string name)
    {
        if(!string.IsNullOrEmpty(PrefixNamespace))
            name = name.Substring(PrefixNamespace.Length);

        dictDescriptor.TryGetValue(name, out var list);
        return list;
    }

    static Descriptor GetFirstDescriptor(string name)
    {
        if (!string.IsNullOrEmpty(PrefixNamespace))
            name = name.Substring(PrefixNamespace.Length);

        if (dictDescriptor.TryGetValue(name, out var list))
        {
            return list[0];
        }
        else
        {
            return null;
        }
    }

    static Descriptor GetMatchDescriptor(string name, List<string> @params)
    {
        if (!string.IsNullOrEmpty(PrefixNamespace))
            name = name.Substring(PrefixNamespace.Length);

        if (!dictDescriptor.TryGetValue(name, out var list))
        {
            return null;
        }

        var matches = new List<Descriptor>();
        foreach (var d in list)
        {
            if (d.@params.Count == @params.Count)
                matches.Add(d);
        }
        var count = @params.Count;
        foreach (var d in matches)
        {
            bool match = true;
            for (var i = 0; i < count; ++i)
            {
                var p0 = d.@params[i];
                var p1 = @params[i];
                if (string.CompareOrdinal(p0.name.ToLower(), p1) != 0)
                {
                    match = false;
                    break;
                }
            }
            if (match)
                return d;
        }
        return null;
    }

    static void ClearXML()
    {
        dictDescriptor.Clear();
        descriptors.Clear();
        lastDescriptor = null;
    }

    static Dictionary<string, List<Descriptor>> dictDescriptor = 
        new Dictionary<string, List<Descriptor>>();
    static List<Descriptor> descriptors = new List<Descriptor>();
    static Descriptor lastDescriptor = null;
    static bool lastSummary = false;
    static bool lastReturn = false;
}

