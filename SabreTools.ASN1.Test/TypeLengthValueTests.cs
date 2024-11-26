using System;
using System.IO;
using Xunit;

namespace SabreTools.ASN1.Test
{
    public class TypeLengthValueTests
    {
        [Fact]
        public void Constructor_EmptyArray_Throws()
        {
            int index = 0;
            byte[] data = [];
            Assert.Throws<InvalidDataException>(() => new TypeLengthValue(data, ref index));
        }

        [Fact]
        public void Constructor_ValidArrayNegativeIndex_Throws()
        {
            int index = -1;
            byte[] data = [0x00];
            Assert.Throws<IndexOutOfRangeException>(() => new TypeLengthValue(data, ref index));
        }

        [Fact]
        public void Constructor_ValidArrayOverIndex_Throws()
        {
            int index = 10;
            byte[] data = [0x00];
            Assert.Throws<IndexOutOfRangeException>(() => new TypeLengthValue(data, ref index));
        }

        [Fact]
        public void Constructor_ValidMinimalArray_Returns()
        {
            int index = 0;
            byte[] data = [0x00];
            var tlv = new TypeLengthValue(data, ref index);

            Assert.Equal(ASN1Type.V_ASN1_EOC, tlv.Type);
            Assert.Equal(default, tlv.Length);
            Assert.Null(tlv.Value);
        }

        [Fact]
        public void Constructor_EmptyStream_Throws()
        {
            Stream data = new MemoryStream([], 0, 0, false, false);
            Assert.Throws<InvalidDataException>(() => new TypeLengthValue(data));
        }

        [Fact]
        public void Constructor_ValidMinimalStream_Returns()
        {
            Stream data = new MemoryStream([0x00]);
            var tlv = new TypeLengthValue(data);

            Assert.Equal(ASN1Type.V_ASN1_EOC, tlv.Type);
            Assert.Equal(default, tlv.Length);
            Assert.Null(tlv.Value);
        }

        [Fact]
        public void Constructor_ValidBoolean_Returns()
        {
            Stream data = new MemoryStream([0x01, 0x01, 0x01]);
            var tlv = new TypeLengthValue(data);

            Assert.Equal(ASN1Type.V_ASN1_BOOLEAN, tlv.Type);
            Assert.Equal(1UL, tlv.Length);
            Assert.NotNull(tlv.Value);

            byte[]? valueAsArray = tlv.Value as byte[];
            Assert.NotNull(valueAsArray);
            byte actual = Assert.Single(valueAsArray);
            Assert.Equal(0x01, actual);
        }

        [Theory]
        [InlineData(new byte[] { 0x26, 0x81, 0x03, 0x01, 0x01, 0x01 })]
        [InlineData(new byte[] { 0x26, 0x82, 0x00, 0x03, 0x01, 0x01, 0x01 })]
        [InlineData(new byte[] { 0x26, 0x83, 0x00, 0x00, 0x03, 0x01, 0x01, 0x01 })]
        [InlineData(new byte[] { 0x26, 0x84, 0x00, 0x00, 0x00, 0x03, 0x01, 0x01, 0x01 })]
        [InlineData(new byte[] { 0x26, 0x85, 0x00, 0x00, 0x00, 0x00, 0x03, 0x01, 0x01, 0x01 })]
        [InlineData(new byte[] { 0x26, 0x86, 0x00, 0x00, 0x00, 0x00, 0x00, 0x03, 0x01, 0x01, 0x01 })]
        [InlineData(new byte[] { 0x26, 0x87, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x03, 0x01, 0x01, 0x01 })]
        [InlineData(new byte[] { 0x26, 0x88, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x03, 0x01, 0x01, 0x01 })]
        public void Constructor_ComplexValue_Returns(byte[] arr)
        {
            Stream data = new MemoryStream(arr);
            var tlv = new TypeLengthValue(data);

            Assert.Equal(ASN1Type.V_ASN1_CONSTRUCTED | ASN1Type.V_ASN1_OBJECT, tlv.Type);
            Assert.Equal(3UL, tlv.Length);
            Assert.NotNull(tlv.Value);

            TypeLengthValue[]? valueAsArray = tlv.Value as TypeLengthValue[];
            Assert.NotNull(valueAsArray);
            TypeLengthValue actual = Assert.Single(valueAsArray);

            Assert.Equal(ASN1Type.V_ASN1_BOOLEAN, actual.Type);
            Assert.Equal(1UL, actual.Length);
            Assert.NotNull(actual.Value);
        }

        [Theory]
        [InlineData(new byte[] { 0x26, 0x80 })]
        [InlineData(new byte[] { 0x26, 0x89 })]
        public void Constructor_ComplexValueInvalidLength_Throws(byte[] arr)
        {
            Stream data = new MemoryStream(arr);
            Assert.Throws<InvalidOperationException>(() => new TypeLengthValue(data));
        }
    }
}