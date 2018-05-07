using System;
using Infrastructure;
using Multiformats.Base;

namespace Shortest.Services
{
    public class IdentifierConverter : IIdentifierConverter
    {
        public static ulong XorValue = 0x259DFA9352AF3EC1;

        public string Encode(long id)
        {
            return IntToString(Rearrange((ulong)id ^ XorValue));
        }

        public long Decode(string id)
        {
            return (long)(Rearrange(StringToInt(id)) ^ XorValue);
        }

        private ulong Rearrange(ulong value)
        {
            ulong newValue = 0;

            for (int i = 0; i < 64; i++, value = value >> 1)
            {
                int newBytePosition = i % 8;
                int newBitPosition = i / 8;

                newValue |= (value & 1) << newBytePosition * 8 + newBitPosition;
            }

            return newValue;
        }

        private string IntToString(ulong value)
        {
            var codedValue = Rearrange(value);
            byte[] codedBytes = BitConverter.GetBytes(codedValue);
            return Multibase.Encode(MultibaseEncoding.Base58Flickr, codedBytes).Substring(1);
        }

        private ulong StringToInt(string value)
        {
            byte[] codedBytes = Multibase.Decode("Z" + value, out MultibaseEncoding _);
            ulong codedValue = BitConverter.ToUInt64(codedBytes, 0);
            return Rearrange(codedValue);
        }
    }
}