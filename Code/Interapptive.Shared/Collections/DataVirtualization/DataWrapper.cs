// Derived from https://github.com/lvaleriu/Virtualization
//
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation
// files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy,
// modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
// OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR
// IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;

namespace DataVirtualization
{
    /// <summary>
    /// Wrapper for a data item in the virtual collection
    /// </summary>
    public class DataWrapper<T> : INotifyPropertyChanged where T : class
    {
        private readonly Action populate;
        private T data;
        private long entityID;

        /// <summary>
        /// Constructor
        /// </summary>
        public DataWrapper(int index, long entityID, Action populate)
        {
            Index = index;
            EntityID = entityID;
            this.populate = populate;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public DataWrapper(int index, T data) : this(index, 0, null)
        {
            Data = data;
        }

        /// <summary>
        /// A property has changed
        /// </summary>
        [Obfuscation(Exclude = true)]
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Index
        /// </summary>
        [Obfuscation(Exclude = true)]
        public int Index { get; }

        /// <summary>
        /// Item number
        /// </summary>
        [Obfuscation(Exclude = true)]
        public int ItemNumber => Index + 1;

        /// <summary>
        /// Is the wrapper loading the data?
        /// </summary>
        [Obfuscation(Exclude = true)]
        public bool IsLoading => Data == null;

        /// <summary>
        /// Data item
        /// </summary>
        [Obfuscation(Exclude = true)]
        public T Data
        {
            get => data ?? FetchData();
            set
            {
                if (data != value)
                {
                    this.data = value;
                    this.OnPropertyChanged(nameof(Data));
                    this.OnPropertyChanged(nameof(IsLoading));
                }
            }
        }

        /// <summary>
        /// Entity ID of the data item
        /// </summary>
        [Obfuscation(Exclude = true)]
        public long EntityID
        {
            get => entityID;
            set
            {
                if (value != entityID)
                {
                    this.entityID = value;
                    this.OnPropertyChanged(nameof(EntityID));
                }
            }
        }

        /// <summary>
        /// Is this data wrapper in use
        /// </summary>
        [Obfuscation(Exclude = true)]
        public bool IsInUse => PropertyChanged != null;

        /// <summary>
        /// A property has changed
        /// </summary>
        private void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        /// <summary>
        /// Clean up the data in the wrapper
        /// </summary>
        public void CleanUp()
        {
            if (!IsInUse)
            {
                Data = null;
                Trace.WriteLine("Cleaned up item " + EntityID);
            }
        }

        /// <summary>
        /// Fetch the data since it is missing
        /// </summary>
        private T FetchData()
        {
            populate();

            return null;
        }
    }
}
