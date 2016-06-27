//	Interapptive.Shared.IO.Text.Csv.CachedCsvReader
//	Copyright (c) 2005 Sébastien Lorion
//
//	MIT license (http://en.wikipedia.org/wiki/MIT_License)
//
//	Permission is hereby granted, free of charge, to any person obtaining a copy
//	of this software and associated documentation files (the "Software"), to deal
//	in the Software without restriction, including without limitation the rights
//	to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies
//	of the Software, and to permit persons to whom the Software is furnished to do so,
//	subject to the following conditions:
//
//	The above copyright notice and this permission notice shall be included in all
//	copies or substantial portions of the Software.
//
//	THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
//	INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR
//	PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE
//	FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//	ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE

using System;
using System.Collections;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using Interapptive.Shared.IO.Text.Csv.Resources;
using Debug = System.Diagnostics.Debug;

namespace Interapptive.Shared.IO.Text.Csv
{
    /// <summary>
    /// Represents a reader that provides fast, cached, dynamic access to CSV data.
    /// </summary>
    /// <remarks>The number of records is limited to <see cref="System.Int32.MaxValue"/> - 1.</remarks>
    public class CachedCsvReader
        : CsvReader, IListSource
    {
        #region Fields

        /// <summary>
        /// Contains the cached records.
        /// </summary>
        private ArrayList _records;

        /// <summary>
        /// Contains the current record index (inside the cached records array).
        /// </summary>
        private long _currentRecordIndex;

        /// <summary>
        /// Indicates if a new record is being read from the CSV stream.
        /// </summary>
        private bool _readingStream;

        /// <summary>
        /// Contains the binding list linked to this reader.
        /// </summary>
        private CsvBindingList _bindingList;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the CsvReader class.
        /// </summary>
        /// <param name="reader">A <see cref="T:TextReader"/> pointing to the CSV file.</param>
        /// <param name="hasHeaders"><see langword="true"/> if field names are located on the first non commented line, otherwise, <see langword="false"/>.</param>
        /// <exception cref="T:ArgumentNullException">
        ///		<paramref name="reader"/> is a <see langword="null"/>.
        /// </exception>
        /// <exception cref="T:ArgumentException">
        ///		Cannot read from <paramref name="reader"/>.
        /// </exception>
        public CachedCsvReader(TextReader reader, bool hasHeaders)
            : this(reader, hasHeaders, DefaultBufferSize)
        {
        }

        /// <summary>
        /// Initializes a new instance of the CsvReader class.
        /// </summary>
        /// <param name="reader">A <see cref="T:TextReader"/> pointing to the CSV file.</param>
        /// <param name="hasHeaders"><see langword="true"/> if field names are located on the first non commented line, otherwise, <see langword="false"/>.</param>
        /// <param name="bufferSize">The buffer size in bytes.</param>
        /// <exception cref="T:ArgumentNullException">
        ///		<paramref name="reader"/> is a <see langword="null"/>.
        /// </exception>
        /// <exception cref="T:ArgumentException">
        ///		Cannot read from <paramref name="reader"/>.
        /// </exception>
        public CachedCsvReader(TextReader reader, bool hasHeaders, int bufferSize)
            : this(reader, hasHeaders, DefaultDelimiter, DefaultQuote, DefaultEscape, DefaultComment, true, bufferSize)
        {
        }

        /// <summary>
        /// Initializes a new instance of the CsvReader class.
        /// </summary>
        /// <param name="reader">A <see cref="T:TextReader"/> pointing to the CSV file.</param>
        /// <param name="hasHeaders"><see langword="true"/> if field names are located on the first non commented line, otherwise, <see langword="false"/>.</param>
        /// <param name="delimiter">The delimiter character separating each field (default is ',').</param>
        /// <exception cref="T:ArgumentNullException">
        ///		<paramref name="reader"/> is a <see langword="null"/>.
        /// </exception>
        /// <exception cref="T:ArgumentException">
        ///		Cannot read from <paramref name="reader"/>.
        /// </exception>
        public CachedCsvReader(TextReader reader, bool hasHeaders, char delimiter)
            : this(reader, hasHeaders, delimiter, DefaultQuote, DefaultEscape, DefaultComment, true, DefaultBufferSize)
        {
        }

        /// <summary>
        /// Initializes a new instance of the CsvReader class.
        /// </summary>
        /// <param name="reader">A <see cref="T:TextReader"/> pointing to the CSV file.</param>
        /// <param name="hasHeaders"><see langword="true"/> if field names are located on the first non commented line, otherwise, <see langword="false"/>.</param>
        /// <param name="delimiter">The delimiter character separating each field (default is ',').</param>
        /// <param name="bufferSize">The buffer size in bytes.</param>
        /// <exception cref="T:ArgumentNullException">
        ///		<paramref name="reader"/> is a <see langword="null"/>.
        /// </exception>
        /// <exception cref="T:ArgumentException">
        ///		Cannot read from <paramref name="reader"/>.
        /// </exception>
        public CachedCsvReader(TextReader reader, bool hasHeaders, char delimiter, int bufferSize)
            : this(reader, hasHeaders, delimiter, DefaultQuote, DefaultEscape, DefaultComment, true, bufferSize)
        {
        }

        /// <summary>
        /// Initializes a new instance of the CsvReader class.
        /// </summary>
        /// <param name="reader">A <see cref="T:TextReader"/> pointing to the CSV file.</param>
        /// <param name="hasHeaders"><see langword="true"/> if field names are located on the first non commented line, otherwise, <see langword="false"/>.</param>
        /// <param name="delimiter">The delimiter character separating each field (default is ',').</param>
        /// <param name="quote">The quotation character wrapping every field (default is ''').</param>
        /// <param name="escape">
        /// The escape character letting insert quotation characters inside a quoted field (default is '\').
        /// If no escape character, set to '\0' to gain some performance.
        /// </param>
        /// <param name="comment">The comment character indicating that a line is commented out (default is '#').</param>
        /// <param name="trimSpaces"><see langword="true"/> if spaces at the start and end of a field are trimmed, otherwise, <see langword="false"/>. Default is <see langword="true"/>.</param>
        /// <exception cref="T:ArgumentNullException">
        ///		<paramref name="reader"/> is a <see langword="null"/>.
        /// </exception>
        /// <exception cref="T:ArgumentException">
        ///		Cannot read from <paramref name="reader"/>.
        /// </exception>
        [NDependIgnoreTooManyParams]
        public CachedCsvReader(TextReader reader, bool hasHeaders, char delimiter, char quote, char escape, char comment, bool trimSpaces)
            : this(reader, hasHeaders, delimiter, quote, escape, comment, trimSpaces, DefaultBufferSize)
        {
        }

        /// <summary>
        /// Initializes a new instance of the CsvReader class.
        /// </summary>
        /// <param name="reader">A <see cref="T:TextReader"/> pointing to the CSV file.</param>
        /// <param name="hasHeaders"><see langword="true"/> if field names are located on the first non commented line, otherwise, <see langword="false"/>.</param>
        /// <param name="delimiter">The delimiter character separating each field (default is ',').</param>
        /// <param name="quote">The quotation character wrapping every field (default is ''').</param>
        /// <param name="escape">
        /// The escape character letting insert quotation characters inside a quoted field (default is '\').
        /// If no escape character, set to '\0' to gain some performance.
        /// </param>
        /// <param name="comment">The comment character indicating that a line is commented out (default is '#').</param>
        /// <param name="trimSpaces"><see langword="true"/> if spaces at the start and end of a field are trimmed, otherwise, <see langword="false"/>. Default is <see langword="true"/>.</param>
        /// <param name="bufferSize">The buffer size in bytes.</param>
        /// <exception cref="T:ArgumentNullException">
        ///		<paramref name="reader"/> is a <see langword="null"/>.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///		<paramref name="bufferSize"/> must be 1 or more.
        /// </exception>
        [NDependIgnoreTooManyParams]
        public CachedCsvReader(TextReader reader, bool hasHeaders, char delimiter, char quote, char escape, char comment, bool trimSpaces, int bufferSize)
            : base(reader, hasHeaders, delimiter, quote, escape, comment, trimSpaces, bufferSize)
        {
            _records = new ArrayList();
            _currentRecordIndex = -1;
        }

        #endregion

        #region Properties

        #region State

        /// <summary>
        /// Gets the current record index in the CSV file.
        /// </summary>
        /// <value>The current record index in the CSV file.</value>
        public override long CurrentRecordIndex
        {
            get
            {
                return _currentRecordIndex;
            }
        }

        /// <summary>
        /// Gets a value that indicates whether the current stream position is at the end of the stream.
        /// </summary>
        /// <value><see langword="true"/> if the current stream position is at the end of the stream; otherwise <see langword="false"/>.</value>
        public override bool EndOfStream
        {
            get
            {
                if (_currentRecordIndex < base.CurrentRecordIndex)
                    return false;
                else
                    return base.EndOfStream;
            }
        }

        #endregion

        #endregion

        #region Indexers

        /// <summary>
        /// Gets the field at the specified index.
        /// </summary>
        /// <value>The field at the specified index.</value>
        /// <exception cref="T:ArgumentOutOfRangeException">
        ///		<paramref name="field"/> must be included in [0, <see cref="M:FieldCount"/>[.
        /// </exception>
        /// <exception cref="T:InvalidOperationException">
        ///		No record read yet. Call ReadLine() first.
        /// </exception>
        /// <exception cref="MissingFieldCsvException">
        ///		The CSV data appears to be missing a field.
        /// </exception>
        /// <exception cref="T:MalformedCsvException">
        ///		The CSV appears to be corrupt at the current position.
        /// </exception>
        /// <exception cref="T:System.ComponentModel.ObjectDisposedException">
        ///		The instance has been disposed of.
        /// </exception>
        public override String this[int field]
        {
            get
            {
                if (_readingStream)
                    return base[field];
                else if (_currentRecordIndex > -1)
                {
                    if (field > -1 && field < this.FieldCount)
                        return ((string[]) _records[(int) _currentRecordIndex])[field];
                    else
                        throw new ArgumentOutOfRangeException("field", field, string.Format(CultureInfo.InvariantCulture, ExceptionMessage.FieldIndexOutOfRange, field));
                }
                else
                    throw new InvalidOperationException(ExceptionMessage.NoCurrentRecord);
            }
        }

        #endregion

        #region Methods

        #region Read

        /// <summary>
        /// Reads the CSV stream from the current position to the end of the stream.
        /// </summary>
        /// <exception cref="T:System.ComponentModel.ObjectDisposedException">
        ///	The instance has been disposed of.
        /// </exception>
        public virtual void ReadToEnd()
        {
            _currentRecordIndex = base.CurrentRecordIndex;

            while (ReadNextRecord())
            {
            }
        }

        /// <summary>
        /// Reads the next record.
        /// </summary>
        /// <param name="onlyReadHeaders">
        /// Indicates if the reader will proceed to the next record after having read headers.
        /// <see langword="true"/> if it stops after having read headers; otherwise, <see langword="false"/>.
        /// </param>
        /// <param name="skipToNextLine">
        /// Indicates if the reader will skip directly to the next line without parsing the current one.
        /// To be used when an error occurs.
        /// </param>
        /// <returns><see langword="true"/> if a record has been successfully reads; otherwise, <see langword="false"/>.</returns>
        /// <exception cref="T:System.ComponentModel.ObjectDisposedException">
        ///	The instance has been disposed of.
        /// </exception>
        protected override bool ReadNextRecord(bool onlyReadHeaders, bool skipToNextLine)
        {
            if (_currentRecordIndex < base.CurrentRecordIndex)
            {
                _currentRecordIndex++;
                return true;
            }
            else
            {
                _readingStream = true;

                try
                {
                    bool canRead = base.ReadNextRecord(onlyReadHeaders, skipToNextLine);

                    if (canRead)
                    {
                        string[] record = new string[this.FieldCount];
                        CopyCurrentRecordTo(record);

                        _records.Add(record);

                        _currentRecordIndex++;
                    }
                    else
                    {
                        // No more records to read, so set array size to only what is needed
                        _records.Capacity = _records.Count;
                    }

                    return canRead;
                }
                finally
                {
                    _readingStream = false;
                }
            }
        }

        #endregion

        #region Move

        /// <summary>
        /// Moves before the first record.
        /// </summary>
        public void MoveToStart()
        {
            _currentRecordIndex = -1;
        }

        /// <summary>
        /// Moves to the last record read so far.
        /// </summary>
        public void MoveToLastCachedRecord()
        {
            _currentRecordIndex = base.CurrentRecordIndex;
        }

        /// <summary>
        /// Moves to the specified record index.
        /// </summary>
        /// <param name="record">The record index.</param>
        /// <exception cref="T:ArgumentOutOfRangeException">
        ///		Record index must be > 0.
        /// </exception>
        /// <exception cref="T:System.ComponentModel.ObjectDisposedException">
        ///		The instance has been disposed of.
        /// </exception>
        public override void MoveTo(long record)
        {
            if (record < -1)
                throw new ArgumentOutOfRangeException("record", record, ExceptionMessage.RecordIndexLessThanZero);

            if (record <= base.CurrentRecordIndex)
                _currentRecordIndex = record;
            else
            {
                _currentRecordIndex = base.CurrentRecordIndex;

                long offset = record - _currentRecordIndex;

                // read to the last record before the one we want
                while (offset-- > 0 && ReadNextRecord())
                {

                }
            }
        }

        #endregion

        #endregion

        #region IListSource Members

        bool IListSource.ContainsListCollection
        {
            get { return false; }
        }

        System.Collections.IList IListSource.GetList()
        {
            if (_bindingList == null)
                _bindingList = new CsvBindingList(this);

            return _bindingList;
        }

        #endregion

        #region CsvBindingList class

        /// <summary>
        /// Represents a binding list wrapper for a CSV reader.
        /// </summary>
        private class CsvBindingList
            : IBindingList, ITypedList, IList
        {
            #region Fields

            /// <summary>
            /// Contains the linked CSV reader.
            /// </summary>
            private CachedCsvReader _csv;

            /// <summary>
            /// Contains the cached record count.
            /// </summary>
            private int _count;

            /// <summary>
            /// Contains the cached property descriptors.
            /// </summary>
            private PropertyDescriptorCollection _properties;

            /// <summary>
            /// Contains the current sort property.
            /// </summary>
            private CsvPropertyDescriptor _sort;

            /// <summary>
            /// Contains the current sort direction.
            /// </summary>
            private ListSortDirection _direction;

            #endregion

            #region Constructors

            /// <summary>
            /// Initializes a new instance of the CsvBindingList class.
            /// </summary>
            /// <param name="csv"></param>
            public CsvBindingList(CachedCsvReader csv)
            {
                _csv = csv;
                _count = -1;
                _direction = ListSortDirection.Ascending;
            }

            #endregion

            #region IBindingList members

            public void AddIndex(PropertyDescriptor property)
            {
            }

            public bool AllowNew
            {
                get
                {
                    return false;
                }
            }

            public void ApplySort(PropertyDescriptor property, System.ComponentModel.ListSortDirection direction)
            {
                _sort = (CsvPropertyDescriptor) property;
                _direction = direction;

                _csv.ReadToEnd();

                _csv._records.Sort(new CsvRecordComparer(_sort.Index, _direction));
            }

            public PropertyDescriptor SortProperty
            {
                get
                {
                    return _sort;
                }
            }

            public int Find(PropertyDescriptor property, object key)
            {
                int fieldIndex = ((CsvPropertyDescriptor) property).Index;
                string value = (string) key;

                int recordIndex = 0;
                int count = this.Count;

                while (recordIndex < count && _csv[recordIndex, fieldIndex] != value)
                    recordIndex++;

                if (recordIndex == count)
                    return -1;
                else
                    return recordIndex;
            }

            public bool SupportsSorting
            {
                get
                {
                    return true;
                }
            }

            public bool IsSorted
            {
                get
                {
                    return _sort != null;
                }
            }

            public bool AllowRemove
            {
                get
                {
                    return false;
                }
            }

            public bool SupportsSearching
            {
                get
                {
                    return true;
                }
            }

            public System.ComponentModel.ListSortDirection SortDirection
            {
                get
                {
                    return _direction;
                }
            }

            public event System.ComponentModel.ListChangedEventHandler ListChanged
            {
                add { }
                remove { }
            }

            public bool SupportsChangeNotification
            {
                get
                {
                    return false;
                }
            }

            public void RemoveSort()
            {
                _sort = null;
                _direction = ListSortDirection.Ascending;
            }

            public object AddNew()
            {
                throw new NotSupportedException();
            }

            public bool AllowEdit
            {
                get
                {
                    return false;
                }
            }

            public void RemoveIndex(PropertyDescriptor property)
            {
            }

            #endregion

            #region ITypedList Members

            public PropertyDescriptorCollection GetItemProperties(PropertyDescriptor[] listAccessors)
            {
                if (_properties == null)
                {
                    PropertyDescriptor[] properties = new PropertyDescriptor[_csv.FieldCount];

                    for (int i = 0; i < properties.Length; i++)
                        properties[i] = new CsvPropertyDescriptor(((System.Data.IDataReader) _csv).GetName(i), i);

                    _properties = new PropertyDescriptorCollection(properties);
                }

                return _properties;
            }

            public string GetListName(PropertyDescriptor[] listAccessors)
            {
                return string.Empty;
            }

            #endregion

            #region IList Members

            public int Add(object value)
            {
                throw new NotSupportedException();
            }

            public void Clear()
            {
                throw new NotSupportedException();
            }

            public bool Contains(object value)
            {
                throw new NotSupportedException();
            }

            public int IndexOf(object value)
            {
                throw new NotSupportedException();
            }

            public void Insert(int index, object value)
            {
                throw new NotSupportedException();
            }

            public bool IsFixedSize
            {
                get { return true; }
            }

            public bool IsReadOnly
            {
                get { return true; }
            }

            public void Remove(object value)
            {
                throw new NotSupportedException();
            }

            public void RemoveAt(int index)
            {
                throw new NotSupportedException();
            }

            public object this[int index]
            {
                get
                {
                    _csv.MoveTo(index);
                    return _csv._records[index];
                }
                set
                {
                    throw new NotSupportedException();
                }
            }

            #endregion

            #region ICollection Members

            public int Count
            {
                get
                {
                    if (_count < 0)
                    {
                        _csv.ReadToEnd();
                        _count = (int) _csv.CurrentRecordIndex + 1;
                    }

                    return _count;
                }
            }

            public void CopyTo(Array array, int index)
            {
                _csv.MoveToStart();

                while (_csv.ReadNextRecord())
                    _csv.CopyCurrentRecordTo((string[]) array.GetValue(index++));
            }

            public bool IsSynchronized
            {
                get { return false; }
            }

            public object SyncRoot
            {
                get { return null; }
            }

            #endregion

            #region IEnumerable Members

            public IEnumerator GetEnumerator()
            {
                return _csv.GetEnumerator();
            }

            #endregion
        }

        #endregion

        #region CsvPropertyDescriptor class

        /// <summary>
        /// Represents a CSV field property descriptor.
        /// </summary>
        private class CsvPropertyDescriptor
            : PropertyDescriptor
        {
            #region Fields

            /// <summary>
            /// Contains the field index.
            /// </summary>
            private int _index;

            #endregion

            #region Constructors

            /// <summary>
            /// Initializes a new instance of the CsvPropertyDescriptor class.
            /// </summary>
            /// <param name="fieldName">The field name.</param>
            /// <param name="index">The field index.</param>
            public CsvPropertyDescriptor(string fieldName, int index)
                : base(fieldName, null)
            {
                _index = index;
            }

            #endregion

            #region Properties

            /// <summary>
            /// Gets the field index.
            /// </summary>
            /// <value>The field index.</value>
            public int Index
            {
                get { return _index; }
            }

            #endregion

            #region Overrides

            public override bool CanResetValue(object component)
            {
                return false;
            }

            public override object GetValue(object component)
            {
                return ((string[]) component)[_index];
            }

            public override void ResetValue(object component)
            {
            }

            public override void SetValue(object component, object value)
            {
            }

            public override bool ShouldSerializeValue(object component)
            {
                return false;
            }

            public override Type ComponentType
            {
                get
                {
                    return typeof(CachedCsvReader);
                }
            }

            public override bool IsReadOnly
            {
                get
                {
                    return true;
                }
            }

            public override Type PropertyType
            {
                get
                {
                    return typeof(string);
                }
            }

            #endregion
        }

        #endregion

        #region CsvRecordComparer class

        /// <summary>
        /// Represents a CSV record comparer.
        /// </summary>
        private class CsvRecordComparer
            : IComparer
        {
            #region Fields

            /// <summary>
            /// Contains the field index of the values to compare.
            /// </summary>
            private int _field;

            /// <summary>
            /// Contains the sort direction.
            /// </summary>
            private ListSortDirection _direction;

            #endregion

            #region Constructors

            /// <summary>
            /// Initializes a new instance of the CsvRecordComparer class.
            /// </summary>
            /// <param name="field">The field index of the values to compare.</param>
            /// <param name="direction">The sort direction.</param>
            public CsvRecordComparer(int field, ListSortDirection direction)
            {
                if (field < 0)
                    throw new ArgumentOutOfRangeException("field", field, string.Format(CultureInfo.InvariantCulture, Resources.ExceptionMessage.FieldIndexOutOfRange, field));

                _field = field;
                _direction = direction;
            }

            #endregion

            #region IComparer Members

            public int Compare(object x, object y)
            {
                Debug.Assert(x != null && y != null && ((string[]) x).Length == ((string[]) y).Length && _field < ((string[]) x).Length);

                int result = String.Compare(((string[]) x)[_field], ((string[]) y)[_field], false, CultureInfo.CurrentCulture);

                return (_direction == ListSortDirection.Ascending ? result : -result);
            }

            #endregion
        }

        #endregion
    }
}
