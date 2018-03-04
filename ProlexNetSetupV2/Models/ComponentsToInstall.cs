using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProlexNetSetupV2.Models
{
    public class ComponentsToInstall
    {
        public IIS IIS;

        public Firebird Firebird;

        public ProlexNetServer ProlexNetServer;

        public ProlexNetClient ProlexNetClient;

        public ProlexNetDatabase ProlexNetDatabase;

        public DotNet DotNet;

        public VisualC VisualC;

        public LinqPad LinqPad;

        public IBExpert IBExpert;
    }

    public class IIS
    {
        public string Name = "IIS - Serviços de Informações da Internet";
        public bool Install { get; set; }
    }

    public class Firebird
    {
        public FbX86 X86;
        public FbX64 X64;
    }
    public class FbX86
    {
        public string Name = "Firebird x86";
        public bool Install;
    }

    public class FbX64
    {
        public string Name = "Firebird x64";
        public bool Install;
    }

    public class ProlexNetServer
    {
        public string Name = "ProlexNet Server";
        public bool Install { get; set; }
    }

    public class ProlexNetClient
    {
        public string Name = "ProlexNet Client";
        public bool Install { get; set; }
    }

    public class ProlexNetDatabase
    {
        public string Name = "ProlexNet Database";
        public bool Install { get; set; }
    }

    public class DotNet
    {
        public string Name = "Microsoft .NET Framework 4.6";
        public bool Install { get; set; }
    }

    public class VisualC
    {
        public VcX86 X86;
        public VcX64 X64;
    }

    public class VcX64
    {
        public string Name = "Microsoft Visual C++ 2013 x64";
        public bool Install { get; set; }
    }

    public class VcX86
    {
        public string Name = "Microsoft Visual C++ 2013 x86";
        public bool Install { get; set; }
    }

    public class IBExpert
    {
        public string Name = "IBExpert";
        public bool Install { get; set; }
    }

    public class LinqPad
    {
        public string Name = "LinqPad 5";
        public bool Install { get; set; }
    }
}