using System;
using System.IO;
using System.Text;
using System.Runtime.InteropServices;

namespace Hoedown
{
    unsafe public abstract class Buffer : IDisposable
    {
        public static readonly int DefaultUnitSize = 1024;

        [StructLayout(LayoutKind.Sequential)]
        internal struct buffer
        {
            public IntPtr data;
            public IntPtr size;
            public IntPtr asize;
            public IntPtr unit;

            public IntPtr realloc;
            public IntPtr free;
        }

        internal bool release;

        internal buffer* cbuffer
        {
            get
            {
                return (buffer*)NativeHandle;
            }
        }

        public IntPtr Data
        {
            get
            {
                return Marshal.ReadIntPtr(NativeHandle);
            }
        }

        public IntPtr Size
        {
            get
            {
                return cbuffer->size;
            }
            set
            {
                cbuffer->size = value;
            }
        }

        public IntPtr AllocatedSize
        {
            get
            {
                return cbuffer->asize;
            }
        }

        public IntPtr UnitSize
        {
            get
            {
                return cbuffer->unit;
            }
        }

        public Encoding Encoding { get; set; }

        public IntPtr NativeHandle { get; protected set; }

        protected Buffer(IntPtr size, bool alloc)
        {
            if (alloc)
            {
                Alloc(size);
            }
            else
            {
                NativeHandle = size;
            }
            Encoding = Encoding.Default;
            release = false;
        }

        ~Buffer()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (release)
            {
                Release();
                release = false;
            }
        }

        public static Buffer From(IntPtr ptr)
        {
            return new NativeBuffer(ptr, false);
        }

        public static Buffer Create()
        {
            return Create(DefaultUnitSize);
        }

        public static Buffer Create(int size)
        {
            return Create((IntPtr)size);
        }

        public static Buffer Create(long size)
        {
            return Create((IntPtr)size);
        }

        public static Buffer Create(IntPtr size)
        {
            return new NativeBuffer(size, true);
        }

        protected abstract void Alloc(IntPtr size);

        public byte[] GetBytes()
        {
            byte[] bytes = new byte[Size.ToInt64()];
            Marshal.Copy(Data, bytes, 0, bytes.Length);
            return bytes;
        }

        public BufferStream GetBufferStream()
        {
            return new BufferStream(this);
        }

        public Stream GetStream()
        {
            return new UnmanagedMemoryStream((byte*)Data, Size.ToInt64());
        }

        #region Put

        public void Put(IntPtr data, int size)
        {
            Put(data, (IntPtr)size);
        }

        public void Put(IntPtr data, long size)
        {
            Put(data, (IntPtr)size);
        }

        public void Put(IntPtr data, IntPtr size)
        {
            hoedown_buffer_put(NativeHandle, data, size);
        }

        public void Put(byte[] bytes, int size)
        {
            Put(bytes, (IntPtr)size);
        }

        public void Put(byte[] bytes, long size)
        {
            Put(bytes, (IntPtr)size);
        }

        public void Put(byte[] bytes, IntPtr size)
        {
            var gchandle = GCHandle.Alloc(bytes, GCHandleType.Pinned);
            Put(gchandle.AddrOfPinnedObject(), size);
            gchandle.Free();
        }

        public void Put(byte[] bytes, int offset, int count)
        {
            Put(bytes, (IntPtr)offset, (IntPtr)count);
        }

        public void Put(byte[] bytes, long offset, long count)
        {
            Put(bytes, (IntPtr)offset, (IntPtr)count);
        }

        public void Put(byte[] bytes, IntPtr offset, IntPtr count)
        {
            var gchandle = GCHandle.Alloc(bytes, GCHandleType.Pinned);
            Put(new IntPtr(gchandle.AddrOfPinnedObject().ToInt64() + offset.ToInt64()), count);
            gchandle.Free();
        }

        public void Put(byte[] bytes)
        {
            Put(bytes, bytes.LongLength);
        }

        public void Put(Encoding encoding, string str)
        {
            Put(encoding.GetBytes(str));
        }

        public void Put(string str)
        {
            Put(Encoding, str);
        }

        public void Put(Encoding encoding, string str, params object[] param)
        {
            Put(encoding, string.Format(str, param));
        }

        public void Put(string str, params object[] param)
        {
            Put(Encoding, str, param);
        }

        public void Put(Buffer buffer)
        {
            Put(buffer.Data, buffer.Size);
        }

        public void Put(byte c)
        {
            hoedown_buffer_putc(NativeHandle, c);
        }

        [DllImport("hoedown", CallingConvention = CallingConvention.Cdecl)]
        private static extern void hoedown_buffer_put(IntPtr buf, IntPtr buffer, IntPtr size);

        [DllImport("hoedown", CallingConvention = CallingConvention.Cdecl)]
        private static extern void hoedown_buffer_puts(IntPtr buf, IntPtr size);

        [DllImport("hoedown", CallingConvention = CallingConvention.Cdecl)]
        private static extern void hoedown_buffer_putc(IntPtr buf, byte c);

        #endregion

        #region Grow
        public void Grow(int size)
        {
            Grow(new IntPtr(size));
        }

        public void Grow(long size)
        {
            Grow(new IntPtr(size));
        }

        public void Grow(IntPtr size)
        {
            hoedown_buffer_grow(NativeHandle, size);
        }

        [DllImport("hoedown", CallingConvention = CallingConvention.Cdecl)]
        private static extern int hoedown_buffer_grow(IntPtr buf, IntPtr size);
        #endregion

        #region Reset
        public void Reset()
        {
            hoedown_buffer_reset(NativeHandle);
        }

        [DllImport("hoedown", CallingConvention = CallingConvention.Cdecl)]
        private static extern void hoedown_buffer_reset(IntPtr buf);
        #endregion

        #region Release
        void Release()
        {
            if (NativeHandle != IntPtr.Zero)
            {
                hoedown_buffer_free(NativeHandle);
                NativeHandle = IntPtr.Zero;
            }
        }

        [DllImport("hoedown", CallingConvention = CallingConvention.Cdecl)]
        private static extern void hoedown_buffer_free(IntPtr buf);
        #endregion

        #region Slurp
        public void Slurp(IntPtr size)
        {
            hoedown_buffer_slurp(NativeHandle, size);
        }

        public void Slurp(int size)
        {
            Slurp(new IntPtr(size));
        }

        public void Slurp(long size)
        {
            Slurp(new IntPtr(size));
        }

        [DllImport("hoedown", CallingConvention = CallingConvention.Cdecl)]
        private static extern void hoedown_buffer_slurp(IntPtr buf, IntPtr size);
        #endregion
    }
}

