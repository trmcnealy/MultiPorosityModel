
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using Apache.Arrow;
using Apache.Arrow.Ipc;
using Apache.Arrow.Memory;


namespace MultiPorosity.Models
{
    public class MultipleWellAnalysis
    {
        public MultipleWellAnalysis()
        {
            var memoryAllocator = new NativeMemoryAllocator(alignment: 64);

            // Build a record batch using the Fluent API

            var recordBatch = new RecordBatch.Builder(memoryAllocator)
                             .Append("Column A", false, col => col.Int32(array => array.AppendRange(Enumerable.Range(0, 10))))
                             .Append("Column B", false, col => col.Float(array => array.AppendRange(Enumerable.Range(0, 10).Select(x => Convert.ToSingle(x * 2)))))
                             .Append("Column C", false, col => col.String(array => array.AppendRange(Enumerable.Range(0, 10).Select(x => $"Item {x +1}"))))
                             .Append("Column D", false, col => col.Boolean(array => array.AppendRange(Enumerable.Range(0, 10).Select(x => x % 2 == 0))))
                             .Build();

            // Print memory allocation statistics

            Console.WriteLine("Allocations: {0}",       memoryAllocator.Statistics.Allocations);
            Console.WriteLine("Allocated: {0} byte(s)", memoryAllocator.Statistics.BytesAllocated);

            // Write record batch to a file

            using (var stream = File.OpenWrite("test.arrow"))
            using (var writer = new ArrowFileWriter(stream, recordBatch.Schema))
            {
                writer.WriteRecordBatchAsync(recordBatch);
                //writer.WriteFooterAsync();
            }
        }
    }
}