using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Text;

namespace NumericalMethods.DataStorage
{
    [SkipLocalsInit]
    [NonVersionable]
    [StructLayout(LayoutKind.Sequential)]
    public readonly unsafe struct Array<T>
        where T : unmanaged
    {
        //[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        //static Array()
        //{
        //    Formatter<Array<T>>.Register((array,
        //                                  writer) =>
        //                                 {
        //                                     writer.Write("<tr>");

        //                                     for(int i = 0; i < array.length; i++)
        //                                     {
        //                                         writer.Write($"<td>{array[i]}</td>");
        //                                     }

        //                                     writer.Write("</tr>");
        //                                 },
        //                                 HtmlFormatter.MimeType);
        //}

        public readonly ulong length;

        public readonly T* data;

        public ref T this[int i0]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
            get { return ref data[i0]; }
        }

        public ref T this[uint i0]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
            get { return ref data[i0]; }
        }

        public ref T this[long i0]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
            get { return ref data[i0]; }
        }

        public ref T this[ulong i0]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
            get { return ref data[i0]; }
        }

        //public string ToHtml()
        //{
        //    StringBuilder sb = new StringBuilder();

        //    sb.Append("<tr>");

        //    for(int i = 0; i < length; i++)
        //    {
        //        sb.Append($"<td>{this[i]}</td>");
        //    }

        //    sb.Append("</tr>");

        //    return sb.ToString();
        //}
    }
}
