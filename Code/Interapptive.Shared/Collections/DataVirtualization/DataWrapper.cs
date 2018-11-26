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

using System.ComponentModel;

namespace DataVirtualization
{
    /// <summary>
    /// Wrapper for a data item in the virtual collection
    /// </summary>
    public class DataWrapper<T> : INotifyPropertyChanged where T : class
    {
        private T data;

        /// <summary>
        /// Constructor
        /// </summary>
        public DataWrapper(int index)
        {
            Index = index;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public DataWrapper(int index, T data) : this(index)
        {
            Data = data;
        }

        /// <summary>
        /// A property has changed
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Index
        /// </summary>
        public int Index { get; }

        /// <summary>
        /// Item number
        /// </summary>
        public int ItemNumber => Index + 1;

        /// <summary>
        /// Is the wrapper loading the data?
        /// </summary>
        public bool IsLoading => Data == null;

        /// <summary>
        /// Data item
        /// </summary>
        public T Data
        {
            get => data;
            set
            {
                this.data = value;
                this.OnPropertyChanged(nameof(Data));
                this.OnPropertyChanged(nameof(IsLoading));
            }
        }

        /// <summary>
        /// Is this data wrapper in use
        /// </summary>
        public bool IsInUse => PropertyChanged != null;

        /// <summary>
        /// A property has changed
        /// </summary>
        private void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
