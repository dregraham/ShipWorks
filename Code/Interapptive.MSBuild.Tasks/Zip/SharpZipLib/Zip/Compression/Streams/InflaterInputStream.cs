// InflaterInputStream.cs
//
// Copyright (C) 2001 Mike Krueger
// Copyright (C) 2004 John Reilly
//
// This file was translated from java, it was part of the GNU Classpath
// Copyright (C) 2001 Free Software Foundation, Inc.
//
// This program is free software; you can redistribute it and/or
// modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation; either version 2
// of the License, or (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place - Suite 330, Boston, MA  02111-1307, USA.
//
// Linking this library statically or dynamically with other modules is
// making a combined work based on this library.  Thus, the terms and
// conditions of the GNU General Public License cover the whole
// combination.
// 
// As a special exception, the copyright holders of this library give you
// permission to link this library with independent modules to produce an
// executable, regardless of the license terms of these independent
// modules, and to copy and distribute the resulting executable under
// terms of your choice, provided that you also meet, for each linked
// independent module, the terms and conditions of the license of that
// module.  An independent module is a module which is not derived from
// or based on this library.  If you modify this library, you may extend
// this exception to your version of the library, but you are not
// obligated to do so.  If you do not wish to do so, delete this
// exception statement from your version.

using System;
using System.IO;

#if !NETCF_1_0
using System.Security.Cryptography;
#endif

using ICSharpCode.SharpZipLib.Zip.Compression;
using ICSharpCode.SharpZipLib.Checksums;

namespace ICSharpCode.SharpZipLib.Zip.Compression.Streams 
{

	/// <summary>
	/// An input buffer customised for use by <see cref="InflaterInputStream"/>
	/// </summary>
	/// <remarks>
	/// The buffer supports decryption of incoming data.
	/// </remarks>
	public class InflaterInputBuffer
	{
		#region Constructors
		/// <summary>
		/// Initialise a new instance of <see cref="InflaterInputBuffer"/> with a default buffer size
		/// </summary>
		/// <param name="stream">The stream to buffer.</param>
		public InflaterInputBuffer(Stream stream) : this(stream , 4096)
		{
		}
		
		/// <summary>
		/// Initialise a new instance of <see cref="InflaterInputBuffer"/>
		/// </summary>
		/// <param name="stream">The stream to buffer.</param>
		/// <param name="bufferSize">The size to use for the buffer</param>
		/// <remarks>A minimum buffer size of 1KB is permitted.  Lower sizes are treated as 1KB.</remarks>
		public InflaterInputBuffer(Stream stream, int bufferSize)
		{
			inputStream = stream;
			if ( bufferSize < 1024 ) {
				bufferSize = 1024;
			}
			rawData = new byte[bufferSize];
			clearText = rawData;
		}
		#endregion

		/// <summary>
		/// Get the length of bytes bytes in the <see cref="RawData"/>
		/// </summary>
		public int RawLength
		{
			get { 
				return rawLength; 
			}
		}
		
		/// <summary>
		/// Get the contents of the raw data buffer.
		/// </summary>
		/// <remarks>This may contain encrypted data.</remarks>
		public byte[] RawData
		{
			get {
				return rawData;
			}
		}
		
		/// <summary>
		/// Get the number of useable bytes in <see cref="ClearText"/>
		/// </summary>
		public int ClearTextLength
		{
			get {
				return clearTextLength;
			}
		}
		
		/// <summary>
		/// Get the contents of the clear text buffer.
		/// </summary>
		public byte[] ClearText
		{
			get {
				return clearText;
			}
		}
		
		/// <summary>
		/// Get/set the number of bytes available
		/// </summary>
		public int Available
		{
			get { return available; }
			set { available = value; }
		}

		/// <summary>
		/// Call <see cref="Inflater.SetInput(byte[], int, int)"/> passing the current clear text buffer contents.
		/// </summary>
		/// <param name="inflater">The inflater to set input for.</param>
		public void SetInflaterInput(Inflater inflater)
		{
			if ( available > 0 ) {
				inflater.SetInput(clearText, clearTextLength - available, available);
				available = 0;
			}
		}

		/// <summary>
		/// Fill the buffer from the underlying input stream.
		/// </summary>
		public void Fill()
		{
			rawLength = 0;
			int toRead = rawData.Length;
			
			while (toRead > 0) {
				int count = inputStream.Read(rawData, rawLength, toRead);
				if ( count <= 0 ) {
					if (rawLength == 0) {
						throw new SharpZipBaseException("Unexpected EOF"); 
					}
					break;
				}
				rawLength += count;
				toRead -= count;
			}
			
#if !NETCF_1_0
			if ( cryptoTransform != null ) {
				clearTextLength = cryptoTransform.TransformBlock(rawData, 0, rawLength, clearText, 0);
			}
			else 
#endif				
			{
				clearTextLength = rawLength;
			}

			available = clearTextLength;
		}
		
		/// <summary>
		/// Read a buffer directly from the input stream
		/// </summary>
		/// <param name="buffer">The buffer to fill</param>
		/// <returns>Returns the number of bytes read.</returns>
		public int ReadRawBuffer(byte[] buffer)
		{
			return ReadRawBuffer(buffer, 0, buffer.Length);
		}

		/// <summary>
		/// Read a buffer directly from the input stream
		/// </summary>
		/// <param name="outBuffer">The buffer to read into</param>
		/// <param name="offset">The offset to start reading data into.</param>
		/// <param name="length">The number of bytes to read.</param>
		/// <returns>Returns the number of bytes read.</returns>
		public int ReadRawBuffer(byte[] outBuffer, int offset, int length)
		{
			if ( length < 0 ) {
				throw new ArgumentOutOfRangeException("length");
			}
			
			int currentOffset = offset;
			int currentLength = length;
			
			while ( currentLength > 0 ) {
				if ( available <= 0 ) {
					Fill();
					if (available <= 0) {
						return 0;
					}
				}
				int toCopy = Math.Min(currentLength, available);
				System.Array.Copy(rawData, rawLength - (int)available, outBuffer, currentOffset, toCopy);
				currentOffset += toCopy;
				currentLength -= toCopy;
				available -= toCopy;
			}
			return length;
		}
		
		/// <summary>
		/// Read clear text data from the input stream.
		/// </summary>
		/// <param name="outBuffer">The buffer to add data to.</param>
		/// <param name="offset">The offset to start adding data at.</param>
		/// <param name="length">The number of bytes to read.</param>
		/// <returns>Returns the number of bytes actually read.</returns>
		public int ReadClearTextBuffer(byte[] outBuffer, int offset, int length)
		{
			if ( length < 0 ) {
				throw new ArgumentOutOfRangeException("length");
			}
			
			int currentOffset = offset;
			int currentLength = length;
			
			while ( currentLength > 0 ) {
				if ( available <= 0 ) {
					Fill();
					if (available <= 0) {
						return 0;
					}
				}
				
				int toCopy = Math.Min(currentLength, available);
				System.Array.Copy(clearText, clearTextLength - (int)available, outBuffer, currentOffset, toCopy);
				currentOffset += toCopy;
				currentLength -= toCopy;
				available -= toCopy;
			}
			return length;
		}
		
		/// <summary>
		/// Read a <see cref="byte"/> from the input stream.
		/// </summary>
		/// <returns>Returns the byte read.</returns>
		public int ReadLeByte()
		{
			if (available <= 0) {
				Fill();
				if (available <= 0) {
					throw new ZipException("EOF in header");
				}
			}
			byte result = rawData[rawLength - available];
			available -= 1;
			return result;
		}
		
		/// <summary>
		/// Read an <see cref="short"/> in little endian byte order.
		/// </summary>
		/// <returns>The short value read case to an int.</returns>
		public int ReadLeShort()
		{
			return ReadLeByte() | (ReadLeByte() << 8);
		}
		
		/// <summary>
		/// Read an <see cref="int"/> in little endian byte order.
		/// </summary>
		/// <returns>The int value read.</returns>
		public int ReadLeInt()
		{
			return ReadLeShort() | (ReadLeShort() << 16);
		}
		
		/// <summary>
		/// Read a <see cref="long"/> in little endian byte order.
		/// </summary>
		/// <returns>The long value read.</returns>
		public long ReadLeLong()
		{
			return (uint)ReadLeInt() | ((long)ReadLeInt() << 32);
		}

#if !NETCF_1_0
		/// <summary>
		/// Get/set the <see cref="ICryptoTransform"/> to apply to any data.
		/// </summary>
		/// <remarks>Set this value to null to have no transform applied.</remarks>
		public ICryptoTransform CryptoTransform
		{
			set { 
				cryptoTransform = value;
				if ( cryptoTransform != null ) {
					if ( rawData == clearText ) {
						if ( internalClearText == null ) {
							internalClearText = new byte[rawData.Length];
						}
						clearText = internalClearText;
					}
					clearTextLength = rawLength;
					if ( available > 0 ) {
						cryptoTransform.TransformBlock(rawData, rawLength - available, available, clearText, rawLength - available);
					}
				} else {
					clearText = rawData;
					clearTextLength = rawLength;
				}
			}
		}
#endif

		#region Instance Fields
		int rawLength;
		byte[] rawData;
		
		int clearTextLength;
		byte[] clearText;
#if !NETCF_1_0		
		byte[] internalClearText;
#endif
		
		int available;
		
#if !NETCF_1_0
		ICryptoTransform cryptoTransform;
#endif		
		Stream inputStream;
		#endregion
	}
	
	/// <summary>
	/// This filter stream is used to decompress data compressed using the "deflate"
	/// format. The "deflate" format is described in RFC 1951.
	///
	/// This stream may form the basis for other decompression filters, such
	/// as the <see cref="ICSharpCode.SharpZipLib.GZip.GZipInputStream">GZipInputStream</see>.
	///
	/// Author of the original java version : John Leuner.
	/// </summary>
	public class InflaterInputStream : Stream
	{
		#region Constructors
		/// <summary>
		/// Create an InflaterInputStream with the default decompressor
		/// and a default buffer size of 4KB.
		/// </summary>
		/// <param name = "baseInputStream">
		/// The InputStream to read bytes from
		/// </param>
		public InflaterInputStream(Stream baseInputStream)
			: this(baseInputStream, new Inflater(), 4096)
		{
		}
		
		/// <summary>
		/// Create an InflaterInputStream with the specified decompressor
		/// and a default buffer size of 4KB.
		/// </summary>
		/// <param name = "baseInputStream">
		/// The source of input data
		/// </param>
		/// <param name = "inf">
		/// The decompressor used to decompress data read from baseInputStream
		/// </param>
		public InflaterInputStream(Stream baseInputStream, Inflater inf)
			: this(baseInputStream, inf, 4096)
		{
		}
		
		/// <summary>
		/// Create an InflaterInputStream with the specified decompressor
		/// and the specified buffer size.
		/// </summary>
		/// <param name = "baseInputStream">
		/// The InputStream to read bytes from
		/// </param>
		/// <param name = "inflater">
		/// The decompressor to use
		/// </param>
		/// <param name = "bufferSize">
		/// Size of the buffer to use
		/// </param>
		public InflaterInputStream(Stream baseInputStream, Inflater inflater, int bufferSize)
		{
			if (baseInputStream == null) {
				throw new ArgumentNullException("baseInputStream");
			}
			
			if (inflater == null) {
				throw new ArgumentNullException("inflater");
			}
			
			if (bufferSize <= 0) {
				throw new ArgumentOutOfRangeException("bufferSize");
			}
			
			this.baseInputStream = baseInputStream;
			this.inf = inflater;
			
			inputBuffer = new InflaterInputBuffer(baseInputStream, bufferSize);
		}
		
		#endregion

		/// <summary>
		/// Get/set flag indicating ownership of underlying stream.
		/// When the flag is true <see cref="Close"/> will close the underlying stream also.
		/// </summary>
		/// <remarks>
		/// The default value is true.
		/// </remarks>
		public bool IsStreamOwner
		{
			get { return isStreamOwner; }
			set { isStreamOwner = value; }
		}
		
		/// <summary>
		/// Skip specified number of bytes of uncompressed data
		/// </summary>
		/// <param name ="count">
		/// Number of bytes to skip
		/// </param>
		/// <returns>
		/// The number of bytes skipped, zero if the end of 
		/// stream has been reached
		/// </returns>
		/// <exception cref="ArgumentOutOfRangeException">
		/// <paramref name="count">The number of bytes</paramref> to skip is less than or equal to zero.
		/// </exception>
		public long Skip(long count)
		{
			if (count <= 0) {
				throw new ArgumentOutOfRangeException("count");
			}
			
			// v0.80 Skip by seeking if underlying stream supports it...
			if (baseInputStream.CanSeek) {
				baseInputStream.Seek(count, SeekOrigin.Current);
				return count;
			} 
			else {
				int length = 2048;
				if (count < length) {
					length = (int) count;
				}

				byte[] tmp = new byte[length];
				int readCount = 1;
				long toSkip = count;

				while ((toSkip > 0) && (readCount > 0) ) {
					if (toSkip < length) {
						length = (int)toSkip;
					}

					readCount = baseInputStream.Read(tmp, 0, length);
					toSkip -= readCount;
				}

				return count - toSkip;
			}
		}
		
		/// <summary>
		/// Clear any cryptographic state.
		/// </summary>		
		protected void StopDecrypting()
		{
#if !NETCF_1_0			
			inputBuffer.CryptoTransform = null;
#endif			
		}

		/// <summary>
		/// Returns 0 once the end of the stream (EOF) has been reached.
		/// Otherwise returns 1.
		/// </summary>
		public virtual int Available 
		{
			get {
				return inf.IsFinished ? 0 : 1;
			}
		}
		
		/// <summary>
		/// Fills the buffer with more data to decompress.
		/// </summary>
		/// <exception cref="SharpZipBaseException">
		/// Stream ends early
		/// </exception>
		protected void Fill()
		{
			inputBuffer.Fill();
			inputBuffer.SetInflaterInput(inf);
		}

		#region Stream Overrides
		/// <summary>
		/// Gets a value indicating whether the current stream supports reading
		/// </summary>
		public override bool CanRead 
		{
			get {
				return baseInputStream.CanRead;
			}
		}
		
		/// <summary>
		/// Gets a value of false indicating seeking is not supported for this stream.
		/// </summary>
		public override bool CanSeek {
			get {
				return false;
			}
		}
		
		/// <summary>
		/// Gets a value of false indicating that this stream is not writeable.
		/// </summary>
		public override bool CanWrite {
			get {
				return false;
			}
		}
		
		/// <summary>
		/// A value representing the length of the stream in bytes.
		/// </summary>
		public override long Length {
			get {
				return inputBuffer.RawLength;
			}
		}
		
		/// <summary>
		/// The current position within the stream.
		/// Throws a NotSupportedException when attempting to set the position
		/// </summary>
		/// <exception cref="NotSupportedException">Attempting to set the position</exception>
		public override long Position {
			get {
				return baseInputStream.Position;
			}
			set {
				throw new NotSupportedException("InflaterInputStream Position not supported");
			}
		}
		
		/// <summary>
		/// Flushes the baseInputStream
		/// </summary>
		public override void Flush()
		{
			baseInputStream.Flush();
		}
		
		/// <summary>
		/// Sets the position within the current stream
		/// Always throws a NotSupportedException
		/// </summary>
		/// <param name="offset">The relative offset to seek to.</param>
		/// <param name="origin">The <see cref="SeekOrigin"/> defining where to seek from.</param>
		/// <returns>The new position in the stream.</returns>
		/// <exception cref="NotSupportedException">Any access</exception>
		public override long Seek(long offset, SeekOrigin origin)
		{
			throw new NotSupportedException("Seek not supported");
		}
		
		/// <summary>
		/// Set the length of the current stream
		/// Always throws a NotSupportedException
		/// </summary>
		/// <param name="value">The new length value for the stream.</param>
		/// <exception cref="NotSupportedException">Any access</exception>
		public override void SetLength(long value)
		{
			throw new NotSupportedException("InflaterInputStream SetLength not supported");
		}
		
		/// <summary>
		/// Writes a sequence of bytes to stream and advances the current position
		/// This method always throws a NotSupportedException
		/// </summary>
		/// <param name="buffer">Thew buffer containing data to write.</param>
		/// <param name="offset">The offset of the first byte to write.</param>
		/// <param name="count">The number of bytes to write.</param>
		/// <exception cref="NotSupportedException">Any access</exception>
		public override void Write(byte[] buffer, int offset, int count)
		{
			throw new NotSupportedException("InflaterInputStream Write not supported");
		}
		
		/// <summary>
		/// Writes one byte to the current stream and advances the current position
		/// Always throws a NotSupportedException
		/// </summary>
		/// <param name="value">The byte to write.</param>
		/// <exception cref="NotSupportedException">Any access</exception>
		public override void WriteByte(byte value)
		{
			throw new NotSupportedException("InflaterInputStream WriteByte not supported");
		}
		
		/// <summary>
		/// Entry point to begin an asynchronous write.  Always throws a NotSupportedException.
		/// </summary>
		/// <param name="buffer">The buffer to write data from</param>
		/// <param name="offset">Offset of first byte to write</param>
		/// <param name="count">The maximum number of bytes to write</param>
		/// <param name="callback">The method to be called when the asynchronous write operation is completed</param>
		/// <param name="state">A user-provided object that distinguishes this particular asynchronous write request from other requests</param>
		/// <returns>An <see cref="System.IAsyncResult">IAsyncResult</see> that references the asynchronous write</returns>
		/// <exception cref="NotSupportedException">Any access</exception>
		public override IAsyncResult BeginWrite(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
		{
			throw new NotSupportedException("InflaterInputStream BeginWrite not supported");
		}
		
		/// <summary>
		/// Closes the input stream.  When <see cref="IsStreamOwner"></see>
		/// is true the underlying stream is also closed.
		/// </summary>
		public override void Close()
		{
			if ( !isClosed ) {
				isClosed = true;
				if ( isStreamOwner ) {
					baseInputStream.Close();
				}
			}
		}

		/// <summary>
		/// Reads decompressed data into the provided buffer byte array
		/// </summary>
		/// <param name ="buffer">
		/// The array to read and decompress data into
		/// </param>
		/// <param name ="offset">
		/// The offset indicating where the data should be placed
		/// </param>
		/// <param name ="count">
		/// The number of bytes to decompress
		/// </param>
		/// <returns>The number of bytes read.  Zero signals the end of stream</returns>
		/// <exception cref="SharpZipBaseException">
		/// Inflater needs a dictionary
		/// </exception>
		public override int Read(byte[] buffer, int offset, int count) 
		{
			if (inf.IsNeedingDictionary)
			{
				throw new SharpZipBaseException("Need a dictionary");
			}

			int remainingBytes = count;
			while (true) {
				int bytesRead = inf.Inflate(buffer, offset, remainingBytes); 
				offset += bytesRead;
				remainingBytes -= bytesRead;

				if (remainingBytes == 0 || inf.IsFinished) {
					break; 
				}

				if ( inf.IsNeedingInput ) {
					Fill();
				}
				else if ( bytesRead == 0 ) {
					throw new ZipException("Dont know what to do");
				}
			}
			return count - remainingBytes;
		}
		#endregion
		#region Instance Fields
		/// <summary>
		/// Decompressor for this stream
		/// </summary>
		protected Inflater inf;

		/// <summary>
		/// <see cref="InflaterInputBuffer">Input buffer</see> for this stream.
		/// </summary>
		protected InflaterInputBuffer inputBuffer;

		/// <summary>
		/// Base stream the inflater reads from.
		/// </summary>
		protected Stream baseInputStream;
		
		/// <summary>
		/// The compressed size
		/// </summary>
		protected long csize;

		/// <summary>
		/// Flag indicating wether this instance has been closed or not.
		/// </summary>
		bool isClosed;

		/// <summary>
		/// Flag indicating wether this instance is designated the stream owner.
		/// When closing if this flag is true the underlying stream is closed.
		/// </summary>
		bool isStreamOwner = true;
		#endregion
	}
}
