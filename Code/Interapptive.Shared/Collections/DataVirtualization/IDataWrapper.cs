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

using System.Reflection;

namespace DataVirtualization
{
    /// <summary>
    /// Wrapper for a data item in the virtual collection
    /// </summary>
    public interface IDataWrapper<T> where T : class
    {
        /// <summary>
        /// Index
        /// </summary>
        [Obfuscation(Exclude = true)]
        int Index { get; }

        /// <summary>
        /// Item number
        /// </summary>
        [Obfuscation(Exclude = true)]
        int ItemNumber { get; }

        /// <summary>
        /// Is the wrapper loading the data?
        /// </summary>
        [Obfuscation(Exclude = true)]
        bool IsLoading { get; }

        /// <summary>
        /// Data item
        /// </summary>
        [Obfuscation(Exclude = true)]
        T Data { get; set; }

        /// <summary>
        /// Entity ID of the data item
        /// </summary>
        [Obfuscation(Exclude = true)]
        long EntityID { get; set; }

        /// <summary>
        /// Is this data wrapper in use
        /// </summary>
        [Obfuscation(Exclude = true)]
        bool IsInUse { get; }

        /// <summary>
        /// Clean up the data in the wrapper
        /// </summary>
        void CleanUp();
    }
}