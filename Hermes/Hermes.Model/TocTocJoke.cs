using System;

namespace Hermes.Model
{
    // TODO: Doc.
    public sealed class TocTocJoke : ICloneable
    {
        public const string EntryText = "Toc, Toc";
        public const string ForcedInteraction = "Quem é?";

        public string UserId { get; set; }

        public string[] ServerInteractions { get; set; }
        public int ServerInteractionIndex { get; set; }

        public string[] ClientInteractions { get; set; }
        public int ClientInteractionIndex { get; set; }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
