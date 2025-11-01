using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace ANTIBigBoss_MGS_Mod_Manager
{
    public static class ResourceFileEditorSupport
    {
        internal static string BaseDirectory { get; set; }
        public interface IResource
        {
            public string Name { get; set; }
            public string ID { get; set; }
            public string Path { get; set; }
        }

        public class Ctxr : IResource
        {
            public string Name { get ; set ; }
            public string ID { get ; set ; }
            public string Path { get ; set ; }
            public string IDMappedTo { get; set; }

            public Ctxr(string resourcePath) 
            {
                Path = resourcePath;
                List<string> resourceDeclarationParts = Path.Split(',').ToList();
                ID = resourceDeclarationParts[2].Substring(resourceDeclarationParts[2].LastIndexOf("/") + 1).Replace(".ctxr","");
                if (resourceDeclarationParts[2].Contains("/cache/"))
                    IDMappedTo = resourceDeclarationParts[2].Substring(resourceDeclarationParts[2].IndexOf("/cache/") + 7, 8);
                else if (resourceDeclarationParts[2].Contains("/resident/"))
                    IDMappedTo = resourceDeclarationParts[2].Substring(resourceDeclarationParts[2].IndexOf("/resident/") + 10, 8);
                else
                    throw new NotImplementedException();

                Name = resourceDeclarationParts[0].Substring(resourceDeclarationParts[0].LastIndexOf("/") + 1) + $"-- ID: {ID} --  MappedTo: {IDMappedTo}";
            }

            [JsonConstructor]
            public Ctxr(string name, string id, string path, string idMappedTo)
            {
                Name = name;
                ID = id;
                Path = path;
                IDMappedTo = idMappedTo;
            }
        }

        public enum CmdlSubtype
        {
            Evm,
            Kms
        }

        public class Cmdl : IResource
        {
            public string Name { get ; set ; }
            public string ID { get ; set ; }
            public string Path { get ; set ; }
            public CmdlSubtype Subtype { get; set; }

            public Cmdl(string resourcePath)
            {
                Path = resourcePath;
                List<string> resourceDeclarationParts = Path.Split(',').ToList();
                Subtype = Path.Contains("/evm/") ? CmdlSubtype.Evm : CmdlSubtype.Kms;
                Name = resourceDeclarationParts[0].Substring(resourceDeclarationParts[0].LastIndexOf("/") + 1);
                ID = resourceDeclarationParts[1].Substring(resourceDeclarationParts[1].LastIndexOf("/") + 1).Replace(".cmdl", "");
            }

            [JsonConstructor]
            public Cmdl(string name, string id, string path, CmdlSubtype subtype)
            {
                Name = name;
                ID = id;
                Path = path;
                Subtype = subtype;
            }
        }

        public class BpAssetsFile
        {
            public List<Ctxr> CtxrResources { get; set; }
            public List<Cmdl> CmdlResources { get; set; }
            public string Path { get; set; }
            public BpAssetsFile(string path)
            {
                Path = path;
                CtxrResources = new List<Ctxr>();
                CmdlResources = new List<Cmdl>();
                string[] fileContents = File.ReadAllLines(Path).Distinct().Where(x=>x!="").ToArray();
                foreach(string resource in fileContents)
                {
                    if (resource.Contains(".ctxr"))
                    {
                        CtxrResources.Add(new Ctxr(resource));
                    }
                    else
                    {
                        CmdlResources.Add(new Cmdl(resource));
                    }
                }
            }

            public void WriteToFile()
            {
                string contents = "";
                foreach(Ctxr ctxr in CtxrResources)
                {
                    contents += ctxr.Path + "\r\r\n";
                }
                foreach(Cmdl cmdl in CmdlResources)
                {
                    contents += cmdl.Path + "\r\r\n";
                }

                File.WriteAllText(Path, contents);
            }
        }

        public class Tri : IResource
        {
            public string Name { get ; set ; }
            public string ID { get ; set ; }
            public string Path { get ; set ; }
            public Tri(string resourcePath)
            {
                Path = resourcePath;
                List<string> resourceDeclarationParts = Path.Split(',').ToList();
                Name = resourceDeclarationParts[0].Substring(resourceDeclarationParts[0].LastIndexOf("/") + 1);
                ID = resourceDeclarationParts[1].Substring(resourceDeclarationParts[1].LastIndexOf("/") + 1).Replace(".tri", "");
            }

            [JsonConstructor]
            public Tri(string name, string id, string path)
            {
                Name = name;
                ID = id;
                Path = path;
            }
        }

        public class Hzx : IResource
        {
            public string Name { get ; set ; }
            public string ID { get ; set ; }
            public string Path { get ; set ; }
            public Hzx(string resourcePath)
            {
                Path = resourcePath;
                List<string> resourceDeclarationParts = Path.Split(',').ToList();
                Name = resourceDeclarationParts[0].Substring(resourceDeclarationParts[0].LastIndexOf("/") + 1);
                ID = resourceDeclarationParts[1].Substring(resourceDeclarationParts[1].LastIndexOf("/") + 1).Replace(".hzx", "");
            }

            [JsonConstructor]
            public Hzx(string name, string id, string path)
            {
                Name = name;
                ID = id;
                Path = path;
            }
        }

        public class Var : IResource
        {
            public string Name { get ; set ; }
            public string ID { get ; set ; }
            public string Path { get ; set ; }
            public Var(string resourcePath)
            {
                Path = resourcePath;
                List<string> resourceDeclarationParts = Path.Split(',').ToList();
                Name = resourceDeclarationParts[0].Substring(resourceDeclarationParts[0].LastIndexOf("/") + 1);
                ID = resourceDeclarationParts[1].Substring(resourceDeclarationParts[1].LastIndexOf("/") + 1).Replace(".var", "");
            }

            [JsonConstructor]
            public Var(string name, string id, string path)
            {
                Name = name;
                ID = id;
                Path = path;
            }
        }

        public class Sar : IResource
        {
            public string Name { get ; set ; }
            public string ID { get ; set ; }
            public string Path { get ; set ; }
            public Sar(string resourcePath)
            {
                Path = resourcePath;
                List<string> resourceDeclarationParts = Path.Split(',').ToList();
                Name = resourceDeclarationParts[0].Substring(resourceDeclarationParts[0].LastIndexOf("/") + 1);
                ID = resourceDeclarationParts[1].Substring(resourceDeclarationParts[1].LastIndexOf("/") + 1).Replace(".sar", "");
            }

            [JsonConstructor]
            public Sar(string name, string id, string path)
            {
                Name = name;
                ID = id;
                Path = path;
            }
        }

        public class Row : IResource
        {
            public string Name { get ; set ; }
            public string ID { get ; set ; }
            public string Path { get ; set ; }
            public Row(string resourcePath)
            {
                Path = resourcePath;
                List<string> resourceDeclarationParts = Path.Split(',').ToList();
                Name = resourceDeclarationParts[0].Substring(resourceDeclarationParts[0].LastIndexOf("/") + 1);
                ID = resourceDeclarationParts[1].Substring(resourceDeclarationParts[1].LastIndexOf("/") + 1).Replace(".row", "");
            }

            [JsonConstructor]
            public Row(string name, string id, string path)
            {
                Name = name;
                ID = id;
                Path = path;
            }
        }
        public class O2d : IResource
        {
            public string Name { get ; set ; }
            public string ID { get ; set ; }
            public string Path { get ; set ; }
            public O2d(string resourcePath)
            {
                Path = resourcePath;
                List<string> resourceDeclarationParts = Path.Split(',').ToList();
                Name = resourceDeclarationParts[0].Substring(resourceDeclarationParts[0].LastIndexOf("/") + 1);
                ID = resourceDeclarationParts[1].Substring(resourceDeclarationParts[1].LastIndexOf("/") + 1).Replace(".o2d", "");
            }

            [JsonConstructor]
            public O2d(string name, string id, string path)
            {
                Name = name;
                ID = id;
                Path = path;
            }
        }
        public class Mar : IResource
        {
            public string Name { get ; set ; }
            public string ID { get ; set ; }
            public string Path { get ; set ; }
            public Mar(string resourcePath)
            {
                Path = resourcePath;
                List<string> resourceDeclarationParts = Path.Split(',').ToList();
                Name = resourceDeclarationParts[0].Substring(resourceDeclarationParts[0].LastIndexOf("/") + 1);
                ID = resourceDeclarationParts[1].Substring(resourceDeclarationParts[1].LastIndexOf("/") + 1).Replace(".mar", "");
            }

            [JsonConstructor]
            public Mar(string name, string id, string path)
            {
                Name = name;
                ID = id;
                Path = path;
            }
        }
        public class Evm : IResource
        {
            public string Name { get; set; }
            public string ID { get; set; }
            public string Path { get; set; }
            public Evm(string resourcePath)
            {
                Path = resourcePath;
                List<string> resourceDeclarationParts = Path.Split(',').ToList();
                Name = resourceDeclarationParts[0].Substring(resourceDeclarationParts[0].LastIndexOf("/") + 1);
                ID = resourceDeclarationParts[1].Substring(resourceDeclarationParts[1].LastIndexOf("/") + 1).Replace(".evm", "");
            }

            [JsonConstructor]
            public Evm(string name, string id, string path)
            {
                Name = name;
                ID = id;
                Path = path;
            }
        }

        public class Kms : IResource
        {
            public string Name { get; set; }
            public string ID { get; set; }
            public string Path { get; set; }
            public Kms(string resourcePath)
            {
                Path = resourcePath;
                List<string> resourceDeclarationParts = Path.Split(',').ToList();
                Name = resourceDeclarationParts[0].Substring(resourceDeclarationParts[0].LastIndexOf("/") + 1);
                ID = resourceDeclarationParts[1].Substring(resourceDeclarationParts[1].LastIndexOf("/") + 1).Replace(".kms", "");
            }

            [JsonConstructor]
            public Kms(string name, string id, string path)
            {
                Name = name;
                ID = id;
                Path = path;
            }
        }

        public class Lt2 : IResource
        {
            public string Name { get ; set ; }
            public string ID { get ; set ; }
            public string Path { get ; set ; }
            public Lt2(string resourcePath)
            {
                Path = resourcePath;
                List<string> resourceDeclarationParts = Path.Split(',').ToList();
                Name = resourceDeclarationParts[0].Substring(resourceDeclarationParts[0].LastIndexOf("/") + 1);
                ID = resourceDeclarationParts[1].Substring(resourceDeclarationParts[1].LastIndexOf("/") + 1).Replace(".lt2", "");
            }

            [JsonConstructor]
            public Lt2(string name, string id, string path)
            {
                Name = name;
                ID = id;
                Path = path;
            }
        }
        public class Far : IResource
        {
            public string Name { get ; set ; }
            public string ID { get ; set ; }
            public string Path { get ; set ; }
            public Far(string resourcePath)
            {
                Path = resourcePath;
                List<string> resourceDeclarationParts = Path.Split(',').ToList();
                Name = resourceDeclarationParts[0].Substring(resourceDeclarationParts[0].LastIndexOf("/") + 1);
                ID = resourceDeclarationParts[1].Substring(resourceDeclarationParts[1].LastIndexOf("/") + 1).Replace(".far", "");
            }

            [JsonConstructor]
            public Far(string name, string id, string path)
            {
                Name = name;
                ID = id;
                Path = path;
            }
        }
        public class Cv2 : IResource
        {
            public string Name { get ; set ; }
            public string ID { get ; set ; }
            public string Path { get ; set ; }
            public Cv2(string resourcePath)
            {
                Path = resourcePath;
                List<string> resourceDeclarationParts = Path.Split(',').ToList();
                Name = resourceDeclarationParts[0].Substring(resourceDeclarationParts[0].LastIndexOf("/") + 1);
                ID = resourceDeclarationParts[1].Substring(resourceDeclarationParts[1].LastIndexOf("/") + 1).Replace(".cv2", "");
            }

            [JsonConstructor]
            public Cv2(string name, string id, string path)
            {
                Name = name;
                ID = id;
                Path = path;
            }
        }
        public class Anm : IResource
        {
            public string Name { get ; set ; }
            public string ID { get ; set ; }
            public string Path { get ; set ; }
            public Anm(string resourcePath)
            {
                Path = resourcePath;
                List<string> resourceDeclarationParts = Path.Split(',').ToList();
                Name = resourceDeclarationParts[0].Substring(resourceDeclarationParts[0].LastIndexOf("/") + 1);
                ID = resourceDeclarationParts[1].Substring(resourceDeclarationParts[1].LastIndexOf("/") + 1).Replace(".anm", "");
            }

            [JsonConstructor]
            public Anm(string name, string id, string path)
            {
                Name = name;
                ID = id;
                Path = path;
            }
        }
        public class Gcx : IResource
        {
            public string Name { get;  set; }
            public string ID { get ; set ; }
            public string Path { get ; set ; }
            public Gcx(string resourcePath)
            {
                Path = resourcePath;
                List<string> resourceDeclarationParts = Path.Split(',').ToList();
                Name = resourceDeclarationParts[0].Substring(resourceDeclarationParts[0].LastIndexOf("/") + 1);
                ID = resourceDeclarationParts[1].Substring(resourceDeclarationParts[1].LastIndexOf("/") + 1).Replace(".gcx", "");
            }

            [JsonConstructor]
            public Gcx(string name, string id, string path)
            {
                Name = name;
                ID = id;
                Path = path;
            }
        }

        public class ManifestFile
        {
            public List<Tri> TriResources { get; set; }
            public List<Hzx> HzxResources { get; set; } 
            public List<Var> VarResources { get; set; } 
            public List<Sar> SarResources { get; set; } 
            public List<Row> RowResources { get; set; } 
            public List<O2d> O2dResources { get; set; } 
            public List<Mar> MarResources { get; set; } 
            public List<Lt2> Lt2Resources { get; set; } 
            public List<Kms> KmsResources { get; set; } 
            public List<Far> FarResources { get; set; } 
            public List<Evm> EvmResources { get; set; } 
            public List<Cv2> Cv2Resources { get; set; } 
            public List<Anm> AnmResources { get; set; } 
            public List<Gcx> GcxResources { get; set; } 
            public string Path { get; set; }

            public ManifestFile(string path)
            {
                Path = path;
                TriResources = new List<Tri>();
                HzxResources = new List<Hzx>();
                VarResources = new List<Var>();
                SarResources = new List<Sar>();
                RowResources = new List<Row>();
                O2dResources = new List<O2d>();
                MarResources = new List<Mar>();
                Lt2Resources = new List<Lt2>();
                KmsResources = new List<Kms>();
                FarResources = new List<Far>();
                EvmResources = new List<Evm>();
                Cv2Resources = new List<Cv2>();
                AnmResources = new List<Anm>();
                GcxResources = new List<Gcx>();
                
                string[] fileContents = File.ReadAllLines(Path).Distinct().Where(x => x != "").ToArray();
                foreach (string resource in fileContents)
                {
                    if (resource.Contains(".tri"))
                    {
                        TriResources.Add(new Tri(resource));
                    }
                    else if (resource.Contains(".hzx"))
                    {
                        HzxResources.Add(new Hzx(resource));
                    }
                    else if (resource.Contains(".var"))
                    {
                        VarResources.Add(new Var(resource));
                    }
                    else if (resource.Contains(".sar"))
                    {
                        SarResources.Add(new Sar(resource));
                    }
                    else if (resource.Contains(".row"))
                    {
                        RowResources.Add(new Row(resource));
                    }
                    else if (resource.Contains(".o2d"))
                    {
                        O2dResources.Add(new O2d(resource));
                    }
                    else if (resource.Contains(".mar"))
                    {
                        MarResources.Add(new Mar(resource));
                    }
                    else if (resource.Contains(".lt2"))
                    {
                        Lt2Resources.Add(new Lt2(resource));
                    }
                    else if (resource.Contains(".kms"))
                    {
                        KmsResources.Add(new Kms(resource));
                    }
                    else if (resource.Contains(".far"))
                    {
                        FarResources.Add(new Far(resource));
                    }
                    else if (resource.Contains(".evm"))
                    {
                        EvmResources.Add(new Evm(resource));
                    }
                    else if (resource.Contains(".cv2"))
                    {
                        Cv2Resources.Add(new Cv2(resource));
                    }
                    else if (resource.Contains(".anm"))
                    {
                        AnmResources.Add(new Anm(resource));
                    }
                    else if (resource.Contains(".gcx"))
                    {
                        GcxResources.Add(new Gcx(resource));
                    }
                }
            }

            public void WriteToFile()
            {
                string contents = "";
                foreach (var resource in TriResources)
                {
                    contents += resource.Path + "\r\r\n";
                }
                foreach (var resource in HzxResources)
                {
                    contents += resource.Path + "\r\r\n";
                }
                foreach (var resource in VarResources)
                {
                    contents += resource.Path + "\r\r\n";
                }
                foreach (var resource in SarResources)
                {
                    contents += resource.Path + "\r\r\n";
                }
                foreach (var resource in RowResources)
                {
                    contents += resource.Path + "\r\r\n";
                }
                foreach (var resource in O2dResources)
                {
                    contents += resource.Path + "\r\r\n";
                }
                foreach (var resource in MarResources)
                {
                    contents += resource.Path + "\r\r\n";
                }
                foreach (var resource in Lt2Resources)
                {
                    contents += resource.Path + "\r\r\n";
                }
                foreach (var resource in KmsResources)
                {
                    contents += resource.Path + "\r\r\n";
                }
                foreach (var resource in FarResources)
                {
                    contents += resource.Path + "\r\r\n";
                }
                foreach (var resource in EvmResources)
                {
                    contents += resource.Path + "\r\r\n";
                }
                foreach (var resource in Cv2Resources)
                {
                    contents += resource.Path + "\r\r\n";
                }
                foreach (var resource in AnmResources)
                {
                    contents += resource.Path + "\r\r\n";
                }
                foreach (var resource in GcxResources)
                {
                    contents += resource.Path + "\r\r\n";
                }

                File.WriteAllText(Path, contents);
            }
        }

        public class Stage
        {
            BpAssetsFile BpAssetsFile { get; set; }
            ManifestFile ManifestFile { get; set; }
            public List<IResource> ResourceList { get; set; }
            public string Name { get; set; }

            public Stage(string stage)
            {
                Name = stage;
                BpAssetsFile = new BpAssetsFile($"{BaseDirectory}/eu/stage/{Name}/bp_assets.txt");
                ManifestFile = new ManifestFile($"{BaseDirectory}/eu/stage/{Name}/manifest.txt");
                ResourceList =
                [
                    .. BpAssetsFile.CtxrResources,
                    .. BpAssetsFile.CmdlResources,
                    .. ManifestFile.TriResources,
                    .. ManifestFile.HzxResources,
                    .. ManifestFile.VarResources,
                    .. ManifestFile.SarResources,
                    .. ManifestFile.RowResources,
                    .. ManifestFile.O2dResources,
                    .. ManifestFile.MarResources,
                    .. ManifestFile.Lt2Resources,
                    .. ManifestFile.KmsResources,
                    .. ManifestFile.FarResources,
                    .. ManifestFile.EvmResources,
                    .. ManifestFile.Cv2Resources,
                    .. ManifestFile.AnmResources,
                    .. ManifestFile.GcxResources,
                ];
            }
        }
    }
}