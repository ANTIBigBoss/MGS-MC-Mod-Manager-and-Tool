using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ANTIBigBoss_MGS_Mod_Manager
{
    internal class MGSModel
    {
        public string LoLoDId { get; set; }
        public string HiLoDId { get; set; }
        public string Name { get; set; }
        public string ResidentStage { get; set; }
        public string Headpiece { get; set; } //All headpiece stuff to be used later when I get around to it
        public bool HasEvmHeadpiece = false;
        public bool HasKmsHeadpiece = false;
        public string HeadpieceId { get; set; }
        public string HeadpieceTri { get; set; }
        public string CodecEvm { get; set; }
        public string CodecTri { get; set; }
        public string CodecId { get; set; }
        public bool UseCutsceneAsCodec { get; set; } = false;
        public string CutsceneEvm { get; set; }
        public string CutsceneTri { get; set; }
        public string CutsceneId { get; set; }
        public string ArmsEvm { get; set; }
        public string ArmsTri { get; set; }
        public string ArmsId { get; set; }
        public string HandsEvm { get; set; }
        public string HandsTri { get; set; }
        public string HandsId { get; set; }
        public string ShadowKms { get; set; }
        public string ShadowId { get; set; }
        public string ShadowTri { get; set; }
        public List<string> HandTextures { get; set; } = new();
        public List<string> ArmTextures { get; set; } = new();
        public List<string> ShadowTextures { get; set; } = new();
        public List<string> CodecTextures { get; set; } = new List<string>();
        public List<string> CutsceneTextures { get; set; } = new List<string>();
        public List<string> LoLoDTextures { get; set; } = new List<string>();
        public List<string> HiLoDTextures { get; set; } = new List<string>();
        public List<string> HeadpieceTextures { get; set; } = new List<string>();
        public string LoLodKms { get; set; }
        public string LoLoDTri { get; set; }
        public string HiLodKms { get; set; }
        public string HiLoDTri { get; set; }

        //TODO: Maybe change from LoLoDKms & HiLodKms to just a list of models that need to be replaced? And replace them all with the same model?
        //      i mean, why bother even having a low and high lod with today's hardware?

        public override string ToString()
        {
            return Name;
        }
    }
}
