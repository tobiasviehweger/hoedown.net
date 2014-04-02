using System;

namespace Hoedown
{
    enum mkd_extensions : uint
    {
        /* block-level extensions */
        HOEDOWN_EXT_TABLES = (1 << 0),
        HOEDOWN_EXT_FENCED_CODE = (1 << 1),
        HOEDOWN_EXT_FOOTNOTES = (1 << 2),

        /* span-level extensions */
        HOEDOWN_EXT_AUTOLINK = (1 << 3),
        HOEDOWN_EXT_STRIKETHROUGH = (1 << 4),
        HOEDOWN_EXT_UNDERLINE = (1 << 5),
        HOEDOWN_EXT_HIGHLIGHT = (1 << 6),
        HOEDOWN_EXT_QUOTE = (1 << 7),
        HOEDOWN_EXT_SUPERSCRIPT = (1 << 8),

        /* other flags */
        HOEDOWN_EXT_LAX_SPACING = (1 << 9),
        HOEDOWN_EXT_NO_INTRA_EMPHASIS = (1 << 10),
        HOEDOWN_EXT_SPACE_HEADERS = (1 << 11),

        /* negative flags */
        HOEDOWN_EXT_DISABLE_INDENTED_CODE = (1 << 12)
    }

    public class MarkdownExtensions
    {
        public bool NoIntraEmphasis { get; set; }
        public bool Tables { get; set; }
        public bool FencedCode { get; set; }
        public bool Footnotes { get; set; }
        public bool Autolink { get; set; }
        public bool Strikethrough { get; set; }
        public bool SpaceHeaders { get; set; }
        public bool SuperScript { get; set; }
        public bool LaxSpacing { get; set; }
        public bool Underline { get; set; }
        public bool Highlight { get; set; }
        public bool Quote { get; set; }
        public bool DisableIndentedCode { get; set; }

        internal uint ToUInt()
        {
            uint ret = 0;

            if (Tables) ret |= (uint)mkd_extensions.HOEDOWN_EXT_TABLES;
            if (FencedCode) ret |= (uint)mkd_extensions.HOEDOWN_EXT_FENCED_CODE;
            if (Footnotes) ret |= (uint)mkd_extensions.HOEDOWN_EXT_FOOTNOTES;
            if (Autolink) ret |= (uint)mkd_extensions.HOEDOWN_EXT_AUTOLINK;
            if (Strikethrough) ret |= (uint)mkd_extensions.HOEDOWN_EXT_STRIKETHROUGH;
            if (Underline) ret |= (uint)mkd_extensions.HOEDOWN_EXT_UNDERLINE;
            if (Highlight) ret |= (uint)mkd_extensions.HOEDOWN_EXT_HIGHLIGHT;
            if (Quote) ret |= (uint)mkd_extensions.HOEDOWN_EXT_QUOTE;
            if (SuperScript) ret |= (uint)mkd_extensions.HOEDOWN_EXT_SUPERSCRIPT;
            if (LaxSpacing) ret |= (uint)mkd_extensions.HOEDOWN_EXT_LAX_SPACING;
            if (NoIntraEmphasis) ret |= (uint)mkd_extensions.HOEDOWN_EXT_NO_INTRA_EMPHASIS;
            if (SpaceHeaders) ret |= (uint)mkd_extensions.HOEDOWN_EXT_SPACE_HEADERS;
            if (DisableIndentedCode) ret |= (uint)mkd_extensions.HOEDOWN_EXT_DISABLE_INDENTED_CODE;

            return ret;
        }
    }
}

