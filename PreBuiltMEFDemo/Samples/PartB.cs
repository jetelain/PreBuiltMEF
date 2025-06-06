﻿using System.ComponentModel.Composition;

namespace PreBuiltMEFDemo.Samples
{
    [Export(typeof(IExportB))]
    [PartMetadata("Metadata1", "MetadataB")]
    internal class PartB : IExportB
    {

        [ImportingConstructor]
        public PartB(IExportA a, [Import("NamedA1")] IExportA1 a1) {

        }

        [Import("NamedA")]
        public IExportA? A { get; set; }

        [Import]
        public IExportA1? A1 { get; set; }

        [Import]
        public Lazy<IExportA1>? A1Lazy { get; set; }

        [Import]
        public Lazy<IExportA,IMetadata1>? ALazy { get; set; }

        [Import("Missing", AllowDefault = true)]
        public IExportA? Missing { get; set; }

        [ImportMany(AllowRecomposition = true)]
        public IEnumerable<IExportA1>? A1Many { get; set; }

        [Import]
        public PartC C { get; set; }

        [Import]
        public PartD D { get; set; }
    }

}
