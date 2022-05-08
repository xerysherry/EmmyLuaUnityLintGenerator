using System;
using System.Collections.Generic;

public static partial class LintGenerator
{
    /// <summary>
    /// 
    /// </summary>
    static string[] AssemblyList = new string[]
    {
        "UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null",
    };
    /// <summary>
    /// 
    /// </summary>
    static string[] AssemblyFile = new string[]
    {
        //@"E:\Unity2019.4.2f1\Editor\Data\Managed\UnityEngine.dll",
        //@"C:\UnityProjects\MyProject\Library\ScriptAssemblies\UnityEngine.UI.dll",
    };
    /// <summary>
    /// Desc Xml
    /// </summary>
    static string[] TypeXmlPath = new string[]
    { };
    /// <summary>
    /// Type List
    /// </summary>
    static Type[] TypeList = new Type[]
    { };
    /// <summary>
    /// 
    /// </summary>
    static List<Type> skipType = new List<Type>
    { };
    /// <summary>
    /// 
    /// </summary>
    static string Folder = "Unity";
    /// <summary>
    /// 
    /// </summary>
    static string PrefixNamespace = "";
    /// <summary>
    /// Generate All Lint In One File
    /// </summary>
    static bool AllInOne = false;
    /// <summary>
    /// 
    /// </summary>
    static string AllInOneFilename = "UnityLint.lua";
    /// <summary>
    /// 
    /// </summary>
    static List<string> LuaKeyword = new List<string>
    {
        "and",
        "break",
        "do",
        "else",
        "elseif",
        "end",
        "false",
        "for",
        "function",
        "goto",
        "if",
        "in",
        "local",
        "nil",
        "not",
        "or",
        "repeat",
        "return",
        "then",
        "true",
        "until",
        "while",
    };
}
