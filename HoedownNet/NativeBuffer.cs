#define marshalcopy

using System;
using System.IO;
using System.Text;
using System.Runtime.InteropServices;

namespace Hoedown
{
    public class NativeBuffer : Buffer
    {
        [DllImport("hoedown", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr hoedown_buffer_new(IntPtr size);

        public NativeBuffer()
            : this(DefaultUnitSize)
        {
        }

        public NativeBuffer(int size)
            : this((IntPtr)size)
        {
        }

        public NativeBuffer(long size)
            : this((IntPtr)size)
        {
        }

        public NativeBuffer(IntPtr size)
            : this((IntPtr)size, true)
        {
        }

        public NativeBuffer(IntPtr size, bool alloc)
            : base(size, alloc)
        {
        }

        protected override void Alloc(IntPtr size)
        {
            NativeHandle = hoedown_buffer_new(size);
        }

        public override string ToString()
        {
#if marshalcopy
            return Encoding.GetString(GetBytes());
#else
            return new StreamReader(GetStream()).ReadToEnd();
#endif
        }
    }
}

