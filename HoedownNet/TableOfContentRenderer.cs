using System;
using System.Runtime.InteropServices;

namespace Hoedown
{
    public class TableOfContentRenderer : Renderer
    {
        internal html_renderopt options = new html_renderopt();
        internal GCHandle optionsgchandle;
        internal uint flags;

        public TableOfContentRenderer()
        {
        }

        ~TableOfContentRenderer()
        {
            if (optionsgchandle.IsAllocated)
            {
                optionsgchandle.Free();
            }
        }

        [DllImport("hoedown", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void hoedown_html_toc_renderer_new(ref md_callbacks callbacks, IntPtr options, uint render_flags);

        protected override void Initialize()
        {
            optionsgchandle = GCHandle.Alloc(options, GCHandleType.Pinned);
            opaque = optionsgchandle.AddrOfPinnedObject();
            hoedown_html_toc_renderer_new(ref callbacks, opaque, 0);
            base.Initialize();
        }
    }
}

