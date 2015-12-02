using System;
using System.Runtime.InteropServices;

namespace PimpMyBeatWrapper {
    public class PMBCSharpLibrary {

        // From c++ Dll (unmanaged)
        [DllImport ( "PMBCPPLibrary" )]
        public static extern float DllFunc ( );


        public static string WrapperFunc ( ) {
            return "WrapperFunction";
        }

    }
}
