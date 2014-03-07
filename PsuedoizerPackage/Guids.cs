// Guids.cs
// MUST match guids.h
using System;

namespace JamesJohnson.PsuedoizerPackage
{
    static class GuidList
    {
        public const string guidPsuedoizerPackagePkgString = "345dbd33-65bb-40ea-a7c9-eafaf8270b7f";
        public const string guidPsuedoizerPackageCmdSetString = "81675c17-07d9-47b5-989d-bccac78b6f16";

        public static readonly Guid guidPsuedoizerPackageCmdSet = new Guid(guidPsuedoizerPackageCmdSetString);
    };
}