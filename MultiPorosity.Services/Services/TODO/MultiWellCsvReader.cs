using System;
using System.Collections.Generic;
using System.IO;

namespace MultiPorosity.Services
{
    public class MultiWellCsvReader : IDisposable
    {
        private FileStream     _inputStream;
        private BufferedStream _inputBuffered;
        private StreamReader   _inputReader;

        public MultiWellCsvReader(string filename)
        {
            if(!File.Exists(filename))
            {
                throw new FileNotFoundException();
            }

            _inputStream = File.Open(filename,
                                     FileMode.Open,
                                     FileAccess.Read,
                                     FileShare.Read);

            _inputBuffered = new BufferedStream(_inputStream);
            _inputReader   = new StreamReader(_inputBuffered);
        }

        public MultiWellCsvReader(FileStream fileStream)
        {
            _inputStream   = fileStream ?? throw new ArgumentNullException(nameof(fileStream));
            _inputBuffered = new BufferedStream(_inputStream);
            _inputReader   = new StreamReader(_inputBuffered);
        }

        public void Dispose()
        {
            //GC.SuppressFinalize(true);

            if(_inputStream != null)
            {
                _inputStream.Close();
                _inputStream = null;
            }

            if(_inputBuffered != null)
            {
                _inputBuffered.Close();
                _inputBuffered = null;
            }

            if(_inputReader != null)
            {
                _inputReader.Close();
                _inputReader = null;
            }
        }

        ~MultiWellCsvReader()
        {
            Dispose();
        }

        //public T ReadContractFile<T>()
        //{
        //    DataContractSerializer dataContractSerializer = new DataContractSerializer(typeof(T));
        //
        //    T output;
        //
        //    using(_inputStream)
        //    {
        //        using(_inputBuffered)
        //        {
        //            using(_inputReader)
        //            {
        //                output = (T)dataContractSerializer.ReadObject(_inputReader);
        //            }
        //        }
        //    }
        //
        //    return output;
        //}

        public List<RowData> ReadFile(int number_of_header_lines)
        {
            List<RowData> row_datas;

            using(_inputStream)
            {
                using(_inputBuffered)
                {
                    using(_inputReader)
                    {
                        ReadOnlySpan<char> data = _inputReader.ReadToEnd();

                        int Length = data.Length;

                        List<int> line_endings = new List<int>(Length / 30);

                        int header_lines = 0;

                        line_endings.Add(0);

                        for(int i = 0; i < Length; ++i)
                        {
                            if(header_lines < number_of_header_lines)
                            {
                                if(data[i] == '\n')
                                {
                                    line_endings[0] = i + 1;
                                    ++header_lines;
                                }

                                continue;
                            }

                            if(data[i] == '\r' && data[i + 1] == '\n')
                            {
                                line_endings.Add(++i + 1);
                                ++i;
                            }
                            else if(data[i - 1] != '\r' && data[i] == '\n')
                            {
                                line_endings.Add(++i);
                            }
                        }

                        row_datas = new List<RowData>(line_endings.Count);

                        int start;
                        int end;

                        for(int i = 0; i < line_endings.Count - 1; ++i)
                        {
                            start = line_endings[i];
                            end   = line_endings[i + 1];

                            row_datas.Add(new RowData(data.Slice(start,
                                                                 end - start)));
                        }
                    }
                }
            }

            return row_datas;
        }
    }
}