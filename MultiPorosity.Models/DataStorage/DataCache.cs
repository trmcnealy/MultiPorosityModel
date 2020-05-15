using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Text;

using Microsoft.DotNet.Interactive.Formatting;

namespace NumericalMethods.DataStorage
{
    [NonVersionable]
    [StructLayout(LayoutKind.Sequential)]
    public readonly struct DataCache
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        static DataCache()
        {
            Formatter<DataCache>.Register((dataCache,
                                           writer) =>
                                          {
                                              writer.Write("<table style=\"width:100%\">");

                                              for(ulong j = 0; j < dataCache.ColumnCount; j++)
                                              {
                                                  writer.Write($"<th>{dataCache.GetHeader((int)j)}</th>");
                                              }

                                              for(ulong i = 0; i < dataCache.RowCount; i++)
                                              {
                                                  writer.Write("<tr>");

                                                  for(ulong j = 0; j < dataCache.ColumnCount; j++)
                                                  {
                                                      writer.Write($"<td>{dataCache[i, j]}</td>");
                                                  }

                                                  writer.Write("</tr>");
                                              }

                                              writer.Write("</table>");
                                          },
                                          HtmlFormatter.MimeType);
        }

        public readonly ulong RowCount;
        public readonly ulong ColumnCount;

        public readonly Array<IntPtr> Headers;
        public readonly Array<double> Data;

        public double this[int i0]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
            get { return Data[i0]; }
        }

        public double this[ulong row_index,
                           ulong column_index]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
            get { return Data[column_index + ColumnCount * row_index]; }
        }

        public string GetHeader(int i0)
        {
            return Marshal.PtrToStringAnsi(Headers[i0]);
        }

        public void ExportToCsv(string file_path)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(GetHeader(0));

            for (int j = 1; j < (int)ColumnCount; ++j)
            {
                sb.Append(",");
                sb.Append(GetHeader(j));
            }

            sb.Append("\n");

            for (ulong i = 0; i < RowCount; ++i)
            {

                sb.Append(this[i, 0]);

                for (ulong j = 1; j < ColumnCount; ++j)
                {
                    sb.Append(",");
                    sb.Append(this[i, j]);
                }

                sb.Append("\n");
            }

            File.WriteAllText(file_path, sb.ToString());
        }
    }
}
