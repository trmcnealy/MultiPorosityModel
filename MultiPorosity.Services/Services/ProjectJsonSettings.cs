using System;
using System.Runtime.CompilerServices;
using System.Text.Encodings.Web;
using System.Text.Json;

namespace MultiPorosity.Services
{
    public static class ProjectJsonSettings
    {
        internal static readonly JsonEncodedText s_metadataId     = JsonEncodedText.Encode("$id",     encoder: DefaultJsonEncoder.Instance);
        internal static readonly JsonEncodedText s_metadataRef    = JsonEncodedText.Encode("$ref",    encoder: DefaultJsonEncoder.Instance);
        internal static readonly JsonEncodedText s_metadataValues = JsonEncodedText.Encode("$values", encoder: DefaultJsonEncoder.Instance);
        
        public static readonly JsonSerializerOptions Default = new JsonSerializerOptions
        {
            //Encoder                     = DefaultJsonEncoder.Instance,
            PropertyNameCaseInsensitive = false,
            WriteIndented               = true,
            IgnoreNullValues            = true,
            PropertyNamingPolicy        = JsonNamingPolicy.CamelCase
        };
    }

    internal sealed class DefaultJsonEncoder : JavaScriptEncoder
    {
        public static readonly DefaultJsonEncoder Instance = new DefaultJsonEncoder();

        public override int MaxOutputCharactersPerInputCharacter
        {
            get { return 12; }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override bool WillEncode(int unicodeScalar)
        {
            return false;
        }

        public override unsafe int FindFirstCharacterToEncode(char* text,
                                                              int   textLength)
        {
            return -1;
        }

        public override int FindFirstCharacterToEncodeUtf8(ReadOnlySpan<byte> utf8Text)
        {
            return -1;
        }

        public override unsafe bool TryEncodeUnicodeScalar(int     unicodeScalar,
                                                           char*   buffer,
                                                           int     bufferLength,
                                                           out int numberOfCharactersWritten)
        {
            numberOfCharactersWritten = 0;

            return false;
        }
    }
}