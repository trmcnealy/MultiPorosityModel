using System;

namespace MultiPorosity.Services
{
    public struct RowData
    {
        public long  API;
        public long  ProdMonth;
        public float BOE;

        public RowData(long  api,
                       long  prodMonth,
                       float boe)
        {
            API       = api;
            ProdMonth = prodMonth;
            BOE       = boe;
        }

        private const char delimiter = ',';

        public RowData(ReadOnlySpan<char> data)
        {
            int first  = data.IndexOf(delimiter);
            int second = data.LastIndexOf(delimiter);

            API = long.Parse(data.Slice(0,
                                        first));

            ProdMonth = int.Parse(data.Slice(first          + 1,
                                             second - first - 1));

            if(data[data.Length - 2] == '\r' && data[data.Length - 1] == '\n')
            {
                BOE = float.Parse(data.Slice(second               + 1,
                                             data.Length - second - 2));
            }
            else if(data[data.Length - 2] != '\r' && data[data.Length - 1] == '\n')
            {
                BOE = float.Parse(data.Slice(second               + 1,
                                             data.Length - second - 1));
            }
            else
            {
                BOE = 0.0f;
            }
        }
    }
}